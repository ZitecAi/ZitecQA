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
    public class AportePage
    {
        private readonly IPage page;
        Metodos metodo;
        AporteElements el = new AporteElements();

        public AportePage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosAporte()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Aporte");
        }
    }
}
