using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestePortalInterno.Repositorys
{
   public class Usuarios
    {
        public static bool VerificaExistenciaUsuario(string nomeUsuario, string emailUsuario)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Usuarios WHERE Nome = @nomeUsuario AND Email = @emailUsuario";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeUsuario", SqlDbType.NVarChar).Value = nomeUsuario;
                        oCmd.Parameters.AddWithValue("@emailUsuario", SqlDbType.NVarChar).Value = emailUsuario;

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
                //Utils.Slack.MandarMsgErroGrupoDev(e.Message, "UsuarioRepository.VerificaExistenciaUsuario()", "Automações Jessica", e.StackTrace);
                Console.WriteLine(e);
            }

            return existe;
        }

        public static bool ApagarUsuario(string nomeUsuario, string emailUsuario)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Usuarios WHERE Nome = @nomeUsuario AND Email = @emailUsuario";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeUsuario", SqlDbType.NVarChar).Value = nomeUsuario;
                        oCmd.Parameters.AddWithValue("@emailUsuario", SqlDbType.NVarChar).Value = emailUsuario;

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
                //Utils.Slack.MandarMsgErroGrupoDev(e.Message, "UsuarioRepository.ApagarUsuario()", "Automações Jessica", e.StackTrace);
                Console.WriteLine(e);
            }

            return apagado;
        }
    }
}
