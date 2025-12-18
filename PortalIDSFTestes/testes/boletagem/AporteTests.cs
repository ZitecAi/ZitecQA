using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.data.boletagem;
using PortalIDSFTestes.elementos.Boletagem;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.boletagem;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.boletagem
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Aporte")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("AporteTests UI")]
    [AllureOwner("Levi")]
    public class AporteTests : TestBase
    {
        Utils metodo;
        AporteElements el = new AporteElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBoletagem, "Clicar na sessão Boletagem no menú hamburguer");
            await metodo.Clicar(el.PaginaAporte, "Clicar na página Aporte");
            await Task.Delay(500);
        }
        public string NomeCotistaReprovacao { get; set; } = $"Cotista Zitec {AporteData.uniqueNumber} Reprovação";

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Aporte")]
        [AllureTag("Regressivos")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var aporte = new AportePage(page);
            await aporte.ValidarAcentosAporte();
        }
        [Test, Order(2)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Realizar Aporte Com Sucesso")]
        public async Task Deve_Realizar_Aporte_Com_Sucesso()
        {
            var aporte = new AportePage(page);
            await aporte.RealizarAporte();
        }
        [Test, Order(3)]
        [AllureTag("Regressivos")]
        [AllureName("Deve consultar Aporte Com Sucesso")]
        public async Task Deve_Consultar_Aporte_Com_Sucesso()
        {
            var aporte = new AportePage(page);
            await aporte.ConsultarAporte();
        }
        [Test, Order(4)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Aprovar Aporte pela custodia")]
        public async Task Deve_Aprovar_Aporte_Pela_Custodia()
        {
            var aporte = new AportePage(page);
            await aporte.AprovarCustodia();
        }
        [Test, Order(5)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Aprovar Aporte pela escrituração")]
        public async Task Deve_Aprovar_Aporte_Pela_Escrituracao()
        {
            var aporte = new AportePage(page);
            await aporte.AprovarEscrituracao();
        }
        [Test, Order(6)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Aprovar Aporte pela Controladoria")]
        public async Task Deve_Aprovar_Aporte_Pela_Controladoria()
        {
            var aporte = new AportePage(page);
            await aporte.AprovarControladoria();
        }
        [Test, Order(7)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Realizar Aporte para reprovação Com Sucesso")]
        public async Task Deve_Realizar_Aporte_Para_Reprovar_Com_Sucesso()
        {
            var dataTest = new AporteData();
            dataTest.NomeCotista = NomeCotistaReprovacao;
            var aporte = new AportePage(page, dataTest);
            await aporte.RealizarAporte();
        }
        [Test, Order(8)]
        [AllureTag("Regressivos")]
        [AllureName("Deve consultar Aporte para reprovar Com Sucesso")]
        public async Task Deve_Consultar_Aporte_Para_Reprovar_Com_Sucesso()
        {
            var dataTest = new AporteData();
            dataTest.NomeCotista = NomeCotistaReprovacao;
            var aporte = new AportePage(page, dataTest);
            await aporte.ConsultarAporte();
        }
        [Test, Order(9)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Reprovar Aporte pela custodia")]
        public async Task Deve_Reprovar_Aporte_Pela_Custodia()
        {
            var dataTest = new AporteData();
            dataTest.NomeCotista = NomeCotistaReprovacao;
            var aporte = new AportePage(page, dataTest);
            await aporte.ReprovarCustodia();
        }
        [Test, Order(10)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Reprovar Aporte pela escrituração")]
        public async Task Deve_Reprovar_Aporte_Pela_Escrituracao()
        {
            var dataTest = new AporteData();
            dataTest.NomeCotista = NomeCotistaReprovacao;
            var aporte = new AportePage(page, dataTest);
            await aporte.ReprovarEscrituracao();
        }
        [Test, Order(11)]
        [AllureTag("Regressivos")]
        [AllureName("Deve Reprovar Aporte pela Controladoria")]
        public async Task Deve_Reprovar_Aporte_Pela_Controladoria()
        {
            var dataTest = new AporteData();
            dataTest.NomeCotista = NomeCotistaReprovacao;
            var aporte = new AportePage(page, dataTest);
            await aporte.ReprovarControladoria();
        }
    }
}
