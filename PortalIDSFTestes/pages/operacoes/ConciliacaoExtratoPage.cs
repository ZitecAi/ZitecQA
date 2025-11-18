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
    public class ConciliacaoExtratoPage
    {
        private IPage page;
        Utils metodo;

        public ConciliacaoExtratoPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosConciliacaoExtratoPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Conciliacao e Extrato");
        }

        
    }
}
