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
    public class ControleCapitalPage
    {
        private readonly IPage page;
        Metodos metodo;
        ControleCapitalElements el = new ControleCapitalElements();

        public ControleCapitalPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosControleCapital()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Controle Capital");
        }
    }
}
