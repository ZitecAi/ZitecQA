using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortalInterno.Repositorys
{
    public class InvestidoresFundInvest
    {
        public static bool VerificaExistenciaInvestidores(string cnpj, string email)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM FundoInvestimento_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaExistenciaInvestidores()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarInvestidores(string cnpj, string email)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM FundoInvestimento_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ApagarInvestidores()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }

        public static int ObterIdCotista(string cnpj, string email)
        {
            int idCotista = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = "SELECT Id FROM FundoInvestimento_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;
                        object result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            idCotista = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ObterIdCotista()", "Automações Jessica", e.StackTrace);
            }

            return idCotista;
        }

        public static string ObterToken(string cnpj, string email)
        {
            string token = null;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = "SELECT Token FROM FundoInvestimento_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;
                        var result = oCmd.ExecuteScalar();
                        token = result?.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ObterToken()", "Automações Jessica", e.StackTrace);
            }

            return token;
        }

        public static bool VerificarStatus(string cnpj, string email)
        {
            var emAnalise = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT Status_Cadastro FROM FundoInvestimento_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        var result = oCmd.ExecuteScalar();
                        if (result != null && result.ToString() == "EM_ANALISE")
                        {
                            emAnalise = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificarStatusEmAnalise()", "Automações Jessica", e.StackTrace);
            }

            return emAnalise;
        }



        public static async Task<bool> BaixarArquivo(IPage Page, string botaoId, string nomeArquivo)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadPath, nomeArquivo);
            bool resultadoDownload = false;

            try
            {
                // Aguarda o download após clicar no botão especificado
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator($"#{botaoId}").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 1500
                    });
                });

                // Remove o arquivo se ele já existir na pasta de download
                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Salva o novo download na pasta
                await download.SaveAsAsync(filePath);

                // Verifica se o download foi concluído com sucesso
                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    resultadoDownload = true;

                    // Exclui o arquivo após verificação
                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    resultadoDownload = false;
                    Console.WriteLine("Erro ao baixar o arquivo");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                resultadoDownload = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                resultadoDownload = false;
            }

            return resultadoDownload;
        }
    }
}
