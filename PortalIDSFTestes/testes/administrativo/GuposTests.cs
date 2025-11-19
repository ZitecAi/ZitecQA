using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.administrativo;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.administrativo
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Grupos")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("GuposTests UI")]
    [AllureOwner("Levi")]
    public class GuposTests : TestBase
    {
        Utils metodo;
        GruposElements el = new GruposElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuAdministrativo, "Clicar na sessão Admninistrativo no menú hamburguer");
            await metodo.Clicar(el.PaginaGrupos, "Clicar na página Enviar Mensagem");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Gupos")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var enviarMensagem = new GruposPage(page);
            await enviarMensagem.ValidarAcentosEnviarMensagemPage();
        }
    }
}
