using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortal.TestePortal.Model; // Garante acesso ao AppSettings

namespace TestePortal.Repository.ConciliacaoExtrato
{
    public class ConciliacaoRepository
    {
        public static int ObterIdConciliacao(int idCarteira, decimal valorEmAberto, string descricao)
        {
            int idConciliacao = 0;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = @"
                    SELECT id 
                    FROM TB_CONCILIACAO 
                    WHERE ID_CARTEIRA = @idCarteira 
                      AND VALOR_EM_ABERTO = @valorEmAberto 
                      AND DESCRICAO = @descricao";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCarteira", SqlDbType.Int).Value = idCarteira;
                        oCmd.Parameters.AddWithValue("@valorEmAberto", SqlDbType.Decimal).Value = valorEmAberto;
                        oCmd.Parameters.AddWithValue("@descricao", SqlDbType.NVarChar).Value = descricao;

                        object result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            idConciliacao = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConciliacaoRepository.ObterIdConciliacao()", "Automações Jessica", e.StackTrace);
            }

            return idConciliacao;
        }

        public static List<int> ObterIdsNaoConciliados(int idCarteira)
        {
            var idsConciliacao = new List<int>();

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = @"
                    SELECT id 
                    FROM TB_CONCILIACAO 
                    WHERE ID_CARTEIRA = @idCarteira 
                    AND STATUS_VALOR = 'NAO_CONCILIADO'";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCarteira", SqlDbType.Int).Value = idCarteira;

                        using (SqlDataReader reader = oCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idsConciliacao.Add(reader.GetInt32(0));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConciliacaoRepository.ObterIdsNaoConciliados()", "Automações Jessica", e.StackTrace);
            }

            return idsConciliacao;
        }

        public static bool VerificarIdsConciliados(List<int> ids)
        {
            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    foreach (var id in ids)
                    {
                        string query = @"
                        SELECT STATUS 
                        FROM TB_CONCILIACAO 
                        WHERE ID = @id";

                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                            object status = oCmd.ExecuteScalar();

                            if (status == null || !status.ToString().Equals("CONCILIADO", StringComparison.OrdinalIgnoreCase))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConciliacaoRepository.VerificarIdsConciliados()", "Automações Jessica", e.StackTrace);
                return false;
            }
        }
    }
}
