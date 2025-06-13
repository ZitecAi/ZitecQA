using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages
{
    public class ControleInternoDiario
    {
        public static async Task<Model.Pagina> Diario(IPage Page, IConfiguration config)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();

            try
            {
                var portalLink = config["Links:Portal"];
                var InformeDiario = await Page.GotoAsync(portalLink + "/Risco/InformeDiario.aspx");

                if (InformeDiario.Status == 200)
                {
                    Console.Write("Informe Diário - Controle Interno : ");
                    Console.WriteLine(InformeDiario.Status);
                 //   pagina.ListaErros = listErros;
                    pagina.StatusCode = InformeDiario.Status;
                    pagina.Nome = "Informe Diário - Controle Interno";
                    listErros.Add("0");
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Informe Diário no tópico Controle Interno: ");
                    Console.WriteLine(InformeDiario.Status);
                    listErros.Add("Erro ao carregar a página de Informe Diário no tópico Controle Interno: ");
                    pagina.Nome = "Informe diário - Controle Interno";
                //    pagina.ListaErros = listErros;
                    pagina.StatusCode = InformeDiario.Status;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex) 
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                return pagina;
            }
            return pagina;
        }
    }
}
