using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.custodia
{
    public class PainelCotasElement
    {
        public string MenuCedentes { get; } = "(//a[@class='nav-link']//p[text()='Custódia'])[1]";
        public string PaginaPainelCotas { get; } = "//p[text()='Painel de Cotas']";
    }
}
