using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.dashboards
{
    public class DashCedentesElements
    {
        public string MenuDashboards { get; } = "(//a[@class='nav-link']//p[text()='Dashboards'])[1]";
        public string PaginaDashCedentes { get; } = "(//p[text()='Cedentes'])[3]";

    }
}
