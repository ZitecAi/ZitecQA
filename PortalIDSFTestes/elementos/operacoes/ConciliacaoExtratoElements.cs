using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.operacoes
{
    public class ConciliacaoExtratoElements
    {
        public string MenuOperacoes { get; } = "(//p[text()='Operações'])[2]";
        public string PaginaConciliacaoExtrato { get; } = "//p[text()='Conciliação & Extrato']";
    }
}
