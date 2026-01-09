using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.runner
{
    public abstract class TestBase
    {
        protected IPage page;
        private IPlaywright? playwright;
        private IBrowser? browser;
        private IBrowserContext? context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            VideoHelper.ClearOldVideos();
        }


        protected async Task<IPage> AbrirBrowserAsync()
        {
            playwright = await Playwright.CreateAsync();

            var isCi = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) ||
                       string.Equals(Environment.GetEnvironmentVariable("TF_BUILD"), "True", StringComparison.OrdinalIgnoreCase);

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
            };

            browser = await playwright.Chromium.LaunchAsync(launchOptions);

            var videosDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "videos");
            Directory.CreateDirectory(videosDir);

            var contextOptions = new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
                IgnoreHTTPSErrors = true,
                RecordVideoDir = videosDir,
                RecordVideoSize = new RecordVideoSize { Width = 1366, Height = 768 }
            };

            context = await browser.NewContextAsync(contextOptions);
            page = await context.NewPageAsync();
            page.SetDefaultTimeout(90000);
            page.SetDefaultNavigationTimeout(90000);

            var config = new ConfigurationManager();
            var envStg = Environment.GetEnvironmentVariable("PORTAL_LINK");
            config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
            var linkPortal = config["Links:Portal"] ?? envStg;
            page.DOMContentLoaded += async (sender, e) =>
            {
                // Injeta o estilo CSS para aplicar o zoom de 75%
                await page.AddStyleTagAsync(new PageAddStyleTagOptions
                {
                    Content = "body { zoom: 0.75; }"
                });
            };
            await page.GotoAsync(linkPortal!);
            return page;
        }

        protected async Task FecharBrowserAsync()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status.ToString();

            try
            {
                if (page != null)
                {
                    await VideoUtils.ForceVideoFinalization(page);
                }

                if (context != null)
                {
                    await context.CloseAsync();
                }

                if (page != null)
                {
                    await VideoHelper.AttachVideoAsync(page, status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar vídeo no teardown: {ex.Message}");
            }
            finally
            {
                try
                {
                    if (browser != null)
                    {
                        await browser.CloseAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao fechar browser: {ex.Message}");
                }

                try
                {
                    playwright?.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao dispose playwright: {ex.Message}");
                }
            }
        }
    }
}
