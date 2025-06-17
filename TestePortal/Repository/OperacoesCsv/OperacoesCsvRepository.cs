using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TestePortal.TestePortal.Model;

namespace TestePortal.Repository.OperacoesCsv
{
    public class OperacoesCsvRepository
    {
        public static int ObterIdOperacaoRec(string nomeArquivoEntrada)
        {
            int idOperacaoRecebivel = 0;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.VerificarStatus()", "Automações Jessica", e.StackTrace);
            }

            return idOperacaoRecebivel;
        }

        public static bool VerificarOperacao(int idOperacaoRecebivel)
        {
            bool operacaoExiste = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                    SELECT COUNT(*) 
                    FROM TB_OPERACAO 
                    WHERE ID_OPERACAO_RECEBIVEL = @idOperacaoRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idOperacaoRecebivel", idOperacaoRecebivel);
                        operacaoExiste = Convert.ToInt32(oCmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.VerificarOperacao()", "Automações Jessica", e.StackTrace);
            }

            return operacaoExiste;
        }

        public static (bool recebivelExiste, long idRecebivel) VerificarRecebivel(int idOperacaoRecebivel)
        {
            bool recebivelExiste = false;
            long idRecebivel = 0;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                    SELECT COUNT(*) 
                    FROM TB_RECEBIVEL_COMPLEMENTO 
                    WHERE ID_RECEBIVEL = @idRecebivel";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idRecebivel", idRecebivel);
                        long resultado = Convert.ToInt64(oCmd.ExecuteScalar());
                        complementoExiste = resultado > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.VerificarRecebivelComplemento()", "Automações Jessica", e.StackTrace);
            }

            return complementoExiste;
        }

        public static bool DeletarRecebivelComplemento(long idRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.DeletarRecebivelComplemento()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }



        public static bool DeletarRecebivelLastro(long idRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.DeletarRecebivelLastro()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }

        public static bool DeletarRecebivel(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.DeletarRecebivel()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }

        public static bool DeletarOperacao(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.DeletarOperacao()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }

        public static bool DeletarOperacaoRecebivel(int idOperacaoRecebivel)
        {
            bool sucesso = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.DeletarOperacaoRecebivel()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }

        public static string VerificarStatus(string nomeArquivoEntrada)
        {
            string statusOperacao = string.Empty;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.VerificarStatus()", "Automações Jessica", e.StackTrace);
            }

            return statusOperacao;
        }

    }
}
