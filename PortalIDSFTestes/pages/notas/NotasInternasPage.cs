using Microsoft.Playwright;
using PortalIDSFTestes.elementos.dashboards;
using PortalIDSFTestes.elementos.notas;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.notas
{
    public class NotasInternasPage
    {
        private IPage page;
        Utils metodo;
        NotasInternasElements el = new NotasInternasElements();

        public NotasInternasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosPagamentosNotasInternasPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Notas Internas ");
        }
    }
}
