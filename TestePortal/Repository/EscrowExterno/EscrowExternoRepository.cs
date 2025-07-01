using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortal.TestePortal.Model;

namespace TestePortal.Repository.EscrowExterno
{
    public class EscrowExternoRepository
    {
        public static bool VerificarExistenciaEscrowExterno(string contrato, string titularDestino)
        {
            bool existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Movimentacoes_EscrowExterno Where CONTRATO = 'testeQA' AND TITULAR_DESTINO = 'testeQA';";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@contrato", SqlDbType.NVarChar).Value = contrato;
                        oCmd.Parameters.AddWithValue("@titularDestino", SqlDbType.NVarChar).Value = titularDestino;

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

        public static bool ApagarMovimentacaoEscrowExterno(string contrato, string titularDestino)
        {
            var apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Movimentacoes_EscrowExterno Where CONTRATO = @contrato AND TITULAR_DESTINO = @titularDestino;";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@contrato", SqlDbType.NVarChar).Value = contrato;
                        oCmd.Parameters.AddWithValue("@titularDestino", SqlDbType.NVarChar).Value = titularDestino;

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
