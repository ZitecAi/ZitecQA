using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortalConsultoria.Repository.Cedentes
{
    public class CedentesRepository
    {
        public static bool VerificaExistenciaCedente(string fundoCnpj, string cedenteCnpj)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Cedentes WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", SqlDbType.NVarChar).Value = fundoCnpj;
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", SqlDbType.NVarChar).Value = cedenteCnpj;

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
                Console.WriteLine(e.Message);
            }

            return existe;
        }

        public static bool ApagarCedente(string fundoCnpj, string cedenteCnpj)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Cedentes WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", SqlDbType.NVarChar).Value = fundoCnpj;
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", SqlDbType.NVarChar).Value = cedenteCnpj;

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
                Console.WriteLine(e.Message);
            }

            return apagado;
        }

    }
}
