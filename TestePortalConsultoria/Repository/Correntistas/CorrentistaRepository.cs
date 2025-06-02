using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.Correntistas
{
    public class CorrentistaRepository
    {

        public static bool VerificaExistenciaCorrentista(string emailCorrentista, string CpfCnpj)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return existe;
        }

        public static bool ApagarCorrentista(string emailCorrentista, string CpfCnpj)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return apagado;
        }

        public static int ObterIdCorrentista(string emailCorrentista, string CpfCnpj)
        {
            int idCorrentista = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return idCorrentista;
        }

        public static string ObterToken(string emailCorrentista, string cpfCnpj)
        {
            string token = string.Empty;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return token;
        }

        public static bool VerificarStatus(string emailCorrentista, string cpfCnpj)
        {
            bool isEmAnalise = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return isEmAnalise;
        }

        public static bool statusAgrAss(string cpfcnpj, string email)
        {
            bool statusAguardandoAssinatura = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return statusAguardandoAssinatura;
        }

        public static bool VerificaStatusAgdConta(string cpfcnpj, string email)
        {
            bool statusAprovado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return statusAprovado;
        }

        public static bool VerificaStsAprovado(string cpfcnpj, string email)
        {
            bool statusAprovado = false;

            try
            {
                
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
     
                Console.WriteLine(e.ToString());
            }

            return statusAprovado;
        }

        public static bool ApagarContaBancaria(string conta, string digConta, string tipoConta, int idCorrentista)
        {
            bool contaApagada = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM ContasCorrentistas WHERE Conta = @conta AND digConta = @digConta AND tipoConta = @tipoConta AND idCorrentista = @idCorrentista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@conta", SqlDbType.NVarChar).Value = conta;
                        oCmd.Parameters.AddWithValue("@digConta", SqlDbType.NVarChar).Value = digConta;
                        oCmd.Parameters.AddWithValue("@tipoConta", SqlDbType.NVarChar).Value = tipoConta;
                        oCmd.Parameters.AddWithValue("@idCorrentista", SqlDbType.Int).Value = idCorrentista;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        contaApagada = rowsAffected > 0; 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return contaApagada;
        }

        public static string ObterDocumentosAutentique(int idCorrentista)
        {
            string idDocumentoAutentique = null;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT TOP 1 ID_DOCUMENTO_AUTENTIQUE FROM dbo.Correntista_Autentique_Enviados WHERE ID_CORRENTISTA = @idCorrentista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCorrentista", idCorrentista);

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
                Console.WriteLine(e.ToString());
            }

            return idDocumentoAutentique;
        }

        public static bool UpdateStatusAguardandoConta(string email, string cpfcnpj)
        {
            bool atualizacaoRealizada = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

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
                Console.WriteLine(e.ToString());
            }

            return atualizacaoRealizada;
        }



    }
}
