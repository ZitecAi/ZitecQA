using Microsoft.Playwright;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.cadastro
{
    public class FundosPage
    {

        private readonly IPage page;
        Utils metodo;
        FundosElements el = new FundosElements();

        public FundosPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosFundos()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Fundos");
        }

    }
}
