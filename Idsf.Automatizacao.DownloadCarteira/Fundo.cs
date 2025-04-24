using System;

namespace Idsf.Automatizacao.DownloadCarteira
{
    public class Fundo
    {


        public int Id { get; set; }
        public string NomeFundo { get; set; }
        public string Servidor { get; set; }

        public string CNPJFundo { get; set; }

        public string SlackWebhook { get; set; }

        public string SlackBotToken { get; set; }

        public string SlackAppToken { get; set; }

        public string SlackSigningSecret { get; set; }

        public string SlackChannelId { get; set; }

        public string SlackChannelName { get; set; }
        public string ShortNameFundo { get; set; }
        public string GestoraCnpj { get; set; }
        public string GestoraNome { get; set; }
        public decimal ValorGestora { get; set; }
        public string CoGestoraCnpj { get; set; }
        public string CoGestoraNome { get; set; }
        public decimal ValorCoGestora { get; set; }
        public string ConsultoraCnpj { get; set; }
        public string ConsultoraNome { get; set; }
        public decimal ValorConsultora { get; set; }
        public string CoConsultoraCnpj { get; set; }
        public string CoConsultoraNome { get; set; }
        public decimal ValorCoConsultora { get; set; }
        public string Status { get; set; }
        public string EmailsNotificacao { get; set; }
        public string EmailsCarteira { get; set; }
        public string WebHookOperacionais { get; set; }
        public string SlackChannelIDOperacionais { get; set; }
        public string ChannelNameOperacionais { get; set; }
        public decimal PcLimiteMaxReembolsoTarifas { get; set; }
        public decimal VlLimiteReembolso { get; set; }
        public DateTime DataFundo { get; set; }
        public enum tiposFundo
        {
            FIDC,
            FIM,
            FII,
            FIP,
            FUNCINE
        }

        public enum tiposArquivo
        {
            ASSEMBLEIA,
            DEMONS_FINANCEIRA,
            FATOS_RELEVANTES,
            INFORMES_PERIODICOS,
            LAMINAS,
            REGULAMENTO,
            REGULATORIOS,
            COMUNICADO_MERCADO,
            AMORTIZACAO,
            OFERTA_COTAS,
            CARTAO_CNPJ_CVM
        }

        public string tipoFundo { get; set; }
        public string StatusFundo { get; set; }
        public string caractFundo { get; set; }
        public string tipoInvestidor { get; set; }
        public decimal taxaAdministracao { get; set; }
        public decimal taxaCustodia { get; set; }
        public decimal taxaControladoria { get; set; }
        public string nomeArquivo { get; set; }


    }
}
