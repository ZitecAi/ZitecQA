using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.operacoes
{
    public class BaixasLoteElements
    {
        public string MenuOperacoes { get; } = "(//p[text()='Operações'])[2]";
        public string PaginaBaixasLote { get; } = "//p[text()='Baixas em Lote']";
    }
}
