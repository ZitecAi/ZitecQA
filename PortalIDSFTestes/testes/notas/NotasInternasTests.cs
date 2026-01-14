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
    [Category("Suíte:  Notas Internas")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("NotasInternasTests UI")]
    [AllureOwner("Levi")]
    public class NotasInternasTests : TestBase
    {
        Utils metodo;
        NotasInternasElements el = new NotasInternasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuNotas, "Clicar na sessão Notas no menú hamburguer");
            await metodo.Clicar(el.PaginaNotasInternas, "Clicar na página Pagamentos notas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Notas Internas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var notasInternas = new NotasInternasPage(page);
            await notasInternas.ValidarAcentosPagamentosNotasInternasPage();
        }



    }
}
