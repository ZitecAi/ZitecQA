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
    public class Reembolso
    {
        public static bool VerificaExistenciaReembolso(string documento, string titularBanco)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM BancoReembolso WHERE NumeroDocumento = @documento AND TitularBanco = @titularBanco";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@documento", SqlDbType.NVarChar).Value = documento;
                        oCmd.Parameters.AddWithValue("@titularBanco", SqlDbType.NVarChar).Value = titularBanco;

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

        public static bool ApagarReembolso(string documento, string titularBanco)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM BancoReembolso WHERE NumeroDocumento = @documento AND TitularBanco = @titularBanco";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@documento", SqlDbType.NVarChar).Value = documento;
                        oCmd.Parameters.AddWithValue("@titularBanco", SqlDbType.NVarChar).Value = titularBanco;

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
