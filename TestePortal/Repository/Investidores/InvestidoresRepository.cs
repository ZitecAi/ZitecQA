using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.Investidores
{
    public class InvestidoresRepository
    {

        public static bool VerificaExistenciaInvestidores(string cpfcnpj, string email)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaExistenciaInvestidores()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarInvestidores(string cpfcnpj, string email)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ApagarInvestidores()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }

        public static int ObterIdCotista(string cpfcnpj, string email)
        {
            int idCotista = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = "SELECT Id FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;
                        object result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            idCotista = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ObterIdCotista()", "Automações Jessica", e.StackTrace);
            }

            return idCotista;
        }

        public static string ObterToken(string cpfcnpj, string email)
        {
            string token = null;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = "SELECT Token FROM Cotista_interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;
                        var result = oCmd.ExecuteScalar();
                        token = result?.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ObterToken()", "Automações Jessica", e.StackTrace);
            }

            return token;
        }

        public static bool VerificarStatus(string cpfcnpj, string email)
        {
            var emAnalise = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT Status_Cadastro FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        var result = oCmd.ExecuteScalar();
                        if (result != null && result.ToString() == "EM_ANALISE")
                        {
                            emAnalise = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificarStatusEmAnalise()", "Automações Jessica", e.StackTrace);
            }

            return emAnalise;
        }

        public static bool VerificaStatusAgdAss(string cpfcnpj, string email)
        {
            bool statusAguardandoAssinatura = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT status FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                      
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                if (oReader["status"].ToString() == "AGUARDANDO_ASSINATURA")
                                {
                                    statusAguardandoAssinatura = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
           
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaStatusAguardandoAssinatura()", "Automações Jessica", e.StackTrace);
            }

            return statusAguardandoAssinatura;
        }

        public static bool VerificaStatusAprovado(string cpfcnpj, string email)
        {
            bool statusAprovado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT status FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                if (oReader["status"].ToString() == "APROVADO")
                                {
                                    statusAprovado = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaStatusAprovado()", "Automações Jessica", e.StackTrace);
            }

            return statusAprovado;
        }

        public static string ObterIdDocumentoAutentique(int idCotista)
        {
            string idDocumentoAutentique = null;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT ID_DOCUMENTO_AUTENTIQUE FROM dbo.Cotista_Interno_Documentos_Enviados WHERE ID_COTISTA = @idCotista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCotista", idCotista);

                        var result = oCmd.ExecuteScalar();

                        if (result != null)
                        {
                            idDocumentoAutentique = result.ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.ObterIdDocumentoAutentique", "Automações Jessica", e.StackTrace);
            }

            return idDocumentoAutentique;
        }

        public static bool UpdateStatusAprovado(string cpfcnpj, string email)
        {
            bool atualizacaoRealizada = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "UPDATE Cotista_Interno SET status = 'APROVADO' WHERE CpfCnpj = @cpfcnpj AND Email = @email";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        atualizacaoRealizada = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.UpdateStatusAprovado()", "Automações Jessica", e.StackTrace);
            }

            return atualizacaoRealizada;
        }


    }


}

