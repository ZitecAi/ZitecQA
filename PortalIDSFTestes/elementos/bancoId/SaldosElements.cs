namespace PortalIDSFTestes.elementos.bancoId
{
    public class SaldosElements
    {
        public string MenuBancoId { get; } = "//p[text()='Banco ID']";
        public string PaginaSaldos { get; } = "//p[text()='Saldos']";
        public string FiltroFundos { get; } = "#filtroFundo";
        public string LimparFiltros { get; } = "//button[text()='Limpar Filtros']";
        public string SelectGestora { get; } = "#gestoras";
        public string SelectConsultorias { get; } = "#consultoras";
        public string BtnCarregar { get; } = "//button[text()='Carregar']";
        public string SaldoNaTabela { get; } = "//tr//td[3]";

    }
}
