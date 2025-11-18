using Microsoft.Playwright;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.administrativo
{
    public class TokensPage
    {
        private readonly IPage page;
        Utils metodo;
        TokensElements el = new TokensElements();

        public TokensPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosTokens()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Tokens");
        }

    }
}
