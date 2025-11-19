using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.Boletagem;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.boletagem;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.boletagem
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Conta Ordem")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ContaOrdemTests UI")]
    [AllureOwner("Levi")]
    public class ContaOrdemTests : TestBase
    {
        Utils metodo;
        ContaOrdemElements el = new ContaOrdemElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBoletagem, "Clicar na sessão Boletagem no menú hamburguer");
            await metodo.Clicar(el.PaginaContaOrdem, "Clicar na página Conta Ordem");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Conta Ordem")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var contaOrdem = new ContaOrdemPage(page);
            await contaOrdem.ValidarAcentosContaOrdem();
        }
    }
}
