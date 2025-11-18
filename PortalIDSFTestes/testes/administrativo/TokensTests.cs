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
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.administrativo
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Tokens")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("TokensTests UI")]
    [AllureOwner("Levi")]
    public class TokensTests : TestBase
    {
        private IPage page;
        Utils metodo;
        TokensElements el = new TokensElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuAdministrativo, "Clicar na sessão Admninistrativo no menú hamburguer");
            await metodo.Clicar(el.PaginaTokens, "Clicar na página Enviar Mensagem");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Tokens")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var tokens = new TokensPage(page);
            await tokens.ValidarAcentosTokens();}
        }
}
