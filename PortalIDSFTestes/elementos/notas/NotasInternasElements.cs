using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.notas
{
    public class NotasInternasElements
    {
        public string MenuNotas { get; } = "(//a[@class='nav-link']//p[text()='Notas'])[1]";
        public string PaginaNotasInternas { get; } = "(//p[text()='Notas Internas'])[1]";

    }
}
