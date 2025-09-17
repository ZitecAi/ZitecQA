using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.operacoes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Conciliacao e Extrato")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    public class ConciliacaoExtratoTests : Executa
    {
        private IPage page;
        Metodos metodo;
        ConciliacaoExtratoElements el = new ConciliacaoExtratoElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaConciliacaoExtrato, "Clicar em Conciliacao e Extrato para acessar a página");
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
            var conciliacaoExtrato = new ConciliacaoExtratoPage(page);
            await conciliacaoExtrato.ValidarAcentosConciliacaoExtratoPage();
        }


    }
}
