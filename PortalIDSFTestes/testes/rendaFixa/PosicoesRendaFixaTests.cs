using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.rendaFixa;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.rendaFixa;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.rendaFixa
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Posicoes Renda Fixa")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("PosicoesRendaFixaTests UI")]
    [AllureOwner("Levi")]
    public class PosicoesRendaFixaTests : TestBase
    {

        Utils metodo;
        PosicoesRendaFixaElements el = new PosicoesRendaFixaElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaPosicoesRendaFixa, "Clicar em Posições Renda Fixa para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Posicoes Renda Fixa")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var posicoesRendaFixa = new PosicoesRendaFixaPage(page);
            await posicoesRendaFixa.ValidarAcentosPosicoesRendaFixaPage();
        }
    }
}
