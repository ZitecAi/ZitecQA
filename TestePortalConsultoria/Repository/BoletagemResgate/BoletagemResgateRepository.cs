using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortalConsultoria.Repository.BoletagemResgate
{
    public class BoletagemResgateRepository
    {

        public static bool VerificaExistenciaBoletagemResgate(string cpfCnpjCotista, string cnpjFundo)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM resgate WHERE CpfCnpjCotista = @cpfCnpjCotista AND CnpjFundo = @cnpjFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfCnpjCotista", SqlDbType.NVarChar).Value = cpfCnpjCotista;
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;

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
                Console.WriteLine(e.ToString());
            }

            return existe;
        }

        public static bool ApagarBoletagemResgate(string cpfCnpjCotista, string cnpjFundo)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM resgate WHERE CpfCnpjCotista = @cpfCnpjCotista AND CnpjFundo = @cnpjFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfCnpjCotista", SqlDbType.NVarChar).Value = cpfCnpjCotista;
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;

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
                Console.WriteLine(e.ToString());
            }

            return apagado;
        }

    }
}
