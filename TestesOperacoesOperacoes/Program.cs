using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using TesteOperacoesOperacoes.Model;
using TesteOperacoesOperacoes;
using TesteOperacoesOperacoes.Util;



namespace TestesOperacoesOperacoes
{
    public class Program
    {
        public static IConfigurationRoot Config { get; set; }
        public static List<Usuario> Usuarios { get; set; } = new List<Usuario>();


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

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Program.Config = builder.Build();
            var usuarios = Util.GetUsuariosForTest();
            var tasks = usuarios.Select(u => ExecutarTesteParaUsuario(browser, u)).ToList();

            var resultados = await Task.WhenAll(tasks);
            Console.WriteLine("Todos os testes foram executados.");

            var listaPagina = resultados.SelectMany(r => r.Item1).ToList();
            var listaFluxos = resultados.SelectMany(r => r.Item2).ToList();
            var listaOperacoes = resultados.SelectMany(r => r.Item3).ToList();

            try
            {
                //var emailPadrao = new EmailPadrao(
                //            "todos@zitec.ai",
                //            "Segue relatório com as páginas mais importantes do portal IDSF testadas.",
                //            TesteOperacoesOperacoes.Util.EnviarEmail.GerarHtml(listaPagina, listaFluxos, listaOperacoes)
                //        );
                //EnviarEmail.SendMailWithAttachment(emailPadrao);
                //Console.WriteLine("Email enviado");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao enviar email: {e.Message}");
            }
        }

        private static async Task<(List<Pagina>, List<FluxosDeCadastros>, List<Operacoes>)> ExecutarTesteParaUsuario(IBrowser browser, Usuario usuario)
        {
            var listaPagina = new List<Pagina>();
            var listaFluxos = new List<FluxosDeCadastros>();
            var listaOperacoes = new List<Operacoes>();
            var operacoes = new Operacoes();
            var operacoesGestora = new Operacoes();

            var page = await browser.NewPageAsync();
            Pagina pagina;

            try
            {
                listaPagina.Add(await TesteOperacoesOperacoes.Page.LoginGeral.Login(page, usuario));

                switch (usuario.Nivel)
                {
                    case TesteOperacoesOperacoes.Model.Usuario.NivelEnum.Master:
                        //esperar bloker
                        //(pagina, var fluxo) = await OperacoesAtivos.Ativos(page, usuario.Nivel);
                        //listaPagina.Add(pagina); listaFluxos.Add(fluxo);

                        //(pagina, operacoes) = await TesteOperacoesOperacoes.Pages.OperacoesPage.OperacoesCustodiaZitec.OperacoesZitecInterno(page, usuario.Nivel, operacoes);
                        //listaPagina.Add(pagina); listaOperacoes.Add(operacoes);

                        //operacoes = new Operacoes();
                        (pagina, operacoes) = await TesteOperacoesOperacoes.Pages.OperacoesPage.CadastroOperacoesZitecCsv.OperacoesZitecCsv(page, usuario.Nivel, operacoesGestora);
                        listaPagina.Add(pagina); listaOperacoes.Add(operacoesGestora);

                        operacoes = new Operacoes();
                        listaPagina.Add(pagina); listaOperacoes.Add(operacoes);
                        break;

                    case Usuario.NivelEnum.Consultoria:
                        (pagina, operacoes) = await TesteOperacoesOperacoes.Pages.OperacoesPage.OperacoesCustodiaZitec.OperacoesZitecConsultoria(page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina); listaOperacoes.Add(operacoes);

                        break;

                    case Usuario.NivelEnum.Gestora:
                        (pagina, operacoes) = await TesteOperacoesOperacoes.Pages.OperacoesPage.OperacoesCustodiaZitec.OperacoesZiteGestora(page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina); listaOperacoes.Add(operacoes);
                        operacoes = new Operacoes();
                        (pagina, operacoes) = await TesteOperacoesOperacoes.Pages.OperacoesPage.CadastroOperacoesZitecCsv.OperacoesZitecCsv(page, usuario.Nivel, operacoesGestora);
                        listaPagina.Add(pagina);
                        break;
                }

                foreach (var pg in listaPagina)
                {
                    if (pg.Perfil == null)
                        pg.Perfil = usuario.Nivel.ToString();
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

            return (listaPagina, listaFluxos, listaOperacoes);
        }
    }
}





