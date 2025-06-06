using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace TestePortalGestora.Repositorys
{
    public class Fundos
    {
        public static bool VerificaExistenciaFundo(string cnpjFundo, string nomeFundo)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Fundo WHERE CnpjFundo = @cnpjFundo AND NomeFundo = @nomeFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@nomeFundo", SqlDbType.NVarChar).Value = nomeFundo;

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

        public static bool ApagarFundo(string cnpjFundo, string nomeFundo)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Fundo WHERE CnpjFundo = @cnpjFundo AND NomeFundo = @nomeFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@nomeFundo", SqlDbType.NVarChar).Value = nomeFundo;

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
