using Microsoft.Playwright;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.bancoId;
using PortalIDSFTestes.pages.cadastro;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.cadastro
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Consultorias Internas")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ConsultorasTests UI")]
    [AllureOwner("Levi")]
    public class ConsultorasTests : TestBase
    {
        private IPage page;
        Utils metodo;
        ConsultorasElements el = new ConsultorasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaConsultoras, "Clicar na página Consultoras");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Consultoras")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var consultoras = new ConsultorasPage(page);
            await consultoras.ValidarAcentosConsultoras();}
        }
}
