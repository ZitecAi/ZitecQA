using Microsoft.Playwright;
using PortalIDSFTestes.elementos.custodia;
using PortalIDSFTestes.elementos.dashboards;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.dashboards
{
    public class DashCedentesPage
    {
        private IPage page;
        Metodos metodo;
        DashCedentesElements el = new DashCedentesElements();

        public DashCedentesPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosDashCedentesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Dashboard Cedentes");
        }
    }
}
