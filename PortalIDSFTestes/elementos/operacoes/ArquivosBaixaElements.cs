using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.operacoes
{
    public class ArquivosBaixaElements
    {

        public string menuOperaoes{ get; } = "(//p[text()='Operações'])[2]";
        public string paginaBaixas { get; } = "//a[@href='/Operacoes/ArquivosBaixa.aspx']";
        public string importarBaixaBtn { get; } = "//button[text()='Importar Baixa']";
        public string selectFundoZitec { get; } = "#select_fundo";
        public string enviarBaixas { get; } = "#fileEnviarBaixas";
        public string btnFecharModal { get; } = "#btnFecharNovoOperacao";
        public string tabelaBaixas { get; } = "#tabelaBaixas";
        public string primeiroTd { get; } = "(//td)[1]";

    }
}
