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
    public class OperacoesApiRepository
    {
        public static bool VerificaProcessamentoFundo(int idFundo)
        {
            bool sucesso = false;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

            string dataHoje = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Primeiro SELECT para pegar a data atual do fundo
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
                        // Executar sp_ReverterFundo
                        using (SqlCommand cmd = new SqlCommand("exec sp_ReverterFundo @idFundo, @dataHoje, 1", connection))
                        {
                            cmd.Parameters.AddWithValue("@idFundo", idFundo);
                            cmd.Parameters.AddWithValue("@dataHoje", dataHoje);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Executar sp_ProcessarFundo
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
                sucesso = false;
            }

            return sucesso;
        }

        public static (bool existe, int idOperacao) VerificaExistenciaOperacao(string arquivoEntrada)
        {
            bool existe = false;
            int idOperacao = 0; // Valor padrão quando não há um ID válido

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT ID_ARQUIVO FROM TB_ARQUIVO WHERE NM_ARQUIVO_ENTRADA = @arquivoEntrada";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@arquivoEntrada", SqlDbType.NVarChar).Value = arquivoEntrada;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                existe = true;
                                idOperacao = oReader["ID_ARQUIVO"] != DBNull.Value ? Convert.ToInt32(oReader["ID_ARQUIVO"]) : 0;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return (existe, idOperacao);
        }

        public static int ObterIdOperacao(string nomeArquivoEntrada)
        {
            int idOperacao = 0;

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
        SELECT ID_ARQUIVO 
        FROM TB_ARQUIVO 
        WHERE NM_ARQUIVO_ENTRADA = @nomeArquivoEntrada
    )";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nomeArquivoEntrada", SqlDbType.NVarChar).Value = nomeArquivoEntrada;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                // Usando Convert.ToInt32 para garantir que o valor seja convertido corretamente para inteiro
                                idOperacao = Convert.ToInt32(oReader["ID_OPERACAO_RECEBIVEL"]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idOperacao; // Retorna corretamente o ID da operação
        }
        public static int ObterIdCertificadora(int idFundo)
        {
            int idCertificadora = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"SELECT ID_CERTIFICADORA FROM TB_FUNDO_CERTIFICADORA WHERE ID_FUNDO = @idFundo";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@idFundo", SqlDbType.Int).Value = idFundo;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                idCertificadora = Convert.ToInt32(oReader["ID_CERTIFICADORA"]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idCertificadora;
        }

    }
}
