using Microsoft.Playwright;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.relatorios
{
    public class RelatorioMeusRelatoriosPage
    {

        private IPage page;
        Metodos metodo;

        public RelatorioMeusRelatoriosPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosRelatorioMeusRelatoriosPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Relatorio Meus Relatorios");
        }

    }
}
