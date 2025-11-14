using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.administrativo;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.administrativo
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Enviar Mensagem")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("EnviarMensagemTests UI")]
    [AllureOwner("Levi")]
    public class EnviarMensagemTests : Executa
    {
        private IPage page;
        Metodos metodo;
        EnviarMensagemElements el = new EnviarMensagemElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuAdministrativo, "Clicar na sessão Admninistrativo no menú hamburguer");
            await metodo.Clicar(el.PaginaEnviarMensagem, "Clicar na página Enviar Mensagem");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Enviar Mensagem")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var enviarMensagem = new EnviarMensagemPage(page);
            await enviarMensagem.ValidarAcentosEnviarMensagemPage();
        }
    }
}
