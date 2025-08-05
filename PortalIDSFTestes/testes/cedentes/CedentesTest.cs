using Microsoft.Playwright;
using PortalIDSFTestes.pages.cedentes;
using PortalIDSFTestes.runner;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalIDSFTestes.elementos.cedentes;

namespace PortalIDSFTestes.testes.cedentes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Cedentes")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class CedentesTest : Executa
    {
        private IPage page;
        Metodos metodo;
        CedentesElements el = new CedentesElements();  

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LoginSucessoInterno();
            await metodo.Clicar(el.MenuCedentes, "Clicar na seção cedentes no menú hamburguer");
            await metodo.Clicar(el.PaginaCedentes, "Clicar na página cedentes");
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
            var cedentes = new CedentesPage(page);
            await cedentes.ValidarAcentosCedentesPage();
        }

        [Test, Order(2)]
        public async Task Deve_Fazer_Download_Excel()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.DownloadExcel();
        }

        [Test, Order(3)]
        public async Task Deve_Cadastrar_Cedente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedente();
        }

        [Test, Order(4)]
        public async Task Deve_Consultar_Cedente_Na_Tabela()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.ConsultarCedente();
        }

        [Test, Order(5)]
        public async Task Deve_Excluir_Cedente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.ExcluirCedente();
        }

        



    }
}
