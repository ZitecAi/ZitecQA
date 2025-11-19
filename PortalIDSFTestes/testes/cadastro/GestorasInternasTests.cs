using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.cadastro;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.cadastro
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Gestoras Internas")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("GestorasInternasTests UI")]
    [AllureOwner("Levi")]
    public class GestorasInternasTests : TestBase
    {
        Utils metodo;
        GestorasInternasElements el = new GestorasInternasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaGestorasInternas, "Clicar na página Gestoras Internas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Gestoras Internas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var gestorasInternas = new GestorasInternasPage(page);
            await gestorasInternas.ValidarAcentosGestorasInternas();
        }
    }
}
