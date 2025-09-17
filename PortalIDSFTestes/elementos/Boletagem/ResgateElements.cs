using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.Boletagem
{
    public class ResgateElements
    {
        public string MenuCadastro { get; } = "(//p[text()='Boletagem'])[1]";
        public string PaginaResgate { get; } = "//p[text()='Resgate']";

    }
}
