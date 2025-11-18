using Microsoft.Playwright;
using PortalIDSFTestes.elementos.custodia;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.custodia
{
    public class PainelCotasPage
    {
        private IPage page;
        Utils metodo;
        PainelCotasElement el = new PainelCotasElement();

        public PainelCotasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosPainelCotasPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Painel de Cotas");
        }
    }
}
