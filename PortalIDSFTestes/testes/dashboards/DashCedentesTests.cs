using Microsoft.Playwright;
using PortalIDSFTestes.elementos.custodia;
using PortalIDSFTestes.elementos.dashboards;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.custodia;
using PortalIDSFTestes.pages.dashboards;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.dashboards
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte:  Dashboards Cedentes")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class DashCedentesTests : Executa
    {
        private IPage page;
        Metodos metodo;
        DashCedentesElements el = new DashCedentesElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuDashboards, "Clicar na sessão Dashboards no menú hamburguer");
            await metodo.Clicar(el.PaginaDashCedentes, "Clicar na página Dashboards cedentes");
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
            var dashCedentes = new DashCedentesPage(page);
            await dashCedentes.ValidarAcentosDashCedentesPage();
        }
    }
}
