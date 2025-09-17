using Microsoft.Playwright;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.relatorios
{
    public class RelatorioOperacoesPage
    {

        private IPage page;
        Metodos metodo;

        public RelatorioOperacoesPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosRelatorioOperacoesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Relatorio Operacoes");
        }

    }
}
