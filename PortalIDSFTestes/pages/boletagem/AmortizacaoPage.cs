using Microsoft.Playwright;
using PortalIDSFTestes.elementos.Boletagem;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.boletagem
{
    public class AmortizacaoPage
    {
        private readonly IPage page;
        Utils metodo;
        AmortizacaoElements el = new AmortizacaoElements();

        public AmortizacaoPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosAmortizacao()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Amortizacao");
        }
    }
}
