using Microsoft.Playwright;
using PortalIDSFTestes.elementos.dashboards;
using PortalIDSFTestes.elementos.notas;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.dashboards;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.notas;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.notas
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte:  Pagamentos notas")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("PagamentosNotasTests UI")]
    [AllureOwner("Levi")]
    public class PagamentosNotasTests : Executa
    {
        private IPage page;
        Metodos metodo;
        PagamentosNotasElements el = new PagamentosNotasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuNotas, "Clicar na sessão Notas no menú hamburguer");
            await metodo.Clicar(el.PaginaPagamentosNotas, "Clicar na página Pagamentos notas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Pagamentos Notas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var pagamentosNotas = new PagamentosNotasPage(page);
            await pagamentosNotas.ValidarAcentosPagamentosNotasPage();}
        }
}
