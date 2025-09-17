using Microsoft.Playwright;
using PortalIDSFTestes.elementos.rendaFixa;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.rendaFixa;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.rendaFixa
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Posicoes Renda Fixa")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    public class PosicoesRendaFixaTests : Executa
    {
        private IPage page;
        Metodos metodo;
        PosicoesRendaFixaElements el = new PosicoesRendaFixaElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaPosicoesRendaFixa, "Clicar em Posições Renda Fixa para acessar a página");
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
            var posicoesRendaFixa = new PosicoesRendaFixaPage(page);
            await posicoesRendaFixa.ValidarAcentosPosicoesRendaFixaPage();
        }

    }
}
