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
    public class PrestServicosPage
    {

        private readonly IPage page;
        Utils metodo;
        PrestServicosElements el = new PrestServicosElements();

        public PrestServicosPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosPrestServicos()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Prest Serviços");
        }

    }
}
