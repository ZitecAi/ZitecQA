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
        public string ObservacaoPagamento { get; } = "#observacoesPagamentos";
        public string BtnEnviarNota { get; } = "#submitButton";

        public string BtnAguardandoAprovacaoCustodia(string observacao) => $"(//td[text()='{observacao}']/ancestor::tr//strong[text()='Aguardando Aprovação da Custódia'])[1]";
        public string BtnAguardandoPagamento(string observacao) => $"(//td[text()='{observacao}']/ancestor::tr//strong[text()='Aguardando Pagamento.'])[1]";
        public string StatusTabela(string observacao) => $"//td[text()='{observacao}']/ancestor::tr/td[10]";
        public string BtnAprovado { get; } = "(//span[text()='Aprovado'])[1]";
        public string BtnReprovado { get; } = "(//span[text()='Reprovado'])[1]";
        public string BtnReprovarNota { get; } = "#reprovarButton";
        public string BtnSubmitStatus { get; } = "#statusButton";
        public string BarraDePesquisa { get; } = "//label//input[@type='search']";
        public string TabelaNotas { get; } = "#tabelaNotas";

    }
}
