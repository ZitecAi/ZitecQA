using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.dashboards;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.dashboards;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.dashboards
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte:  Dashboards Cedentes")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("DashCedentesTests UI")]
    [AllureOwner("Levi")]
    public class DashCedentesTests : TestBase
    {
        Utils metodo;
        DashCedentesElements el = new DashCedentesElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuDashboards, "Clicar na sessão Dashboards no menú hamburguer");
            await metodo.Clicar(el.PaginaDashCedentes, "Clicar na página Dashboards cedentes");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Dash Cedentes")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var dashCedentes = new DashCedentesPage(page);
            await dashCedentes.ValidarAcentosDashCedentesPage();
        }
    }
}
