using Microsoft.Playwright;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.rendaFixa
{
    public class PosicoesRendaFixaPage
    {

        private IPage page;
        Metodos metodo;

        public PosicoesRendaFixaPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosPosicoesRendaFixaPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Posições Renda Fixa");
        }

    }
}
