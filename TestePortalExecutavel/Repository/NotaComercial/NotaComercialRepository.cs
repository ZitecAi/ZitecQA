using System;
using System.Data;
using Microsoft.Data.SqlClient;
using TestePortalExecutavel.TestePortalExecutavel.Model;// ou ajuste para o namespace correto do seu AppSettings

namespace TestePortalExecutavel.Repository.NotaComercial
{
    public class NotaComercialRepository
    {
        public static bool VerificaExistenciaNotaComercial(string fundo, string observacoes)
        {
            var existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("MyConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM NC_Operacoes WHERE Fundo = @fundo AND Observacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.Add("@observacoes", SqlDbType.NVarChar).Value = observacoes;

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

        public static bool ApagarNotaComercial(string fundo, string observacoes)
        {
            var apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("MyConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM NC_Operacoes WHERE Fundo = @fundo AND Observacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.Add("@observacoes", SqlDbType.NVarChar).Value = observacoes;

                        apagado = oCmd.ExecuteNonQuery() > 0;
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
