using System;
using System.Data;
using System.Data.SqlClient;
using TestePortal.TestePortal.Model; // para acessar AppSettings

namespace TestePortal.Repository.BoletagemAporte
{
    public class BoletagemAporteRepository
    {
        private static readonly string connectionString = AppSettings.GetConnectionString("myConnectionString");

        public static bool VerificaExistenciaBoletagemAporte(string nomeCotista, string tipoCota)
        {
            bool existe = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Boleta WHERE NomeCotista = @nomeCotista AND TipoCota = @tipoCota";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", nomeCotista);
                        oCmd.Parameters.AddWithValue("@tipoCota", tipoCota);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                                existe = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(
                    e.Message,
                    "BoletagemAporteRepository.VerificaExistenciaBoletagemAporte()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return existe;
        }

        public static bool ApagarBoletagemAporte(string nomeCotista, string tipoCota)
        {
            bool apagado = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Boleta WHERE NomeCotista = @nomeCotista AND TipoCota = @tipoCota";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", nomeCotista);
                        oCmd.Parameters.AddWithValue("@tipoCota", tipoCota);

                        apagado = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(
                    e.Message,
                    "BoletagemAporteRepository.ApagarBoletagemAporte()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return apagado;
        }
    }
}
