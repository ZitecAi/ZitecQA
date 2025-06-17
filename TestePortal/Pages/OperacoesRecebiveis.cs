using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class OperacoesRecebiveis
    {
        public static async Task<Model.Pagina> Recebiveis(IPage Page, IConfiguration config)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var portalLink = config["Links:Portal"];
                var OperacoesRecebiveis = await Page.GotoAsync(portalLink + "/operacoes/recebiveis.aspx");
              

                if (OperacoesRecebiveis.Status == 200)
                {
                    Console.Write("Recebiveis  - Operações : ");
                    pagina.Nome = "Recebiveis - Operações";
                    pagina.StatusCode = OperacoesRecebiveis.Status;
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                }

                else
                {
                    Console.Write("Erro ao carregar a página de Recebiveis no tópico Operações: ");
                    pagina.Nome = "Recebiveis - Operações";
                    pagina.StatusCode = OperacoesRecebiveis.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
              
            } catch (Exception ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }

            pagina.TotalErros = errosTotais;
            return pagina; 
        }
    }
}
