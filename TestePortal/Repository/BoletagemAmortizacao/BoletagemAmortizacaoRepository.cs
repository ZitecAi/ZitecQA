using System;
using System.Data;
using System.Data.SqlClient;
using TestePortal.TestePortal.Model; // Importa a classe AppSettings

namespace TestePortal.Repository.BoletagemAmortizacao
{
    public class BoletagemAmortizacaoRepository
    {
        private static readonly string connectionString = AppSettings.GetConnectionString("myConnectionString");

        public static bool VerificaExistenciaBoletagemAmortizacao(string nomeCotista, string cpfCotista)
        {
            var existe = false;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "SELECT 1 FROM Amortizacao WHERE NomeCotista = @nomeCotista AND CpfCnpj = @cpfCotista";
                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", nomeCotista);
                        oCmd.Parameters.AddWithValue("@cpfCotista", cpfCotista);

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
                Utils.Slack.MandarMsgErroGrupoDev(
                    e.Message,
                    "BoletagemAmortizacaoRepository.VerificaExistenciaBoletagemAmortizacao()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return existe;
        }

        public static bool ApagarBoletagemAmortizacao(string nomeCotista, string cpfCotista)
        {
            var apagado = false;

            try
            {
                using (var myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Amortizacao WHERE NomeCotista = @nomeCotista AND CpfCnpj = @cpfCotista";
                    using (var oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", nomeCotista);
                        oCmd.Parameters.AddWithValue("@cpfCotista", cpfCotista);

                        apagado = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(
                    e.Message,
                    "BoletagemAmortizacaoRepository.ApagarBoletagemAmortizacao()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return apagado;
        }
    }
}
