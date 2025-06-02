using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using TestePortalGestora.Utils;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestePortalGestora.Repositorys
{
    public class Ativos
    {
        public static bool VerificaExistenciaAtivos(string fundo, string observacoes)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM contratos WHERE Fundo = @fundo AND OBservacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.AddWithValue("@observacoes", SqlDbType.NVarChar).Value = observacoes;

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

        public static bool ApagarAtivos(string fundo, string observacoes)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM contratos WHERE Fundo = @fundo AND OBservacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.AddWithValue("@observacoes", SqlDbType.NVarChar).Value = observacoes;

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
                //Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.ApagarAtivos()", "Automações Jessica", e.StackTrace);
                Console.WriteLine(e);
            }

            return apagado;
        }

        public static bool statusAgrAss(string fundo, string observacoes)
        {
            bool statusAguardandoAssinatura = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT status FROM Contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.AddWithValue("@observacoes", SqlDbType.NVarChar).Value = observacoes;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                if (oReader["status"].ToString() == "AGUARDANDO_ASSINATURAS")
                                {
                                    statusAguardandoAssinatura = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return statusAguardandoAssinatura;
        }

        public static int RetornaIdAtivo(string fundo, string observacoes)
        {
            int idAtivo = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT id FROM Contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.AddWithValue("@observacoes", SqlDbType.NVarChar).Value = observacoes;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                idAtivo = Convert.ToInt32(oReader["id"]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idAtivo;
        }

        public static string ObterDocAutentique(int idAtivo)
        {
            string idDocumentoAutentique = null;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT TOP 1 ID_DOCUMENTO_AUTENTIQUE FROM dbo.Ativos_Autentique_Enviados WHERE ID_ATIVO = @idAtivo";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idAtivo", idAtivo);

                        var result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            idDocumentoAutentique = result.ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idDocumentoAutentique;
        }

        public static bool statusAprovado(string fundo, string observacoes)
        {
            bool statusAguardandoLiquidacao = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT status FROM Contratos WHERE Fundo = @fundo AND Observacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.AddWithValue("@observacoes", SqlDbType.NVarChar).Value = observacoes;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                if (oReader["status"].ToString() == "AGUARDANDO_LIQUIDACAO")
                                {
                                    statusAguardandoLiquidacao = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return statusAguardandoLiquidacao;
        }

    }
}
