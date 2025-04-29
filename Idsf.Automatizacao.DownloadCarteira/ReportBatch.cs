using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idsf.Automatizacao.DownloadCarteira
{
    internal class ReportBatch
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public static void ExecuteNonQuerySafe(SqlCommand command)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO ExecuteNonQuerySafe]: {ex}");
                // Aqui você poderia enviar para um log, Slack, etc.
            }
        }

        public static void UpdateDsStatus(string status, int idReportBatch)
        {
            var command = new SqlCommand("UPDATE MeusRelatorios SET DS_STATUS = @Status WHERE ID_REPORT_BATCH = @IdReportBatch");
            command.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
            command.Parameters.Add("@IdReportBatch", SqlDbType.Int).Value = idReportBatch;

            ExecuteNonQuerySafe(command);
        }

        public static void UpdatePath(string path, string fileName, int idReportBatch)
        {
            var command = new SqlCommand("UPDATE MeusRelatorios SET NM_ARQUIVO = @FileName, LOCALIZACAO_RELATORIO = @Path WHERE ID_REPORT_BATCH = @IdReportBatch");
            command.Parameters.Add("@FileName", SqlDbType.VarChar).Value = fileName;
            command.Parameters.Add("@Path", SqlDbType.VarChar).Value = path;
            command.Parameters.Add("@IdReportBatch", SqlDbType.Int).Value = idReportBatch;

            ExecuteNonQuerySafe(command);
        }

        // Essa função é útil apenas se quiser LOGAR o SQL final para DEBUG
        public static string GetCompleteSqlWithParameters(SqlCommand sqlCommand)
        {
            string completeSql = sqlCommand.CommandText;

            foreach (SqlParameter parameter in sqlCommand.Parameters)
            {
                string parameterName = parameter.ParameterName;
                string parameterValue;

                if (parameter.Value == null)
                {
                    parameterValue = "NULL";
                }
                else if (parameter.Value is string)
                {
                    parameterValue = $"'{parameter.Value}'";
                }
                else if (parameter.Value is DateTime date)
                {
                    parameterValue = $"'{date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}'";
                }
                else
                {
                    parameterValue = parameter.Value.ToString();
                }

                completeSql = completeSql.Replace(parameterName, parameterValue);
            }

            return completeSql;
        }
    }
}
