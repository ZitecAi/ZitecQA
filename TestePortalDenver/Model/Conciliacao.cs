using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortalDenver.Model
{
    public class Conciliacao
    {
        public string ExtratoEnviado { get; set; }

        public string AprovacaoModal { get; set; }

        public string AprovacaoEmLote { get; set; }

        public string StatusConciliado { get; set; }

        public int TotalErros { get; set; }
        public List<string> ListaErros { get; set; }

        public Conciliacao()
        {
            ListaErros = new List<string>();
        }
    }
}
