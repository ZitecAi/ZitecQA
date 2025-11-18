using Microsoft.Playwright;
using PortalIDSFTestes.elementos.Boletagem;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.boletagem
{
    public class ResgatePage
    {
        private readonly IPage page;
        Utils metodo;
        ResgateElements el = new ResgateElements();

        public ResgatePage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosResgate()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Resgate");
        }
    }
}
