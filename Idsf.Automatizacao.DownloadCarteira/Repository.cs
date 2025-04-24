using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Idsf.Automatizacao.DownloadCarteira
{
    public class Repository
    {
        public static Fundo GetFundo(string CnpjFundo)
        {
            Fundo fundo = new Fundo();
            var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

            using (SqlConnection myConnection = new SqlConnection(con))
            {
                myConnection.Open();
                SqlCommand oCmd = new SqlCommand("SELECT * FROM Fundo Where CnpjFundo = @CnpjFundo", myConnection);
                oCmd.Parameters.AddWithValue("@CnpjFundo", CnpjFundo);

                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        fundo.Id = (int)oReader["Id"];
                        fundo.NomeFundo = oReader["NomeFundo"].ToString();
                        fundo.CNPJFundo += oReader["CnpjFundo"].ToString();
                        fundo.Servidor = oReader["Server"].ToString();
                        fundo.SlackWebhook = oReader["SlackWebhook"].ToString();
                        fundo.SlackBotToken = oReader["SlackBotToken"].ToString();
                        fundo.SlackAppToken = oReader["SlackAppToken"].ToString();
                        fundo.SlackSigningSecret = oReader["SlackSigningSecret"].ToString();
                        fundo.SlackChannelId = oReader["SlackChannelId"].ToString();
                        fundo.SlackChannelName = oReader["SlackChannelName"].ToString();
                        fundo.ShortNameFundo = oReader["ShortNameFundo"].ToString();
                        fundo.GestoraCnpj = oReader["GestoraCnpj"].ToString();
                        fundo.ConsultoraCnpj = oReader["ConsultoraCnpj"].ToString();
                        fundo.Status = oReader["Status"].ToString();
                        fundo.EmailsNotificacao = oReader["EmailsNotificacao"].ToString();
                        fundo.EmailsCarteira = oReader["emailCarteira"].ToString();
                        fundo.WebHookOperacionais = oReader["WebhookOperacionais"].ToString();
                        fundo.SlackChannelIDOperacionais = oReader["SlackChannelIDOperacionais"].ToString();
                        fundo.ChannelNameOperacionais = oReader["ChannelNameOperacionais"].ToString();
                        fundo.CoGestoraCnpj = oReader["CoGestoraCNPJ"].ToString();
                        fundo.CoConsultoraCnpj = oReader["CoConsultoraCNPJ"].ToString();

                        var ValorGestora = oReader["ValorGestora"].ToString();
                        if (!String.IsNullOrEmpty(ValorGestora))
                            fundo.ValorGestora = Decimal.Parse(ValorGestora);

                        var ValorCoGestora = oReader["ValorCoGestora"].ToString();
                        if (!String.IsNullOrEmpty(ValorCoGestora))
                            fundo.ValorCoGestora = Decimal.Parse(ValorCoGestora);

                        var ValorConsultora = oReader["ValorConsultora"].ToString();
                        if (!String.IsNullOrEmpty(ValorConsultora))
                            fundo.ValorConsultora = Decimal.Parse(ValorConsultora.ToString());

                        var ValorCoConsultora = oReader["ValorCoConsultora"].ToString();
                        if (!String.IsNullOrEmpty(ValorCoConsultora))
                            fundo.ValorCoConsultora = Decimal.Parse(ValorCoConsultora.ToString());
                        //fundo.tipoFundo = oReader["tipoFundo"].ToString();
                        //fundo.caractFundo = oReader["caractFundo"].ToString();
                        //fundo.tipoInvestidor = oReader["tipoInvestidor"].ToString();

                        //var taxaAdministracao = oReader["taxaAdministracao"].ToString();
                        //if (!String.IsNullOrEmpty(taxaAdministracao))
                        //    fundo.taxaAdministracao = decimal.Parse(oReader["taxaAdministracao"].ToString());

                        //var taxaCustodia = oReader["taxaCustodia"].ToString();
                        //if (!String.IsNullOrEmpty(taxaCustodia))
                        //    fundo.taxaCustodia = decimal.Parse(oReader["taxaCustodia"].ToString());

                        //var taxaControladoria = oReader["taxaControladoria"].ToString();
                        //if (!String.IsNullOrEmpty(taxaControladoria))
                        //    fundo.taxaControladoria = decimal.Parse(oReader["taxaControladoria"].ToString());

                    }
                    myConnection.Close();
                }
            }
            return fundo;
        }

        public static void UpdateHoraEnvioRelatorioSlack(int iD)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    using (SqlCommand oCmd = new SqlCommand("UPDATE CarteirasDownload SET EnvioRelatorio = @horarioEnvio WHERE ID = @id", myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@horarioEnvio", DateTime.Now);
                        oCmd.Parameters.AddWithValue("@id", iD);

                        oCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void UpdateDataCarteira(string idCarteira)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    using (SqlCommand oCmd = new SqlCommand("UPDATE carteiraFundo SET DataCarteira = @dataCarteira WHERE Id = @id", myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@dataCarteira", DateTime.Today);
                        oCmd.Parameters.AddWithValue("@id", idCarteira);

                        oCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
