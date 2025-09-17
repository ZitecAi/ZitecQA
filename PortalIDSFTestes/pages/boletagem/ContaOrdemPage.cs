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
    public class ContaOrdemPage
    {
        private readonly IPage page;
        Metodos metodo;
        ContaOrdemElements el = new ContaOrdemElements();

        public ContaOrdemPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosContaOrdem()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Conta Ordem");
        }
    }
}
