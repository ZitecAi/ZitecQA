using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idsf.Automatizacao.DownloadCarteira
{
    public class RegistroDeLogs
    {
        public static void InserirLogBritechCarteira(string funcao, int? idCarteira, DateTime? dataCarteira, int sequencial)
        {
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(@"
                    INSERT INTO TB_LOG_BRITECH_CARTEIRA (DataLog, Funcao, IdCarteira, DataCarteira, Sequencial)
                    VALUES (@DataLog, @Funcao, @IdCarteira, @DataCarteira, @Sequencial)", connection))
                    {
                        command.Parameters.AddWithValue("@DataLog", DateTime.Now);
                        command.Parameters.AddWithValue("@Funcao", funcao ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IdCarteira", idCarteira ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DataCarteira", dataCarteira ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Sequencial", sequencial);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao inserir log: {ex.Message}");
                // Aqui você pode logar o erro de outra forma, se preferir
            }
        }
    }

}
