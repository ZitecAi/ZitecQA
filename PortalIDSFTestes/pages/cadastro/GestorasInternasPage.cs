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
    public class GestorasInternasPage
    {

        private readonly IPage page;
        Metodos metodo;
        GestorasInternasElements el = new GestorasInternasElements();

        public GestorasInternasPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosGestorasInternas()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Gestoras Internas");
        }

    }
}
