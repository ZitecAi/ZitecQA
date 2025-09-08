using Microsoft.Playwright;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.administrativo;
using PortalIDSFTestes.pages.bancoId;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.bancoId
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Correntistas")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class CorrentistasTests : Executa
    {

        private IPage page;
        Metodos metodo;
        CorrentistasElements el = new CorrentistasElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuAdministrativo, "Clicar na sessão Admninistrativo no menú hamburguer");
            await metodo.Clicar(el.PaginaCorrentistas, "Clicar na página Usuarios");
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
            var correntistas = new CorrentistasPage(page);
            await correntistas.validarAcentosCorrentistas();
        }

    }
}
