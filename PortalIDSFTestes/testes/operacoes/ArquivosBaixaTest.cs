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
            await metodo.Clicar(el.menuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.paginaBaixas, "Clicar arquivos baixas 2.0 para acessar a pagina");
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.ValidarAcentosArquivosBaixaPage();
        }

        [Test, Order(2)]
        public async Task Deve_Enviar_Arquivo_Baixa()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.EnviarArquivoBaixa();
        }

        [Test, Order(3)]
        public async Task Deve_Consultar_Arquivo_Baixa()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.ConsultarArquivoBaixa();
        }

        [Test, Order(4)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Deve_Fazer_Download_Relatorio_Titulos()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.BaixarRelatorioDeTitulos();
        }

        [Test, Order(5)]
        public async Task Deve_Fazer_Download_Relatorio_Movimentos()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.BaixarRelatorioDeMovimentos();
        }

        [Test, Order(6)]
        public async Task Deve_Fazer_Download_Arquivo_CNAB()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.GerarArquivoCnab();
        }



    }
}
