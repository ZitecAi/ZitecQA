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
    public class DashOperacoesPage
    {
        private IPage page;
        Metodos metodo;
        DashOperacoesElements el = new DashOperacoesElements();

        public DashOperacoesPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosDashOperacoesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Dashboard Operações");
        }
    }
}
