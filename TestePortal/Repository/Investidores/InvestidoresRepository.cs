using System;
using System.Data;
using System.Data.SqlClient;
using TestePortal.TestePortal.Model;

namespace TestePortal.Repository.Investidores
{
    public class InvestidoresRepository
    {
        private static string GetConnection() =>
            AppSettings.GetConnectionString("MyConnectionString");

        public static bool VerificaExistenciaInvestidores(string cpfcnpj, string email)
        {
            var existe = false;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "SELECT * FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            existe = oReader.Read();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaExistenciaInvestidores()", "Automações Jessica", e.StackTrace);
            }
            return existe;
        }

        public static bool ApagarInvestidores(string cpfcnpj, string email)
        {
            var apagado = false;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "DELETE FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        apagado = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ApagarInvestidores()", "Automações Jessica", e.StackTrace);
            }
            return apagado;
        }

        public static int ObterIdCotista(string cpfcnpj, string email)
        {
            int idCotista = 0;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "SELECT Id FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        var result = oCmd.ExecuteScalar();
                        if (result != null) idCotista = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ObterIdCotista()", "Automações Jessica", e.StackTrace);
            }
            return idCotista;
        }

        public static string ObterToken(string cpfcnpj, string email)
        {
            string token = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "SELECT Token FROM Cotista_interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        var result = oCmd.ExecuteScalar();
                        token = result?.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.ObterToken()", "Automações Jessica", e.StackTrace);
            }
            return token;
        }

        public static bool VerificarStatus(string cpfcnpj, string email)
        {
            var emAnalise = false;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "SELECT Status_Cadastro FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        var result = oCmd.ExecuteScalar();
                        emAnalise = result?.ToString() == "EM_ANALISE";
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificarStatusEmAnalise()", "Automações Jessica", e.StackTrace);
            }
            return emAnalise;
        }

        public static bool VerificaStatusAgdAss(string cpfcnpj, string email)
        {
            bool aguardandoAssinatura = false;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "SELECT status FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                                aguardandoAssinatura = oReader["status"].ToString() == "AGUARDANDO_ASSINATURA";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaStatusAguardandoAssinatura()", "Automações Jessica", e.StackTrace);
            }
            return aguardandoAssinatura;
        }

        public static bool VerificaStatusAprovado(string cpfcnpj, string email)
        {
            bool aprovado = false;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "SELECT status FROM Cotista_Interno WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                                aprovado = oReader["status"].ToString() == "APROVADO";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.VerificaStatusAprovado()", "Automações Jessica", e.StackTrace);
            }
            return aprovado;
        }

        public static string ObterIdDocumentoAutentique(int idCotista)
        {
            string idDocumento = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "SELECT ID_DOCUMENTO_AUTENTIQUE FROM dbo.Cotista_Interno_Documentos_Enviados WHERE ID_COTISTA = @idCotista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCotista", idCotista);
                        var result = oCmd.ExecuteScalar();
                        if (result != null) idDocumento = result.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.ObterIdDocumentoAutentique", "Automações Jessica", e.StackTrace);
            }
            return idDocumento;
        }

        public static bool UpdateStatusAprovado(string cpfcnpj, string email)
        {
            bool atualizou = false;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(GetConnection()))
                {
                    myConnection.Open();
                    string query = "UPDATE Cotista_Interno SET status = 'APROVADO' WHERE CpfCnpj = @cpfcnpj AND Email = @email";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cpfcnpj", cpfcnpj);
                        oCmd.Parameters.AddWithValue("@email", email);
                        atualizou = oCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "InvestidoresRepository.UpdateStatusAprovado()", "Automações Jessica", e.StackTrace);
            }
            return atualizou;
        }
    }
}
