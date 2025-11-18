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
    [Category("Suíte: Controle Escrow")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ControleEscrowTests UI")]
    [AllureOwner("Levi")]
    public class ControleEscrowTests : TestBase
    {

        private IPage page;
        Utils metodo;
        ControleEscrowElements el = new ControleEscrowElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBancoId, "Clicar na sessão Banco Id no menú hamburguer");
            await metodo.Clicar(el.PaginaControleEscrow, "Clicar na página  Controle Escrow");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Controle Escrow")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var ControleEscrow = new ControleEscrowPage(page);
            await ControleEscrow.ValidarAcentosControleEscrow();}
        }
}
