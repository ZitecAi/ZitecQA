using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Idsf.Automatizacao.DownloadCarteira
{
    public class CarteirasDownload
    {
        public int ID { get; set; }
        public string IdCarteira { get; set; }
        public DateTime Data { get; set; }
        public string CnpjFundo { get; set; }
        public int Tentativas { get; set; }
        public status Status { get; set; }
        public int ID_RELATORIO { get; set; }
        public enum status
        {
            NAO_INICIADO,
            EM_PROCESSAMENTO,
            SUCESSO
        }

        public static List<CarteirasDownload> GetCarteirasForDownload()
        {
            List<CarteirasDownload> lstCarteiras = new List<CarteirasDownload>();
            var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

            using (SqlConnection myConnection = new SqlConnection(con))
            {
                myConnection.Open();
                SqlCommand oCmd = new SqlCommand("SELECT * FROM CarteirasDownload Where Status != 'SUCESSO' and Tentativas < 10 and Status != 'PROCESSANDO'  ", myConnection);

                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        CarteirasDownload carteiras = new CarteirasDownload();
                        carteiras.IdCarteira = oReader["IdCarteira"].ToString();
                        carteiras.Data = DateTime.Parse(oReader["Data"].ToString());
                        carteiras.CnpjFundo += oReader["CnpjFundo"].ToString();
                        carteiras.Status = GetStatus(oReader["Status"].ToString());
                        carteiras.Tentativas = (int)oReader["Tentativas"];
                        carteiras.ID_RELATORIO = (int)oReader["ID_RELATORIO"];
                        lstCarteiras.Add(carteiras);
                    }
                }
                myConnection.Close();
            }
            return lstCarteiras;
        }

        public static CarteirasDownload GetCarteira()
        {
            // Definir a string de conexão com o banco de dados
            var connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            CarteirasDownload carteira = new CarteirasDownload();
            // Nome da stored procedure
            string storedProcedureName = "GetDownloadCarteira";
            // Criar a conexão com o banco de dados
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Abrir a conexão
                    connection.Open();

                    // Criar o comando para chamar a stored procedure
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Executar a stored procedure
                        // Se a stored procedure retorna um resultado, você pode usar ExecuteReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Ler os resultados, se necessário
                            while (reader.Read())
                            {
                                carteira.IdCarteira = reader["IdCart"].ToString();
                                carteira.CnpjFundo += reader["CnpjFundo"].ToString();
                                carteira.Data = DateTime.Parse(reader["DataDMA"].ToString());

                                carteira.Status = GetStatus(reader["Status"].ToString());
                                carteira.Tentativas = (int)reader["Tentativas"];
                                carteira.ID_RELATORIO = (int)reader["ID_RELATORIO"];

                                carteira.ID = (int)reader["ID"];
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO]: {ex.Message}");
                }
            }
            return carteira;
        }

        public static void UpdateStatus(string IdCarteira, DateTime DataProcessamento, CarteirasDownload.status newStatus)
        {
            var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString = "Update CarteirasDownload set Status = @newStatus,Tentativas =Tentativas+1  Where IdCarteira = @IdCarteira and Data = @Data";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);

                // oCmd.Parameters.AddWithValue("@CedenteID", 2);
                oCmd.Parameters.AddWithValue("@IdCarteira", IdCarteira);
                oCmd.Parameters.AddWithValue("@Data", DataProcessamento);
                oCmd.Parameters.AddWithValue("@newStatus", newStatus.ToString());

                myConnection.Open();
                try
                {
                    oCmd.CommandType = CommandType.Text;
                    oCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                }
                myConnection.Close();
            }
        }


        public static status GetStatus(string status)
        {
            if (status == CarteirasDownload.status.SUCESSO.ToString())
                return CarteirasDownload.status.SUCESSO;

            if (status == CarteirasDownload.status.EM_PROCESSAMENTO.ToString())
                return CarteirasDownload.status.EM_PROCESSAMENTO;

            return CarteirasDownload.status.NAO_INICIADO;
        }

    }
    public class CarteiraFundo
    {
        public int IdInterno { get; set; }
        public string IdCarteira { get; set; }
        public string Nome { get; set; }
        public string CnpjFundo { get; set; }
        public string Tipo { get; set; }
        public string RelacaoHistoricoCota { get; set; }
        public string EmailsHistoricoCota { get; set; }
        public string EmailsCarteira { get; set; }
        public bool EnviarCarteira { get; set; }
        public bool EnviarHistoricoCota { get; set; }

        public static CarteiraFundo GetCarteiraByIdCarteira(string idCarteira)
        {
            var carteira = new CarteiraFundo();
            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string oString = "select * from carteiraFundo where Id = @id";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    oCmd.Parameters.AddWithValue("@id", idCarteira);
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            carteira.IdCarteira = oReader["Id"].ToString();
                            carteira.CnpjFundo = oReader["CnpjFundo"].ToString();
                            carteira.Nome = oReader["Nome"].ToString();
                            carteira.RelacaoHistoricoCota = oReader["RelacaoHistoricoCota"].ToString();
                            carteira.EmailsHistoricoCota = oReader["EmailsHistoricoCota"].ToString();
                            carteira.Tipo = oReader["Tipo"].ToString();
                            carteira.IdInterno = Int32.Parse(oReader["idInterno"].ToString());
                            carteira.EnviarHistoricoCota = bool.Parse(oReader["EnviarHistoricoCota"].ToString());
                            carteira.EnviarCarteira = bool.Parse(oReader["EnviarCarteira"].ToString());
                            carteira.EmailsCarteira = oReader["EmailsCarteira"].ToString();

                        }
                    }
                }
            }
            catch (Exception e)
            {
                var teste = e;

            }

            return carteira;
        }

    }

}

