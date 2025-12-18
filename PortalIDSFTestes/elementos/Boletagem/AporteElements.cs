namespace PortalIDSFTestes.elementos.Boletagem
{
    public class AporteElements
    {
        public string MenuBoletagem { get; } = "(//p[text()='Boletagem'])[1]";
        public string PaginaAporte { get; } = "//p[text()='Aporte']";
        public string BtnNovo { get; } = "#tableButtonNovo";
        public string Calendario { get; } = "#dataAporte";
        public string ValorAporte { get; } = "//input[@id='ValorAporte']";
        public string CnpjCotista { get; } = "#CPFCotista";
        public string InputNomeCotista { get; } = "#NomeCotista";
        public string SelectCota { get; } = "#tipoCota";
        public string ValorCota { get; } = "#valorCota";
        public string TipoAporte { get; } = "#tipoAporte";
        public string SelectFundo { get; } = "#Fundos";
        public string SelectCarteira { get; } = "#Carteiras";
        public string BtnEnviar { get; } = "#submitButton";
        public string Filtro { get; } = "//div[@id]//input[@type='search']";
        public string AprovacaoCustodia(string nomeCotista) => $"//td[contains(text(),'{nomeCotista}')]/ancestor::tr/td[11]//span";
        public string AprovacaoEscrituracao(string nomeCotista) => $"//td[contains(text(),'{nomeCotista}')]/ancestor::tr/td[12]//span";
        public string AprovacaoControladoria(string nomeCotista) => $"//td[contains(text(),'{nomeCotista}')]/ancestor::tr/td[13]//span";
        public string BtnAprovado { get; } = "//span[text()='Aprovado']";
        public string BtnRejeitado { get; } = "//span[text()='Rejeitado']";
        public string Descricao { get; } = "#descricao";
        public string BtnConfirmar { get; } = "#statusButton";
        public string StatusBoletado { get; } = "(//tr)[2]//td[7]//span";
        public string VisualizarInformacoes { get; } = "//i[@class='fas fa-eye']";
        public string ModalInformacoes { get; } = "//h5[text()='Visualização da Boleta']";
        public string PosicaoFundoNaTabela { get; } = "(//tr)[2]//td[4]";
        public string PosicaoCotistaNaTabela { get; } = "(//tr)[2]//td[6]";
        public string ConfirmarAprovacao { get; } = "#statusButton";
    }
}
