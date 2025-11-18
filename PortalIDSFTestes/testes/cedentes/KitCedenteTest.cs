using Microsoft.Playwright;
using PortalIDSFTestes.pages.cedentes;
using PortalIDSFTestes.runner;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalIDSFTestes.elementos.cedentes;
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.cedentes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Kit Cedentes")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("KitCedenteTest UI")]
    [AllureOwner("Levi")]
    public class KitCedenteTest : TestBase
    {
        private IPage page;
        Utils metodo;
        KitCedenteElements el = new KitCedenteElements();  

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCedentes, "Clicar na sessão cedentes no menú hamburguer");
            await metodo.Clicar(el.PaginaKitCedente, "Clicar na página Kit cedente");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Kit Cedente")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var kitCedente = new KitCedentePage(page);
            await kitCedente.ValidarAcentosKitCedentePage();}
        }
}
