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
    public class TipoRecebivelRepository
    {
        public static bool VerificarExistenciaTpRec(string nmTipoRecebivel)
        {
            bool registroEncontrado = false;

            try
            {

                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();


                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM TB_TIPO_RECEBIVEL WHERE NM_TIPO_RECEBIVEL = @nmTipoRecebivel";


                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nmTipoRecebivel", SqlDbType.NVarChar).Value = nmTipoRecebivel;


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

        public static bool ApagarTipoRecebivel(string nmTipoRecebivel)
        {
            bool registroEncontrado = false;

            try
            {

                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();


                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_TIPO_RECEBIVEL WHERE NM_TIPO_RECEBIVEL = @nmTipoRecebivel";


                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nmTipoRecebivel", SqlDbType.NVarChar).Value = nmTipoRecebivel;


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
