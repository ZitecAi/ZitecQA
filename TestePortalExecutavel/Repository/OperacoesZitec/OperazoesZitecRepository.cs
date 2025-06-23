using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TestePortalExecutavel.TestePortalExecutavel.Model;

namespace TestePortalExecutavel.Repository.OperacoesZitec
{
    public class OperacoesZitecRepository
    {
            private static readonly string connectionString = AppSettings.GetConnectionString("ConnectionZitec");

        public static (bool existe, string idOperacao) VerificaExistenciaOperacao(string arquivoEntrada)
        {
            bool existe = false;
            string idOperacao = string.Empty;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "SELECT ID_ARQUIVO FROM TB_ARQUIVO WHERE NM_ARQUIVO_ENTRADA = @arquivoEntrada";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@arquivoEntrada", arquivoEntrada);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                existe = true;
                                idOperacao = oReader["ID_ARQUIVO"] != DBNull.Value ? oReader["ID_ARQUIVO"].ToString() : string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return (existe, idOperacao);
        }


        public static string VerificarStatus(string nomeArquivoEntrada)
            {
                string statusOperacao = string.Empty;

                try
                {
                    using (SqlConnection myConnection = new SqlConnection(connectionString))
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
                    Console.WriteLine(e.ToString());
                }

                return statusOperacao;
            }

            public static bool ExcluirRemessa(string nomeArquivoEntrada)
            {
                bool sucesso = false;

                try
                {
                    using (SqlConnection myConnection = new SqlConnection(connectionString))
                    {
                        myConnection.Open();

                        string query = @"
                    DELETE FROM TB_STG_REMESSA 
                    WHERE ID_OPERACAO_RECEBIVEL = (
                        SELECT ID_OPERACAO_RECEBIVEL 
                        FROM TB_OPERACAO_RECEBIVEL 
                        WHERE ID_ARQUIVO = (
                            SELECT ID_ARQUIVO 
                            FROM TB_ARQUIVO 
                            WHERE NM_ARQUIVO_ENTRADA = @nomeArquivoEntrada
                        )
                    )";

                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.AddWithValue("@nomeArquivoEntrada", SqlDbType.NVarChar).Value = nomeArquivoEntrada;

                            int rowsAffected = oCmd.ExecuteNonQuery();
                            sucesso = rowsAffected > 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return sucesso;
            }

            public static bool ExcluirTbTed(string nomeArquivoEntrada)
            {
                bool sucesso = false;

                try
                {
                    using (SqlConnection myConnection = new SqlConnection(connectionString))
                    {
                        myConnection.Open();

                        string query = @"
                    DELETE FROM dbo.TB_TED 
                    WHERE ID_OPERACAO_RECEBIVEL = (
                        SELECT ID_OPERACAO_RECEBIVEL 
                        FROM TB_OPERACAO_RECEBIVEL 
                        WHERE ID_ARQUIVO = (
                            SELECT ID_ARQUIVO 
                            FROM TB_ARQUIVO 
                            WHERE NM_ARQUIVO_ENTRADA = @nomeArquivoEntrada
                        )
                    )";

                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.AddWithValue("@nomeArquivoEntrada", SqlDbType.NVarChar).Value = nomeArquivoEntrada;

                            int rowsAffected = oCmd.ExecuteNonQuery();
                            sucesso = rowsAffected > 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return sucesso;
            }

            public static bool ExcluirOperacao(string nomeArquivoEntrada)
            {
                bool sucesso = false;

                try
                {
                    using (SqlConnection myConnection = new SqlConnection(connectionString))
                    {
                        myConnection.Open();

                        string query = @"
                    DELETE FROM TB_OPERACAO_RECEBIVEL 
                    WHERE ID_ARQUIVO = (
                        SELECT ID_ARQUIVO 
                        FROM TB_ARQUIVO 
                        WHERE NM_ARQUIVO_ENTRADA = @nomeArquivoEntrada
                    )";

                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.AddWithValue("@nomeArquivoEntrada", SqlDbType.NVarChar).Value = nomeArquivoEntrada;

                            int rowsAffected = oCmd.ExecuteNonQuery();
                            sucesso = rowsAffected > 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return sucesso;
            }

        public static bool ExcluirOperacaoCertificadora(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = @"DELETE FROM TB_OPERACAO_CERTIFICADORA 
                                     WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", idOperacaoRecebivel);

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        sucesso = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return sucesso;
        }

        public static int ObterIdOperacaoRecebivel(string nomeArquivoEntrada)
        {
            int idOperacaoRecebivel = -1;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = @"
                    SELECT TOP 1 ID_OPERACAO_RECEBIVEL 
                    FROM TB_OPERACAO_RECEBIVEL 
                    WHERE ID_ARQUIVO = (
                        SELECT ID_ARQUIVO 
                        FROM TB_ARQUIVO 
                        WHERE NM_ARQUIVO_ENTRADA = @nomeArquivoEntrada
                    )";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeArquivoEntrada", SqlDbType.NVarChar).Value = nomeArquivoEntrada;

                        using (SqlDataReader reader = oCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                idOperacaoRecebivel = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return idOperacaoRecebivel;
        }

        public static bool ExcluirAvalista(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = @"DELETE FROM TB_AVALISTA_OPERACAO_RECEBIVEL 
                                     WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", SqlDbType.Int).Value = idOperacaoRecebivel;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        sucesso = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return sucesso;
        }

        public static bool VerificaProcessamentoFundo(int idFundo)
        {
            bool sucesso = false;
            string dataHoje = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string queryDataFundo = "SELECT dt_fundo FROM tb_fundo WHERE id_fundo = @idFundo";
                    DateTime dataFundo;

                    using (SqlCommand cmd = new SqlCommand(queryDataFundo, connection))
                    {
                        cmd.Parameters.AddWithValue("@idFundo", idFundo);
                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                            return false;

                        dataFundo = Convert.ToDateTime(result);
                    }

                    if (dataFundo.Date == DateTime.Today)
                    {
                        return true;
                    }
                    else if (dataFundo.Date > DateTime.Today)
                    {
                        using (SqlCommand cmd = new SqlCommand("exec sp_ReverterFundo @idFundo, @dataHoje, 1", connection))
                        {
                            cmd.Parameters.AddWithValue("@idFundo", idFundo);
                            cmd.Parameters.AddWithValue("@dataHoje", dataHoje);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("exec sp_ProcessarFundo @idFundo, @dataHoje, 1", connection))
                        {
                            cmd.Parameters.AddWithValue("@idFundo", idFundo);
                            cmd.Parameters.AddWithValue("@dataHoje", dataHoje);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(queryDataFundo, connection))
                    {
                        cmd.Parameters.AddWithValue("@idFundo", idFundo);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            DateTime novaDataFundo = Convert.ToDateTime(result);
                            sucesso = novaDataFundo.Date == DateTime.Today;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                sucesso = false;
            }

            return sucesso;
        }

        public static bool DeletarArquivoGeral(int idArquivo)
        {
            bool sucesso = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_ARQUIVO_GERAL WHERE ID_ARQUIVO = @idArquivo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idArquivo", SqlDbType.Int).Value = idArquivo;

                        int linhasAfetadas = oCmd.ExecuteNonQuery();
                        sucesso = linhasAfetadas > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return sucesso;
        }
    }
}
