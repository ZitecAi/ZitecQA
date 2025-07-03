using Microsoft.Playwright;
using TesteCedente.Model;
using TesteCedente.Pages.CedentesPage;
using TesteCedente.Pages;
using TesteCedente.Utils;

namespace TesteCedente
{
    class Program
    {
        public static List<Usuario> Usuarios { get; set; }

        public static async Task Main(string[] args)
        {
            var playwright = await Playwright.CreateAsync();
            IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Channel = "chrome",
                Headless = false,
                SlowMo = 50,
                Timeout = 0,
                Args = new List<string>
                {
                    "--window-size=2120,1120",
                    "--disable-web-security",
                    "--disable-site-isolation-trials",
                    "--disable-features=IsolateOrigins,site-per-process",
                    "--start-maximized"
                }
            });

            Usuarios = Util.GetUsuariosForTest();
            var tasks = Usuarios.Select(u => ExecutarTesteParaUsuario(browser, u)).ToList();
            var resultados = await Task.WhenAll(tasks);

            var listaPagina = resultados.SelectMany(r => r).ToList();

            try
            {
                var emailPadrao = new EmailPadrao(
                    "jt@zitec.ai",
                    "Segue relatório com as páginas mais importantes do portal IDSF testadas.",
                    EnviarEmail.GerarHtml(listaPagina)
                );
                EnviarEmail.SendMailWithAttachment(emailPadrao);
                Console.WriteLine("Email enviado");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao enviar email: {e.Message}");
            }
        }

        private static async Task<List<Pagina>> ExecutarTesteParaUsuario(IBrowser browser, Usuario usuario)
        {
            var listaPagina = new List<Pagina>();
            var page = await browser.NewPageAsync();

            try
            {
                listaPagina.Add(await LoginGeral.Login(page, usuario));

                switch (usuario.Nivel)
                {
                    case Usuario.NivelEnum.Master:
                    case Usuario.NivelEnum.Consultoria:
                    case Usuario.NivelEnum.Gestora:
                        listaPagina.Add(await CadastroCedentes.CedentesPJ(page));
                        listaPagina.Add(await CadastroCedentes.CedentesPf(page));
                        break;
                }

                foreach (var pg in listaPagina)
                {
                    pg.Perfil ??= usuario.Nivel.ToString();
                }

                await page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro com usuário {usuario.Email}: {ex.Message}");
            }
            finally
            {
                await page.CloseAsync();
            }

            return listaPagina;
        }
    }
}
