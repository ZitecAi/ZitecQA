using Microsoft.Playwright;
using PortalIDSFTestes.elementos.controleInterno;
using PortalIDSFTestes.elementos.risco;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.controleInterno;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.risco;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.risco
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Fundos Desenquadrados")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    public class FundosDesenquadradosTests : Executa
    {
        private IPage page;
        Metodos metodo;
        FundosDesenquadradosElements el = new FundosDesenquadradosElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuRisco, "Clicar em Controle interno menu hamburguer");
            await metodo.Clicar(el.PaginaFundosDesenquadrados, "Clicar em Fundos Desenquadrados para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var fundosDesenquadrados = new FundosDesenquadradosPage(page);
            await fundosDesenquadrados.ValidarAcentosFundosDesenquadradosPage();
        }
    }
}
