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
    public class LaminasPage
    {

        private readonly IPage page;
        Utils metodo;
        LaminasElements el = new LaminasElements();

        public LaminasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosLaminas()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Laminas");
        }

    }
}
