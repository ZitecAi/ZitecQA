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
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.bancoId
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Escrow Externo")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("EscrowExternoTests UI")]
    [AllureOwner("Levi")]
    public class EscrowExternoTests : TestBase
    {

        private IPage page;
        Utils metodo;
        EscrowExternoElements el = new EscrowExternoElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBancoId, "Clicar na sessão Banco ID no menú hamburguer");
            await metodo.Clicar(el.PaginaEscrowExterno, "Clicar na página Usuarios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Escrow Externo")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var escrowExterno = new EscrowExternoPage(page);
            await escrowExterno.ValidarAcentosEscrowExterno();}
        }
}
