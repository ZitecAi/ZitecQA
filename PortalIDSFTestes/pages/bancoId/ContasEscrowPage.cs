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


    public class ContasEscrowPage
    {
        private readonly IPage page;
        Metodos metodo;
        ContasEscrowElements el = new ContasEscrowElements();

        public ContasEscrowPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosContasEscrow()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página ContasEscrow");
        }

    }

}
