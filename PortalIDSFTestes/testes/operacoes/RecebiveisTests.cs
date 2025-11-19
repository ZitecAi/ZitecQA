using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.operacoes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Enviar Lastros")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("RecebiveisTests UI")]
    [AllureOwner("Levi")]
    public class RecebiveisTests : TestBase
    {

        Utils metodo;
        RecebiveisElements el = new RecebiveisElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaRecebiveis, "Clicar em Enviar Recebiveis para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Recebiveis")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var recebiveis = new RecebiveisPage(page);
            await recebiveis.ValidarAcentosRecebiveisPage();
        }
    }
}
