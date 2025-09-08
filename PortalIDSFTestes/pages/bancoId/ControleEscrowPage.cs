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


    public class ControleEscrowPage
    {
        private readonly IPage page;
        Metodos metodo;
        ControleEscrowElements el = new ControleEscrowElements();

        public ControleEscrowPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosControleEscrow()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página ControleEscrow");
        }

    }

}
