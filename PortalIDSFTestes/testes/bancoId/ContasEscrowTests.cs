using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.bancoId;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.bancoId
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Contas Escrow")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ContasEscrowTests UI")]
    [AllureOwner("Levi")]
    public class ContasEscrowTests : Executa
    {

        private IPage page;
        Metodos metodo;
        ContasEscrowElements el = new ContasEscrowElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBancoId, "Clicar na sessão Banco Id no menú hamburguer");
            await metodo.Clicar(el.PaginaContasEscrow, "Clicar na página ContasEscrow");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Contas Escrow")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var contasEscrow = new ContasEscrowPage(page);
            await contasEscrow.ValidarAcentosContasEscrow();
        }
    }

}