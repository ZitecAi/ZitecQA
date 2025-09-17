using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.relatorios
{
    public class RelatorioCedentesElements
    {

        public string MenuRelatorios { get; } = "//p[text()='Relatórios']";
        public string PaginaRelatorioCedentes { get; } = "(//p[text()='Cedentes'])[4]";


    }
}
