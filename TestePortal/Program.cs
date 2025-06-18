using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestePortal;
using TestePortal.Model;
using TestePortal.Utils;
using TestePortal.Pages;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows.Controls;
using System.Net.Http.Headers;
using System.Drawing;
using System.Linq;
using TestePortal.Pages.AdministrativoPage;
using TestePortal.Pages.BancoIdPage;
using TestePortal.Pages.BoletagemPage;
using TestePortal.Pages.CadastroPage;
using TestePortal.Pages.CedentesPage;
using TestePortal.Pages.ControleInternoPage;
using TestePortal.Pages.LoginPage;
using TestePortal.Pages.RelatoriosPage;
using TestePortal.Pages.NotaComercialPage;
using TestePortal.Pages.NotasPage;
using TestePortal.Pages.OperacoesPage;

namespace TestePortalIDSF
{
    class Program
    {
        public static IPage Page { get; set; }
        public static List<Usuario> Usuarios { get; set; }

        public static Operacoes operacoes = new Operacoes();
        public static IConfigurationRoot Config {  get; set; }

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

            var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Config = builder.Build();


            #region VerificaçãoDeStatusDasPáginas

            try

            {
                foreach (var usuario in Usuarios)

                    
                {
                    listaPagina.Add(await TestePortal.Pages.LoginPage.LoginGeral.Login(Page, usuario));

                    if (usuario.Nivel == Usuario.NivelEnum.Master)
                    {
                        listaPagina.Add(await AdministrativoGrupos.Grupos(Page));
                        listaPagina.Add(await AdministrativoUsuarios.Usuarios(Page));
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await BancoIdCorrentista.CorrentistaMov(Page, context, usuario.Nivel);
                        listaPagina.Add(pagina);
                        listaFluxos.Add(fluxoDeCadastros);
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await BancoIdCorrentista.CorrentistaSelic(Page, context, usuario.Nivel);
                        listaFluxos.Add(fluxoDeCadastros);
                        listaPagina.Add(await CadastroCarteira.Carteira(Page));
                        listaPagina.Add(await BancoIdReembolso.Reembolso(Page, usuario.Nivel));
                        (pagina, fluxoDeCadastros) = await BancoIdCorrentista.CorrentistaCobranca(Page, context, usuario.Nivel);
                        listaFluxos.Add(fluxoDeCadastros);
                        listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                        listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                        await Task.Delay(500);
                        listaPagina.Add(await BancoIdZeragem.Zeragem(Page));
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await CadastroInvestidores.InvestidoresPf(Page, context, usuario.Nivel);
                        listaPagina.Add(pagina);
                        listaFluxos.Add(fluxoDeCadastros);
                        await Task.Delay(500);
                        listaPagina.Add(await CadastroConsultoras.Consultoras(Page));
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await CadastroConsultorias.Consultorias(Page, context, usuario.Nivel);
                        listaPagina.Add(pagina);
                        listaFluxos.Add(fluxoDeCadastros);
                        await Task.Delay(500);
                        listaPagina.Add(await CadastroCotistas.Cotista(Page));
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await CadastroInvestidores.InvestidoresPj(Page, context, usuario.Nivel);
                        listaPagina.Add(pagina);
                        listaFluxos.Add(fluxoDeCadastros);
                        await Task.Delay(500);
                        listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel));
                        await Task.Delay(500);
                        listaPagina.Add(await CadastroFundosTransferencia.FundosTransf(Page, usuario.Nivel));
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await CadastroInvestidores.InvestidoresFundoDeInvestimento(Page, context, usuario.Nivel);
                        listaFluxos.Add(fluxoDeCadastros);
                        listaPagina.Add(await CadastroGestoras.Gestoras(Page));
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await CadastroGestorasInternas.GestorasInternas(Page, context, usuario.Nivel);
                        listaPagina.Add(pagina);
                        listaFluxos.Add(fluxoDeCadastros);
                        await Task.Delay(500);
                        listaPagina.Add(await CadastroPrestServico.PrestServico(Page));
                        listaPagina.Add(await CadastroOfertas.Ofertas(Page));
                        listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                        listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                        listaPagina.Add(await BoletagemAmortizacao.Amortizacao(Page));
                        listaPagina.Add(await CedentesCedentes.CedentesPJ(Page));
                        listaPagina.Add(await CedentesCedentes.CedentesPf(Page));
                        listaPagina.Add(await CedentesKitCedente.KitCedentes(Page));
                        listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                        await Task.Delay(500);
                        listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        listaPagina.Add(await OperacoesArquivosBaixa.ArquivosBaixa(Page));
                        (pagina, fluxoDeCadastros) = await OperacoesAtivos.Ativos(Page, usuario.Nivel);
                        listaFluxos.Add(fluxoDeCadastros);
                        listaPagina.Add(await BoletagemControleCapital.ControleCapital(Page));
                        listaPagina.Add(await OperacoesBaixaEmLote.BaixaLote(Page));
                        listaPagina.Add(await OperacoesEnviarLastros.EnviarLastros(Page));
                        await Task.Delay(500);
                        (pagina, fluxoDeCadastros) = await BancoIdCorrentista.CorrentistaCetip(Page, context, usuario.Nivel);
                        listaFluxos.Add(fluxoDeCadastros);
                        listaPagina.Add(await BancoIdContasEscrow.ContasEscrow(Page, usuario.Nivel));
                        await Task.Delay(600);
                        (pagina, fluxoDeCadastros) = await BancoIdCorrentista.CorrentistaEscrow(Page, context, usuario.Nivel);
                        listaFluxos.Add(fluxoDeCadastros);
                        (pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZitec(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        (pagina, operacoes) = await CadastroOperacoesZitecCsv.OperacoesZitecCsv(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        listaOperacoes.Add(operacoes);
                        listaPagina.Add(await OperacoesRecebiveis.Recebiveis(Page));
                        listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                        await Task.Delay(500);
                        (pagina, fluxoDeConciliacao) = await OperacoesConciliacaoExtrato.ConciliacaoExtrato(Page, usuario.Nivel);
                        listaPagina.Add(pagina);
                        listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                        listaPagina.Add(await RelatorioCedentes.Cedentes(Page));
                        listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                        listaPagina.Add(await RelatorioFundos.Fundos(Page));
                        listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                        listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
                        listaPagina.Add(await ControleInternoPoliticas.Politicas(Page));
                        listaPagina.Add(await ControleInternoDiario.Diario(Page));
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

                        listaPagina.Add(await BancoIdContasEscrow.ContasEscrow(Page, usuario.Nivel));
                        listaPagina.Add(await BancoIdReembolso.Reembolso(Page, usuario.Nivel));
                        listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                        listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                        listaPagina.Add(await BancoIdZeragem.Zeragem(Page));
                        listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel));
                        listaPagina.Add(await CadastroPrestServico.PrestServico(Page));
                        listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                        listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                        listaPagina.Add(await CedentesCedentes.CedentesPJ(Page));
                        listaPagina.Add(await CedentesCedentes.CedentesPf(Page));
                        listaPagina.Add(await CedentesKitCedente.KitCedentes(Page));
                        listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                        listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        listaPagina.Add(await OperacoesArquivosBaixa.ArquivosBaixa(Page));
                        listaPagina.Add(await OperacoesEnviarLastros.EnviarLastros(Page));
                        (pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZitec(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        listaPagina.Add(await OperacoesRecebiveis.Recebiveis(Page));
                        listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                        listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                        listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                        listaPagina.Add(await RelatorioFundos.Fundos(Page));
                        listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                        listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
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
                        listaPagina.Add(await BancoIdContasEscrow.ContasEscrow(Page, usuario.Nivel));
                        await Task.Delay(500);
                        listaPagina.Add(await BancoIdReembolso.Reembolso(Page, usuario.Nivel));
                        listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                        listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                        listaPagina.Add(await BancoIdZeragem.Zeragem(Page));
                        listaPagina.Add(await CadastroCarteira.Carteira(Page));
                        listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel));
                        listaPagina.Add(await CadastroFundosTransferencia.FundosTransf(Page, usuario.Nivel));
                        await Task.Delay(500);
                        listaPagina.Add(await CadastroPrestServico.PrestServico(Page));
                        listaPagina.Add(await CadastroOfertas.Ofertas(Page));
                        listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                        listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                        listaPagina.Add(await CedentesCedentes.CedentesPJ(Page));
                        listaPagina.Add(await CedentesCedentes.CedentesPf(Page));
                        listaPagina.Add(await CedentesKitCedente.KitCedentes(Page));
                        listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                        await Task.Delay(500);
                        listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        listaPagina.Add(await OperacoesArquivosBaixa.ArquivosBaixa(Page));
                        listaPagina.Add(await OperacoesEnviarLastros.EnviarLastros(Page));
                        (pagina, operacoes) = await OperacoesCustodiaZitec.OperacoesZitec(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        (pagina, operacoes) = await CadastroOperacoesZitecCsv.OperacoesZitecCsv(Page, usuario.Nivel, operacoes);
                        listaPagina.Add(pagina);
                        listaPagina.Add(await OperacoesRecebiveis.Recebiveis(Page));
                        listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                        listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                        listaPagina.Add(await RelatorioCedentes.Cedentes(Page));
                        listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                        await Task.Delay(500);
                        listaPagina.Add(await RelatorioFundos.Fundos(Page));
                        listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                        listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
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
                        listaPagina.Add(await AdministrativoGrupos.Grupos(Page));
                        listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                        listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                        listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel));
                        await Task.Delay(600);
                        listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                        await Task.Delay(600);
                        listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                        listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                        await Task.Delay(500);
                        listaPagina.Add(await NotaComercial.NotasComerciais(Page, usuario.Nivel));
                        listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                        listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                        listaPagina.Add(await RelatorioCedentes.Cedentes(Page));
                        listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                        listaPagina.Add(await RelatorioFundos.Fundos(Page));
                        listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                        listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
                        listaPagina.Add(await ControleInternoPoliticas.Politicas(Page));
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

