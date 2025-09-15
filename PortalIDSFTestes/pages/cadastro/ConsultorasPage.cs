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
    public class ConsultorasPage
    {

        private readonly IPage page;
        Metodos metodo;
        ConsultorasElements el = new ConsultorasElements();

        public ConsultorasPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosConsultoras()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Consultoras");
        }

    }
}
