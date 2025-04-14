using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomacaoZCustodia.Models
{
    public class Operacoes
    {
        public int Id { get; set; }
        public string NovoNomeArquivo { get; set; }
        public string TipoOperacao { get; set; }

        public string AprovacaoGestora { get; set; }

        public string AprovacaoConsultoria { get; set; }

        public string InsertOperacao { get; set; }

        public int totalErros{ get; set; }
     
        public List<string> ListaErros1 { get; set; }
       
    }
}
