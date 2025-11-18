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
    public class CotistasPage
    {

        private readonly IPage page;
        Utils metodo;
        CotistasElements el = new CotistasElements();

        public CotistasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosCotistas()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Cotistas");
        }

    }
}
