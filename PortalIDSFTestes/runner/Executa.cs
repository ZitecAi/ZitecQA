using Allure.NUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PortalIDSFTestes.runner
{
    [AllureNUnit]
    public abstract class Executa
    {
        private IPlaywright playwright;
        private IBrowser browser;
        private IBrowserContext context;

        protected async Task<IPage> AbrirBrowserAsync()
        {
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            context = await browser.NewContextAsync();
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
                await context.CloseAsync();
            if (browser != null)
                await browser.CloseAsync();
            playwright?.Dispose();
        }


    }
}
