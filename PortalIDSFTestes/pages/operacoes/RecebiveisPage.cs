using Microsoft.Extensions.Configuration;

using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.operacoes
{
    public class RecebiveisPage
    {
        private IPage page;
        Utils metodo;
        RecebiveisElements el = new RecebiveisElements();

        public RecebiveisPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosRecebiveisPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Recebiveis");
        }

        
    }
}
