using System;
using System.Data.SqlClient;
using System.Data;
using TestePortal.Model;
using TestePortal.TestePortal.Model;

namespace TestePortal.Repository.Correntistas
{
    public class CorrentistaRepository
    {
        public static bool VerificaExistenciaCorrentista(string emailCorrentista, string CpfCnpj)
        {
            var existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Correntista WHERE Email = @emailCorrentista AND CPFCNPJ = @CpfCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@emailCorrentista", SqlDbType.NVarChar).Value = emailCorrentista;
                        oCmd.Parameters.AddWithValue("@CPFCNPJ", SqlDbType.NVarChar).Value = CpfCnpj;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.VerificaExistenciaCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarCorrentista(string emailCorrentista, string CpfCnpj)
        {
            var apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Correntista WHERE Email = @emailCorrentista AND CPFCNPJ = @CpfCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@emailCorrentista", SqlDbType.NVarChar).Value = emailCorrentista;
                        oCmd.Parameters.AddWithValue("@CPFCNPJ", SqlDbType.NVarChar).Value = CpfCnpj;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "UsuarioRepository.ApagarCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }

        public static int ObterIdCorrentista(string emailCorrentista, string CpfCnpj)
        {
            int idCorrentista = 0;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT Id FROM Correntista WHERE Email = @emailCorrentista AND CPFCNPJ = @CpfCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@emailCorrentista", SqlDbType.NVarChar).Value = emailCorrentista;
                        oCmd.Parameters.AddWithValue("@CpfCnpj", SqlDbType.NVarChar).Value = CpfCnpj;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                idCorrentista = oReader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.ObterIdCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return idCorrentista;
        }

        public static string ObterToken(string emailCorrentista, string cpfCnpj)
        {
            string token = string.Empty;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT Token FROM Correntista WHERE CPFCNPJ = @CpfCnpj AND Email = @EmailCorrentista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@CpfCnpj", SqlDbType.NVarChar).Value = cpfCnpj;
                        oCmd.Parameters.AddWithValue("@EmailCorrentista", SqlDbType.NVarChar).Value = emailCorrentista;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                token = oReader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.ObterToken()", "Automações Jessica", e.StackTrace);
            }

            return token;
        }


        public static bool VerificarStatus(string emailCorrentista, string cpfCnpj)
        {
            bool isEmAnalise = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT Status FROM Correntista WHERE CPFCNPJ = @CpfCnpj AND Email = @EmailCorrentista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@CpfCnpj", SqlDbType.NVarChar).Value = cpfCnpj;
                        oCmd.Parameters.AddWithValue("@EmailCorrentista", SqlDbType.NVarChar).Value = emailCorrentista;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                string status = oReader.GetString(0);
                                if (status == "EM_ANALISE")
                                {
                                    isEmAnalise = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.VerificarStatus()", "Automações Jessica", e.StackTrace);
            }

            return isEmAnalise;
        }

        public static bool statusAgrAss(string cpfcnpj, string email)
        {
            bool statusAguardandoAssinatura = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT status FROM Correntista WHERE CPFCNPJ = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                if (oReader["status"].ToString() == "AGUARDANDO_ASSINATURAS")
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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaStatusCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return statusAguardandoAssinatura;
        }

        public static bool VerificaStatusAgdConta(string cpfcnpj, string email)
        {
            bool statusAprovado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT status FROM Correntista WHERE CPFCNPJ = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                if (oReader["status"].ToString() == "AGUARDANDO_CONTA")
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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaStatusAprovadoCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return statusAprovado;
        }

        public static bool VerificaStsAprovado(string cpfcnpj, string email)
        {
            bool statusAprovado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT status FROM Correntista WHERE CPFCNPJ = @cpfcnpj AND Email = @email";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", SqlDbType.NVarChar).Value = cpfcnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                if (oReader["status"].ToString() == "FINALIZADO")
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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaStatusCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return statusAprovado;
        }

        public static bool ApagarContaBancaria(string conta, string digConta, string tipoConta)
        {
            bool contaApagada = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM ContasCorrentistas WHERE Conta = @conta AND digConta = @digConta AND tipoConta = @tipoConta";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@conta", SqlDbType.NVarChar).Value = conta;
                        oCmd.Parameters.AddWithValue("@digConta", SqlDbType.NVarChar).Value = digConta;
                        oCmd.Parameters.AddWithValue("@tipoConta", SqlDbType.NVarChar).Value = tipoConta;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        contaApagada = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ApagarContaBancaria()", "Automações Jessica", e.StackTrace);
            }

            return contaApagada;
        }

        public static string ObterDocumentosAutentique(int idCorrentista)
        {
            string idDocumentoAutentique = null;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT TOP 1 ID_DOCUMENTO_AUTENTIQUE FROM dbo.Correntista_Autentique_Enviados WHERE ID_CORRENTISTA = @idCorrentista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCorrentista", SqlDbType.Int).Value = idCorrentista;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.ObterDocumentosAutentique", "Automações Jessica", e.StackTrace);
            }

            return idDocumentoAutentique;
        }

        public static bool UpdateStatusAguardandoConta(string email, string cpfcnpj)
        {
            bool atualizacaoRealizada = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "UPDATE Correntista SET status = 'AGUARDANDO_CONTA' WHERE CPFCNPJ = @cpfcnpj AND Email = @email";

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.UpdateStatusAguardandoConta()", "Automações Jessica", e.StackTrace);
            }

            return atualizacaoRealizada;
        }
    }
}