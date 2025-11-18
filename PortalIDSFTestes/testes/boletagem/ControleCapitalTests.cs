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
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.boletagem
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Controle Capital")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ControleCapitalTests UI")]
    [AllureOwner("Levi")]
    public class ControleCapitalTests : TestBase
    {
        private IPage page;
        Utils metodo;
        ControleCapitalElements el = new ControleCapitalElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBoletagem, "Clicar na sessão Boletagem no menú hamburguer");
            await metodo.Clicar(el.PaginaResgate, "Clicar na página Controle Capital");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Controle Capital")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var controleCapital = new ControleCapitalPage(page);
            await controleCapital.ValidarAcentosControleCapital();}
        }
}
