using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace AutomacaoZCustodia.Repository
{
    public class EntidadeRepository
    {
        public static int ObterIdPessoa(string nomePessoa, string email)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT id_pessoa FROM TB_PESSOA WHERE nm_pessoa = @nomePessoa AND ds_email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomePessoa", SqlDbType.NVarChar).Value = nomePessoa;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        var result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return 0; // Retorna 0 caso não encontre o ID ou em caso de erro
        }


        public static int ObterIdRepresentante(string email, string cpfCnpj)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT id_representante FROM tb_representante WHERE ds_email = @email AND nu_cpf_cnpj = @cpfCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;
                        oCmd.Parameters.AddWithValue("@cpfCnpj", SqlDbType.NVarChar).Value = cpfCnpj;

                        var result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return 0; // Retorna 0 se não encontrar ou em caso de erro
        }


        public static bool DeletarPessoa(string nomePessoa, string email)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_PESSOA WHERE nm_pessoa = @nomePessoa AND ds_email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomePessoa", SqlDbType.NVarChar).Value = nomePessoa;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public static bool DeletarRepresentante(string email, string cpfCnpj)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM tb_representante WHERE ds_email = @email AND nu_cpf_cnpj = @cpfCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;
                        oCmd.Parameters.AddWithValue("@cpfCnpj", SqlDbType.NVarChar).Value = cpfCnpj;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
        public static bool DeletarAssociacoesDoRepresentante(int idRepresentante)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_ASSOC_CEDENTE_REPRESENTANTE WHERE ID_REPRESENTANTE = @idRepresentante";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idRepresentante", idRepresentante);

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0; // true se deletou algo, false se não achou nenhum registro
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool DeletarAssociacoesDaPessoa(int idPessoa)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_ASSOC_PESSOA_REPRESENTANTE WHERE ID_PESSOA = @idPessoa";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idPessoa", SqlDbType.Int).Value = idPessoa;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public static bool DeletarContaCorrente(int idPessoa)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_CONTA_CORRENTE_ORIGINADOR WHERE ID_PESSOA = @idPessoa";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idPessoa", SqlDbType.Int).Value = idPessoa;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public static bool DeletarParteRelacionada(int idPessoa)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_PESSOA_PARTE_RELACIONADA WHERE ID_PESSOA = @idPessoa";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idPessoa", SqlDbType.Int).Value = idPessoa;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


    }
}
