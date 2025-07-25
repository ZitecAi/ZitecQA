using Microsoft.Playwright;
using PortaIDSFTestes.pages.login;
using PortaIDSFTestes.pages.operacoes;
using PortaIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortaIDSFTestes.testes.operacoes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("ArquivosBaixaTest")]
    public class ArquivosBaixaTest : Executa
    {
        private IPage page;

        [SetUp]
        public async Task SetUp()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            await login.LoginSucessoInterno();
        }

        [TearDown]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        public async Task naoDeveConterAcentosQuebrados()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.validarAcentosArquivosBaixaPage();
        }

        [Test, Order(2)]
        public async Task deveEnviarArquivoBaixa()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.enviarArquivoBaixa();
        }



    }
}
