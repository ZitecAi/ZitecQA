using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Idsf.Automatizacao.DownloadCarteira
{
    public class Repository
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public static Fundo GetFundo(string cnpjFundo)
        {
            var fundo = new Fundo();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Fundo WHERE CnpjFundo = @CnpjFundo", connection))
                {
                    command.Parameters.AddWithValue("@CnpjFundo", cnpjFundo);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fundo.Id = (int)reader["Id"];
                            fundo.NomeFundo = reader["NomeFundo"].ToString();
                            fundo.CNPJFundo = reader["CnpjFundo"].ToString(); // Corrigido para "="
                            fundo.Servidor = reader["Server"].ToString();
                            fundo.SlackWebhook = reader["SlackWebhook"].ToString();
                            fundo.SlackBotToken = reader["SlackBotToken"].ToString();
                            fundo.SlackAppToken = reader["SlackAppToken"].ToString();
                            fundo.SlackSigningSecret = reader["SlackSigningSecret"].ToString();
                            fundo.SlackChannelId = reader["SlackChannelId"].ToString();
                            fundo.SlackChannelName = reader["SlackChannelName"].ToString();
                            fundo.ShortNameFundo = reader["ShortNameFundo"].ToString();
                            fundo.GestoraCnpj = reader["GestoraCnpj"].ToString();
                            fundo.ConsultoraCnpj = reader["ConsultoraCnpj"].ToString();
                            fundo.Status = reader["Status"].ToString();
                            fundo.EmailsNotificacao = reader["EmailsNotificacao"].ToString();
                            fundo.EmailsCarteira = reader["emailCarteira"].ToString();
                            fundo.WebHookOperacionais = reader["WebhookOperacionais"].ToString();
                            fundo.SlackChannelIDOperacionais = reader["SlackChannelIDOperacionais"].ToString();
                            fundo.ChannelNameOperacionais = reader["ChannelNameOperacionais"].ToString();
                            fundo.CoGestoraCnpj = reader["CoGestoraCNPJ"].ToString();
                            fundo.CoConsultoraCnpj = reader["CoConsultoraCNPJ"].ToString();

                            fundo.ValorGestora = SafeParseDecimal(reader["ValorGestora"]).GetValueOrDefault();
                            fundo.ValorCoGestora = SafeParseDecimal(reader["ValorCoGestora"]).GetValueOrDefault();
                            fundo.ValorConsultora = SafeParseDecimal(reader["ValorConsultora"]).GetValueOrDefault();
                            fundo.ValorCoConsultora = SafeParseDecimal(reader["ValorCoConsultora"]).GetValueOrDefault();

                            // Campos que estavam comentados no original:
                            // fundo.tipoFundo = reader["tipoFundo"].ToString();
                            // fundo.caractFundo = reader["caractFundo"].ToString();
                            // fundo.tipoInvestidor = reader["tipoInvestidor"].ToString();

                            // var taxaAdministracao = reader["taxaAdministracao"].ToString();
                            // if (!String.IsNullOrEmpty(taxaAdministracao))
                            //     fundo.taxaAdministracao = decimal.Parse(reader["taxaAdministracao"].ToString());

                            // var taxaCustodia = reader["taxaCustodia"].ToString();
                            // if (!String.IsNullOrEmpty(taxaCustodia))
                            //     fundo.taxaCustodia = decimal.Parse(reader["taxaCustodia"].ToString());

                            // var taxaControladoria = reader["taxaControladoria"].ToString();
                            // if (!String.IsNullOrEmpty(taxaControladoria))
                            //     fundo.taxaControladoria = decimal.Parse(reader["taxaControladoria"].ToString());
                        }
                    }
                }
            }

            return fundo;
        }

        public static void UpdateHoraEnvioRelatorioSlack(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("UPDATE CarteirasDownload SET EnvioRelatorio = @horarioEnvio WHERE ID = @id", connection))
                    {
                        command.Parameters.AddWithValue("@horarioEnvio", DateTime.Now);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO UpdateHoraEnvioRelatorioSlack]: {ex}");
            }
        }

        public static void UpdateDataCarteira(string idCarteira)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("UPDATE carteiraFundo SET DataCarteira = @dataCarteira WHERE Id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@dataCarteira", DateTime.Today);
                        command.Parameters.AddWithValue("@id", idCarteira);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO UpdateDataCarteira]: {ex}");
            }
        }

        private static decimal? SafeParseDecimal(object value)
        {
            if (value == DBNull.Value || value == null)
                return null;

            if (decimal.TryParse(value.ToString(), out decimal result))
                return result;

            return null;
        }
    }
}
