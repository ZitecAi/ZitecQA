using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using PortalIDSFTestes.elementos.cedentes;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.cedentes
{
    public class KitCedentePage
    {
        private IPage page;
        Metodos metodo;
        KitCedenteElements el = new KitCedenteElements();

        public KitCedentePage(IPage page) 
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosKitCedentePage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Kit Cedente");
        }       


    }
}
