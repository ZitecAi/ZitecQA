using Microsoft.Playwright;
using PortalIDSFTestes.elementos.controleInterno;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.controleInterno;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.controleInterno
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Politicas")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("PoliticasTests UI")]
    [AllureOwner("Levi")]
    public class PoliticasTests : TestBase
    {
        private IPage page;
        Utils metodo;
        PoliticasElements el = new PoliticasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuControleInterno, "Clicar em Controle interno menu hamburguer");
            await metodo.Clicar(el.PaginaPoliticas, "Clicar em Politicas para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Politicas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var politicas = new PoliticasPage(page);
            await politicas.ValidarAcentosPoliticasPage();}
        }
}
