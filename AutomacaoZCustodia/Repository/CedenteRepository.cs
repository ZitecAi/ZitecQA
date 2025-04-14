using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AutomacaoZCustodia.Repository
{
    public class CedenteRepository
    {

        public static int ObterIdCedente(string nuCpfCnpj, string nomeCedente, int idFundo)
        {
            int idCedente = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT id_cedente FROM tb_fundo_cedente WHERE NU_CPF_CNPJ = @nuCpfCnpj AND NM_CEDENTE = @nomeCedente AND ID_FUNDO = @idFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nuCpfCnpj", SqlDbType.NVarChar).Value = nuCpfCnpj;
                        oCmd.Parameters.AddWithValue("@nomeCedente", SqlDbType.NVarChar).Value = nomeCedente;
                        oCmd.Parameters.AddWithValue("@idFundo", SqlDbType.Int).Value = idFundo;

                        idCedente = Convert.ToInt32(oCmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idCedente;
        }

        public static int ObterIdRepresentante(int idCedente)
        {
            int idRepresentante = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT TOP 1 id_representante FROM tb_assoc_cedente_representante WHERE id_cedente = @idCedente";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCedente", SqlDbType.Int).Value = idCedente;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read() && oReader["id_representante"] != DBNull.Value)
                            {
                                idRepresentante = Convert.ToInt32(oReader["id_representante"]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idRepresentante;
        }



        public static bool ApagarAssoc(int idRepresentante)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM tb_assoc_cedente_representante WHERE id_representante = @idRepresentante";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idRepresentante", SqlDbType.Int).Value = idRepresentante;

                        sucesso = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return sucesso;
        }
        public static bool VerificaExistenciaCedente(string nuCpfCnpj, string nomeCedente, int idFundo)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM tb_fundo_cedente WHERE NU_CPF_CNPJ = @nuCpfCnpj AND NM_CEDENTE = @nomeCedente AND ID_FUNDO = @idFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nuCpfCnpj", SqlDbType.NVarChar).Value = nuCpfCnpj;
                        oCmd.Parameters.AddWithValue("@nomeCedente", SqlDbType.NVarChar).Value = nomeCedente;
                        oCmd.Parameters.AddWithValue("@idFundo", SqlDbType.NVarChar).Value = idFundo;
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

        public static bool ApagarAvalista(int idCedente)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_AVALISTA WHERE ID_CEDENTE = @idCedente";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCedente", SqlDbType.Int).Value = idCedente;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        sucesso = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao apagar avalista: {e.Message}");
            }

            return sucesso;
        }



        public static bool ApagarCedente(string nuCpfCnpj, string nomeCedente, int idFundo)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM tb_fundo_cedente WHERE NU_CPF_CNPJ = @nuCpfCnpj AND NM_CEDENTE = @nomeCedente AND ID_FUNDO = @idFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nuCpfCnpj", SqlDbType.NVarChar).Value = nuCpfCnpj;
                        oCmd.Parameters.AddWithValue("@nomeCedente", SqlDbType.NVarChar).Value = nomeCedente;
                        oCmd.Parameters.AddWithValue("@idFundo", SqlDbType.Int).Value = idFundo;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        sucesso = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return sucesso;
        }
        public static bool ApagarContaCorrente(int idCedente)
        {
            bool sucesso = false;

            try
            {
                // Recuperar a string de conexão do arquivo de configuração
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    // Comando SQL para deletar as contas correntes
                    string query = "DELETE FROM TB_CONTA_CORRENTE WHERE ID_CEDENTE = @idCedente";

                    using (SqlCommand cmd = new SqlCommand(query, myConnection))
                    {
                        // Adicionar parâmetro da consulta
                        cmd.Parameters.AddWithValue("@idCedente", SqlDbType.Int).Value = idCedente;

                        // Executar o comando
                        int rowsAffected = cmd.ExecuteNonQuery();
                        sucesso = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao apagar conta corrente: {e.Message}");
            }

            return sucesso;
        }

        public static bool ApagarEntidadeLigada(int idCedente)
        {
            bool sucesso = false;

            try
            {
                // Recuperar a string de conexão do arquivo de configuração
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    // Comando SQL para deletar as entidades ligadas
                    string query = "DELETE FROM TB_ENTIDADE_LIGADA WHERE ID_CEDENTE = @idCedente";

                    using (SqlCommand cmd = new SqlCommand(query, myConnection))
                    {
                        // Adicionar parâmetro da consulta
                        cmd.Parameters.AddWithValue("@idCedente", SqlDbType.Int).Value = idCedente;

                        // Executar o comando
                        int rowsAffected = cmd.ExecuteNonQuery();
                        sucesso = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao apagar entidade ligada: {e.Message}");
            }

            return sucesso;
        }

    }
}

