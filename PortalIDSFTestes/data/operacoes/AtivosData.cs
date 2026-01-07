namespace PortalIDSFTestes.data.operacoes
{
    public class AtivosData
    {

        public static string GenerateNameUnique()
        {
            try
            {
                Random random = new Random();

                int numeroUnico = random.Next(0, 9999);
                string nomeUnico = $"Teste NUnit {numeroUnico}";

                return nomeUnico;
            }
            catch
            {
                throw new Exception("Erro ao gerar número único para o nome");
            }
        }
        // Textos para método Escrever
        public static string NomeAleatorio { get; set; } = GenerateNameUnique();

        public string NomeAtivo = NomeAleatorio;
        public string CnpjFundoZitec { get; set; } = "54638076000176";
        public string NomeCedenteTeste { get; set; } = "Cedente Teste NUnit";
        public string CnpjCedenteTeste { get; set; } = "533.006.080-00106";
        public string AgenciaPadrao { get; set; } = "0001";
        public string ContaPadrao { get; set; } = "460915";
        public string CpfAtivoTeste { get; set; } = "49624866830";
        public string ValorAtivo { get; set; } = "10";
        public string ResumoOperacao { get; set; } = "Teste NUnit";
        public string ObservacaoAprovacao { get; set; } = "Teste Aprovação";
        public string ObservacaoReprovacao { get; set; } = "Teste Reprovação";
        public string TextoPesquisaAtivo { get; set; } = "Teste NUnit";

        // Mensagens de validação
        public static string MsgContratoAprovadoSucesso => "Contrato aprovado com sucesso!";

    }
}