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
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.dashboards
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte:  Dashboards Operações")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("DashOperacoesTests UI")]
    [AllureOwner("Levi")]
    public class DashOperacoesTests : TestBase
    {
        private IPage page;
        Utils metodo;
        DashOperacoesElements el = new DashOperacoesElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuDashboards, "Clicar na sessão Dashboards no menú hamburguer");
            await metodo.Clicar(el.PaginaDashOperacoes, "Clicar na página Dashboards cedentes");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Dash Operacoes")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var dashOperacoes = new DashOperacoesPage(page);
            await dashOperacoes.ValidarAcentosDashOperacoesPage();}
        }
}
