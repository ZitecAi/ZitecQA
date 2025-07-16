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

            // Executa os testes para cada usuário, mas abrindo duas páginas paralelas:
            var tasks = Usuarios.Select(u => ExecutarTesteParaleloParaUsuario(browser, u)).ToList();
            var resultados = await Task.WhenAll(tasks);

            // Combina todas as listas de páginas e cedentes
            var listaPagina = resultados.SelectMany(r => r.paginas).ToList();
            var listaCedente = resultados.SelectMany(r => r.cedentes).ToList();

            try
            {
                var emailPadrao = new EmailPadrao(
                    "jt@zitec.ai",
                    "Segue relatório da página de cedentes.",
                    EnviarEmail.GerarHtml(listaPagina, listaCedente)
                );
                EnviarEmail.SendMailWithAttachment(emailPadrao);
                Console.WriteLine("Email enviado");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao enviar email: {e.Message}");
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        private static async Task<(List<Pagina> paginas, List<Cedente> cedentes)> ExecutarTesteParaleloParaUsuario(IBrowser browser, Usuario usuario)
        {
            var listaPagina = new List<Pagina>();
            var listaCedente = new List<Cedente>();

            // Cria duas páginas para rodar os testes em paralelo: uma para PF e outra para PJ
            var pagePf = await browser.NewPageAsync();
            var pagePj = await browser.NewPageAsync();

            try
            {
                listaPagina.Add(await LoginGeral.Login(pagePf, usuario));
                listaPagina.Add(await LoginGeral.Login(pagePj, usuario));

                // Executa os testes em paralelo
                var taskPj = Task.Run(async () =>
                {
                    var (paginaPj, cedentePj) = await CadastroCedentes.CedentesPJ(pagePj);
                    return (paginaPj, cedentePj);
                });

                var taskPf = Task.Run(async () =>
                {
                    var (paginaPf, cedentePf) = await CadastroCedentes.CedentesPf(pagePf);
                    return (paginaPf, cedentePf);
                });

                await Task.WhenAll(taskPj, taskPf);

                // Coleta os resultados
                var resultadoPj = taskPj.Result;
                var resultadoPf = taskPf.Result;

                listaPagina.Add(resultadoPj.paginaPj);
                listaCedente.Add(resultadoPj.cedentePj);

                listaPagina.Add(resultadoPf.paginaPf);
                listaCedente.Add(resultadoPf.cedentePf);

                foreach (var pg in listaPagina)
                {
                    pg.Perfil ??= usuario.Nivel.ToString();
                }

                // Logout nas duas páginas
                await Task.WhenAll(
                    pagePf.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync(),
                    pagePj.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync()
                );

                await Task.WhenAll(
                    pagePf.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync(),
                    pagePj.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro com usuário {usuario.Email}: {ex.Message}");
            }
            finally
            {
                await Task.WhenAll(pagePf.CloseAsync(), pagePj.CloseAsync());
            }

            return (listaPagina, listaCedente);
        }
    }
}
