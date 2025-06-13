using System;
using System.Data;
using System.Data.SqlClient;
using TestePortal.TestePortal.Model; // usando a sua classe AppSettings

namespace TestePortal.Repository.Ativos
{
    public class AtivosRepository
    {
        private static readonly string connectionString = AppSettings.GetConnectionString("myConnectionString");

        public static bool VerificaExistenciaAtivos(string fundo, string observacoes)
        {
            var existe = false;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();
                    string query = "SELECT 1 FROM contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";

                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", fundo);
                        oCmd.Parameters.AddWithValue("@observacoes", observacoes);

                        using (var oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                                existe = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.VerificaExistenciaAtivos()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarAtivos(string fundo, string observacoes)
        {
            var apagado = false;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();
                    string query = "DELETE FROM contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";

                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", fundo);
                        oCmd.Parameters.AddWithValue("@observacoes", observacoes);

                        apagado = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.ApagarAtivos()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }

        public static bool StatusAgrAss(string fundo, string observacoes)
        {
            bool aguardando = false;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();
                    string query = "SELECT status FROM Contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";

                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", fundo);
                        oCmd.Parameters.AddWithValue("@observacoes", observacoes);

                        using (var oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read() && oReader["status"].ToString() == "AGUARDANDO_ASSINATURAS")
                                aguardando = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.StatusAgrAss()", "Automações Jessica", e.StackTrace);
            }

            return aguardando;
        }

        public static int RetornaIdAtivo(string fundo, string observacoes)
        {
            int idAtivo = 0;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();
                    string query = "SELECT id FROM Contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";

                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", fundo);
                        oCmd.Parameters.AddWithValue("@observacoes", observacoes);

                        using (var oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                                idAtivo = Convert.ToInt32(oReader["id"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.RetornaIdAtivo()", "Automações Jessica", e.StackTrace);
            }

            return idAtivo;
        }

        public static string ObterDocAutentique(int idAtivo)
        {
            string idDocumento = null;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();
                    string query = "SELECT TOP 1 ID_DOCUMENTO_AUTENTIQUE FROM dbo.Ativos_Autentique_Enviados WHERE ID_ATIVO = @idAtivo";

                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idAtivo", idAtivo);

                        var result = oCmd.ExecuteScalar();
                        if (result != null)
                            idDocumento = result.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.ObterDocAutentique", "Automações Jessica", e.StackTrace);
            }

            return idDocumento;
        }

        public static bool StatusAprovado(string fundo, string observacoes)
        {
            bool aguardandoLiquidacao = false;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();
                    string query = "SELECT status FROM Contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";

                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", fundo);
                        oCmd.Parameters.AddWithValue("@observacoes", observacoes);

                        using (var oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read() && oReader["status"].ToString() == "AGUARDANDO_LIQUIDACAO")
                                aguardandoLiquidacao = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.StatusAprovado()", "Automações Jessica", e.StackTrace);
            }

            return aguardandoLiquidacao;
        }
    }
}
