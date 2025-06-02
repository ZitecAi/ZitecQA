using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortalConsultoria.Repository.ContasEscrows
{
    public class ContasEscrows
    {
        public static bool VerificaExistenciaContasEscrow (string contratoBanco, string titularBanco)
        {
            var existe = false; 

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM bancoEscrow WHERE contratoBanco = @contratoBanco AND titularBanco = @titularBanco";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@titularBanco", SqlDbType.NVarChar).Value = titularBanco;
                        oCmd.Parameters.AddWithValue("@contratoBanco", SqlDbType.NVarChar).Value = contratoBanco;

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
                //Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ContasEscrowRepository.VerificaExistenciaContasEscrow()", "Automações Jessica", e.StackTrace);
                Console.WriteLine(e);
            }

            return existe;
        }

        public static bool ApagarContasEscrow(string contratoBanco, string titularBanco)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM bancoEscrow WHERE contratoBanco = @contratoBanco AND titularBanco = @titularBanco";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@titularBanco", SqlDbType.NVarChar).Value = titularBanco;
                        oCmd.Parameters.AddWithValue("@contratoBanco", SqlDbType.NVarChar).Value = contratoBanco;

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
