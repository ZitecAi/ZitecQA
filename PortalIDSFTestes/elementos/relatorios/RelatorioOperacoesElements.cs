using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.relatorios
{
    public class RelatorioOperacoesElements
    {

        public string MenuRelatorios { get; } = "//p[text()='Relatórios']";
        public string PaginaRelatorioOperacoes { get; } = "(//p[text()='Operações'])[4]";


    }
}
