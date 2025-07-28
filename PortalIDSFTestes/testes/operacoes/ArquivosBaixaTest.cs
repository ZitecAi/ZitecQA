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
    [Category("Suíte: Arquivos Baixa")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class ArquivosBaixaTest : Executa
    {
        private IPage page;
        Metodos metodo;
        ArquivosBaixaElements el = new ArquivosBaixaElements();

        [SetUp]
        public async Task SetUp()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LoginSucessoInterno();
            await metodo.Clicar(el.menuOperaoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.paginaBaixas, "Clicar arquivos baixas 2.0 para acesasr a pagina");
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task naoDeveConterAcentosQuebrados()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.validarAcentosArquivosBaixaPage();
        }

        [Test, Order(2)]
        public async Task deveEnviarArquivoBaixa()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.enviarArquivoBaixa();
        }

        [Test, Order(3)]
        public async Task deveFazerDownloadRelatorioTitulos()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.baixarRelatorioDeTitulos();
        }

        [Test, Order(4)]
        public async Task deveFazerDownloadRelatorioMovimentos()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.baixarRelatorioDeMovimentos();
        }

        [Test, Order(6)]
        public async Task deveFazerDownloadArquivoCNAB()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.gerarArquivoCnab();
        }



    }
}
