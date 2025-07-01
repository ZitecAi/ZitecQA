using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TestePortalExecutavel.TestePortalExecutavel.Model;

namespace TestePortalExecutavel.Repository.Baixas
{
    public class ArquivoBaixas
    {
        private static readonly string connectionString = AppSettings.GetConnectionString("ConnectionZitec");

        public static (bool existe, int idMovimentoAberto) VerificaMovimento(int idRecebivel, int idTipoMovimento, int idFundo)
        {
            bool existe = false;
            int idMovimento = 0;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = @"
                SELECT id_movimento_aberto 
                FROM TB_MOVIMENTO_ABERTO
                WHERE id_recebivel = @idRecebivel 
                  AND id_tipo_movimento = @idTipoMovimento 
                  AND id_fundo = @idFundo";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idRecebivel", idRecebivel);
                        oCmd.Parameters.AddWithValue("@idTipoMovimento", idTipoMovimento);
                        oCmd.Parameters.AddWithValue("@idFundo", idFundo);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                existe = true;
                                idMovimento = oReader["id_movimento_aberto"] != DBNull.Value ? Convert.ToInt32(oReader["id_movimento_aberto"]) : 0;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao consultar TB_MOVIMENTO: {e}");
            }

            return (existe, idMovimento);
        }


        public static bool ExcluirMovimento(int idMovimentoAberto)
        {
            bool sucesso = false;

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_MOVIMENTO_ABERTO WHERE ID_MOVIMENTO_ABERTO = @idMovimentoAberto";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idMovimentoAberto", idMovimentoAberto);

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        sucesso = rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao excluir movimento {idMovimentoAberto}: {e}");
            }

            return sucesso;
        }
    }
}
