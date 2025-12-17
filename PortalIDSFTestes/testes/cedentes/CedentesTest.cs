using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.data.cedentes;
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
    public class CedentesTest : TestBase
    {
        Utils metodo;
        CedentesElements el = new CedentesElements();
        CedentesData _data = new CedentesData();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
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
        [AllureTag("Regressivos")]
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
        [AllureName("Deve Cadastrar Cedente")]
        [AllureTag("Regressivos")]
        public async Task Deve_Cadastrar_Cedente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedente(_data.CnpjCedente);
        }
        [Test, Order(4)]
        [AllureName("Deve Consultar cedente")]
        public async Task Deve_Consultar_Cedente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.ConsultarCedente(_data.CnpjCedente);
        }
        [Test, Order(5)]
        [AllureName("Deve Aprovar cedente nivel Gestora")]
        public async Task Deve_Aprovar_Cedente_nivel_Gestora()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.AprovarGestora(_data.CnpjCedente);
        }
        [Test, Order(6)]
        [AllureName("Deve Aprovar cedente nivel Compliance")]
        public async Task Deve_Aprovar_Cedente_nivel_Compliance()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.AprovarCompliance(_data.CnpjCedente);
        }
        [Test, Order(7)]
        [AllureName("Deve Aprovar cedente nivel Cadastro")]
        public async Task Deve_Aprovar_Cedente_nivel_Cadastro()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.AprovarCadastro(_data.CnpjCedente);
        }
        [Test, Order(8)]
        [AllureName("Deve Enviar contrato mae do cedente")]
        public async Task Deve_Enviar_Contrato_Mae()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.EnviarContratoMae(_data.CnpjCedente);
        }
        [Test, Order(9)]
        [AllureName("Deve Aprovar contrato mae do cedente")]
        public async Task Deve_Aprovar_Contrato_mae()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.AprovarContratoMae(_data.CnpjCedente);
        }
        [Test, Order(10)]
        [AllureName("Deve Excluir cedente")]
        public async Task Deve_Excluir_Cedente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.ExcluirCedente(_data.CnpjCedente);
        }

        [Test, Order(11)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjFundo Invalido")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Invalido()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("CnpjFundoInvalido_49624866830_N.zip");
        }
        [Test, Order(12)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Invalido_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_49624866830_N.zip");
        }
        [Test, Order(13)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjCedente Invalido")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Invalido()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_CnpjCedenteInvalido_N.zip");
        }
        [Test, Order(14)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjFundo Em Branco")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Em_Branco()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("_49624866830_N.zip");
        }
        [Test, Order(15)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjFundo Inexistente")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjFundo_Inexistente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52221175000190_49624866830_N.zip");
        }
        [Test, Order(16)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjCedente Em Branco")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Em_Branco()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160__N.zip");
        }
        [Test, Order(17)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpjCedente Invalido Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpjCedente_Invalido_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("50963249000170_33786902000154_N.zip");
        }
        [Test, Order(18)]
        [AllureName("Nao Deve Cadastrar Cedente ComCnpj Cedente Inexistente")]
        public async Task Nao_Deve_Cadastrar_Cedente_ComCnpj_Cedente_Inexistente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("36614123000160_52221175000191_N.zip");
        }
        [Test, Order(19)]
        [AllureName("Nao Deve Cadastrar Cedente Com CNPJ Cedente Invalido Na Conta Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_CNPJ_Cedente_Invalido_Na_Conta_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("50963249000170_33786902000154_N.zip");
        }
        [Test, Order(20)]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_CNPJ_Fundo_Invalido_Na_Conta_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_53572360463_N.zip");
        }
        [Test, Order(21)]
        [AllureName("Nao Deve Cadastrar Cedente Com Dados Bancarios Em Branco Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_Dados_Bancarios_Em_Branco_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_07277165810_N.zip");
        }
        [Test, Order(22)]
        [AllureName("Nao Deve Cadastrar Cedente Com Dados Bancarios Invalidos Template")]
        public async Task Nao_Deve_Cadastrar_Cedente_Com_Dados_Bancarios_Invalidos_Template()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedenteNegativo("52115758000179_62692437772_N.zip");
        }
    }
}
