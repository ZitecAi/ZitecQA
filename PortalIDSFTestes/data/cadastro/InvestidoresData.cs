using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.data.cadastro
{
    public class InvestidoresData
    {
        public string NomeCotista { get; set; } = "LEVI DA PAZ ALVES";
        public string CpfCotista { get; set; } = DataGenerator.Generate(DocumentType.Cpf);
        public string Sexo { get; set; } = "Masculino";
        public string DataNascimento { get; set; } = "29/01/2003";
        public string NomePai { get; set; } = "JOAO ALVES";
        public string NomeMae { get; set; } = "MARIA DA PAZ";
        public string EstadoCivil { get; set; } = "Solteiro";
        public string SituacaoLegal { get; set; } = "Maior";
        public string Email { get; set; } = "alveslevi@icloud.com";
        public string TelefoneContato { get; set; } = "11987654321";
        public string MsgEletronica { get; set; } = "Desejo receber comunicações por meios eletrônicos.";
        public string TipoDocumento { get; set; } = "Carteira de Trabalho";
        public string NumeroDocumento { get; set; } = "1234567/1234";
        public string OrgaoExpedidor { get; set; } = "SP";
        public string PaisExpedidor { get; set; } = "BRASIL";
        public string UfExpedidor { get; set; } = "São Paulo";
        public string EstadoNascido { get; set; } = "São Paulo";
        public string CidadeNascido { get; set; } = "Osasco";
        public string OcupacaoProfissional { get; set; } = "Analista de Testes";
        public string MotivoDispensa { get; set; } = "admCartAutoriz";
        public string PerfilSuitability { get; set; } = "Moderado";

        public string MensagemLinkCopiado { get; } = "Link copiado!";



    }
}
