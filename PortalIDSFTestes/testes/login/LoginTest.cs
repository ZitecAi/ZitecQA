using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.login
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Su�te: Login")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("LoginTest UI")]
    [AllureOwner("Levi")]
    public class LoginTest : TestBase
    {

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Deve Realizar Login Com Sucesso Nivel Interno")]
        public async Task Deve_Realizar_Login_Com_Sucesso_Nivel_Interno()
        {
            var login = new LoginPage(page);
            await login.LoginSucessoInterno();
        }

        [Test, Order(2)]
        [Ignore("Este teste esta em manutenção")]
        [AllureName("Nao Deve Conter Acentos Quebrados Login")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var login = new LoginPage(page);
            await login.validarAcentosLoginPage();
        }
    }
}
