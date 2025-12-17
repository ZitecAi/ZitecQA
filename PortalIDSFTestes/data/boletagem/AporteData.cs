namespace PortalIDSFTestes.data.boletagem
{
    public class AporteData
    {
        // Textos para método Escrever
        public string ValorCota { get; set; } = "1000";
        public string DescricaoAprovacao { get; set; } = "Aprovado";
        
        // Mensagens de validação
        public static string MsgBoletaRecebidaSucesso => "Boleta recebida com sucesso!";
        public static string MsgAprovadoCustodiaSucesso => "Aprovado pela Custódia com sucesso!";
        public static string MsgAprovadoEscrituracaoSucesso => "Nota aprovada pela Escrituração com sucesso!";
        public static string MsgAprovadoControladoriaSucesso => "Aprovado pela Controladoria com sucesso!";
        
    }
}