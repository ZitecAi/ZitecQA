using Microsoft.Playwright;
using PortalIDSFTestes.elementos.controleInterno;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.controleInterno;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace PortalIDSFTestes.testes.controleInterno
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Comunicado")]
    [Category("Criticidade: Alta")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ComunicadoTests UI")]
    [AllureOwner("Levi")]
    public class ComunicadoTests : Executa
    {
        private IPage page;
        Metodos metodo;
        ComunicadoElements el = new ComunicadoElements();

        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuControleInterno, "Clicar em Controle interno menu hamburguer");
            await metodo.Clicar(el.PaginaComunicado, "Clicar em Comunicado para acessar a página");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Comunicado")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
             var comunicado = new ComunicadoPage(page);
            await comunicado.ValidarAcentosComunicadoPage();}
        }
}
