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
    public class ConsultoriasInternasPage
    {

        private readonly IPage page;
        Utils metodo;
        ConsultoriasInternasElements el = new ConsultoriasInternasElements();

        public ConsultoriasInternasPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosConsultoriasInternas()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Consultorias Internas");
        }

    }
}
