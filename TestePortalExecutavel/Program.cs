using Microsoft.Playwright;
using TestePortalExecutavel.Model;
using TestePortalExecutavel.Pages.CedentesPage;
using TestePortalExecutavel.Pages.NotaComercialPage;
using TestePortalExecutavel.Pages.OperacoesPage;
using TestePortalExecutavel.Utils;
using Microsoft.Extensions.Configuration;
using TestePortalExecutavel.Pages;
using System.IO;

namespace TestePortalExecutavel
{
    class Program
    {
        public static List<Usuario> Usuarios { get; set; }
        public static IConfigurationRoot Config { get; set; }

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

            Config = builder.Build();
            Usuarios = Util.GetUsuariosForTest();

            var tasks = Usuarios.Select(u => ExecutarTesteParaUsuario(browser, u)).ToList();
            var resultados = await Task.WhenAll(tasks);

            var listaPagina = resultados.SelectMany(r => r.Item1).ToList();
            var listaFluxos = resultados.SelectMany(r => r.Item2).ToList();
            var listaOperacoes = resultados.SelectMany(r => r.Item3).ToList();

            try
            {
                var emailPadrao = new EmailPadrao(
                    "jt@zitec.ai",
                    "Segue relatório com as páginas mais importantes do portal IDSF testadas.",
                    EnviarEmail.GerarHtml(listaPagina, listaFluxos, listaOperacoes)
                );
                EnviarEmail.SendMailWithAttachment(emailPadrao);
                Console.WriteLine("Email enviado");
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
                listaPagina.Add(await LoginGeral.Login(page, usuario));

                switch (usuario.Nivel)
                {
                    case Usuario.NivelEnum.Master:
                        //listaPagina.Add(await CedentesCedentes.CedentesPJ(page));
                        //listaPagina.Add(await CedentesCedentes.CedentesPf(page));
                        //listaPagina.Add(await NotaComercial.NotasComerciais(page, usuario.Nivel));

                        //esperar bloker
                        //(pagina, var fluxo) = await OperacoesAtivos.Ativos(page, usuario.Nivel);
                        //listaPagina.Add(pagina); listaFluxos.Add(fluxo);

                        (pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZitecInterno(page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina); listaOperacoes.Add(operacoes);

                        operacoes = new Operacoes();
                        (pagina, operacoes) = await CadastroOperacoesZitecCsv.OperacoesZitecCsv(page, usuario.Nivel, operacoesGestora);
                        listaPagina.Add(pagina); listaOperacoes.Add(operacoesGestora);

                        operacoes = new Operacoes();
                        (pagina, operacoes) = await ArquivoBaixas.Baixas(page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina); listaOperacoes.Add(operacoes);
                        break;

                    //case Usuario.NivelEnum.Consultoria:
                    //    listaPagina.Add(await NotaComercial.NotasComerciais(page, usuario.Nivel));
                    //    (pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZitecConsultoria(page, usuario.Nivel, operacoes);
                    //    listaPagina.Add(pagina); listaOperacoes.Add(operacoes);
                    //    listaPagina.Add(await CedentesCedentes.CedentesPJ(page));
                    //    listaPagina.Add(await CedentesCedentes.CedentesPf(page));
                    //    break;

                    //case Usuario.NivelEnum.Gestora:
                    //    (pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZiteGestora(page, usuario.Nivel, operacoes);
                    //    listaPagina.Add(pagina); listaOperacoes.Add(operacoes);
                    //    listaPagina.Add(await NotaComercial.NotasComerciais(page, usuario.Nivel));
                    //    listaPagina.Add(await CedentesCedentes.CedentesPJ(page));
                    //    listaPagina.Add(await CedentesCedentes.CedentesPf(page));
                    //    operacoes = new Operacoes();
                    //    (pagina, operacoes) = await CadastroOperacoesZitecCsv.OperacoesZitecCsv(page, usuario.Nivel, operacoesGestora);
                    //    listaPagina.Add(pagina);
                        //break;
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
