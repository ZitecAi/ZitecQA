using Microsoft.Playwright;
using PortalIDSFTestes.elementos.notaComercial;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.notaComercial;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.notaComercial
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Nota Comercial")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class NotaComercialTest : Executa
    {
        private IPage page;
        Metodos metodo;
        NotaComercialElements el = new NotaComercialElements();

        [SetUp]
        public async Task SetUp()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuNotaComercial, "Clicar em Nota Comercial no Menu hambúrguer");
            await metodo.Clicar(el.PaginaNotaComercial, "Clicar em Nota Comercial para acessar a página");
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
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.ValidarAcentosNotaComercialPage();
        }

        [Test, Order(2)]
        public async Task Deve_Fazer_Download_Excel()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.DownloadExcel();
        }

        [Test, Order(3)]
        public async Task Deve_Cadastrar_Nova_Nota_Comercial()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.CadastrarNotaComercial();
        }

        [Test, Order(4)]
        public async Task Deve_Consultar_Nota_Comercial_Pela_Tabela()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.ConsultarNotaComercialNaTabela();
        }
        [Test, Order(5)]
        public async Task Deve_Fazer_Download_Minuta()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.DownloadMinuta();
        }

        [Test, Order(6)]
        public async Task Deve_Cancelar_Nota_Comercial()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.CancelarNotaComercialNaTabela();
        }

        
        [Test, Order(7)]
        public async Task Nao_Deve_Cadastrar_Nota_Comercial_Com_Campos_Em_Branco()
        {
            var notaComercial = new NotaComercialPage(page);
             
            await notaComercial.CadastrarNotaComercialNegativa("CamposEmBranco");
        }
        [Test, Order(8)]
        public async Task Nao_Deve_Cadastrar_Nota_Comercial_Com_Campos_Em_Branco_Sessao_Envolvidos()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.CadastrarNotaComercialNegativa("CamposEmBrancoEnvolvidos");
        }
        [Test, Order(9)]
        public async Task Nao_Deve_Cadastrar_Nota_Comercial_Com_Campos_Em_Branco_Sessao_Operacoes()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.CadastrarNotaComercialNegativa("CamposEmBrancoOperacao");
        }
        [Test, Order(10)]
        public async Task Nao_Deve_Cadastrar_Nota_Comercial_Com_Amortizacao_Maior_Que_Duracao()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.CadastrarNotaComercialNegativa("AmortizacaoMaiorQueDuracao");
        }
        [Test, Order(11)]
        public async Task Nao_Deve_Cadastrar_Nota_Comercial_Com_Carencia_Juros_Maior_Que_Juros_Principal()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.CadastrarNotaComercialNegativa("CarenciaJurosMaiorQueJurosPrincipal");
        }
    }
}
