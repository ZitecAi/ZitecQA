using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.notas;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.notas;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.notas
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte:  Pagamentos notas")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("PagamentosNotasTests UI")]
    [AllureOwner("Levi")]
    public class PagamentosNotasTests : TestBase
    {
        Utils metodo;
        PagamentosNotasElements el = new PagamentosNotasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuNotas, "Clicar na sessão Notas no menú hamburguer");
            await metodo.Clicar(el.PaginaPagamentosNotas, "Clicar na página Pagamentos notas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureTag("Regressivos")]
        [AllureName("Nao Deve Conter Acentos Quebrados Pagamentos Notas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var pagamentosNotas = new PagamentosNotasPage(page);
            await pagamentosNotas.ValidarAcentosPagamentosNotasPage();
        }
    }
}
