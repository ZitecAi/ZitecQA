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
    [Category("Suíte: Grupos")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class GuposTests : Executa
    {
        private IPage page;
        Metodos metodo;
        GruposElements el = new GruposElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuAdministrativo, "Clicar na sessão Admninistrativo no menú hamburguer");
            await metodo.Clicar(el.PaginaGrupos, "Clicar na página Enviar Mensagem");
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
            var enviarMensagem = new GruposPage(page);
            await enviarMensagem.validarAcentosEnviarMensagemPage();
        }

    }
}
