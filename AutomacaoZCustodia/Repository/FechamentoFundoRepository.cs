using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;



namespace AutomacaoZCustodia.Repository
{
    public class FechamentoFundoRepository
    {

        public static bool VerificarDataFundo(int idFundo, DateTime dataComparacao)
        {
            bool dataIgual = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["ConnectionZitec"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT dt_fundo FROM tb_fundo WHERE id_fundo = @idFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.Add("@idFundo", SqlDbType.Int).Value = idFundo;

                        var resultado = oCmd.ExecuteScalar();

                        if (resultado != null)
                        {
                            
                            if (DateTime.TryParse(resultado.ToString(), out DateTime dtFundo))
                            {
                              
                                dataIgual = dtFundo.Date == dataComparacao.Date;

                              
                                Console.WriteLine($"Data no banco: {dtFundo.Date}, Data Comparação: {dataComparacao.Date}, Igual: {dataIgual}");
                            }
                            else
                            {
                                Console.WriteLine("Erro ao converter a data retornada do banco.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nenhum resultado retornado do banco.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar data: {e.Message}");
            }

            return dataIgual;
        }

    }
}
