using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using TestePortal.Repository.Operacoes;
using System.Windows.Controls;


namespace TestePortal.Repository.Operacoes
{
    public class OperacoesRepository
    {

        public static bool VerificaExistenciaOperacao(string arquivoEntrada)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["FROMTISPROC.PRODUCAO.ConnectionString"].ToString();////

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM TB_ARQUIVO WHERE NM_ARQUIVO_ENTRADA = @arquivoEntrada";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@arquivoEntrada", SqlDbType.NVarChar).Value = arquivoEntrada;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.VerificaExistenciaOperacoes()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }



        public static string VerificarStatus(string nomeArquivoEntrada)
        {
            string statusOperacao = string.Empty;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["FROMTISPROC.PRODUCAO.ConnectionString"].ToString();

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
        public static bool ExcluirRemessa(string nomeArquivoEntrada)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["FROMTISPROC.PRODUCAO.ConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
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
                        sucesso = rowsAffected > 0; // Se pelo menos uma linha for deletada, o sucesso é verdadeiro
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.ExcluirRemessa()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }
        

        public static bool ExcluirTbTed(string nomeArquivoEntrada)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["FROMTISPROC.PRODUCAO.ConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
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
                        sucesso = rowsAffected > 0; // Se pelo menos uma linha for deletada, o sucesso é verdadeiro
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.ExcluirTbTed()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }

        public static bool ExcluirOperacao(string nomeArquivoEntrada)
        {
            bool sucesso = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["FROMTISPROC.PRODUCAO.ConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
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
                        sucesso = rowsAffected > 0; // Se pelo menos uma linha for deletada, o sucesso é verdadeiro
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.ExcluirOperacao()", "Automações Jessica", e.StackTrace);
            }

            return sucesso;
        }
    }

}



//public async Task<bool> ExcluirOperacaoAsync(IPage Page, string nomeArquivo)
//{

//    try
//    {

//        await Task.Delay(2000);
//        await Page.ReloadAsync(new() { Timeout = 60000 });
//        await Task.Delay(1000);
//        await Page.ReloadAsync();
//        await Page.GetByLabel("Pesquisar").ClickAsync(); 
//        await Task.Delay(2000);
//        await Page.GetByLabel("Pesquisar").FillAsync(nomeArquivo);
//        var primeiroTr = Page.Locator("#listaCedentes tr").First;
//        var primeiroTd = primeiroTr.Locator("td").First;
//        await primeiroTd.ClickAsync();
//        var trElement = Page.Locator("tr.child");
//        var ulElement = trElement.Locator("ul.dtr-details");
//        var excluirButton = ulElement.Locator("button[title='Excluir Arquivo']");
//        await excluirButton.ClickAsync();
//        await Page.Locator("#motivoExcluirArquivo").FillAsync("teste");
//        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();


//        // Verifica se a mensagem de sucesso aparece
//        var successMessageLocator = Page.Locator("text='Arquivo excluído com sucesso!'");
//        bool isMessageVisible = await successMessageLocator.IsVisibleAsync();

//        return isMessageVisible;

//    }
//    catch (TimeoutException ex) {

//        Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
//        Console.WriteLine($"Exceção: {ex.Message}");
//        return false;

//    }

//}
