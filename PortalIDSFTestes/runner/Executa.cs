using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using NUnit.Framework.Interfaces;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.runner
{
    public abstract class Executa
    {
        protected IPage page;
        private IPlaywright? playwright;
        private IBrowser? browser;
        private IBrowserContext? context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ScreenshotHelper.ClearOldScreenshots();
        }

        protected async Task<IPage> AbrirBrowserAsync()
        {
            playwright = await Playwright.CreateAsync();

            // Detecta CI (Azure DevOps define TF_BUILD=true)
            var isCi = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
                       || string.Equals(Environment.GetEnvironmentVariable("TF_BUILD"), "True", StringComparison.OrdinalIgnoreCase);

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = true, // Headless no CI, pode ser false local
                //Headless = isCi, 
                Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
            };

            browser = await playwright.Chromium.LaunchAsync(launchOptions);

            // >>>>> A MUDANÇA CRUCIAL ESTÁ AQUI <<<<<
            // Crie opções de contexto para definir o viewport e outras configurações
            var contextOptions = new BrowserNewContextOptions()
            {
                ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 },
                IgnoreHTTPSErrors = true
            };
            context = await browser.NewContextAsync(contextOptions);
            page = await context.NewPageAsync();

            var config = new ConfigurationManager();
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var linkCustodia = config["Links:Portal"];
            await page.GotoAsync(linkCustodia);
            return page;
        }

        protected async Task FecharBrowserAsync()
        {
            // Captura screenshot em caso de falha
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed && page != null)
            {
                var testName = ScreenshotHelper.GetTestName();
                await ScreenshotHelper.CaptureAndAttachScreenshotAsync(page, testName, TestStatus.Failed.ToString());
            }
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed && page != null)
            {
                var testName = ScreenshotHelper.GetTestName();
                await ScreenshotHelper.CaptureAndAttachScreenshotAsync(page, testName, TestStatus.Passed.ToString());
            }
            if (context != null)
                await context.CloseAsync();
            if (browser != null)
                await browser.CloseAsync();
            playwright?.Dispose();
        }
    }
}