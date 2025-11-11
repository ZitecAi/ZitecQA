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
    [Category("Suíte: Amortizacao")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("AmortizacaoTests UI")]
    [AllureOwner("Levi")]
    public class AmortizacaoTests : Executa
    {
        private IPage page;
        Metodos metodo;
        AmortizacaoElements el = new AmortizacaoElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBoletagem, "Clicar na sessão Boletagem no menú hamburguer");
            await metodo.Clicar(el.PaginaAmortizacao, "Clicar na página Amortizacao");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Amortizacao")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var amortizacao = new AmortizacaoPage(page);
            await amortizacao.ValidarAcentosAmortizacao();}
        }
}
