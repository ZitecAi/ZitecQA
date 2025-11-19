using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.custodia;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.custodia;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.custodia
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte:  Painel de Cotas")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("PainelCotasTests UI")]
    [AllureOwner("Levi")]
    public class PainelCotasTests : TestBase
    {
        Utils metodo;
        PainelCotasElement el = new PainelCotasElement();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCedentes, "Clicar na sessão custodia no menú hamburguer");
            await metodo.Clicar(el.PaginaPainelCotas, "Clicar na página Painel de Cotas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Painel Cotas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var painelCotas = new PainelCotasPage(page);
            await painelCotas.ValidarAcentosPainelCotasPage();
        }
    }
}
