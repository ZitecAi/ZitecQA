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
    public class NotaPagamento
    {
        public static bool VerificaExistenciaNotaPagamento(string cnpjFundo, string observacao)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM pagamentosNotas WHERE CnpjFundo = @cnpjFundo AND observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@observacao", SqlDbType.NVarChar).Value = observacao;

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

        public static bool ApagarNotaPagamento(string cnpjFundo, string observacao)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM pagamentosNotas WHERE CnpjFundo = @cnpjFundo AND observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@observacao", SqlDbType.NVarChar).Value = observacao;

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
