using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace AutomacaoZCustodia.Utils
{
    public class ObterListaFeriados
    {
        public static List<DateTime> ObterFeriados(DateTime dataInicio, DateTime dataFim)
        {
            List<DateTime> feriados = new List<DateTime>();
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["Sinqia"].ConnectionString;

                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    myConnection.Open();

                    string query = @"
                    SELECT DT_FERIADO 
                    FROM TB_FERIADOS 
                    WHERE DT_FERIADO BETWEEN @DataInicio AND @DataFim";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@DataInicio", dataInicio);
                        oCmd.Parameters.AddWithValue("@DataFim", dataFim);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                DateTime feriado = (DateTime)oReader["DT_FERIADO"];
                                feriados.Add(feriado);
                            }
                        }
                    }
                    myConnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao buscar feriados: {e.Message}");
            }

            return feriados;
        }

        public static DateTime ObterProximoDiaUtil(DateTime data)
        {
            List<DateTime> feriados = ObterFeriados(data, data.AddMonths(1)); 

            while (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday || feriados.Contains(data))
            {
                data = data.AddDays(1);
            }

            return data;
        }
    }
}
