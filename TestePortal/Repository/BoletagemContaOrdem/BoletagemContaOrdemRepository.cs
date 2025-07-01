using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortal.TestePortal.Model; // para acessar AppSettings

namespace TestePortal.Repository.BoletagemContaOrdem
{
    public class BoletagemContaOrdemRepository
    {
        private static readonly string connectionString = AppSettings.GetConnectionString("myConnectionString");

        public static bool VerificaExistenciaBoletagemContaOrdem(string usuario, string distribuidor)
        {
            bool existe = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM ContaOrdem WHERE Usuario = @usuario AND Distribuidor = @distribuidor";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@usuario", usuario);
                        oCmd.Parameters.AddWithValue("@distribuidor", distribuidor);

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
                    "BoletagemContaOrdemRepository.VerificaExistenciaBoletagemContaOrdem()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return existe;
        }

        public static bool ApagarBoletagemContaOrdem(string usuario, string distribuidor)
        {
            bool apagado = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "DELETE FROM ContaOrdem WHERE Usuario = @usuario AND Distribuidor = @distribuidor";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@usuario", usuario);
                        oCmd.Parameters.AddWithValue("@distribuidor", distribuidor);

                        apagado = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(
                    e.Message,
                    "BoletagemContaOrdemRepository.ApagarBoletagemContaOrdem()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return apagado;
        }

        public static bool ApagarAporteBoletagemContaOrdem(int id)
        {
            bool apagado = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Boleta WHERE idUsuario = @id";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@id", id);
                        

                        apagado = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(
                    e.Message,
                    "AporteBoletagemContaOrdemRepository.ApagarAporteBoletagemContaOrdem()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return apagado;
        }

        public static bool ApagarResgateBoletagemContaOrdem(string email)
        {
            bool apagado = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "DELETE resgate WHERE EmailCadastro = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@email", email);


                        apagado = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(
                    e.Message,
                    "ResgateBoletagemContaOrdemRepository.ApagarResgateBoletagemContaOrdem()",
                    "Automações Jessica",
                    e.StackTrace
                );
            }

            return apagado;
        }




    }
}
