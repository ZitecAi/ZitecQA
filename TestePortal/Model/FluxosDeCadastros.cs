using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Model
{
    public class FluxosDeCadastros
    {

        public string Fluxo  { get; set; }
        public string Formulario { get; set; }

        public string StatusEmAnalise { get; set; }

        public string FormularioCompletoNoPortal { get; set; }

        public int TotalErros { get; set; }
        public List<string> ListaErros { get; set; }

        public string DocumentoAssinado { get; set; }

        public string EmailRecebido { get; set; }

        public string statusAprovado { get; set; }
        public FluxosDeCadastros()
        {
            ListaErros = new List<string>();
        }

    }
}
