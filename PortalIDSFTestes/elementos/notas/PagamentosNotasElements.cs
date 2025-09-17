using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.notas
{
    public class PagamentosNotasElements
    {
        public string MenuNotas { get; } = "(//a[@class='nav-link']//p[text()='Notas'])[1]";
        public string PaginaPagamentosNotas { get; } = "(//p[text()='Pagamentos Notas'])[1]";

    }
}
