using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Sql;
using Microsoft.Data.SqlClient;

namespace AutomacaoZCustodia.Repository
{
    public class RamoAtividadeRepository
    {

        public static bool VerificarRamoAtividade(string nomeRamoAtividade)
        {
            bool registroEncontrado = false;

            try
            {
               
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

               
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT 1 FROM TB_RAMO_ATIVIDADE WHERE NM_RAMO_ATIVIDADE = @nomeRamoAtividade";

               
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nomeRamoAtividade", SqlDbType.NVarChar).Value = nomeRamoAtividade;

                
                        var resultado = oCmd.ExecuteScalar();

                        
                        registroEncontrado = resultado != null;
                    }
                }
            }
            catch (Exception e)
            {
    
                Console.WriteLine($"Erro ao verificar ramo de atividade: {e.Message}");
            }

            return registroEncontrado;
        }

        public static bool ApagarRamoAtividade(string nomeRamoAtividade)
        {
            bool registroEncontrado = false;

            try
            {

                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();


                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_RAMO_ATIVIDADE WHERE NM_RAMO_ATIVIDADE = @nomeRamoAtividade";


                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nomeRamoAtividade", SqlDbType.NVarChar).Value = nomeRamoAtividade;


                        var resultado = oCmd.ExecuteScalar();


                        registroEncontrado = resultado != null;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Erro ao verificar ramo de atividade: {e.Message}");
            }

            return registroEncontrado;
        }
    }
}
