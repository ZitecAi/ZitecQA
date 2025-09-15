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
    public class CarteirasPage
    {

        private readonly IPage page;
        Metodos metodo;
        CarteirasElements el = new CarteirasElements();

        public CarteirasPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosCarteiras()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Carteiras");
        }

    }
}
