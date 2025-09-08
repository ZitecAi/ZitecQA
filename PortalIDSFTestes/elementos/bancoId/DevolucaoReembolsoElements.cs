using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.bancoId
{
    public class DevolucaoReembolsoElements
    {
        public string MenuAdministrativo { get; } = "//p[text()='Banco ID']";
        public string PaginaDevolucaoReembolsos { get; } = "//p[text()='Devolução/Reembolso']";

    }
}
