using Allure.NUnit;
using Allure.NUnit.Attributes;
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
    }
}
