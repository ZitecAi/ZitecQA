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
    public class EnviarLastrosPage
    {
        private IPage page;
        Utils metodo;
        EnviarLastrosElements el = new EnviarLastrosElements();

        public EnviarLastrosPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosEnviarLastrosPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina Enviar Lastros");
        }

        
    }
}
