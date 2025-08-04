using Microsoft.Playwright;
using PortalIDSFTestes.pages.cedentes;
using PortalIDSFTestes.runner;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalIDSFTestes.elementos.cedentes;

namespace PortalIDSFTestes.testes.cedentes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Cedentes")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class CedentesTest : Executa
    {
        private IPage page;
        Metodos metodo;
        CedentesElements el = new CedentesElements();  

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LoginSucessoInterno();
            await metodo.Clicar(el.MenuCedentes, "Clicar na seção cedentes no menú hamburguer");
            await metodo.Clicar(el.PaginaCedentes, "Clicar na página cedentes");
            await Task.Delay(500);
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test]
        public async Task NaoDeveConterAcentosQuebrados()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.ValidarAcentosCedentesPage();
        }

        [Test]
        public async Task DeveValidarExcel()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.DownloadExcel();
        }



        [Test]
        public async Task DeveCadastrarCedente()
        {
            var cedentes = new CedentesPage(page);
            await cedentes.CadastrarCedente();
        }

    }
}
