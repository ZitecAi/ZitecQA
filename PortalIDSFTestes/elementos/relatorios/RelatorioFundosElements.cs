using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.relatorios
{
    public class RelatorioFundosElements
    {

        public string MenuRelatorios { get; } = "//p[text()='Relatórios']";
        public string PaginaRelatorioFundos { get; } = "(//p[text()='Fundos'])[2]";


    }
}
