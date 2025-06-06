using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestePortalGestora.Repositorys
{
    public class PrestadorServico
    {
        public static bool VerificaExistenciaPrestadorServico(string tipoServico, string cep)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Prestadores WHERE Tipo_Servico = @tipoServico AND CEP = @cep";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@tipoServico", SqlDbType.NVarChar).Value = tipoServico;
                        oCmd.Parameters.AddWithValue("@cep", SqlDbType.NVarChar).Value = cep;

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
                Console.WriteLine(e);
            }

            return existe;
        }

        public static bool ApagarPrestadorServico(string tipoServico, string cep)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Prestadores WHERE Tipo_Servico = @tipoServico AND CEP = @cep";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@tipoServico", SqlDbType.NVarChar).Value = tipoServico;
                        oCmd.Parameters.AddWithValue("@cep", SqlDbType.NVarChar).Value = cep;

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
                Console.WriteLine(e);
            }

            return apagado;
        }
    }
}
