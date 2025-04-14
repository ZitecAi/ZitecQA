using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace AutomacaoZCustodia.Repository
{
    public class SacadoRepository
    {
        public static (bool existe, int idSacado) VerificaExistenciaSacado(string nuCpfCnpj, string dsEmail)
        {
            bool existe = false;
            int idSacado = 0; 

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT ID_SACADO FROM TB_FUNDO_SACADO WHERE NU_CPF_CNPJ = @nuCpfCnpj AND DS_EMAIL = @dsEmail";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nuCpfCnpj", SqlDbType.NVarChar).Value = nuCpfCnpj;
                        oCmd.Parameters.Add("@dsEmail", SqlDbType.NVarChar).Value = dsEmail;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read()) // Se encontrou um registro
                            {
                                existe = true;
                                idSacado = oReader.GetInt32(0); // Obtém o ID_SACADO
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar existência do sacado: {e.Message}");
            }

            return (existe, idSacado);
        }



        public static bool ApagarSacado(string nuCpfCnpj, string dsEmail)
        {
            bool deletado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_FUNDO_SACADO WHERE NU_CPF_CNPJ = @nuCpfCnpj AND DS_EMAIL = @dsEmail";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nuCpfCnpj", SqlDbType.NVarChar).Value = nuCpfCnpj;
                        oCmd.Parameters.Add("@dsEmail", SqlDbType.NVarChar).Value = dsEmail;

                        // ExecuteNonQuery retorna o número de linhas afetadas
                        int linhasAfetadas = oCmd.ExecuteNonQuery();

                        // Se pelo menos uma linha foi excluída, retorna true
                        deletado = linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao excluir sacado: {e.Message}");
            }

            return deletado;
        }
        public static int ObterIdRepresentante(int idSacado)
        {
            int idRepresentante = 0; // Valor padrão caso não encontre

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT ID_REPRESENTANTE FROM TB_ASSOC_SACADO_REPRESENTANTE WHERE ID_SACADO = @idSacado";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@idSacado", SqlDbType.Int).Value = idSacado;

                        object result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            idRepresentante = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao obter ID do representante: {e.Message}");
            }

            return idRepresentante;
        }

        public static bool DeletarRepresentante(int idRepresentante)
        {
            bool deletado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_REPRESENTANTE WHERE ID_REPRESENTANTE = @idRepresentante";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@idRepresentante", SqlDbType.Int).Value = idRepresentante;

                        int linhasAfetadas = oCmd.ExecuteNonQuery();
                        deletado = linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao excluir representante: {e.Message}");
            }

            return deletado;
        }

        public static bool DeletarAssociacaoSacadoRepresentante(int idSacado)
        {
            bool deletado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_ASSOC_SACADO_REPRESENTANTE WHERE ID_SACADO = @idSacado";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@idSacado", SqlDbType.Int).Value = idSacado;

                        int linhasAfetadas = oCmd.ExecuteNonQuery();
                        deletado = linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao excluir associação do sacado: {e.Message}");
            }

            return deletado;
        }


    }
}
