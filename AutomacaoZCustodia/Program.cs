using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Drawing;
using System.Configuration;
using AutomacaoZCustodia.Models;
using AutomacaoZCustodia.Utils;


namespace AutomacaoZCustodia
{
    class Program

    {
        public static IPage Page { get; set; }

        public static async Task Main(string[] args)
        {
            Pagina pagina = new Pagina();
            Operacoes operacoes = new Operacoes();

            var playwright = await Playwright.CreateAsync();

            int screenWidth = 1120; // Largura da janela (pixels)
            int screenHeight = 1000; // Altura da janela (pixels)

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

            var listaPagina = new List<Pagina>();
            var listaOperacoes = new List<Operacoes>();


            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                AcceptDownloads = true,
                IgnoreHTTPSErrors = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"

            });

            try
            {
                listaPagina.Add(await AutomacaoZCustodia.Pages.LoginZCustodia.Login(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.Operacoes.OperacoesCnab(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.AdministrativoGrupos.CadastroGrupos(Page));//em ajuste
                listaPagina.Add(await AutomacaoZCustodia.Pages.AdministrativoFechamentoFundo.FechamentoFundo(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.AdministrativoAlterarDtFundo.AlterarDtFundo(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroCedentes.Cedentes(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.AdministrativoBancos.CadastroBancos(Page)); //não cadastra
                listaPagina.Add(await AutomacaoZCustodia.Pages.AdministrativoRamoAtividade.RamoAtividade(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroUsuarios.CadastroUsuario(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroTipoRecebível.CadastroNvRecebivel(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroTipoMovimento.CadastroDeTipoMovimento(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroLayout.CadastroDeLayout(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroLayoutMov.CadastroDeLayoutMov(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroSacado.CadastroDeSacado(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroCertificadora.CadastroDeCertificadora(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroEntidade.CadastroDeEntidade(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.CadastroFundos.CadastroDeFundos(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.ImportacaoArquivoRemessa.ImpArquivoRemessa(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.RetornoCobranca.RetCobranca(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.EnquadramentoPdd.EnquadrPdd(Page));
                listaPagina.Add(await AutomacaoZCustodia.Pages.PerfilEnquadramento.PerfilEnquadr(Page));
                listaOperacoes.Add(await FluxoOperacoesApi.OperacoesApi.EnviarArquivoAsync(operacoes));
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);

            }
            finally
            {
                await browser.CloseAsync();
            }
            #region EnviarEmail

            Console.WriteLine("Status Code salvos em Paginas.txt");

            try
            {
                EmailPadrao emailPadrao = new EmailPadrao(
                    "todos@zitec.ai",
                    "Relatório das páginas do Zitec Custódia.",
                    EnviarEmail.GerarHtml(listaPagina,listaOperacoes),
                    "C:\\Temp\\Paginas.txt"
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