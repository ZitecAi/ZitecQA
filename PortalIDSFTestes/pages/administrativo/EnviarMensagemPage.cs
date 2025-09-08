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
    public class EnviarMensagemPage
    {
        private readonly IPage page;
        Metodos metodo;
        EnviarMensagemElements el = new EnviarMensagemElements();

        public EnviarMensagemPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosEnviarMensagemPage()
        {
            await metodo.ValidarAcentosAsync(page,"Validar Acentos na Página Administrativo - Enviar mensagem");
        }


    }
}
