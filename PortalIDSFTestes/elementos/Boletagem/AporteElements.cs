using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.Boletagem
{
    public class AporteElements
    {
        public string MenuCadastro { get; } = "(//p[text()='Boletagem'])[1]";
        public string PaginaAporte { get; } = "//p[text()='Aporte']";

    }
}
