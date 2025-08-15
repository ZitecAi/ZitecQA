using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.operacoes
{
    public class ArquivosBaixaElements
    {

        public string MenuOperacoes{ get; } = "(//p[text()='Operações'])[2]";
        public string PaginaBaixas { get; } = "//a[@href='/Operacoes/ArquivosBaixa.aspx']";
        public string ImportarBaixaBtn { get; } = "//button[text()='Importar Baixa']";
        public string SelectFundoZitec { get; } = "#select_fundo";
        public string EnviarBaixas { get; } = "#fileEnviarBaixas";
        public string BtnFecharModal { get; } = "#btnFecharNovoOperacao";
        public string TabelaBaixas { get; } = "#tabelaBaixas";
        public string PrimeiroTd { get; } = "(//td)[1]";
        public string MsgArquivoRecebido { get; } = "//div[text()='Arquivo recebido com sucesso! Aguarde a Validação']";
        public string BarraDePesquisa { get; } = "//input[@aria-controls='tabelaBaixas']";
        public string BtnDownloadRelatorioTitulos { get; } = "(//button[@title='Relatório de Títulos'])[2]";
        public string BtnDownloadRelatorioMovimentos { get; } = "(//button[@title='Relatório de Movimentos'])[2]";
        public string BtnDownloadArquivoCNAB { get; } = "(//button[@title='Relatório de Movimentos'])[2]";
        public string QtdTitulos { get; } = "//tr[@class='odd']//td[6]";
        public string QtdOcorrencias { get; } = "//tr[@class='odd']//td[7]";

    }
}
