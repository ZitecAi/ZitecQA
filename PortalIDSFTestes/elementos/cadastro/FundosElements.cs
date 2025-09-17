using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.cadastro
{
    public class FundosElements
    {
        public string MenuCadastro { get; } = "(//p[text()='Cadastro'])[1]";
        public string PaginaFundos { get; } = "(//p[text()='Fundos'])[1]";



    }
}
