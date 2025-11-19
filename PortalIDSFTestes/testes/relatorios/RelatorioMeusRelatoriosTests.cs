using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.relatorios;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.relatorios;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.relatorios
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Relatorio Meus Relatorios")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("RelatorioMeusRelatoriosTests UI")]
    [AllureOwner("Levi")]
    public class RelatorioMeusRelatoriosTests : TestBase
    {

        Utils metodo;
        RelatorioMeusRelatoriosElements el = new RelatorioMeusRelatoriosElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuRelatorios, "Clicar em Relatorios menu hamburguer");
            await metodo.Clicar(el.PaginaRelatorioMeusRelatorios, "Clicar em Cedentes para acessar a página Meus Relatorios na sessão relatorios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Relatorio Meus Relatorios")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var meusRelatorios = new RelatorioMeusRelatoriosPage(page);
            await meusRelatorios.ValidarAcentosRelatorioMeusRelatoriosPage();
        }
    }
}
