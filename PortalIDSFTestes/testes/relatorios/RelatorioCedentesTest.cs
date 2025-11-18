using Microsoft.Playwright;
using PortalIDSFTestes.elementos.relatorios;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.relatorios;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.relatorios
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Relatorio Cedentes")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("RelatorioCedentesTest UI")]
    [AllureOwner("Levi")]
    public class RelatorioCedentesTest : TestBase
    {
        private IPage page;
        Utils metodo;
        RelatorioCedentesElements el = new RelatorioCedentesElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuRelatorios, "Clicar em Relatorios menu hamburguer");
            await metodo.Clicar(el.PaginaRelatorioCedentes, "Clicar em Cedentes para acessar a página Cedentes na sessão relatorios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Relatorio Cedentes")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var relatorioCedentes = new RelatorioCedentesPage(page);
            await relatorioCedentes.ValidarAcentosRelatorioCedentesPage();}
        }
}
