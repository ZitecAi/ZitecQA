using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace PortalIDSFTestes.runner
{
    public abstract class Executa
    {
        private IPlaywright? playwright;
        private IBrowser? browser;
        private IBrowserContext? context;
        protected async Task<IPage> AbrirBrowserAsync()
        {
            playwright = await Playwright.CreateAsync();

            // Detecta CI (Azure DevOps define TF_BUILD=true)
            var isCi = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
                       || string.Equals(Environment.GetEnvironmentVariable("TF_BUILD"), "True", StringComparison.OrdinalIgnoreCase);

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = true, // Headless no CI, pode ser false local
                Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
            };

            browser = await playwright.Chromium.LaunchAsync(launchOptions);

            var contextOptions = new BrowserNewContextOptions()
            {
                ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 },
                IgnoreHTTPSErrors = true
            };

            context = await browser.NewContextAsync(contextOptions);

            await context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            var page = await context.NewPageAsync();

            var config = new ConfigurationManager();
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var linkPortal = config["Links:Portal"];
            await page.GotoAsync(linkPortal);
            return page;
        }
        protected async Task FecharBrowserAsync()
        {
            if (context != null)
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    var traceDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "playwright-traces");
                    Directory.CreateDirectory(traceDir);

                    var tracePath = Path.Combine(traceDir, $"{TestContext.CurrentContext.Test.Name}.zip");

                    await context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                }
                else
                {
                    await context.Tracing.StopAsync();
                }

                await context.CloseAsync();
            }

            if (browser != null)
                await browser.CloseAsync();
            playwright?.Dispose();
        }
    }
}