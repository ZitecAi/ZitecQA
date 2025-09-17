using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.dashboards
{
    public class DashOperacoesElements
    {
        public string MenuDashboards { get; } = "(//a[@class='nav-link']//p[text()='Dashboards'])[1]";
        public string PaginaDashOperacoes { get; } = "(//p[text()='Operações'])[1]";

    }
}
