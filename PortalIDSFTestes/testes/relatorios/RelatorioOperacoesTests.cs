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
    [Category("Suíte: Relatorio Operacoes")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("RelatorioOperacoesTests UI")]
    [AllureOwner("Levi")]
    public class RelatorioOperacoesTests : TestBase
    {

        Utils metodo;
        RelatorioOperacoesElements el = new RelatorioOperacoesElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuRelatorios, "Clicar em Relatorios menu hamburguer");
            await metodo.Clicar(el.PaginaRelatorioOperacoes, "Clicar em Cedentes para acessar a página Relatorio Operacoes na sessão relatorios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Relatorio Operacoes")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var relatorioOperacoes = new RelatorioOperacoesPage(page);
            await relatorioOperacoes.ValidarAcentosRelatorioOperacoesPage();
        }
    }
}
