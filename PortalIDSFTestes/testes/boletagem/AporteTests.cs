using Microsoft.Playwright;
using PortalIDSFTestes.elementos.Boletagem;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.boletagem;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.boletagem
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Aporte")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    public class AporteTests : Executa
    {
        private IPage page;
        Metodos metodo;
        AporteElements el = new AporteElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaAporte, "Clicar na página Aporte");
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
            var aporte = new AportePage(page);
            await aporte.ValidarAcentosAporte();
        }

    }
}
