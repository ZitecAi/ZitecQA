namespace PortalIDSFTestes.data.notas
{
    public class PagamentosNotasData
    {
        public string NomeUsuarioQA { get; } = "qazitec01@gmail.com";
        public string NomeFundoZitec { get; } = "Zitec Tecnologia";
        public string MsgSucessoNotaEnviada { get; } = "Nota Enviada!";
        public string MsgSucessoNotaAtualizadaStatus { get; } = "A nota foi atualizada!";
        public string TipoNota { get; } = "Administracao";
        public string TipoNotaComissao { get; } = "COMISSAO POR ORIGINACAO";
        public string ValorNota { get; } = "10";
        public string FundoZitec { get; } = "54638076000176";
        public string PrestadorGrid { get; } = "410";
        public string ObservacaoPagamento { get; } = "Pagamento Reprovado de teste";
        public string ContaBancaria { get; } = "Banco: 439 Ag: 0001 CC: 45815 - 0 TipoConta: Corrente";
        public static string Observacao { get; } = "Teste Pagamento Nota QA " + new Random().Next(0, 9999);
        public string StatusAguardandoPagamento { get; } = "Aguardando Pagamento.";
        public string StatusReprovadoPelaLiquidacao { get; } = "Reprovado pela Liquidação";


    }
}
