using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.relatorios
{
    public class RelatoriosCotistasElements
    {

        public string MenuRelatorios { get; } = "//p[text()='Relatórios']";
        public string PaginaRelatorioCotistas { get; } = "(//p[text()='Cotistas'])[2]";


    }
}
