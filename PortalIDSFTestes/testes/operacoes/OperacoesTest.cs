using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.operacoes;
using PortalIDSFTestes.runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.testes.operacoes
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte: Operações")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    public class OperacoesTest : Executa
    {
        private IPage page;
        Metodos metodo;
        OperacoesElements el = new OperacoesElements();

        [SetUp]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Metodos(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuOperacoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.PaginaOperacoes, "Clicar em Operações para acessar a página");
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
            var operacoes = new OperacoesPage(page);
            await operacoes.ValidarAcentosOperacoesPage();
        }

        [Test, Order(2)]
        public async Task Deve_Enviar_Uma_Operacao_CNAB()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.EnviarOperacaoCNAB("cnabNovoTesteOP.txt");
        }

        [Test, Order(3)]
        public async Task Deve_Consultar_Uma_Operacao_CNAB_Pelo_Historico_Importacoes()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.ConsultarCNABPeloHistoricoImportacoes();
        }

        [Test, Order(4)]
        public async Task Deve_Fazer_Download_Relatorio_Movimento_Layout()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.DownloadValidacaoMovimento_Layout();
        }
        
        [Test, Order(5)]
        public async Task Deve_Fazer_Download_Excel()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.DownloadExcel();
        }

        [Test, Order(6)]
        public async Task Deve_Enviar_Uma_Operacao_CSV()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.EnviarOperacaoCSV();
        }            

        [Test, Order(7)]
        [Ignore("Esse teste está em manutenção.")]
        public async Task Deve_Excluir_Uma_Operacao()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.ExcluirArquivo();
        }

        [Test, Order(8)]
        public async Task Nao_Deve_Aceitar_Uma_Operacao_CNAB_Negativo()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.EnviarOperacoesCNABNegativo("CTN-01_CnpjOriginadorEmBranco.txt",
                                                        "CTN-02_CnpjOriginadorInvalido13Char.txt",
                                                        "CTN-03_CnpjOriginadorInvalido15Char.txt",
                                                        "CTN-04_NomeCedenteEmBranco.txt",
                                                        "CTN-05_NomeCedenteInexistente.txt",
                                                        "CTN-06_NomeCedenteInvalido.txt",
                                                        "CTN-07_CnpjCedenteEmBranco.txt",
                                                        "CTN-08_CnpjCedenteInvalido13Char.txt",
                                                        "CTN-09_CnpjCedenteInvalido15Char.txt",
                                                        "CTN-10_NomeSacadoEmBranco.txt",
                                                        "CTN-11_NomeSacadoInexistente.txt",
                                                        "CTN-12_NomeSacadoInvalido.txt",
                                                        "CTN-13_CnpjSacadoEmBranco.txt",
                                                        "CTN-14_CnpjSacadoInvalido13Char.txt",
                                                        "CTN-15_CnpjSacadoInvalido15Char.txt",
                                                        "CTN-16_DataVencimentoFormatoInválido.txt",
                                                        "CTN-17_DataVencimentoEmBranco.txt",
                                                        "CTN-18_DataVencimentoNoPassado.txt",
                                                        "CTN-19_DataEmissãoEmBranco.txt",
                                                        "CTN-20_DataEmissãoNoPassado.txt",
                                                        "CTN-21_DataAquisiçãoFormatoInválido.txt",
                                                        "CTN-22_DataAquisiçãoEmBranco.txt",
                                                        "CTN-23_DataAquisiçãoNoPassado.txt",
                                                        "CTN-24_NumeroDocumentoEmBranco.txt",
                                                        "CTN-25_NumeroDocumentoInexistente.txt",
                                                        "CTN-26_NumeroDocumentoInvalido.txt",
                                                        "CTN-27_SeuNumeroEmBranco.txt",
                                                        "CTN-28_SeuNumeroInexistente.txt",
                                                        "CTN-29_SeuNumeroInvalido.txt");
        }


        [Test, Order(9)]
        public async Task Nao_Deve_Aceitar_Uma_Operacao_CSV_Negativo()
        {
            var operacoes = new OperacoesPage(page);
            await operacoes.EnviarOperacoesCSVNegativo("TesteNegativoCnpjCedenteEmBranco.csv",
                                                        "TesteNegativoCnpjCedenteInvalido13Char.csv",
                                                        "TesteNegativoCnpjCedenteInvalido15Char.csv",
                                                        "TesteNegativoCnpjOriginadorEmBranco - Copia.csv",
                                                        "TesteNegativoCnpjOriginadorInvalido13Char.csv",
                                                        "TesteNegativoCnpjOriginadorInvalido15Char.csv",
                                                        "TesteNegativoCnpjSacadoEmBranco.csv",
                                                        "TesteNegativoCnpjSacadoInvalido13Char.csv",
                                                        "TesteNegativoCnpjSacadoInvalido15Char.csv",
                                                        "TesteNegativoDataAqEmBranco.csv",
                                                        "TesteNegativoDataAqFormatoInv.csv",
                                                        "TesteNegativoDataAqPassado.csv",
                                                        "TesteNegativoDataEmisEmBranco.csv",
                                                        "TesteNegativoDataEmissPassado.csv",
                                                        "TesteNegativoDataVencEmBranco.csv",
                                                        "TesteNegativoDataVencFormatoInv.csv",
                                                        "TesteNegativoDataVencPassado.csv",
                                                        "TesteNegativoNomeCedenteEmBranco.csv",
                                                        "TesteNegativoNomeCedenteInexistente.csv",
                                                        "TesteNegativoNomeCedenteInvalido.csv",
                                                        "TesteNegativoNomeSacadoEmBranco.csv",
                                                        "TesteNegativoNomeSacadoInexistente.csv",
                                                        "TesteNegativoNomeSacadoInvalido.csv",
                                                        "TesteNegativoNuDocEmBranco.csv",
                                                        "TesteNegativoNuDocInexistente.csv",
                                                        "TesteNegativoNuDocInvalido.csv",
                                                        "TesteNegativoSeuNumeroEmBranco.csv",
                                                        "TesteNegativoSeuNumeroInexistente.csv",
                                                        "TesteNegativoSeuNumeroInvalido.csv");
        }





    }
}
