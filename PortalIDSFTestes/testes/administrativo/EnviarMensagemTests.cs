using Microsoft.Playwright;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.administrativo;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.administrativo
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Enviar Mensagem")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class EnviarMensagemTests : Executa
    {
        private IPage page;
        Metodos metodo;
        EnviarMensagemElements el = new EnviarMensagemElements();

        [SetUp]
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
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var enviarMensagem = new EnviarMensagemPage(page);
            await enviarMensagem.validarAcentosEnviarMensagemPage();
        }



    }
}
