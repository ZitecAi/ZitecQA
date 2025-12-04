using Microsoft.Playwright;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.bancoId
{


    public class ExtratosPage
    {
        private readonly IPage page;
        Utils metodo;
        ExtratosElements el = new ExtratosElements();


        public ExtratosPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosExtratos()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Extratos");
        }

        public async Task GerarExtratoPdf()
        {
            await metodo.Clicar(el.BtnGerarExtrato, "Clicar em Gerar extrato para abrir modal");
            await metodo.ClicarNoSeletor(el.SelectFundo, "63629011000155", "Selecionar Fundo catanzaro");
            await Task.Delay(500);
            await metodo.ValidateDownloadAndLength(page, el.BtnGerar, ".pdf", "Validar download do extrato em PDF");
            //await metodo.ValidarTextoPresente(el.TextoRelatorioGerado, "Validar mensagem Extrato gerado com sucesso! presente na tela");

        }

    }

}
