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
    [Category("Suíte: Prestadores de Serviços")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("PrestServicosTests UI")]
    [AllureOwner("Levi")]
    public class PrestServicosTests : TestBase
    {
        private IPage page;
        Utils metodo;
        PrestServicosElements el = new PrestServicosElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaPrestServ, "Clicar na página Prestadores de Serviços");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Prest Servicos")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var prestServicos = new PrestServicosPage(page);
            await prestServicos.ValidarAcentosPrestServicos();}
        }
}
