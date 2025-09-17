using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.cedentes
{
    public class KitCedenteElements
    {
        public string MenuCedentes { get; } = "(//a[@class='nav-link']//p[text()='Cedentes'])[1]";
        public string PaginaKitCedente { get; } = "//p[text()='Kit Cedente']";        


    }
}
