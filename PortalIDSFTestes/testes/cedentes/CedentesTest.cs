using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.cedentes;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.cedentes;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.cedentes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Cedentes")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("CedentesTest UI")]
    [AllureOwner("Levi")]
    public class CedentesTest : Executa
    {
        private IPage page;
        Metodos metodo;
        CedentesElements el = new CedentesElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCedentes, "Clicar na sessão cedentes no menú hamburguer");
            await metodo.Clicar(el.PaginaCedentes, "Clicar na página cedentes");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Cedentes")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.ValidarAcentosCedentesPage();
        }

        [Test, Order(2)]
        [AllureName("Deve Fazer Download Excel")]
        public async Task Deve_Fazer_Download_Excel()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.DownloadExcel();
        }

        [Test, Order(3)]
        [AllureName("Deve Cadastrar Cedente E Ativar Cedente")]
        public async Task Deve_Cadastrar_Cedente_E_Ativar_Cedente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedente("21.465.218/0001-91");
        }

        [Test, Order(6)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjFundo Invalido")]
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
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjCedente Invalido")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Invalido()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_CnpjCedenteInvalido_N.zip");
        }
        [Test, Order(9)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjFundo Em Branco")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Em_Branco()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("_49624866830_N.zip");
        }
        [Test, Order(10)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjFundo Inexistente")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Inexistente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52221175000190_49624866830_N.zip");
        }
        [Test, Order(11)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjCedente Em Branco")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Em_Branco()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160__N.zip");
        }
        [Test, Order(12)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjCedente Invalido Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Invalido_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("50963249000170_33786902000154_N.zip");
        }
        [Test, Order(13)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpj Cedente Inexistente")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpj_Cedente_Inexistente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_52221175000191_N.zip");
        }
        [Test, Order(14)]
        [AllureName("Nao Deve Cadastrar Cedente Com CNPJ Cedente Invalido Na Conta Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_CNPJ_Cedente_Invalido_Na_Conta_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("50963249000170_33786902000154_N.zip");
        }
        [Test, Order(15)]
        //[Ignore("Esse teste está em manutenção.")]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_CNPJ_Fundo_Invalido_Na_Conta_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_53572360463_N.zip");
        }
        [Test, Order(16)]
        [AllureName("Nao Deve Cadastrar Cedente Com Dados Bancarios Em Branco Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_Dados_Bancarios_Em_Branco_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_07277165810_N.zip");
        }
        [Test, Order(17)]
        [AllureName("Nao Deve Cadastrar Cedente Com Dados Bancarios Invalidos Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_Dados_Bancarios_Invalidos_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_62692437772_N.zip");
        }
    }
}
