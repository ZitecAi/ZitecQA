namespace PortalIDSFTestes.elementos.notas
{
    public class PagamentosNotasElements
    {
        public string MenuNotas { get; } = "(//a[@class='nav-link']//p[text()='Notas'])[1]";
        public string PaginaPagamentosNotas { get; } = "(//p[text()='Pagamentos Notas'])[1]";
        public string BtnNovo { get; } = "//span[text()='Novo +']";
        public string SelectTipoNota { get; } = "(//select[@id='tipoNota'])[1]";
        public string ValorNota { get; } = "#ValorNota";
        public string SelectFundos { get; } = "#Fundos";
        public string SelectPrestadores { get; } = "#Prestadores";
        public string InputEnviarNota { get; } = "#filePagamentosNotas";
        public string ContaBanco { get; } = "#ContaBanco";
        public string Observacao { get; } = "#mensagemObservacao";
        public string BtnEnviarNota { get; } = "#submitButton";


    }
}
