using NUnit.Framework;
using PortaIDSFTestes.pages.login;
using PortaIDSFTestes.runner;
using Microsoft.Playwright;

namespace PortaIDSFTestes.testes.login
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("LoginTest")]
    public class LoginTest : Executa
    {
        private IPage page;

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task deveRealizarLoginComSucessoNivelInterno()
        {
            var login = new LoginPage(page);
            await login.LoginSucessoInterno();
        }

        [Test, Order(2)]
        public async Task naoDeveRealizarLoginComSenhaInvalidaNivelInterno()
        {
            var login = new LoginPage(page);
            await login.LoginNegativoInterno("qazitec01@gmail.com", "invalida");
        }

        [Test, Order(3)]
        public async Task deveRealizarLoginComSucessoNivelConsultoria()
        {
            var login = new LoginPage(page);
            await login.LoginSucessoConsultoria();
        }

        [Test, Order(4)]
        public async Task naoDeveRealizarLoginComSenhaInvalidaNivelConsultoria()
        {
            var login = new LoginPage(page);
            await login.LoginNegativoConsultoria("jessica.tavares@aluno.ifsp.edu.br", "invalida");
        }

        [Test, Order(5)]
        public async Task deveRealizarLoginComSucessoNivelGestora()
        {
            var login = new LoginPage(page);
            await login.LoginSucessoGestora();
        }

        [Test, Order(6)]
        public async Task naoDeveRealizarLoginComSenhaInvalidaNivelGestora()
        {
            var login = new LoginPage(page);
            await login.LoginNegativoGestora("caiooliweira@gmail.com", "invalida");
        }

        [Test, Order(7)]
        public async Task deveRealizarLoginComSucessoNivelDenver()
        {
            var login = new LoginPage(page);
            await login.LoginSucessoDenver();
        }

        [Test, Order(8)]
        public async Task naoDeveRealizarLoginComSenhaInvalidaNivelDenver()
        {
            var login = new LoginPage(page);
            await login.LoginNegativoDenver("jessica.vitoria.tavares044@gmail.com", "invalida");
        }

        [Test, Order(9)]
        public async Task naoDeveConterAcentosQuebrados()
        {
            var login = new LoginPage(page);
            await login.validarAcentosLoginPage();
        }

       
    }
}
