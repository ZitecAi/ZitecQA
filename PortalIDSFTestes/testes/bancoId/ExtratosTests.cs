using Microsoft.Playwright;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.administrativo;
using PortalIDSFTestes.pages.bancoId;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.bancoId
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Extratos")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class ExtratosTests : Executa
    {

        private IPage page;
        Metodos metodo;
        ExtratosElements el = new ExtratosElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuBancoId, "Clicar na sessão Banco ID no menú hamburguer");
            await metodo.Clicar(el.PaginaExtratos, "Clicar na página Usuarios");
            await Task.Delay(500);
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var extratos = new ExtratosPage(page);
            await extratos.ValidarAcentosExtratos();
        }
        [Test, Order(2)]    
        public async Task Deve_Gerar_Extrato_Pdf_Com_Sucesso()
        {
            var extratos = new ExtratosPage(page);
            await extratos.GerarExtratoPdf();
        }

    }
}
