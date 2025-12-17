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
    [Category("Suíte: Aporte")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("AporteTests UI")]
    [AllureOwner("Levi")]
    public class AporteTests : TestBase
    {
        Utils metodo;
        AporteElements el = new AporteElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBoletagem, "Clicar na sessão Boletagem no menú hamburguer");
            await metodo.Clicar(el.PaginaAporte, "Clicar na página Aporte");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Aporte")]
        [AllureTag("Regressivos")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var aporte = new AportePage(page);
            await aporte.ValidarAcentosAporte();
        }
        [Test, Order(2)]
        //[Ignore("Teste ignorado temporariamente para manutenção.")]
        [AllureTag("Regressivos")]
        [AllureName("Deve Realizar Aporte Com Sucesso")]
        public async Task Deve_Realizar_Aporte_Com_Sucesso()
        {
            var aporte = new AportePage(page);
            await aporte.RealizarAporte();
        }
    }
}
