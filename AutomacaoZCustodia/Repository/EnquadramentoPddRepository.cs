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
    public class EnquadramentoPddRepository
    {
      
            public static bool VerificarPdd(string nomePdd)
            {
                bool registroEncontrado = false;

                try
                {
                    var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                    using (SqlConnection myConnection = new SqlConnection(con))
                    {
                        myConnection.Open();

                        string query = "SELECT 1 FROM TB_PDD WHERE NM_PDD = @nomePdd";

                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.Add("@nomePdd", SqlDbType.NVarChar).Value = nomePdd;

                            var resultado = oCmd.ExecuteScalar();

                            registroEncontrado = resultado != null;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao verificar PDD: {e.Message}");
                }

                return registroEncontrado;
            }

            public static bool DeletarPdd(string nomePdd)
            {
                bool deletado = false;

                try
                {
                    var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                    using (SqlConnection myConnection = new SqlConnection(con))
                    {
                        myConnection.Open();

                        string query = "DELETE FROM TB_PDD WHERE NM_PDD = @nomePdd";

                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.Add("@nomePdd", SqlDbType.NVarChar).Value = nomePdd;

                            int rowsAffected = oCmd.ExecuteNonQuery();

                            deletado = rowsAffected > 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao deletar PDD: {e.Message}");
                }

                return deletado;
            }
    }
}

