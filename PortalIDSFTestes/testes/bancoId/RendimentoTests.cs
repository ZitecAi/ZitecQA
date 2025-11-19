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
    [Category("Suíte: Rendimento")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("RendimentoTests UI")]
    [AllureOwner("Levi")]
    public class RendimentoTests : TestBase
    {

        Utils metodo;
        RendimentoElements el = new RendimentoElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBancoId, "Clicar na sessão Banco ID no menú hamburguer");
            await metodo.Clicar(el.PaginaRendimento, "Clicar na página Rendimento");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Rendimento")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var rendimento = new RendimentoPage(page);
            await rendimento.ValidarAcentosRendimento();
        }
    }
}
