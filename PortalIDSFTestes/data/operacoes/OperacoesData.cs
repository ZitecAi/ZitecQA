namespace PortalIDSFTestes.data.operacoes
{
    public class OperacoesData
    {
        // Textos para método Escrever
        public string MotivoExclusaoArquivo { get; set; } = "Teste Exclusão";
        public string ObservacaoCSV { get; set; } = "Teste de Arquivo CSV";
        public string ObservacaoCSVNegativo { get; set; } = "Teste de Arquivo CSV Negativo";
        public string ExtensaoArquivoPesquisa { get; set; } = ".rem";
        public string MensagemSucessoOperacaoAprovada { get; } = "Operação aprovada com sucesso!";
        public string StatusAguardandoAssinaturaDigital { get; } = "Aguardando Assinatura Digital";
        public string StatusAguardandoEnvioCertificadoraNaFila { get; } = "Aguardando Envio Certificadora na Fila";
        public string StatusAguardandoAprovacaoDoGestor { get; } = "Aguardando aprovação do Gestor";
        public string StatusAguardandoAprovacaoDoGestorEAssinaturas { get; } = "Aguardando aprovação do Gestor e Assinaturas";
        public string MsgOperacaoExcluidaComSucesso { get; } = "Arquivo excluído com sucesso!";
        public string MsgExclusaoSolicitadaComSucessoAguardeAprovacaoCustodia { get; } = "Solicitação recebida com sucesso, aguarde aprovação da Custódia!";
        public string MsgExclusaoSolicitadaComSucesso { get; } = "Solicitação recebida com sucesso!";



    }
}