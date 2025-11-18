using Microsoft.Playwright;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.cadastro
{
    public class OfertasPage
    {

        private readonly IPage page;
        Utils metodo;
        OfertasElements el = new OfertasElements();

        public OfertasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosOfertas()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Ofertas");
        }

    }
}
