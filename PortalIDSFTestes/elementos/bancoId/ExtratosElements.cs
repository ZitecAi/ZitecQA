using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.bancoId
{
    public class ExtratosElements
    {
        public string MenuBancoId { get; } = "//p[text()='Banco ID']";
        public string PaginaExtratos { get; } = "//p[text()='Extratos']";
        public string BtnGerarExtrato { get; } = "//button[@onclick]//i";
        public string SelectFundo { get; } = "#FundoFiltroExtrato";
        public string BtnGerar { get; } = "#Gerar";


    }
}
