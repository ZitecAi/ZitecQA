namespace PortalIDSFTestes.elementos.cedentes
{
    public class CedentesElements
    {
        public string MenuCedentes { get; } = "(//a[@class='nav-link']//p[text()='Cedentes'])[1]";
        public string PaginaCedentes { get; } = "//a[@href='/Cedentes.aspx']";
        public string BtnNovoCedente { get; } = "#btonNovoCedente";
        public string InputNovoCedente { get; } = "#fileNovoCedente";
        public string MsgAcaoSucesso { get; } = "Ação Executada com Sucesso";
        public string BtnDownloadExcel { get; } = "#btnExportaExcel";
        public string BarraPesquisaCedentes { get; } = "//label[text()='Pesquisar']";
        public string TabelaCedentes { get; } = "#tabelaCedentes";
        public string TabelaCedentesCadastrado { get; } = "//table//tbody";
        public string BtnLixeiraCedentes { get; } = "//button[@class='buttonExcluirCedente btn btn-danger']";
        public string CampoObservacaoExcluir { get; } = "#TextAreaExclusao";
        public string BtnConfirmarExcluir { get; } = "#submitButtonExcluirCedente";
        public string MsgSucessoRetornada { get; } = "//div[@class='toast toast-success']";
        public string MsgErroRetornada { get; } = "//div[@class='toast toast-warning']";
        public string BtnFecharModal { get; } = "#btnFecharNovoCedente";
        public string BtnFecharMensagemSucesso { get; } = "//button[@class='toast-close-button']";
        public string BtnAprovarCadastro(string cnpjCedente) => $"(//td[contains(normalize-space(.), '{cnpjCedente}')]/ancestor::tr//button[.//i[@class='fas fa-ban']])[1]";
        public string BtnAprovarGestora(string cnpjCedente) => $"//td[contains(normalize-space(.), '{cnpjCedente}')]/ancestor::tr//button[text()='Em espera']";
        public string BtnAprovarCompliance(string cnpjCedente) => $"(//td[contains(normalize-space(.), '{cnpjCedente}')]/ancestor::tr//button[.//i[@class='fas fa-ban']])[2]";
        public string BtnAprovado { get; } = "//div[@class='radioButtons']//label[@for='option-1']";
        public string Obs { get; } = "#obsParecerDepartamento";
        public string BtnEnviarParecerDepartamento { get; } = "#btnEnviarStatusDepartamento";
        public string TdAprovadoGestora(string posicaoTd) => $"(//tr//td)[{posicaoTd}]//button"; //começa no 6 e vai ate o 8
        public string TdAprovados(string posicaoTd) => $"(//tr//td)[{posicaoTd}]//p"; //começa no 6 e vai ate o 8
        public string StatusCedente { get; } = "(//tr//td)[4]//small[1]";
        public string TdReprovado { get; } = "(//tr//td)[4]";
        public string BtnReprovarCedente(string cnpjCedente) => $"//td[contains(normalize-space(.), '{cnpjCedente}')]/ancestor::tr//i[@class='fas fa-times']";
        public string ObsReprovar { get; } = "#TextAreaReprovar";
        public string ButtonReprovar { get; } = "#submitButtonReprovacao";
        public string BtnContratoMae(string cnpjCedente) => $"//td[contains(normalize-space(.), '{cnpjCedente}')]/ancestor::tr//i[@class='fas fa-check']";
        public string InputContratoMae { get; } = "#fileAtivaCedente";
        public string CheckBoxAssinante { get; } = "#assinante_0";
        public string BotaoEnviarAssinante { get; } = "#Enviar";
        public string ObsAtivarContratoMae { get; } = "#TextAreaAtivar";
        public string ButtonAtivacao { get; } = "#submitButtonAtivacao";
        public string ButtonAprovado { get; } = "(//span[text()='Aprovado'])[1]";
        public string BtnEnviarParaAssinatura { get; } = "//div[@class='custom-control custom-switch mb-0']";
        public string ObsContratoMae { get; } = "#mensagemContratoMae";
        public string BtnAtivarCedente { get; } = "#submitBtnContratoMae";
        public string ButtonAtivarContratoMae(string cnpjCedente) => $"//td[contains(normalize-space(.), '{cnpjCedente}')]/ancestor::tr//i[@class='fa fa-file-pdf']";

    }
}
