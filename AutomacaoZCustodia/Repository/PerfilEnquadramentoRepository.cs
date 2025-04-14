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
    public class PerfilEnquadramentoRepository
    {
        public static int ObterIdEnqPerfil(string dsEnqPerfil, string nmEnqPerfil)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT ID_ENQ_PERFIL FROM TB_ENQ_PERFIL WHERE DS_ENQ_PERFIL = @dsEnqPerfil AND NM_ENQ_PERFIL = @nmEnqPerfil";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@dsEnqPerfil", SqlDbType.NVarChar).Value = dsEnqPerfil;
                        oCmd.Parameters.AddWithValue("@nmEnqPerfil", SqlDbType.NVarChar).Value = nmEnqPerfil;

                        var result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return -1;
        }


        public static bool DeletarRegraAssociado(int idEnqPerfil)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_ENQ_REGRA_ASSOCIADO WHERE ID_ENQ_PERFIL = @idEnqPerfil";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idEnqPerfil", SqlDbType.Int).Value = idEnqPerfil;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool DeletarEnqPerfil(int idEnqPerfil)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_ENQ_PERFIL WHERE ID_ENQ_PERFIL = @idEnqPerfil";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idEnqPerfil", SqlDbType.Int).Value = idEnqPerfil;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


    }
}
