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
    public class BaixasLotePage
    {
        private IPage page;
        Metodos metodo;
        BaixasLoteElements el = new BaixasLoteElements();

        public BaixasLotePage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosBaixasLotePage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Baixas em Lote");
        }

        
    }
}
