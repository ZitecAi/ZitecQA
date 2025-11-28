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
        public string BtnBaixarArquivo { get; } = "(//button[@title='Baixar arquivos'])[2]";
        public string BtnEmAnalise(string posicao) => $"(//button[text()='Análise'])[{posicao}]";
        public string BtnEmAnalise2(string posicao) => $"(//button//strong[text()='Análise'])[{posicao}]";
        public string BtnEmAnaliseRisco(string nomeAtivo) => $"(//td[text()='{nomeAtivo}']/ancestor::tr//button[text()='Análise'])[2]";
        public string BtnEmTDJuridico(string status) => $"//span[text()='Jurídico']/ancestor::li//button[text()='{status}']";
        public string BtnEmTDGestor(string status) => $"//span[text()='Gestor']/ancestor::li//button[text()='{status}']";
        public string BtnEmTDRisco(string status) => $"//span[text()='Risco/Administração']/ancestor::li//button[text()='{status}']";
        public string BtnJuridico(string status) => $"(//th[text()='Jurídico']/ancestor::table//tbody//td)[12]//button[text()='{status}']";
        public string BtnEmAnaliseCadastro(string status) => $"//span[text()='Cadastro']/ancestor::li//button[text()='{status}']";
        public string BtnEmTDCadastro(string status) => $"//span[text()='Cadastro']/ancestor::li//strong[text()='{status}']";

        public string BtnAprovado { get; } = "//label[@for='option-1']";
        public string CampoObservacaoParecer { get; } = "#msgParecerContrato";
        public string BtnAprovadoGestora { get; } = "#submitButtonGestora";
        public string TotalAtivos { get; } = "//p[text()='Total de Ativos']/ancestor::div[@class='totalInvest']//div//p";
        public string StatusTabela(string status, string posicao) => $"(//strong[text()='{status}'])[{posicao}]";
    }
}
