using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework; // Adicione este using para acessar o TestContext
using System.IO;      // Adicione este para manipular caminhos de arquivo

namespace PortalIDSFTestes.runner
{
    [AllureNUnit]
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
                Headless = isCi, // Headless no CI, pode ser false local
                Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
            };

            browser = await playwright.Chromium.LaunchAsync(launchOptions);

            // >>>>> A MUDANÇA CRUCIAL ESTÁ AQUI <<<<<
            // Crie opções de contexto para definir o viewport e outras configurações
            var contextOptions = new BrowserNewContextOptions()
            {
                // Define um tamanho de tela padrão para evitar problemas com elementos fora de vista
                ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 },

                // Ignorar erros de certificado HTTPS (comum em ambientes de staging/homologação)
                IgnoreHTTPSErrors = true
            };

            // Passe as opções ao criar o contexto
            context = await browser.NewContextAsync(contextOptions);

            // >>>>> BÔNUS: ATIVAR O TRACE VIEWER PARA DEBUG NO CI <<<<<
            // Inicia o rastreamento para ajudar a debugar falhas no CI
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
            // >>>>> BÔNUS: SALVAR O TRACE SE O TESTE FALHAR <<<<<
            if (context != null)
            {
                // Verifica se o teste atual falhou
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    // Cria um diretório para os traces se não existir
                    var traceDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "playwright-traces");
                    Directory.CreateDirectory(traceDir);

                    // Define o nome do arquivo de trace
                    var tracePath = Path.Combine(traceDir, $"{TestContext.CurrentContext.Test.Name}.zip");

                    // Para o rastreamento e salva o arquivo
                    await context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                }
                else
                {
                    // Se o teste passou, apenas para o rastreamento sem salvar
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