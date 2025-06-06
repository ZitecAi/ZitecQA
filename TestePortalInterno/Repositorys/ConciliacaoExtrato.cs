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
    public class ConciliacaoExtrato
    {
        public static int ObterIdConciliacao(int idCarteira, decimal valorEmAberto, string descricao)
        {
            int idConciliacao = 0;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = @"
                SELECT id 
                FROM TB_CONCILIACAO 
                WHERE ID_CARTEIRA = @idCarteira 
                  AND VALOR_EM_ABERTO = @valorEmAberto 
                  AND DESCRICAO = @descricao";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        // Adiciona os parâmetros ao comando
                        oCmd.Parameters.AddWithValue("@idCarteira", SqlDbType.Int).Value = idCarteira;
                        oCmd.Parameters.AddWithValue("@valorEmAberto", SqlDbType.Decimal).Value = valorEmAberto;
                        oCmd.Parameters.AddWithValue("@descricao", SqlDbType.NVarChar).Value = descricao;

                        object result = oCmd.ExecuteScalar();
                        if (result != null)
                        {
                            idConciliacao = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idConciliacao;
        }

        public static List<int> ObterIdsNaoConciliados(int idCarteira)
        {
            var idsConciliacao = new List<int>();

            try
            {

                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();
                    string query = @"
                    SELECT id 
                    FROM TB_CONCILIACAO 
                    WHERE ID_CARTEIRA = @idCarteira 
                    AND STATUS_VALOR = 'NAO_CONCILIADO'";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@idCarteira", SqlDbType.Int).Value = idCarteira;


                        using (SqlDataReader reader = oCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idsConciliacao.Add(reader.GetInt32(0));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                    Console.WriteLine(e);
            }

            return idsConciliacao;
        }

        public static bool VerificarIdsConciliados(List<int> ids)
        {
            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    foreach (var id in ids)
                    {
                        string query = @"
                SELECT STATUS 
                FROM TB_CONCILIACAO 
                WHERE ID = @id";

                        using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                        {
                            oCmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                            object status = oCmd.ExecuteScalar(); // Retorna o valor da coluna STATUS

                            // Verifica se o status não é "CONCILIADO"
                            if (status == null || !status.ToString().Equals("CONCILIADO", StringComparison.OrdinalIgnoreCase))
                            {
                                return false; // Se encontrar qualquer ID não conciliado, retorna falso
                            }
                        }
                    }
                }

                return true; // Se todos os IDs tiverem o status "CONCILIADO", retorna verdadeiro
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false; // Retorna falso caso ocorra uma exceção
            }
        }

    }
}
