using Microsoft.Playwright;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.controleInterno
{
    public class InformeDiarioPage
    {

        private IPage page;
        Utils metodo;

        public InformeDiarioPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosInformeDiarioPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Informe Diario");
        }


    }
}
