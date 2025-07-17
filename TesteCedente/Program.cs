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

            // Corrigido: separa cada item da tupla
            var listaPagina = resultados.SelectMany(r => r.listaPagina).ToList();
            var listaCedente = resultados.SelectMany(r => r.listaCedente).ToList();

            try
            {
                var emailPadrao = new EmailPadrao(
                    "jt@zitec.ai",
                    "Segue relatório da página de cedentes do portal IDSF.",
                    EnviarEmail.GerarHtml(listaPagina, listaCedente)
                );

                EnviarEmail.SendMailWithAttachment(emailPadrao);
                Console.WriteLine("Email enviado");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao enviar email: {e.Message}");
            }
        }

        private static async Task<(List<Pagina> listaPagina, List<Cedente> listaCedente)> ExecutarTesteParaUsuario(IBrowser browser, Usuario usuario)
        {
            Cedente cedente;
            Pagina pagina;
            var listaCedente = new List<Cedente>();
            var listaPagina = new List<Pagina>();
            var page = await browser.NewPageAsync();

            try
            {
                listaPagina.Add(await LoginGeral.Login(page, usuario));

                switch (usuario.Nivel)
                {
                    case Usuario.NivelEnum.Master:
                        //(pagina, cedente) = await CadastroCedentes.CedentesConsultoria(page);
                        //(pagina, cedente) = await CadastroCedentes.CedentesGestora(page);

                        (pagina, cedente) = await CadastroCedentes.CedentesPJ(page);
                        listaPagina.Add(pagina);
                        listaCedente.Add(cedente);

                        (pagina, cedente) = await CadastroCedentes.CedentesPf(page);
                        listaPagina.Add(pagina);
                        listaCedente.Add(cedente);
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

            return (listaPagina, listaCedente);
        }
    }
}
