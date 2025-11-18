using Microsoft.Playwright;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.elementos.bancoId;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.bancoId
{


    public class RendimentoPage
    {
        private readonly IPage page;
        Utils metodo;
        RendimentoElements el = new RendimentoElements();


        public RendimentoPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosRendimento()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Rendimento");
        }

    }

}
