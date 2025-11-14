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
    [Category("Suíte: Ofertas")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("OfertasTests UI")]
    [AllureOwner("Levi")]
    public class OfertasTests : Executa
    {
        private IPage page;
        Metodos metodo;
        OfertasElements el = new OfertasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaOfertas, "Clicar na página Prestadores de Ofertas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Ofertas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var ofertas = new OfertasPage(page);
            await ofertas.ValidarAcentosOfertas();}
        }
}
