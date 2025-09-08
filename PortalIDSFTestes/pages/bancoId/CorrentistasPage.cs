using Microsoft.Playwright;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.bancoId
{


    public class CorrentistasPage
    {
        private readonly IPage page;
        Metodos metodo;
        CorrentistasElements el = new CorrentistasElements();

        public CorrentistasPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task validarAcentosCorrentistas()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Correntistas");
        }

    }

}
