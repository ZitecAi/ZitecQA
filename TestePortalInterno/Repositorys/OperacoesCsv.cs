using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestePortalInterno.Repositorys
{
    public class OperacoesCsv
    {
        public static int ObterIdOperacaoRec(string nomeArquivoEntrada)
        {
            int idOperacaoRecebivel = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            SELECT ID_OPERACAO_RECEBIVEL 
FROM TB_OPERACAO_RECEBIVEL 
WHERE ID_ARQUIVO = (
    SELECT TOP 1 ID_ARQUIVO 
    FROM TB_ARQUIVO 
    WHERE NM_ARQUIVO_ENTRADA = @nomeArquivoEntrada
    ORDER BY ID_ARQUIVO DESC
);";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeArquivoEntrada", nomeArquivoEntrada);
                        Console.WriteLine("Valor do parâmetro nomeArquivoEntrada: " + nomeArquivoEntrada);
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                idOperacaoRecebivel = oReader.GetInt32(oReader.GetOrdinal("ID_OPERACAO_RECEBIVEL"));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idOperacaoRecebivel;
        }

        public static bool VerificarOperacao(int idOperacaoRecebivel)
        {
            bool operacaoExiste = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            SELECT *
            FROM TB_OPERACAO 
            WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", idOperacaoRecebivel);

                        operacaoExiste = (int)oCmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return operacaoExiste;
        }

        public static (bool recebivelExiste, long idRecebivel) VerificarRecebivel(int idOperacaoRecebivel)
        {
            bool recebivelExiste = false;
            long idRecebivel = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            SELECT ID_RECEBIVEL 
            FROM TB_RECEBIVEL 
            WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", idOperacaoRecebivel);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                recebivelExiste = true;
                                idRecebivel = oReader.GetInt64(oReader.GetOrdinal("ID_RECEBIVEL"));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.VerificarRecebivel()", "Automações Jessica", e.StackTrace);
            }

            return (recebivelExiste, idRecebivel);
        }

        public static bool VerificarRecebivelComplemento(long idRecebivel)
        {
            bool complementoExiste = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
    SELECT COUNT(*) 
    FROM TB_RECEBIVEL_COMPLEMENTO 
    WHERE ID_RECEBIVEL = @idRecebivel"; // Usando COUNT(*) para retornar um valor numérico

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idRecebivel", idRecebivel);

                        // Convertendo o valor de ExecuteScalar para long
                        long resultado = Convert.ToInt64(oCmd.ExecuteScalar());

                        complementoExiste = resultado > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return complementoExiste;
        }


        public static bool DeletarRecebivelComplemento(long idRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            DELETE FROM TB_RECEBIVEL_COMPLEMENTO 
            WHERE ID_RECEBIVEL = @idRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idRecebivel", idRecebivel);
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

        public static bool DeletarRecebivelLastro(long idRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            DELETE FROM TB_RECEBIVEL_LASTRO 
            WHERE ID_RECEBIVEL = @idRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idRecebivel", idRecebivel);
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

        public static bool DeletarRecebivel(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            DELETE FROM TB_RECEBIVEL 
            WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", idOperacaoRecebivel);
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

        public static bool DeletarOperacao(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            DELETE FROM TB_OPERACAO 
            WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", idOperacaoRecebivel);
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

        public static bool DeletarOperacaoRecebivel(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
            DELETE FROM TB_OPERACAO_RECEBIVEL 
            WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", idOperacaoRecebivel);
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

        public static string VerificarStatus(string nomeArquivoEntrada)
        {
            string statusOperacao = string.Empty;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                    SELECT ST_OPERACAO, * 
                    FROM TB_OPERACAO_RECEBIVEL 
                    WHERE ID_ARQUIVO = (
                        SELECT ID_ARQUIVO 
                        FROM TB_ARQUIVO 
                        WHERE NM_ARQUIVO_ENTRADA = @nomeArquivoEntrada
                    )";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeArquivoEntrada", SqlDbType.NVarChar).Value = nomeArquivoEntrada;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                statusOperacao = oReader["ST_OPERACAO"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                    Console.WriteLine(e);
            }

            return statusOperacao;
        }
    }
}
