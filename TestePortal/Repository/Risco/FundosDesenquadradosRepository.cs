using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortal.TestePortal.Model;

namespace TestePortal.Repository.Risco
{
    public class FundosDesenquadradosRepository
    {

        public static bool VerificarExistenciaFundoDesenquadrado(string nome, string motivo)
        {
            bool existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Desenquadramento WHERE NOME = @nome AND Motivo = @motivo;";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nome", SqlDbType.NVarChar).Value = nome;
                        oCmd.Parameters.AddWithValue("@motivo", SqlDbType.NVarChar).Value = motivo;


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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return existe;
        }

        public static bool ApagarFundoDesenquadrado(string nome, string motivo)
        {
            var apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Desenquadramento WHERE NOME = @nome AND Motivo = @motivo;";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nome", SqlDbType.NVarChar).Value = nome;
                        oCmd.Parameters.AddWithValue("@motivo", SqlDbType.NVarChar).Value = motivo;


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
