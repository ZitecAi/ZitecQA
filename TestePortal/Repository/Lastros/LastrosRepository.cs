using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TestePortal.Repository.Lastros
{
    public class LastrosRepository
    {
        public static bool VerificaExistenciaLastros(string cnpjFundo, string observacao)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM lastros WHERE CnpjFundo = @cnpjFundo AND Observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = observacao;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                existe = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "LastrosRepository.VerificaExistenciaLastros()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarLastros(string cnpjFundo, string observacao)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM lastros WHERE CnpjFundo = @cnpjFundo AND Observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = observacao;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        apagado = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "LastrosRepository.ApagarLastros()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }
    }
}
