using System;
using System.Data;
using System.Data.SqlClient;
using TestePortal.Utils; // Certifique-se que este é o namespace correto da sua classe de configuração

namespace TestePortal.Repository.NotaPagamento
{
    public class NotaPagamentoRepository
    {
        public static bool VerificaExistenciaNotaPagamento(string cnpjFundo, string observacao)
        {
            var existe = false;

            try
            {
                var con = ConfigurationHelper.GetConnectionString("MyConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM pagamentosNotas WHERE CnpjFundo = @cnpjFundo AND observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = observacao;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "NotaPagamentoRepository.VerificaExistenciaNotaPagamento()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarNotaPagamento(string cnpjFundo, string observacao)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationHelper.GetConnectionString("MyConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM pagamentosNotas WHERE CnpjFundo = @cnpjFundo AND observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = observacao;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        apagado = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "NotaPagamentosRepository.ApagarNotaPagamentos()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }
    }
}
