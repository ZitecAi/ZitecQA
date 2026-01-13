using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.cadastro;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.cadastro
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Investidores")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("InvestidoresTests UI")]
    [AllureOwner("Levi")]
    public class InvestidoresTests : TestBase
    {
        Utils metodo;
        InvestidoresElements el = new InvestidoresElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaInvestidores, "Clicar na página Investidores");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Investidores")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var investidores = new InvestidoresPage(page);
            await investidores.ValidarAcentosInvestidores();
        }
        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Investidores")]
        public async Task Deve_Registrar_Cotista()
        {
            var investidores = new InvestidoresPage(page);

            await investidores.PesquisarCotista();
            await investidores.ClicarLinkFormulario();
            await investidores.ValidarMensagemLinkCopiado();
            IPage novaPagina = await investidores.AbrirFormularioNovaGuia();
            var investidoreForm = new InvestidoresPage(novaPagina);
            await investidoreForm.PreencherDadosDoInvestidor(novaPagina);
            await investidoreForm.AvancarEtapaFormulario("Endereço");
            await investidoreForm.PreencherEnderecoDoCotista();
            await investidoreForm.AvancarEtapaFormulario("Situação Patrimonial");
            await investidoreForm.PreencherSituacaoPatrimonial();
            await investidoreForm.AvancarEtapaFormulario("Representantes");
            await investidoreForm.AvancarEtapaFormulario("Conta Bancaria");
            await investidoreForm.AvancarEtapaFormulario("Suitability");
            await investidoreForm.PreencherSuitability();
            await investidoreForm.AvancarEtapaFormulario("Suitability Finalizado");
            await investidoreForm.AvancarEtapaFormulario("Termos e Condições");
            await investidoreForm.AceitarTermos();
            await investidoreForm.AvancarEtapaFormulario("Enviar Formulários");
            await investidoreForm.EnviarFormulario();



        }
    }
}
