using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TestePortal.Utils;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages.BancoIdPage
{
    public class BancoIdExtratos
    {
        public static async Task<Model.Pagina> Extratos (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var BancoIdExtratos = await Page.GotoAsync(portalLink + "/Relatorios/BancoID.aspx");

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
                    pagina.Acentos = Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    string seletorGerarExtrato = ".modal-content";
                    //await Page.PauseAsync();

                    await Page.GetByRole(AriaRole.Button, new() { Name = " Gerar Extrato" }).ClickAsync();
                    pagina.Acentos = Utils.Acentos.ValidarAcentosDeAlgumElemento(Page, seletorGerarExtrato).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    await Page.Locator("#FundoFiltroExtrato").SelectOptionAsync(new[] { "53300608000106" });
                    bool sucesso = await Excel.BaixarExtrato(Page);

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

                    //await Task.Delay(1000);
                    //await Page.GetByText("Relatório gerado com sucesso").ClickAsync();
                    var contaEscrowCriada = await Page.WaitForSelectorAsync("text=Relatório gerado com sucesso", new PageWaitForSelectorOptions

                    {

                        Timeout = 90000

                    });


                    

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Extratos no tópico Banco ID ");
                    Console.WriteLine(BancoIdExtratos.Status);
                    listErros.Add("Erro ao carregar a página de Extratos no tópico Banco ID ");
                    pagina.Nome = "Extratos";
                    pagina.StatusCode = BancoIdExtratos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                   
                }

            }
            catch (TimeoutException ex) {
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
