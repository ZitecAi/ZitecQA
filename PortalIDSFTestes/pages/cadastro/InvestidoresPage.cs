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
    public class InvestidoresPage
    {

        private readonly IPage page;
        Metodos metodo;
        InvestidoresElements el = new InvestidoresElements();

        public InvestidoresPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosInvestidores()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Investidores");
        }

    }
}
