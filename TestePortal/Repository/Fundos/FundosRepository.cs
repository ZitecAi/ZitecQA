using System;
using System.Data;
using System.Data.SqlClient;
using TestePortal.TestePortal.Model;

namespace TestePortal.Repository.Fundos
{
    public class FundosRepository
    {
        public static bool VerificaExistenciaFundo(string cnpjFundo, string nomeFundo)
        {
            bool existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Fundo WHERE CnpjFundo = @cnpjFundo AND NomeFundo = @nomeFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@nomeFundo", SqlDbType.NVarChar).Value = nomeFundo;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "FundosRepository.VerificaExistenciaFundo()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarFundo(string cnpjFundo, string nomeFundo)
        {
            bool apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Fundo WHERE CnpjFundo = @cnpjFundo AND NomeFundo = @nomeFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@nomeFundo", SqlDbType.NVarChar).Value = nomeFundo;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            apagado = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "FundosRepository.ApagarFundo()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }
    }
}
