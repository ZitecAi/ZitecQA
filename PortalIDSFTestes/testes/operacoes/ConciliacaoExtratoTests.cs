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
    [Category("Suíte: Conciliacao e Extrato")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ConciliacaoExtratoTests UI")]
    [AllureOwner("Levi")]
    public class ConciliacaoExtratoTests : TestBase
    {
        Utils metodo;
        ConciliacaoExtratoElements el = new ConciliacaoExtratoElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaConciliacaoExtrato, "Clicar em Conciliacao e Extrato para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Conciliacao Extrato")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var conciliacaoExtrato = new ConciliacaoExtratoPage(page);
            await conciliacaoExtrato.ValidarAcentosConciliacaoExtratoPage();
        }
    }
}
