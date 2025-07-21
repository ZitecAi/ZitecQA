using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteOperacoesOperacoes.Model;

namespace TesteOperacoesOperacoes.Util
{


    public static class RegistroTestesNegativos
    {
        public static List<TesteNegativoResultado> Resultados { get; } = new();

        public static void Registrar(string idDoTeste, bool resultado)
        {
            var existente = Resultados.FirstOrDefault(x => x.IdDoTeste == idDoTeste);
            if (existente != null)
            {
                existente.Resultado = resultado ? "✅" : "❌"; // Atualiza se já existir
            }
            else
            {
                Resultados.Add(new TesteNegativoResultado
                {
                    IdDoTeste = idDoTeste,
                    Resultado = resultado ? "✅" : "❌"
                });
            }
        }
    }


}
