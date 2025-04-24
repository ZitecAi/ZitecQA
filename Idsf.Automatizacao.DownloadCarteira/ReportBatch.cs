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
        public static string ConnectionSQLPortalIDSF = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
        public static void ExecuteQuery(SqlCommand query, string connectionSql)
        {
            try
            {
                string ComandoSql = GetCompleteSqlWithParameters(query);

                using (SqlConnection myConnection = new SqlConnection(connectionSql))
                {
                    myConnection.Open();
                    SqlCommand oCmd = new SqlCommand(ComandoSql, myConnection);



                    oCmd.CommandType = CommandType.Text;
                    oCmd.ExecuteNonQuery();


                    myConnection.Close();
                }
            }
            catch (Exception e)
            {
                //Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ReportBatchRepository", "Portal IDSF", e.StackTrace);
            }
        }
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
                else if (parameter.Value is DateTime)
                {
                    parameterValue = $"'{((DateTime)parameter.Value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}'";
                }
                else
                {
                    parameterValue = parameter.Value.ToString();
                }

                completeSql = completeSql.Replace(parameterName, parameterValue);
            }

            return completeSql;
        }

        public static void UpdateDsStatus(string status, int IdReportBatch)
        {
            SqlCommand oCmd = new SqlCommand("UPDATE MeusRelatorios SET DS_STATUS = @STATUS WHERE ID_REPORT_BATCH = @ID_REPORT_BATCH");
            oCmd.Parameters.AddWithValue("@STATUS", status);
            oCmd.Parameters.AddWithValue("@ID_REPORT_BATCH", IdReportBatch);
            ExecuteQuery(oCmd, ConnectionSQLPortalIDSF);
        }   

        public static void UpdatePath(string path, string FileName, int IdReportBatch)
        {
            SqlCommand oCmd = new SqlCommand("UPDATE MeusRelatorios SET NM_ARQUIVO = '" + FileName + "', LOCALIZACAO_RELATORIO = '" + path + "' where ID_REPORT_BATCH = " + IdReportBatch);
            ExecuteQuery(oCmd, ConnectionSQLPortalIDSF);
        }
    }
}
