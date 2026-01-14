namespace PortalIDSFTestes.elementos.cadastro
{
    public class InvestidoresElements
    {
        public string MenuCadastro { get; } = "(//p[text()='Cadastro'])[1]";
        public string PaginaInvestidores { get; } = "//p[text()='Investidores']";
        public string TabelaCotistas { get; } = "//div[@id='tabelaCotista_filter']//input[@type='search']";
        public string BotaoLinkFormulario { get; } = "//button[@title='Copiar Link']";

        //Dados do investidor
        public string CampoNome { get; } = "#nomeCompletoPf";
        public string CampoCpf { get; } = "#cpf";

        public string SeletorSexo { get; } = "#sexo";
        public string DataNascimento { get; } = "#dtNascimento";
        public string NomePai { get; } = "#nomePai";
        public string NomeMae { get; } = "#nomeMae";
        public string EstadoCivil { get; } = "#estadoCivil";
        public string SituacaoLegal { get; } = "#situacaoLegal";
        public string Email { get; } = "#email";
        public string TelefoneContato { get; } = "#telContato";
        public string MsgEletronica { get; } = "#msgEletronica";
        public string TipoDocumento { get; } = "#tipoDocumento";
        public string NumeroDocumento { get; } = "#numDocumento";
        public string OrgaoExpedidor { get; } = "#orgExpedidor";
        public string PaisExpedidor { get; } = "#paisExpedidor";
        public string UfExpedidor { get; } = "#ufExpedidor";
        public string UfNascimento { get; } = "#ufNascimento";
        public string Naturalidade { get; } = "#naturalidade";
        public string OcupacaoProf { get; } = "#ocupacaoProf";
        public string BotaoAvancar { get; } = "#btnAvancar";

        //Endereço

        public string PaisResidencia { get; } = "#paisResidencia";
        public string LogradouroResidencia { get; } = "#logResidencia";

        //Situação Patrimonial

        public string DataSituacaoPatrimonial { get; } = "#dtSituacaoPatrim";
        public string PatrimoniEstimado { get; } = "#patrimEstimado";
        public string RendimentoMensal { get; } = "#rendMensal";
        public string OutrosRendimentos { get; } = "#outrosRend";

        //Suitability

        public string MotivoDispensa { get; } = "#motivoRecuseSuitability";
        public string PerfilSuitability { get; } = "#perfilSuitability";

        //Termos

        public string AceiteTermos { get; } = "#aceiteDeclaracao";

        public string BotaoEnviarFormulario { get; } = "#btnFinalizar";


    }
}
