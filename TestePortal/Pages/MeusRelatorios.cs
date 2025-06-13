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
    public class MeusRelatorios
    {
        public static async Task<Model.Pagina> Relatorios (IPage Page, IConfiguration config)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = config["Links:Portal"];
                var MeusRelatorios = await Page.GotoAsync(portalLink + "/Relatorios/MeusRelatorios.aspx");

                if (MeusRelatorios.Status == 200)
                {
                    string seletorTabela = "#tabelaRelatorio";

                    Console.Write("Meus Relatorios  - Relatorios : ");
                    pagina.Nome = "Meus Relatorios - Relatorios";
                    pagina.StatusCode = MeusRelatorios.Status;
                    pagina.BaixarExcel = "❓ ";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página Meus Relatórios : ");
                    pagina.Nome = "Meus Relatórios - Relatorios";
                    pagina.StatusCode = MeusRelatorios.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex) {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
