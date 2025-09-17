using Microsoft.Playwright;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.controleInterno
{
    public class PoliticasPage
    {

        private IPage page;
        Metodos metodo;

        public PoliticasPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosPoliticasPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Politicas");
        }


    }
}
