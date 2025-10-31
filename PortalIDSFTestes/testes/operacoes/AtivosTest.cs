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
    [Category("Suíte: Ativos")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class AtivosTest : Executa
    {
        private IPage page;
        Metodos metodo;
        AtivosElements el = new AtivosElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaAtivos, "Clicar em Ativos para acessar a página");
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
            var ativos = new AtivosPage(page);
            await ativos.ValidarAcentosAtivosPage();
        }


        [Test, Order(2)]
        public async Task Deve_Fazer_Download_Excel()
        {
            var ativos = new AtivosPage(page);
            await ativos.DownloadExcel();
        }

        [Test, Order(3)]
        public async Task Deve_Cadastrar_Um_Ativo()
        {
            var ativos = new AtivosPage(page);
            await ativos.CadastrarAtivo();
        }

        [Test, Order(4)]
        public async Task Deve_Consultar_Um_Ativo()
        {
            var ativos = new AtivosPage(page);
            await ativos.ConsultarAtivo();
        }

        [Test, Order(5)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Deve_Aprovar_Gestor()
        {
            var ativos = new AtivosPage(page);
            await ativos.AprovarGestor();
        }

        [Test, Order(6)]
        public async Task Deve_Fazer_Download_Arquivo()
        {
            var ativos = new AtivosPage(page);
            await ativos.DownloadArquivo();
        }
        [Test, Order(7)]
        public async Task Deve_Conter_Quantidade_Total_Correta_De_Ativos()
        {
            var ativos = new AtivosPage(page);
            await ativos.ContagemDeAtivosTotais();
        }

    }
}
