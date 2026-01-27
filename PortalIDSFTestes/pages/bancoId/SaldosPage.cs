using Microsoft.Playwright;
using PortalIDSFTestes.data.bancoId;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.bancoId
{


    public class SaldosPage
    {
        private readonly IPage page;
        Utils metodo;
        SaldosElements _el = new SaldosElements();
        SaldosData _data = new SaldosData();


        public SaldosPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosSaldos()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Saldos");
        }

        public async Task AbrirModalDeFiltro()
        {
            await metodo.Clicar(_el.FiltroFundos, "Abrir modal de filtro");
        }
        public async Task LimparFiltros()
        {
            await metodo.Clicar(_el.LimparFiltros, "Limpar Filtros padrões");
        }
        public async Task SelcionarGestora()
        {
            await metodo.ClicarNoSeletor(_el.SelectGestora, "16007398000128", "Selecionar Gestora IDSF");
        }
        public async Task SelcionarConsultoria()
        {
            await metodo.ClicarNoSeletor(_el.SelectConsultorias, "21057026000146", "Selecionar Consultoria IDSF");
        }
        public async Task InserirNomeFundo()
        {
            await metodo.Escrever(_el.InputTextoFundo, _data.NomeFundo, "Escrever nome do fundo");
        }
        public async Task ClicarNoFundo()
        {
            await metodo.Clicar(_el.SelectFundo, "Selecionar fundo");
        }
        public async Task ClicarEmCarregarParaTrazerSaldos()
        {
            await metodo.Clicar(_el.BtnCarregar, "Clicar em carregar para trazer saldos");
        }




    }

}
