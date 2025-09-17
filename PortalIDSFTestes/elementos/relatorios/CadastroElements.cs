using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.relatorios
{
    public class CadastroElements
    {

        public string MenuRelatorios { get; } = "//p[text()='Relatórios']";
        public string PaginaCadastro { get; } = "(//p[text()='Cadastro'])[2]";


    }
}
