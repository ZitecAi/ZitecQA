using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.cadastro
{
    public class INRPage
    {

        private IPage page;
        Utils metodo;
        OperacoesElements el = new OperacoesElements();

        public INRPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosINRPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Investidores não residentes");
        }


    }
}
