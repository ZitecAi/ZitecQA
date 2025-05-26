using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TestePortalInterno.Utils;

namespace TestePortalInterno.Pages
{
    public class BancoIdExtratos
    {
        public static async Task<Model.Pagina> Extratos(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var BancoIdExtratos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Relatorios/BancoID.aspx");

                if (BancoIdExtratos.Status == 200)
                {
                    Console.Write("Extratos - Banco ID: ");
                    Console.WriteLine(BancoIdExtratos.Status);
                    pagina.StatusCode = BancoIdExtratos.Status;
                    pagina.Nome = "Extratos";

                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Excluir = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    await Page.GetByRole(AriaRole.Button, new() { Name = " Gerar Extrato" }).ClickAsync();
                    await Page.Locator("#FundoFiltroExtrato").SelectOptionAsync(new[] { "54638076000176" });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Gerar", Exact = true }).ClickAsync();

                    //bool sucesso = await BaixarExtrato.BaixarRelatorio(Page);
                    bool sucesso = false;
                    if (sucesso)
                    {
                        Console.WriteLine("Teste de download do relatório passou!");
                        pagina.BaixarExcel = "✅";
                    }
                    else
                    {
                        Console.WriteLine("Teste de download do relatório falhou!");
                        pagina.BaixarExcel = "❌";
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Devolução/Reembolso no tópico Banco ID ");
                    Console.WriteLine(BancoIdExtratos.Status);
                    listErros.Add("Erro ao carregar a página de Devolução/Reembolso no tópico Banco ID ");
                    pagina.Nome = "Extratos";
                    pagina.StatusCode = BancoIdExtratos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");

                }

            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}
