using Microsoft.Playwright;
using PortalIDSFTestes.elementos.notaComercial;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.notaComercial;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.notaComercial
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Nota Comercial")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class NotaComercialTest : Executa
    {
        private IPage page;
        Metodos metodo;
        NotaComercialElements el = new NotaComercialElements();

        [SetUp]
        public async Task SetUp()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LoginSucessoInterno();
            await metodo.Clicar(el.MenuNotaComercial, "Clicar em Nota Comercial no Menu hambúrguer");
            await metodo.Clicar(el.PaginaNotaComercial, "Clicar em Nota Comercial para acessar a página");
            await Task.Delay(500);
        }
        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task NaoDeveConterAcentosQuebrados()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.ValidarAcentosNotaComercialPage();
        }

        [Test, Order(2)]
        public async Task DeveFazerDownloadExcel()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.DownloadExcel();
        }
        [Test, Order(3)]
        public async Task DeveCadastrarNovaNotaComercial()
        {
            var notaComercial = new NotaComercialPage(page);
            await notaComercial.CadastrarNotaComercial();
        }




    }
}
