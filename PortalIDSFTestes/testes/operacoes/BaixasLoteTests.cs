using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.operacoes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Baixas em Lote")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("BaixasLoteTest UI")]
    [AllureOwner("Levi")]
    public class BaixasLoteTest : Executa
    {
        private IPage page;
        Metodos metodo;
        OperacoesElements el = new OperacoesElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaOperacoes, "Clicar em Operações para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Baixas em Lote")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.ValidarAcentosOperacoesPage();
        }
    }
}
