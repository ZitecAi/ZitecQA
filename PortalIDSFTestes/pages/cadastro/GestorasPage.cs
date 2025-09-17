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
    public class GestorasPage
    {

        private readonly IPage page;
        Metodos metodo;
        GestorasElements el = new GestorasElements();

        public GestorasPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosGestoras()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Gestora");
        }

    }
}
