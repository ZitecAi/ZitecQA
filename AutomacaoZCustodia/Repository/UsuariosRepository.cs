using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace AutomacaoZCustodia.Repository
{
    public class UsuariosRepository
    {

        public static bool VerificarExistenciaUsuarios(string nuCpfCnpj)
        {
            bool registroEncontrado = false;

            try
            {

                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();


                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM TB_SEG_USUARIO WHERE nu_cnpj_cpf = @nuCpfCnpj";


                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nuCpfCnpj", SqlDbType.NVarChar).Value = nuCpfCnpj;


                        var resultado = oCmd.ExecuteScalar();


                        registroEncontrado = resultado != null;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Erro ao verificar ramo de atividade: {e.Message}");
            }

            return registroEncontrado;
        }

        public static bool ApagarUsuario(string nuCpfCnpj)
        {
            bool registroEncontrado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM TB_SEG_USUARIO WHERE nu_cnpj_cpf = @nuCpfCnpj";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@nuCpfCnpj", SqlDbType.NVarChar).Value = nuCpfCnpj;

                        // ExecuteNonQuery retorna o número de linhas afetadas
                        int linhasAfetadas = oCmd.ExecuteNonQuery();

                        // Se pelo menos uma linha foi excluída, retorna true
                        registroEncontrado = linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao excluir usuário: {e.Message}");
            }

            return registroEncontrado;
        }


    }
}
