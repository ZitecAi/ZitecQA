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

namespace PortalIDSFTestes.testes.relatorios
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Relatorio Cedentes")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    public class RelatorioCedentesTest : Executa
    {
        private IPage page;
        Metodos metodo;
        RelatorioCedentesElements el = new RelatorioCedentesElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuRelatorios, "Clicar em Relatorios menu hamburguer");
            await metodo.Clicar(el.PaginaRelatorioCedentes, "Clicar em Cedentes para acessar a página Cedentes na sessão relatorios");
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
            var relatorioCedentes = new RelatorioCedentesPage(page);
            await relatorioCedentes.ValidarAcentosRelatorioCedentesPage();
        }


    }
}
