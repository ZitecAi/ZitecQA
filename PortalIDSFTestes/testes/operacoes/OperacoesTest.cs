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
    public class OperacoesTest :Executa
    {
        private IPage page;
        Metodos metodo;
        OperacoesElements el = new OperacoesElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LoginSucessoInterno();
            await metodo.Clicar(el.menuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.paginaOperacoes, "Clicar em Operações para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test]
        public async Task naoDeveConterAcentosQuebrados()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.validarAcentosOperacoesPage();
        }

        [Test]
        public async Task deveEnviarUmaOperacaoCNAB()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.enviarOperacaoCNAB();
        }


    }
}
