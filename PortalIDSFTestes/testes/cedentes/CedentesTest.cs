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
            await login.LogarInterno();
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
            await cedentes.CadastrarCedente("FUNDO QA");
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

        [Test, Order(6)]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Invalido()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("CnpjFundoInvalido_49624866830_N.zip");
        }
        [Test, Order(7)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Invalido_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_49624866830_N.zip");
        }
        [Test, Order(8)]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Invalido()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_CnpjCedenteInvalido_N.zip");
        }
        [Test, Order(9)]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Em_Branco()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("_49624866830_N.zip");
        }
        [Test, Order(10)]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Inexistente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52221175000190_49624866830_N.zip");
        }
        [Test, Order(11)]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Em_Branco()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160__N.zip");
        }
        [Test, Order(12)]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Invalido_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("50963249000170_33786902000154_N.zip");
        }
        [Test, Order(13)]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpj_Cedente_Inexistente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_52221175000191_N.zip");
        }
        [Test, Order(14)]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_CNPJ_Cedente_Invalido_Na_Conta_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("50963249000170_33786902000154_N.zip");
        }
        [Test, Order(15)]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_CNPJ_Fundo_Invalido_Na_Conta_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_53572360463_N.zip");
        }
        [Test, Order(15)]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_Dados_Bancarios_Em_Branco_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_07277165810_N.zip");
        }
        [Test, Order(15)]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_Dados_Bancarios_Invalidos_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_62692437772_N.zip");
        }



        



    }
}
