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
    public class PagamentosNotasPage
    {
        private IPage page;
        Utils metodo;
        PagamentosNotasElements el = new PagamentosNotasElements();

        public PagamentosNotasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosPagamentosNotasPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Pagamentos Notas ");
        }
    }
}
