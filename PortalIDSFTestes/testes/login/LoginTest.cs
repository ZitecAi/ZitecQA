using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.login
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Su�te: Login")]
    [Category("Crit�cidade: Cr�tica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("LoginTest UI")]
    [AllureOwner("Levi")]
    public class LoginTest : Executa
    {
        private IPage page;

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

        //[Test, Order(2)]
        //public async Task Nao_Deve_Realizar_Login_Com_Senha_Invalida_Nivel_Interno()
        //{
        //    var login = new LoginPage(page);
        //    await login.LoginNegativoInterno("qazitec01@gmail.com", "invalida");
        //}

        //[Test, Order(3)]
        //public async Task Deve_Realizar_Login_Com_Sucesso_Nivel_Consultoria()
        //{
        //    var login = new LoginPage(page);
        //    await login.LoginSucessoConsultoria();
        //}

        //[Test, Order(4)]
        //public async Task Nao_Deve_Realizar_Login_Com_Senha_Invalida_Nivel_Consultoria()
        //{
        //    var login = new LoginPage(page);
        //    await login.LoginNegativoConsultoria("jessica.tavares@aluno.ifsp.edu.br", "invalida");
        //}

        //[Test, Order(5)]
        //public async Task Deve_Realizar_Login_Com_Sucesso_Nivel_Gestora()
        //{
        //    var login = new LoginPage(page);
        //    await login.LoginSucessoGestora();
        //}

        //[Test, Order(6)]
        //public async Task Nao_Deve_Realizar_Login_Com_Senha_Invalida_Nivel_Gestora()
        //{
        //    var login = new LoginPage(page);
        //    await login.LoginNegativoGestora("caiooliweira@gmail.com", "invalida");
        //}

        //[Test, Order(7)]
        //public async Task Deve_Realizar_Login_Com_Sucesso_Nivel_Denver()
        //{
        //    var login = new LoginPage(page);
        //    await login.LoginSucessoDenver();
        //}

        //[Test, Order(8)]
        //public async Task Nao_Deve_Realizar_Login_Com_Senha_Invalida_Nivel_Denver()
        //{
        //    var login = new LoginPage(page);
        //    await login.LoginNegativoDenver("jessica.vitoria.tavares044@gmail.com", "invalida");
        //}

        [Test, Order(9)]
        [Ignore("Este teste esta em manutenção")]
        [AllureName("Nao Deve Conter Acentos Quebrados Login")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var login = new LoginPage(page);
            await login.validarAcentosLoginPage();
        }
    }
}
