namespace PortalIDSFTestes.data.cedentes
{
    public class CedentesData
    {
        public string CnpjCedente { get; set; } = "52.721.175/0001-91";
        public string StatusAprovado { get; set; } = "Aprovado";
        
        // Textos para método Escrever
        public string TextoFundoQA { get; set; } = "FUNDO QA";
        public string TextoAtivacaoCedente { get; set; } = "Teste ativação cedente";
        
        // Mensagens de validação
        public static string MsgStatusAtualizado => "Status atualizado!";
        public static string MsgContratoRecebidoSucesso => "Contrato recebido com sucesso, em breve ele será ativado";
        public static string MsgCedenteExcluidoSucesso => "Cedente excluído com sucesso.";

    }
}