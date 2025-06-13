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
    public class OperacoesBaixaEmLote
    {
        public static async Task<Model.Pagina> BaixaLote (IPage Page, IConfiguration config)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = config["Links:Portal"];
                var OperacoesBaixaLote = await Page.GotoAsync(portalLink + "/Operacoes/Contratos.aspx");

                if (OperacoesBaixaLote.Status == 200)
                {
                    string seletorTabela = "#tabelaLastros";

                    Console.Write("Operações - Baixa em Lote : ");
                    pagina.Nome = "Operações - Baixa em Lote";                  
                    pagina.StatusCode = OperacoesBaixaLote.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = "❓";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de baixas de lote no tópico Operações: ");
                    pagina.Nome = "Operações - baixas de lote";                   
                    pagina.StatusCode = OperacoesBaixaLote.Status;
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
