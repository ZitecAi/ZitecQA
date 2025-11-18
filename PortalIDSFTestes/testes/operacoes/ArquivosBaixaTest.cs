using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.operacoes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Arquivos Baixa")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("ArquivosBaixaTest UI")]
    [AllureOwner("Levi")]
    public class ArquivosBaixaTest : TestBase
    {
        private IPage page;
        Utils metodo;
        ArquivosBaixaElements el = new ArquivosBaixaElements();

        [SetUp]
        [AllureBefore]
        public async Task SetUp()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaBaixas, "Clicar arquivos baixas 2.0 para acessar a pagina");
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();
        }

        [Test, Order(1)]
        [AllureName("Nao Deve Conter Acentos Quebrados Arquivos Baixa")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.ValidarAcentosArquivosBaixaPage();
        }

        [Test, Order(2)]
        [AllureName("Deve Enviar Arquivo Baixa")]
        public async Task Deve_Enviar_Arquivo_Baixa()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.EnviarArquivoBaixa();
        }

        [Test, Order(3)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Deve_Fazer_Download_Relatorio_Titulos()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.BaixarRelatorioDeTitulos();
        }

        [Test, Order(4)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Deve_Fazer_Download_Relatorio_Movimentos()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.BaixarRelatorioDeMovimentos();
        }

        [Test, Order(5)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Deve_Fazer_Download_Arquivo_CNAB()
        {
            var baixa = new ArquivosBaixaPage(page);
            await baixa.GerarArquivoCnab();
        }

        //[Test, Order(6)]

        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Data_Geracao_Invalida()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_01.txt", "Arquivo Baixa com Header com data de geração inválida");
        //}
        //[Test, Order(7)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Data_Banco_inválido()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_02.txt", "Arquivo Baixa com Banco inválido");
        //}
        //[Test, Order(8)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_título_zero()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_03.txt", "Arquivo Baixa com título zero");
        //}
        //[Test, Order(9)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Caractere_não_numérico()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_04.txt", "Arquivo Baixa com Caractere não-numérico");
        //}
        //[Test, Order(10)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_total_de_registros_incorreto()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_05.txt", "Arquivo Baixa com total de registros incorreto");
        //}
        //[Test, Order(11)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Sequencial_Duplicado()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_06.txt", "Arquivo Baixa com Sequencial duplicado");
        //}
        //[Test, Order(12)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Nosso_número_vazio()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_07.txt", "Arquivo Baixa com Nosso número vazio");
        //}
        //[Test, Order(13)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Nome_do_sacado_em_branco()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_08.txt", "Arquivo Baixa com Nome do sacado em branco");
        //}
        //[Test, Order(14)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Data_Vencimento_inválido()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_09.txt", "Arquivo Baixa com Data Vencimento inválido");
        //}
        //[Test, Order(15)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_Header_com_443_colunas()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_10.txt", "Arquivo Baixa com Header com 443 colunas");
        //}
        //[Test, Order(16)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_CNPJ_cedente_com_letra()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_11.txt", "Arquivo Baixa com CNPJ do cedente com letra");
        //}
        //[Test, Order(17)]
        //public async Task Nao_Deve_Aceitar_Arquivo_Baixa_Com_DV_da_agência_conta_ausente()
        //{
        //    var baixa = new ArquivosBaixaPage(page);
        //    await baixa.EnviarArquivoBaixaNegativo("cnab_neg_12.txt", "Arquivo Baixa com DV da agência/conta ausente");
        //}
    }
}
