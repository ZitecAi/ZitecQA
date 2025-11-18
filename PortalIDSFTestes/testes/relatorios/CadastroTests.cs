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
    [Category("Suíte: Cadastro")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("CadastroTests UI")]
    [AllureOwner("Levi")]
    public class CadastroTests : TestBase
    {
        private IPage page;
        Utils metodo;
        CadastroElements el = new CadastroElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuRelatorios, "Clicar em Relatorios menu hamburguer");
            await metodo.Clicar(el.PaginaCadastro, "Clicar em Cadastro para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Cadastro")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var cadastro = new CadastroPage(page);
            await cadastro.ValidarAcentosCadastroPage();}
        }
}
