using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.operacoes
{
    public class OperacoesElements
    {
        public string menuOperacoes { get; } = "(//p[text()='Operações'])[2]";
        public string paginaOperacoes { get; } = "//a[@href='/Operacoes/Operacoes2.0.aspx']";

        public string btnNovaOperacaoCNAB { get; } = "//span[text()='Nova Operação - CNAB']";
        public string selectFundo { get; } = "#selectFundo";
        public string enviarOperacaoInput { get; } = "#fileEnviarOperacoes";
        public string msgSucessoRetornada { get; } = "//div[@class='toast toast-success']";
        public string btnHistoricoImportacoes { get; } = "//button[@class='btn btn-secondary btn-Historico']";
        public string barraPesquisaHistorico { get; } = "//input[@aria-controls='tabelaHistorico']";
        public string tabelaHistoricoImportacoes { get; } = "#tabelaHistorico";
        public string btnDownloadValidacaoMovimento { get; } = "//button[@title='Validação Movimento']";
        public string btnDownloadValidacaoLayout { get; } = "//button[@title='Validação Layout']";




    }
}
