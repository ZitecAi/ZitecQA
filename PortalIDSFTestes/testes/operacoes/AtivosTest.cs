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
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.operacoes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Ativos")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("AtivosTest UI")]
    [AllureOwner("Levi")]
    public class AtivosTest : Executa
    {
        private IPage page;
        Metodos metodo;
        AtivosElements el = new AtivosElements();

        [SetUp]
        [AllureBefore]
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
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Ativos")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var ativos = new AtivosPage(page);
            await ativos.ValidarAcentosAtivosPage();}
        [Test, Order(2)]
        [AllureName("Deve Conter Quantidade Total Correta De Ativos")]
        public async Task Deve_Conter_Quantidade_Total_Correta_De_Ativos()
        {
             var ativos = new AtivosPage(page);
            await ativos.ContagemDeAtivosTotais();}
        [Test, Order(3)]
        [AllureName("Deve Fazer Download Excel")]
        public async Task Deve_Fazer_Download_Excel()
        {
             var ativos = new AtivosPage(page);
            await ativos.DownloadExcel();}

        [Test, Order(4)]
        [AllureName("Deve Cadastrar e consultar Um Ativo")]
        public async Task Deve_Cadastrar_e_consultar_Um_Ativo()
        {
             var ativos = new AtivosPage(page);
            await ativos.CadastrarEConsultarAtivo();}
        [Test, Order(5)]
        [AllureName("Deve Aprovar Um Ativo Por Gestor")]
        public async Task Deve_Aprovar_Um_Ativo_Por_Gestor()
        {
             var ativos = new AtivosPage(page);
            await ativos.AprovarGestor();}
        [Test, Order(6)]
        [AllureName("Deve Aprovar Um Ativo Pelo Juridico")]
        public async Task Deve_Aprovar_Um_Ativo_Pelo_Juridico()
        {
             var ativos = new AtivosPage(page);
            await ativos.AprovarJuridico();}
        [Test, Order(7)]
        [AllureName("Deve Aprovar Um Ativo Por Risco")]
        public async Task Deve_Aprovar_Um_Ativo_Por_Risco()
        {
             var ativos = new AtivosPage(page);
            await ativos.AprovarRisco();}
        [Test, Order(8)]
        [AllureName("Deve Aprovar Um Ativo Por Cadastro")]
        public async Task Deve_Aprovar_Um_Ativo_Por_Cadastro()
        {
             var ativos = new AtivosPage(page);
            await ativos.AprovarCadastro();}

        [Test, Order(9)]
        [AllureName("Deve Fazer Download Arquivo")]
        public async Task Deve_Fazer_Download_Arquivo()
        {
             var ativos = new AtivosPage(page);
            await ativos.DownloadArquivo();}
        }
}
