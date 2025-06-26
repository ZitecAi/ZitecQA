using Microsoft.Playwright;
using TestePortalExecutavel.Model;
using TestePortalExecutavel.Pages.CedentesPage;
using TestePortalExecutavel.Pages.NotaComercialPage;
using TestePortalExecutavel.Pages.OperacoesPage;
using TestePortalExecutavel.Utils;
using Microsoft.Extensions.Configuration;
using TestePortalExecutavel.Pages;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace TestePortalExecutavel
{
    class Program
    {
        public static IPage Page { get; set; }
        public static List<Usuario> Usuarios { get; set; }

        public static Operacoes operacoes = new Operacoes();
        public static IConfigurationRoot Config { get; set; }

        public static async Task Main(string[] args)
        {
            Pagina pagina = new Pagina();

            var playwright = await Playwright.CreateAsync();

            int screenWidth = 2120; // Largura da janela (pixels)
            int screenHeight = 1120; // Altura da janela (pixels)

            IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Channel = "chrome",
                Headless = false,
                SlowMo = 50,
                Timeout = 0,
                Args = new List<string>
            {
               $"--window-size={screenWidth},{screenHeight}",
               "--disable-web-security",
               "--disable-site-isolation-trials",
               "--disable-features=IsolateOrigins,site-per-process",
               "--start-maximized"
            }
            });

            Page = await browser.NewPageAsync();

            var builder = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Config = builder.Build();


            //var context = await browser.NewContextAsync();
            var listaPagina = new List<Pagina>();
            var listaFluxos = new List<FluxosDeCadastros>();

            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                AcceptDownloads = true,
                IgnoreHTTPSErrors = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"

            });

            Usuarios = Util.GetUsuariosForTest();

            var fluxoDeCadastros = new FluxosDeCadastros();
            var listaOperacoes = new List<Operacoes>();
            var conciliacao = new Conciliacao();
            var fluxoDeConciliacao = new Conciliacao();



            #region VerificaçãoDeStatusDasPáginas

            try

            {
                foreach (var usuario in Usuarios)


                {
                    listaPagina.Add(await LoginGeral.Login(Page, usuario));

                    if (usuario.Nivel == Usuario.NivelEnum.Master)
                    {

                        //listaPagina.Add(await CedentesCedentes.CedentesPJ(Page));
                        //listaPagina.Add(await CedentesCedentes.CedentesPf(Page));
                        //listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        //(pagina, fluxoDeCadastros) = await OperacoesAtivos.Ativos(Page, usuario.Nivel);
                        //listaFluxos.Add(fluxoDeCadastros);
                        //(pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZitecInterno (Page, usuario.Nivel, operacoes);
                        //listaPagina.Add(pagina);
                        //listaOperacoes.Add(operacoes);
                        //(pagina, operacoes) = await CadastroOperacoesZitecCsv.OperacoesZitecCsv(Page, usuario.Nivel, operacoes);
                        //listaPagina.Add(pagina);
                        //listaOperacoes.Add(operacoes);
                        (pagina, operacoes) = await ArquivoBaixas.Baixas(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        listaFluxos.Add(fluxoDeCadastros);
                        await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();

                        foreach (var page in listaPagina)
                        {
                            if (page.Perfil == null)
                                page.Perfil = usuario.Nivel.ToString();
                        }

                    }
                    else if (usuario.Nivel == Usuario.NivelEnum.Consultoria)
                    {

                        //listaPagina.Add(await CedentesCedentes.CedentesPJ(Page));
                        //listaPagina.Add(await CedentesCedentes.CedentesPf(Page));
                        //listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        //(pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZitecConsultoria (Page, usuario.Nivel, operacoes);
                        //listaPagina.Add(pagina);
                        await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();

                        foreach (var page in listaPagina)
                        {
                            if (page.Perfil == null)
                                page.Perfil = usuario.Nivel.ToString();
                        }

                    }
                    else if (usuario.Nivel == Usuario.NivelEnum.Gestora)
                    {

                        //listaPagina.Add(await CedentesCedentes.CedentesPJ(Page));
                        //listaPagina.Add(await CedentesCedentes.CedentesPf(Page));
                        //listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        (pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZiteGestora(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        (pagina, operacoes) = await CadastroOperacoesZitecCsv.OperacoesZitecCsv(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        await Task.Delay(500);
                        await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();


                        foreach (var page in listaPagina)
                        {
                            if (page.Perfil == null)
                                page.Perfil = usuario.Nivel.ToString();

                        }

                    }
                    else if (usuario.Nivel == Usuario.NivelEnum.Denver)
                    {
                        await Task.Delay(600);
                        listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();

                        foreach (var page in listaPagina)
                        {
                            if (page.Perfil == null)
                                page.Perfil = usuario.Nivel.ToString();
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro {ex}");
            }

            #endregion


            #region EnviarEmail - Txt

            //using (StreamWriter writer = new StreamWriter("C:\\Temp\\Paginas.txt"))
            //{
            //    foreach (var item in listaPagina)
            //    {
            //        writer.WriteLine(item.ToFormattedString());
            //        writer.WriteLine(new string('-', 40));
            //    }
            //}

            #endregion

            #region EnviarEmail

            Console.WriteLine("Status Code salvos em Paginas.txt");

            try
            {
                EmailPadrao emailPadrao = new EmailPadrao(
                    "jt@zitec.ai",
                    "Relatório das páginas do portal em produção.",
                    EnviarEmail.GerarHtml(listaPagina, listaFluxos, listaOperacoes, conciliacao)
                  //EnviarEmail.GerarHtml(listaPagina, listaFluxos, listaOperacoes, conciliacao, operacoes),
                  //"C:\\Temp\\Paginas.txt"
                  );

                EnviarEmail.SendMailWithAttachment(emailPadrao);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Email enviado");

            #endregion
            //todos@zitec.ai -- todos da zitec 
        }

    }
}

