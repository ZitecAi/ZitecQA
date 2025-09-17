using Microsoft.Playwright;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.bancoId;
using PortalIDSFTestes.pages.cadastro;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.cadastro
{
    public class GestorasInternasTests : Executa
    {
        private IPage page;
        Metodos metodo;
        GestorasInternasElements el = new GestorasInternasElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuCadastro, "Clicar na sessão Cadastro no menú hamburguer");
            await metodo.Clicar(el.PaginaGestorasInternas, "Clicar na página Gestoras Internas");
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
            var gestorasInternas = new GestorasInternasPage(page);
            await gestorasInternas.ValidarAcentosGestorasInternas();
        }


    }
}
