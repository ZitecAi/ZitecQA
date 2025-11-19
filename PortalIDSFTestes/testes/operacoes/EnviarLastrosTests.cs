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
    [AllureSuite("EnviarLastrosTests UI")]
    [AllureOwner("Levi")]
    public class EnviarLastrosTests : TestBase
    {

        Utils metodo;
        EnviarLastrosElements el = new EnviarLastrosElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaEnviarLastros, "Clicar em Enviar Lastros para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Enviar Lastros")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var enviarLastros = new EnviarLastrosPage(page);
            await enviarLastros.ValidarAcentosEnviarLastrosPage();
        }
    }
}
