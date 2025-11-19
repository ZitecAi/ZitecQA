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
    [Category("Suíte: Devolução/Reembolso")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("DevolucaoReembolsoTests UI")]
    [AllureOwner("Levi")]
    public class DevolucaoReembolsoTests : TestBase
    {

        Utils metodo;
        DevolucaoReembolsoElements el = new DevolucaoReembolsoElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBancoId, "Clicar na sessão Banco ID no menú hamburguer");
            await metodo.Clicar(el.PaginaDevolucaoReembolsos, "Clicar na página Usuarios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Devolucao Reembolso")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var devolucaoReembolso = new DevolucaoReembolsoPage(page);
            await devolucaoReembolso.ValidarAcentosDevolucaoReembolso();
        }
    }
}
