using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCedente.Model;

namespace TesteCedente.Repository.Cedentes
{
    public class CedentesRepository
    {
        public static bool VerificaExistenciaCedente(string fundoCnpj, string cedenteCnpj)
        {
            var existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Cedentes WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", SqlDbType.NVarChar).Value = fundoCnpj;
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", SqlDbType.NVarChar).Value = cedenteCnpj;

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
                Console.WriteLine(e);
            }

            return existe;
        }

        public static bool ApagarCedente(string fundoCnpj, string cedenteCnpj)
        {
            var apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Cedentes WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", SqlDbType.NVarChar).Value = fundoCnpj;
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", SqlDbType.NVarChar).Value = cedenteCnpj;

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
                Console.WriteLine(e);
            }

            return apagado;
        }

        public static bool CedenteEmFormalizacao(string fundoCnpj, string cedenteCnpj)
        {
            bool emFormalizacao = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT CedenteStatus 
                FROM Cedentes 
                WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", fundoCnpj);
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", cedenteCnpj);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                var status = oReader["CedenteStatus"]?.ToString();
                                emFormalizacao = status != null && status.Equals("FORMALIZACAO", StringComparison.OrdinalIgnoreCase);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar status do cedente: {e.Message}");
            }

            return emFormalizacao;
        }


        public static bool CedenteAtivo(string fundoCnpj, string cedenteCnpj)
        {
            bool emFormalizacao = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT CedenteStatus 
                FROM Cedentes 
                WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", fundoCnpj);
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", cedenteCnpj);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                var status = oReader["CedenteStatus"]?.ToString();
                                emFormalizacao = status != null && status.Equals("ATIVO", StringComparison.OrdinalIgnoreCase);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar status do cedente: {e.Message}");
            }

            return emFormalizacao;
        }

        public static bool CedenteCadastrodoZCust(string nuCpfCnpj, string dsEmail)
        {
            bool cadastrado = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT 1
                FROM tb_fundo_cedente 
                WHERE NU_CPF_CNPJ = @nuCpfCnpj AND DS_EMAIL = @dsEmail";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nuCpfCnpj", nuCpfCnpj);
                        oCmd.Parameters.AddWithValue("@dsEmail", dsEmail); // Corrigido: o parâmetro estava escrito com nome errado

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                cadastrado = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar cadastro do cedente: {e.Message}");
            }

            return cadastrado;
        }

    }
}
