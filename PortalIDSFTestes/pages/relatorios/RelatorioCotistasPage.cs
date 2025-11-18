using Microsoft.Playwright;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.relatorios
{
    public class RelatorioCotistasPage
    {

        private IPage page;
        Utils metodo;

        public RelatorioCotistasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosRelatorioCotistasPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Relatorio Cotistas");
        }

    }
}
