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
    [Category("Suíte: Consultorias Internas")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ConsultoriasInternasTests UI")]
    [AllureOwner("Levi")]
    public class ConsultoriasInternasTests : TestBase
    {
        Utils metodo;
        ConsultoriasInternasElements el = new ConsultoriasInternasElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaConsultoriasInternas, "Clicar na página Consultorias Internas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Consultorias Internas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var consultoriasInternas = new ConsultoriasInternasPage(page);
            await consultoriasInternas.ValidarAcentosConsultoriasInternas();
        }
    }
}
