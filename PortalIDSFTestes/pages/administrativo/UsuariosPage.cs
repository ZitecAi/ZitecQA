using Microsoft.Playwright;
using PortalIDSFTestes.elementos.administrativo;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.administrativo
{
    public class UsuariosPage
    {
        private readonly IPage page;
        Metodos metodo;
        UsuariosElements el = new UsuariosElements();

        public UsuariosPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosUsuarios()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Usuarios");
        }

    }
}
