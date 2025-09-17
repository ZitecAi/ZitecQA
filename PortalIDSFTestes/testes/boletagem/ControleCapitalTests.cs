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

namespace PortalIDSFTestes.testes.boletagem
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Controle Capital")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    public class ControleCapitalTests : Executa
    {
        private IPage page;
        Metodos metodo;
        ControleCapitalElements el = new ControleCapitalElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaResgate, "Clicar na página Controle Capital");
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
            var controleCapital = new ControleCapitalPage(page);
            await controleCapital.ValidarAcentosControleCapital();
        }

    }
}
