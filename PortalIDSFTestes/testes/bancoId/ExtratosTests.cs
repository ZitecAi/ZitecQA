using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.bancoId;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.bancoId
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Extratos")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ExtratosTests UI")]
    [AllureOwner("Levi")]
    public class ExtratosTests : TestBase
    {

        Utils metodo;
        ExtratosElements el = new ExtratosElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBancoId, "Clicar na sessão Banco ID no menú hamburguer");
            await metodo.Clicar(el.PaginaExtratos, "Clicar na página Usuarios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Extratos")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var extratos = new ExtratosPage(page);
            await extratos.ValidarAcentosExtratos();
        }
        [Test, Order(2)]
        [AllureName("Deve Gerar Extrato Pdf Com Sucesso")]
        public async Task Deve_Gerar_Extrato_Pdf_Com_Sucesso()
        {
            var extratos = new ExtratosPage(page);
            await extratos.GerarExtratoPdf();
        }
    }
}
