using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Model
{
    public class Operacoes
    {
        public string NovoNomeArquivo { get; set; }

        public string StatusTrocados { get; set; }

        public int totalErros { get; set; }

        public string AprovacoesRealizadas { get; set; }

        public List<string> ListaErros { get; set; }
        public Operacoes()
        {
            ListaErros = new List<string>();
        }

    }
}
