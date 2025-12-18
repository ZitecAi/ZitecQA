namespace PortalIDSFTestes.data.boletagem
{
    public class AporteData
    {
        // Textos para método Escrever
        public string ValorCota { get; set; } = "1000";
        public string DescricaoAprovacao { get; set; } = "Aprovado";
        public string CpfCotista { get; set; } = "496.248.668-30";
        public string NomeFundo { get; set; } = "Zitec Tecnologia LTDA";
        public string CnpjFundo { get; set; } = "54.638.076/0001-76";

        public static string uniqueNumber = new Random().Next(1, 9999).ToString();
        public string NomeCotista { get; set; } = $"Cotista Zitec {AporteData.uniqueNumber}";



        // Mensagens de validação
        public static string MsgBoletaRecebidaSucesso = "Boleta recebida com sucesso!";
        public static string MsgAprovadoCustodiaSucesso = "Aprovado pela Custódia com sucesso!";
        public static string MsgReprovadoCustodiaSucesso = "Nota reprovada pela área de Custódia.";
        public static string MsgAprovadoEscrituracaoSucesso = "Nota aprovada pela Escrituração com sucesso!";
        public static string MsgReprovadoEscrituracaoSucesso = "Nota reprovada pela área da Escrituracao.";
        public static string MsgAprovadoControladoriaSucesso = "Aprovado pela Controladoria com sucesso!";
        public static string MsgReprovadoControladoriaSucesso = "Nota reprovada pela área da Controladoria.";

    }
}