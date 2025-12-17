namespace PortalIDSFTestes.data.notaComercial
{
    public class NotaComercialData
    {
        // Textos para método Escrever
        
        // Textos para método Escrever
        public string TomadorNome { get; set; } = "CEDENTE TESTE";
        public string ObservacaoCadastro { get; set; } = "Cadastro Nota Comercial Test";
        public string ValorSolicitado { get; set; } = "10000";
        public string TaxaJurosPadrao { get; set; } = "1";
        public string TaxaJurosNegativa { get; set; } = "20";
        public string DuracaoPadrao { get; set; } = "10";
        public string DuracaoNegativa { get; set; } = "12";
        public string CarenciaAmortizacaoPadrao { get; set; } = "5";
        public string CarenciaAmortizacaoNegativa { get; set; } = "15";
        public string CarenciaAmortizacaoCenario2 { get; set; } = "10";
        public string DiaVencimento { get; set; } = "05";
        public string CarenciaJuros { get; set; } = "25";
        public string TextoPesquisaFundo { get; set; } = "Zitec";
        public string ObservacaoCancelamento { get; set; } = "Teste Cancelamento";
        
        // Textos para método ValidarTextoPresente
        public static string MsgSucessoSimulacao => "Ótimo! Agora você pode visualizar a simulação.";
        public static string MsgSucessoCadastro => "Nota Comercial cadastrada com sucesso";
        public static string MsgPreenchaInfo => "Preencha corretamente a Aba de Informações.";
        public static string MsgPreenchaTodos => "Preencha todos os campos para simular";
        public static string MsgCamposObrigatorios => "Preencha todos os campos obrigatórios";
        public static string MsgCarenciaMaiorDuracao => "A carência de amortização não pode ser igual ou maior que a duração";
        public static string MsgParcelasIncorretas => "Parcelas devem ser geradas corretamente";
        public static string MsgCarenciaJurosMaior => "A carência de juros não pode ser maior que a carência do principal!";
    }
}