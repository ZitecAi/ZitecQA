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


    public class ExtratosPage
    {
        private readonly IPage page;
        Metodos metodo;
        ExtratosElements el = new ExtratosElements();


        public ExtratosPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosExtratos()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Extratos");
        }

    }

}
