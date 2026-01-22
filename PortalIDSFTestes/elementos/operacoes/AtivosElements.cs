namespace PortalIDSFTestes.elementos.operacoes
{
    public class AtivosElements
    {
        public string MenuOperacoes { get; } = "(//p[text()='Operações'])[2]";
        public string PaginaAtivos { get; } = "//a[@href='/Operacoes/Contratos.aspx']";
        public string BtnNovoAtivo { get; } = "//span[text()='+ Novo']";
        public string SelectFundo { get; } = "#Fundos";
        public string CampoNomeCedente { get; } = "#nomeCedente";
        public string CampoCpfPjCedente { get; } = "#cpfCnpjCedente";
        public string SelectTipoContrato { get; } = "#tipoNota";
        public string CampoAgencia { get; } = "#AgenciaAtivos";
        public string CampoConta { get; } = "#ContaAtivos";
        public string CampoRazSocialDestinatario { get; } = "#RazSocDestino";
        public string CampoCpfPj { get; } = "#CpfCnpjAtivos";
        public string CampoValor { get; } = "#valorAtivos";
        public string CampoResumoOperacao { get; } = "#mensagemObservacao";
        public string BtnAnexos { get; } = "#submitButton";
        public string InputAnexos { get; } = "//form[@id='FormAnexo0']//input[@id='filePagamentosNotas']";
        public string BtnVoltar { get; } = "#salvar";
        public string CheckBoxTermo { get; } = "#termoRespCheck";
        public string BtnSalvar { get; } = "#salvarAnexos";
        public string MsgSucessoRetornada { get; } = "//div[@class='toast toast-success']//div[@class='toast-message']";
        public string BtnExcel { get; } = "//span[text()='Excel']";
        public string BarraPesquisa { get; } = "//label[text()='Pesquisar']";
        public string TabelaAtivos { get; } = "#tabelaContratos";
        public string PrimeiroTd { get; } = "(//td[@class='dtr-control sorting_1'])[1]";
        public string BtnBaixarArquivo { get; } = "(//button[@title='Baixar arquivos'])[1]";
        public string BtnEmAnalise2(string nomeAtivo, string posicao) => $"(//td[text()='{nomeAtivo}']/ancestor::tr//button//strong[text()='Análise'])[{posicao}]";


        public string BtnAprovado { get; } = "//label[@for='option-1']";
        public string BtnReprovado { get; } = "//label[@for='option-2']";
        public string CampoObservacaoParecer { get; } = "#msgParecerContrato";
        public string BtnAprovadoGestora { get; } = "#submitButtonGestora";
        public string TotalAtivos { get; } = "//p[text()='Total de Ativos']/ancestor::div[@class='totalInvest']//div//p";
        public string StatusTabela(string status, string posicao) => $"(//strong[text()='{status}'])[{posicao}]";
    }
}
