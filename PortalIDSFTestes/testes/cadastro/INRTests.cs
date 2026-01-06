using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.cadastro;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.cadastro
{
    public class INRTests
    {

        [Parallelizable(ParallelScope.Self)]
        [TestFixture]
        [Category("Suíte: Investidores não residentes")]
        [Category("Criticidade: Alta")]
        [Category("Regressivos")]
        [AllureNUnit]
        [AllureSuite("Investidores Nao Residentes Tests UI")]
        [AllureOwner("Levi")]
        public class CarteirasTests : TestBase
        {
            Utils metodo;
            INRElements el = new INRElements();

            [SetUp]
            [AllureBefore]
            public async Task Setup()
            {
                page = await AbrirBrowserAsync();
                var login = new LoginPage(page);
                metodo = new Utils(page);
                await login.LogarInterno();
                await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
                await metodo.Clicar(el.PaginaINR, "Clicar na página investidores não residentes");
                await Task.Delay(500);
            }

            [TearDown]
            [AllureAfter]
            public async Task TearDown()
            {
                await FecharBrowserAsync();
            }

            [Test, Order(1)]
            [AllureName("Nao Deve Conter Acentos Quebrados investidores não residentes")]
            public async Task Nao_Deve_Conter_Acentos_Quebrados()
            {
                var inr = new INRPage(page);
                await inr.ValidarAcentosINRPage();
            }
        }
    }
}
