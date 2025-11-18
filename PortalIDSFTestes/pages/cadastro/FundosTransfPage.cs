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
    public class FundosTransfPage
    {

        private readonly IPage page;
        Utils metodo;
        FundosTransfElements el = new FundosTransfElements();

        public FundosTransfPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosFundosTransf()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Fundos de Transferencia");
        }

    }
}
