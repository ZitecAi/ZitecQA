using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCedente.Model;

namespace TesteCedente.Repository.Cedentes
{
    public class CedentesRepository
    {
        public static bool VerificaExistenciaCedente(string fundoCnpj, string cedenteCnpj)
        {
            var existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Cedentes WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", SqlDbType.NVarChar).Value = fundoCnpj;
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", SqlDbType.NVarChar).Value = cedenteCnpj;

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
                Console.WriteLine(e);
            }

            return existe;
        }

        public static bool ApagarCedente(string fundoCnpj, string cedenteCnpj)
        {
            var apagado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Cedentes WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", SqlDbType.NVarChar).Value = fundoCnpj;
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", SqlDbType.NVarChar).Value = cedenteCnpj;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            apagado = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return apagado;
        }

        public static bool CedenteEmFormalizacao(string fundoCnpj, string cedenteCnpj)
        {
            bool emFormalizacao = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT CedenteStatus 
                FROM Cedentes 
                WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", fundoCnpj);
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", cedenteCnpj);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                var status = oReader["CedenteStatus"]?.ToString();
                                emFormalizacao = status != null && status.Equals("FORMALIZACAO", StringComparison.OrdinalIgnoreCase);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar status do cedente: {e.Message}");
            }

            return emFormalizacao;
        }


        public static bool CedenteAtivo(string fundoCnpj, string cedenteCnpj)
        {
            bool emFormalizacao = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT CedenteStatus 
                FROM Cedentes 
                WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", fundoCnpj);
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", cedenteCnpj);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                var status = oReader["CedenteStatus"]?.ToString();
                                emFormalizacao = status != null && status.Equals("ATIVO", StringComparison.OrdinalIgnoreCase);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar status do cedente: {e.Message}");
            }

            return emFormalizacao;
        }

        public static bool CedenteCadastrodoZCust(string nuCpfCnpj, string dsEmail)
        {
            bool cadastrado = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT 1
                FROM tb_fundo_cedente 
                WHERE NU_CPF_CNPJ = @nuCpfCnpj AND DS_EMAIL = @dsEmail";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nuCpfCnpj", nuCpfCnpj);
                        oCmd.Parameters.AddWithValue("@dsEmail", dsEmail); // Corrigido: o parâmetro estava escrito com nome errado

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                cadastrado = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar cadastro do cedente: {e.Message}");
            }

            return cadastrado;
        }
        public static bool ExcluirCedenteZCustodia(string nuCpfCnpj)
        {
            bool excluido = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    // 1. Obter o ID do cedente pelo CPF/CNPJ
                    string queryId = @"
                SELECT ID_CEDENTE
                FROM TB_FUNDO_CEDENTE
                WHERE NU_CPF_CNPJ = @nuCpfCnpj";

                    int idCedente = 0;

                    using (SqlCommand cmdId = new SqlCommand(queryId, myConnection))
                    {
                        cmdId.Parameters.AddWithValue("@nuCpfCnpj", nuCpfCnpj);

                        var result = cmdId.ExecuteScalar();
                        if (result != null)
                        {
                            idCedente = Convert.ToInt32(result);
                        }
                        else
                        {
                            Console.WriteLine("Cedente não encontrado.");
                            return false;
                        }
                    }

                    // 2. Excluir dependentes da TB_ASSOC_CEDENTE_REPRESENTANTE
                    string queryDeleteAssociacoes = @"
                DELETE FROM TB_ASSOC_CEDENTE_REPRESENTANTE
                WHERE ID_CEDENTE = @idCedente";

                    using (SqlCommand cmdDelAssoc = new SqlCommand(queryDeleteAssociacoes, myConnection))
                    {
                        cmdDelAssoc.Parameters.AddWithValue("@idCedente", idCedente);
                        cmdDelAssoc.ExecuteNonQuery();
                    }

                    // 3. Excluir dependentes da TB_CONTA_CORRENTE
                    string queryDeleteContaCorrente = @"
                DELETE FROM TB_CONTA_CORRENTE
                WHERE ID_CEDENTE = @idCedente";

                    using (SqlCommand cmdDelConta = new SqlCommand(queryDeleteContaCorrente, myConnection))
                    {
                        cmdDelConta.Parameters.AddWithValue("@idCedente", idCedente);
                        cmdDelConta.ExecuteNonQuery();
                    }

                    // 4. Excluir dependentes da TB_ENTIDADE_LIGADA
                    string queryDeleteEntidadeLigada = @"
                DELETE FROM TB_ENTIDADE_LIGADA
                WHERE ID_CEDENTE = @idCedente";

                    using (SqlCommand cmdDelEntidade = new SqlCommand(queryDeleteEntidadeLigada, myConnection))
                    {
                        cmdDelEntidade.Parameters.AddWithValue("@idCedente", idCedente);
                        cmdDelEntidade.ExecuteNonQuery();
                    }
                    //Excluir da tb avalista
                    string queryDeleteAvalistas = @"
                DELETE FROM TB_AVALISTA
                WHERE ID_CEDENTE = @idCedente";

                    using (SqlCommand cmdDelAvalista = new SqlCommand(queryDeleteAvalistas, myConnection))
                    {
                        cmdDelAvalista.Parameters.AddWithValue("@idCedente", idCedente);
                        cmdDelAvalista.ExecuteNonQuery();
                    }
                    // 5. Excluir da tabela TB_FUNDO_CEDENTE
                    string queryDeleteCedente = @"
                DELETE FROM TB_FUNDO_CEDENTE
                WHERE ID_CEDENTE = @idCedente";

                    using (SqlCommand cmdDelCedente = new SqlCommand(queryDeleteCedente, myConnection))
                    {
                        cmdDelCedente.Parameters.AddWithValue("@idCedente", idCedente);
                        int linhasAfetadas = cmdDelCedente.ExecuteNonQuery();
                        excluido = linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao excluir cedente: {e.Message}");
            }

            return excluido;
        }

        public static bool VerificaAtualizacaoCedente(string nome, string email)
        {
            bool existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT 1 
                FROM tb_fundo_cedente 
                WHERE NM_CEDENTE = @nome AND DS_EMAIL = @email";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nome", nome);
                        oCmd.Parameters.AddWithValue("@email", email);

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
                Console.WriteLine("Erro ao verificar cedente por nome e e-mail: " + e.Message);
            }

            return existe;
        }


        public static bool RepresentantesComEmailsCadastrados(List<string> listaEmails)
        {
            bool existe = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    // Monta os parâmetros dinamicamente
                    var emailParams = new List<string>();
                    for (int i = 0; i < listaEmails.Count; i++)
                    {
                        emailParams.Add($"@email{i}");
                    }

                    string query = $@"
                SELECT COUNT(*) 
                FROM TB_REPRESENTANTE
                WHERE DS_EMAIL IN ({string.Join(",", emailParams)})";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        // Adiciona os parâmetros com valores
                        for (int i = 0; i < listaEmails.Count; i++)
                        {
                            oCmd.Parameters.AddWithValue($"@email{i}", listaEmails[i]);
                        }

                        int count = (int)oCmd.ExecuteScalar();

                        // Se encontrou pelo menos 1, retorna true
                        if (count > 0)
                        {
                            existe = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar representantes: {e.Message}");
            }

            return existe;
        }

        public static bool RepresentanteAssinaIso(string email)
        {
            bool assinaIsoladamente = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
                SELECT IC_ASS_ISOLADAMENTE
                FROM TB_REPRESENTANTE
                WHERE DS_EMAIL = @email";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@email", email);

                        object resultado = oCmd.ExecuteScalar();

                        if (resultado != null && resultado != DBNull.Value)
                        {
                            int valor = Convert.ToInt32(resultado);
                            if (valor == 1)
                            {
                                assinaIsoladamente = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar assinatura isolada do representante: {e.Message}");
            }

            return assinaIsoladamente;
        }

        public static bool ExcluirRepresentantesPorEmail(List<string> listaEmails)
        {
            bool excluido = false;

            try
            {
                var con = AppSettings.GetConnectionString("ConnectionZitec");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    // Monta os parâmetros dinamicamente
                    var emailParams = new List<string>();
                    for (int i = 0; i < listaEmails.Count; i++)
                    {
                        emailParams.Add($"@email{i}");
                    }

                    string inClause = string.Join(",", emailParams);

                    // 1. Excluir da TB_ASSOC_CEDENTE_REPRESENTANTE
                    string queryDeleteAssociacoes = $@"
                DELETE FROM TB_ASSOC_CEDENTE_REPRESENTANTE
                WHERE ID_REPRESENTANTE IN (
                    SELECT ID_REPRESENTANTE FROM TB_REPRESENTANTE
                    WHERE DS_EMAIL IN ({inClause})
                )";

                    using (SqlCommand cmdDelAssoc = new SqlCommand(queryDeleteAssociacoes, myConnection))
                    {
                        for (int i = 0; i < listaEmails.Count; i++)
                        {
                            cmdDelAssoc.Parameters.AddWithValue($"@email{i}", listaEmails[i]);
                        }

                        cmdDelAssoc.ExecuteNonQuery();
                    }

                    // 2. Excluir da TB_REPRESENTANTE
                    string queryDeleteRepresentantes = $@"
                DELETE FROM TB_REPRESENTANTE
                WHERE DS_EMAIL IN ({inClause})";

                    using (SqlCommand cmdDelRep = new SqlCommand(queryDeleteRepresentantes, myConnection))
                    {
                        for (int i = 0; i < listaEmails.Count; i++)
                        {
                            cmdDelRep.Parameters.AddWithValue($"@email{i}", listaEmails[i]);
                        }

                        int linhasAfetadas = cmdDelRep.ExecuteNonQuery();
                        excluido = linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao excluir representantes: {e.Message}");
            }

            return excluido;
        }
        public static bool CedenteReprovado(string fundoCnpj, string cedenteCnpj)
        {
            bool reprovado = false;

            try
            {
                var con = AppSettings.GetConnectionString("myConnectionString");

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = @"
        SELECT CedenteStatus 
        FROM Cedentes 
        WHERE FundoCNPJ = @fundoCnpj AND CedenteCNPJ = @cedenteCnpj";

                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundoCnpj", fundoCnpj);
                        oCmd.Parameters.AddWithValue("@cedenteCnpj", cedenteCnpj);

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                var status = oReader["CedenteStatus"]?.ToString();
                                reprovado = status != null && status.Equals("REPROVADO", StringComparison.OrdinalIgnoreCase);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao verificar status do cedente: {e.Message}");
            }

            return reprovado;
        }

    }
}
