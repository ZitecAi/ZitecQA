using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestePortalDenver;
using TestePortalDenver.Model;
using TestePortalDenver.Utils;
using TestePortalDenver.Pages;
using System.IO;
//using System.Windows.Controls;
using System.Net.Http.Headers;
using System.Drawing;
using System.Linq;

namespace TestePortalDenver
{
    class Program
    {
        public static IPage Page { get; set; }
        public static List<Usuario> Usuarios { get; set; }

        public static Operacoes operacoes = new Operacoes();

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
                    listaPagina.Add(await TestePortalDenver.Pages.LoginGeral.Login(Page, usuario));

                    
                    if (usuario.Nivel == Usuario.NivelEnum.Denver)
                    {
                        listaPagina.Add(await BancoIdReembolso.Reembolso(Page, usuario.Nivel));
                        listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                        listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                        await Task.Delay(600);
                        listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                        await Task.Delay(600);
                        listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                        listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                        await Task.Delay(500);
                        listaPagina.Add(await OperacoesCustodiaZitec.OperacoesZitec(Page, usuario.Nivel));
                        listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                        listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                        listaPagina.Add(await RelatorioFundos.Fundos(Page));
                        listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                        listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
                        listaPagina.Add(await ControleInternoDiario.Diario(Page));
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
                    "todos@zitec.ai",
                    "Relatório das páginas do portal em produção teste.",
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

