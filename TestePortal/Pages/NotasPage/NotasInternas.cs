using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages.NotasPage
{
    public class NotasInternas
    {
        public static async Task<Model.Pagina> NotassInternas (IPage Page, IConfiguration config)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = config["Links:Portal"];
                var NotasInternas = await Page.GotoAsync(portalLink + "/Notas/NotasInternas.aspx");


                if (NotasInternas.Status == 200)
                {
                    string seletorTabela = "#tabelaCedentes";


                    Console.Write("Notas Internas - Notas : ");
                    pagina.Nome = "Notas Internas";                  
                    pagina.StatusCode = NotasInternas.Status;
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
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;
                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();


                }
                else
                {
                    Console.Write("Erro ao carregar a página de Notas Internas no tópico Notas ");
                    pagina.Nome = "Notas internas";
                    pagina.StatusCode = NotasInternas.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex) 
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
