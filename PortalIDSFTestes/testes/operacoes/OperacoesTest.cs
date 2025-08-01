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
    [Category("Suíte: Operações")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class OperacoesTest : Executa
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
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaOperacoes, "Clicar em Operações para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task NaoDeveConterAcentosQuebrados()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.ValidarAcentosOperacoesPage();
        }

        [Test, Order(2)]
        public async Task DeveEnviarUmaOperacaoCNAB()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.EnviarOperacaoCNAB();
        }
        [Test, Order(3)]
        public async Task DeveConsultarUmaOperacaoCNABPeloHistoricoImportacoes()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.ConsultarCNABPeloHistoricoImportacoes();
        }
        [Test, Order(4)]
        public async Task DeveFazerDownloadRelatorioMovimento_Layout()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.DownloadValidacaoMovimento_Layout();
        }
        
        [Test, Order(5)]
        public async Task DeveFazerDownloadExcel()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.DownloadExcel();
        }

        [Test, Order(6)]
        public async Task DeveEnviarUmaOperacaoCSV()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.EnviarOperacaoCSV();
        }            
        [Test, Order(7)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task DeveExcluirUmaOperacao()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.ExcluirArquivo();
        }            
     
        // esperar Correção na paginação para arquivo teste. arquivo enviado não aparece na tabela

    }
}
