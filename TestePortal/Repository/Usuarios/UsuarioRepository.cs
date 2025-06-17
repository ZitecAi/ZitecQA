using System;
using System.Data;
using System.Data.SqlClient;
using TestePortal.Utils;
using TestePortal.Model;
using TestePortal.TestePortal.Model;

namespace TestePortal.Repository.Usuarios
{
    public class UsuarioRepository
    {
        public static bool VerificaExistenciaUsuario(string nomeUsuario, string emailUsuario)
        {
            var existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "UsuarioRepository.VerificaExistenciaUsuario()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarUsuario(string nomeUsuario, string emailUsuario)
        {
            var apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Usuarios WHERE Nome = @nomeUsuario AND Email = @emailUsuario";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeUsuario", SqlDbType.NVarChar).Value = nomeUsuario;
                        oCmd.Parameters.AddWithValue("@emailUsuario", SqlDbType.NVarChar).Value = emailUsuario;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        apagado = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "UsuarioRepository.ApagarUsuario()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }
    }
}
