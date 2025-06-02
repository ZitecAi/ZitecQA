using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.GestoraInterna
{
    public class GestoraInternaRepository
    {

        public static bool VerificaExistenciaGestoraInterna(string cnpj, string email)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Gestora_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "GestoraInternaRepository.VerificaExistenciaGestoraInterna()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarGestoraInterna(string cnpj, string email)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Gestora_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "GestoraInternaRepository.ApagarGestoraInterna()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }

        
            public static int? ObterIdGestora(string cnpj, string email)
            {
                int? idGestora = null;

                try
                {
                    var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                    using (SqlConnection myConnection = new SqlConnection(con))
                    {
                        myConnection.Open();

                        string query = "SELECT id FROM Gestora_Interno WHERE Cnpj = @cnpj AND Email = @email";
                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                            oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                            using (SqlDataReader oReader = oCmd.ExecuteReader())
                            {
                                if (oReader.Read())
                                {
                                    idGestora = Convert.ToInt32(oReader["id"]);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.Slack.MandarMsgErroGrupoDev(e.Message, "GestoraInternaRepository.ObterIdGestoraInterna()", "Automações Jessica", e.StackTrace);
                }

                return idGestora;
            }

        public static string ObterTokenGestora(string cnpj, string email)
        {
            string token = null;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT Token FROM Gestora_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                token = oReader["Token"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "GestoraInternaRepository.ObterTokenGestoraInterna()", "Automações Jessica", e.StackTrace);
            }

            return token;
        }

        public static bool VerificarStatus(string cnpj, string email)
        {
            var emAnalise = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT Status FROM Gestora_Interno WHERE Cnpj = @cnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

                        var result = oCmd.ExecuteScalar();
                        if (result != null && result.ToString() == "EM_ANALISE")
                        {
                            emAnalise = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConsultoriasRepository.VerificarStatus()", "Automações Jessica", e.StackTrace);
            }

            return emAnalise;
        }
    }
}
