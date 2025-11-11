using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.administrativo;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.administrativo
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Usuarios")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("UsuariosTests UI")]
    [AllureOwner("Levi")]
    public class UsuariosTests : Executa
    {
        private IPage page;
        Metodos metodo;
        UsuariosElements el = new UsuariosElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuAdministrativo, "Clicar na sessão Admninistrativo no menú hamburguer");
            await metodo.Clicar(el.PaginaUsuarios, "Clicar na página Usuarios");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Usuarios")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var usuarios = new UsuariosPage(page);
            await usuarios.ValidarAcentosUsuarios();
        }
    }
}
