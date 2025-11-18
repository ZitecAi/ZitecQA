using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.risco;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.risco;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.risco
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Fundos Desenquadrados")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("FundosDesenquadradosTests UI")]
    [AllureOwner("Levi")]
    public class FundosDesenquadradosTests : TestBase
    {
        private IPage page;
        Utils metodo;
        FundosDesenquadradosElements el = new FundosDesenquadradosElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuRisco, "Clicar em Controle interno menu hamburguer");
            await metodo.Clicar(el.PaginaFundosDesenquadrados, "Clicar em Fundos Desenquadrados para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Fundos Desenquadrados")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var fundosDesenquadrados = new FundosDesenquadradosPage(page);
            await fundosDesenquadrados.ValidarAcentosFundosDesenquadradosPage();
        }
    }
}
