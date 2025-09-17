using Microsoft.Playwright;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.risco
{
    public class FundosDesenquadradosPage
    {

        private IPage page;
        Metodos metodo;

        public FundosDesenquadradosPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosFundosDesenquadradosPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Fundos Desenquadrados");
        }

    }
}
