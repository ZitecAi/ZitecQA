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


    public class ZeragemPage
    {
        private readonly IPage page;
        Metodos metodo;
        ZeragemElements el = new ZeragemElements();


        public ZeragemPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosZeragem()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Zeragem");
        }

    }

}
