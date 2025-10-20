using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.Boletagem
{
    public class AporteElements
    {
        public string MenuBoletagem { get; } = "(//p[text()='Boletagem'])[1]";
        public string PaginaAporte { get; } = "//p[text()='Aporte']";
        public string BtnNovo { get; } = "#tableButtonNovo";
        public string Calendario { get; } = "#dataAporte";
        public string ValorAporte { get; } = "//input[@id='ValorAporte']";
        public string CnpjCotista { get; } = "#CPFCotista";
        public string NomeCotista { get; } = "#NomeCotista";
        public string SelectCota { get; } = "#tipoCota";
        public string ValorCota { get; } = "#valorCota";
        public string TipoAporte { get; } = "#tipoAporte";
        public string SelectFundo { get; } = "#Fundos";
        public string SelectCarteira { get; } = "#Carteiras";
        public string BtnEnviar { get; } = "#submitButton";

    }
}
