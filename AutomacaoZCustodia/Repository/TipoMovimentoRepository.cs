using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace AutomacaoZCustodia.Repository
{
    public class TipoMovimentoRepository
    {

        public static bool VerificarExistenciaTpMov(string nmTipoMovimento, string cdMovimentacao)
        {
            bool registroEncontrado = false;

            try
            {

                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();


                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM TB_TIPO_MOVIMENTO WHERE NM_TIPO_MOVIMENTO = @nmTipoMovimento and CD_TIPO_MOVIMENTACAO =  @cdMovimentacao";


                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nmTipoMovimento", SqlDbType.NVarChar).Value = nmTipoMovimento;
                        oCmd.Parameters.Add("@cdMovimentacao", SqlDbType.NVarChar).Value = cdMovimentacao;


                        var resultado = oCmd.ExecuteScalar();


                        registroEncontrado = resultado != null;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Erro: {e.Message}");
            }

            return registroEncontrado;
        }

        public static bool ApagarTpMov(string nmTipoMovimento, string cdMovimentacao)
        {
            bool registroEncontrado = false;

            try
            {

                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();


                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_TIPO_MOVIMENTO WHERE NM_TIPO_MOVIMENTO = @nmTipoMovimento and CD_TIPO_MOVIMENTACAO =  @cdMovimentacao";


                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nmTipoMovimento", SqlDbType.NVarChar).Value = nmTipoMovimento;
                        oCmd.Parameters.Add("@cdMovimentacao", SqlDbType.NVarChar).Value = cdMovimentacao;


                        var resultado = oCmd.ExecuteScalar();


                        registroEncontrado = resultado != null;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Erro: {e.Message}");
            }

            return registroEncontrado;
        }
    }
}
