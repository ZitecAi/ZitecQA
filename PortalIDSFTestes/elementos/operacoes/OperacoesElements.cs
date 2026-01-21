namespace PortalIDSFTestes.elementos.operacoes
{
    public class OperacoesElements
    {
        public string MenuOperacoes { get; } = "(//p[text()='Operações'])[2]";
        public string PaginaOperacoes { get; } = "//a[@href='/Operacoes/Operacoes2.0.aspx']";

        //Feat 7982
        public string BtnNovaOperacaoCNAB { get; } = "//span[text()='Nova Operação - CNAB']";
        public string BtnNovaOperacaoCSV { get; } = "//button//span[text()='Nova Operação - CSV']";
        public string SelectFundo { get; } = "#selectFundo";
        public string SelectFundoCSV { get; } = "#selectFundoCsv";
        public string EnviarOperacaoInput { get; } = "#fileEnviarOperacoes";
        public string EnviarOperacaoInputCSV { get; } = "#fileEnviarOperacoesCsv";
        public string InputEnviarLastro { get; } = "#fileEnviarLastro";
        public string CampoObservacao { get; } = "#observacaoOperacao";
        public string BtnEnviarOperacaoCSV { get; } = "//div[@class='modal-footer']//button[text()='Confirmar']";
        public string MsgSucessoRetornada { get; } = "//div[@class='toast toast-success']";
        public string MsgArquivoExcluido { get; } = "";
        public string BtnHistoricoImportacoes { get; } = "//button[@class='btn btn-secondary btn-Historico']";
        public string BarraPesquisaHistorico { get; } = "//input[@aria-controls='tabelaHistorico']";
        public string BarraPesquisaTabela { get; } = "//div[@id='divTabelaCedentes_filter']//input";
        public string CampoPesquisaTabela { get; } = "//label[text()='Pesquisar']";
        public string TabelaHistoricoImportacoes { get; } = "#tabelaHistorico";
        public string BtnDownloadValidacaoMovimento { get; } = "//span//button[@title='Validação Movimento']";
        public string BtnDownloadValidacaoLayout { get; } = "//span//button[@title='Validação Layout']";
        public string BtnDownloadExcel { get; } = "#BtnBaixarExcel";
        public string PrimeiroTdHistorico { get; } = "(//tbody[@id='listaHistorico']//td[@class='dtr-control'])[1]";
        public string PrimeiroTdTabela { get; } = "//td[@class='dtr-control']";
        public string BtnLixeira(string nomeArquivo) => $"//td[contains(normalize-space(.),'{nomeArquivo}')]/ancestor::tr//i[@class='fas fa-trash']";
        public string BtModalAprovacao(string nomeArquivo) => $"//td[contains(normalize-space(.),'{nomeArquivo}')]/ancestor::tr//i[@class='fas fa-check']";
        public string BtnAprovarConsultoria { get; } = "#aprovaOpConsultoriaBtn";
        public string BtnAprovarGestora { get; } = "#aprovaOpGestoraBtn";
        public string BtnConfirmarExclusao { get; } = "#aprovaOpGestoraBtn";
        public string CampoMotivoExcluirArquivo { get; } = "#motivoExcluirArquivo";
        public string BtnConfirmarExclusaoName { get; } = "//button[@name='btnAprovaOpGestora']";
        public string BtnFecharModalOperacaoCnab { get; } = "#btnFecharNovoOperacao";
        public string BtnFecharModalOperacaoCsv { get; } = "#btnFecharNovoOperacaoCsv";
        public string TabelaOperacoes { get; } = "#divTabelaCedentes";
        public string MsgSucesso { get; } = "//div[@class='toast toast-success']";
        public string StatusTabela(string NomeArquivo) => $"//td[contains(text(),'{NomeArquivo}')]/ancestor::tr//td[14]";

        //Feat 7980
        //public string btnNovaOperacaoCNAB { get; } = "//span[text()='Nova Operação - CNAB']";
        //public string selectFundo { get; } = "#selectFundo";
        //public string enviarOperacaoInput { get; } = "#fileEnviarOperacoes";
        //public string msgSucessoRetornada { get; } = "//div[@class='toast toast-success']";
        //public string btnHistoricoImportacoes { get; } = "//button[@class='btn btn-secondary btn-Historico']";
        //public string barraPesquisaHistorico { get; } = "//input[@aria-controls='tabelaHistorico']";
        //public string tabelaHistoricoImportacoes { get; } = "#tabelaHistorico";
        //public string btnDownloadValidacaoMovimento { get; } = "//button[@title='Validação Movimento']";
        //public string btnDownloadValidacaoLayout { get; } = "//button[@title='Validação Layout']";




    }
}
