using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestesOperacoesOperacoes.Model
{
    
    public class Configurações
    {
        public class linkSettings
        {
            public string Portal { get; set; }
            public string FichaConsultoria { get; set; }
            public string FichaCotista { get; set; }
            public string FichaCotistaPJ { get; set; }
            public string FichaGestora { get; set; }
            public string FichaCorrentista { get; set; }
            public string FichaCorrentistaEscrow { get; set; }
            public string FichaCotistaFundInvst { get; set; }
        }
        public class TokenSettings
        {
            public string TokenPortal { get; set; }
            public string AutentiqueApiToken { get; set; }
        }

        public class PathSettings
        {
            public string Arquivo { get; set; }
        }
    }
}

