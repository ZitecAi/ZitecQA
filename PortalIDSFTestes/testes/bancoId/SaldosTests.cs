using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.bancoId;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.bancoId
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Saldos")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("SaldosTests UI")]
    [AllureOwner("Levi")]
    public class SaldosTests : TestBase
    {

        Utils _util;
        SaldosElements _el = new SaldosElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            _util = new Utils(page);
            await login.LogarInterno();
            await _util.Clicar(_el.MenuBancoId, "Clicar na sessão Banco ID no menú hamburguer");
            await _util.Clicar(_el.PaginaSaldos, "Clicar na página Usuarios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Saldos")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var saldos = new SaldosPage(page);
            await saldos.ValidarAcentosSaldos();
        }

        [Test, Order(2)]
        [AllureName("Deve Filtrar Saldos Por Gestora E Consultoria")]
        public async Task Deve_Trazer_Saldos_Por_Gestora_E_Consultoria()
        {
            var saldos = new SaldosPage(page);
            await saldos.AbrirModalDeFiltro();
            await saldos.LimparFiltros();
            await saldos.SelcionarGestora();
            await saldos.SelcionarConsultoria();
            await saldos.InserirNomeFundo();
            await saldos.ClicarNoFundo();
            await saldos.ClicarEmCarregarParaTrazerSaldos();
            string saldo = await _util.ValidarSeElementoPossuiValorDeTexto(_el.SaldoNaTabela, "Validar se Saldo está presente na tabela");
            Console.WriteLine(saldo);
        }

    }
}
