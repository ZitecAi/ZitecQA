using AutomacaoZCustodia.Models;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutomacaoZCustodia.Utils
{
    public class VerificarAcentos
    {
        public static async Task<string> ValidarAcentos(IPage Page)
        {
            string Acentos = "";
            var pagina = new Models.Pagina();
            string[] textos = { "Ã£", "Ã§", "Ü", "Ã¡", "Ã©", "Ã¨", "Ã§Ã£", "Ã§Ã" };
            var contadores = new Dictionary<string, int>();
            var listErros = new List<string>();
            int errosTotais = 0;

            foreach (var texto in textos)
            {
                int count = await Page.GetByText(texto).CountAsync();
                contadores[texto] = count;
            }

            foreach (var item in contadores)
            {
                if (item.Value > 0)
                {
                    Console.WriteLine($"O texto '{item.Key}' foi encontrado na página.");
                    Acentos = "❌";
                    listErros.Add("Erro de acentuação");
                    errosTotais++;
                    pagina.TotalErros = errosTotais;
                    break;
                }
                else
                {
                    Acentos = "✅";
                    Console.WriteLine($"Nenhum dos textos '{item.Key}' foi encontrado na página.");
                }
            }
            if (errosTotais == 0)
            {
                listErros.Add("0");
            
            }
            return Acentos;
        }
    }
}
