using Azure;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AutomacaoZCustodia.Utils;

namespace AutomacaoZCustodia.Pages
{
    public class AdministrativoFechamentoFundo
    {
        public static async Task<Models.Pagina> FechamentoFundo (IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();

            try
            {
                var fechamentoFundo = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/funds/closing");

                if (fechamentoFundo.Status == 200)
                {

                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";
                    
                    Console.Write("Fechamento fundo: ");
                    pagina.Nome = "Fechamento fundo";
                    pagina.StatusCode = fechamentoFundo.Status;
                    pagina.Acentos = Utils.VerificarAcentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Utils.VerificarListagem.Listagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                    DateTime dataAtual = AutomacaoZCustodia.Utils.ObterListaFeriados.ObterProximoDiaUtil(DateTime.Now.AddDays(+2));
                    string dataFormatada = dataAtual.ToString("dd/MM/yyyy");
                    
                    
                    
                    await Page.Locator(".mat-mdc-form-field-infix").First.ClickAsync();
                    await Page.GetByLabel("Nome").FillAsync("Zi");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Row, new() { Name = "9991 54.638.076/0001-76 Zitec" }).GetByLabel("").CheckAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Data até").FillAsync(dataFormatada);
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Fechar" }).ClickAsync();
                    await Task.Delay(50000);

                    var verificarDataFundo = AutomacaoZCustodia.Repository.FechamentoFundoRepository.VerificarDataFundo(9991, dataAtual);

                    if (verificarDataFundo)
                    {
                        pagina.InserirDados = "✅";
                        pagina.Excluir = "❓";
                        pagina.BaixarExcel = "❓";
                        Console.WriteLine("Fundo fechado");
                    }
                    else 
                    {
                        Console.WriteLine("erro ao fechar fundo");
                        pagina.BaixarExcel = "❓";
                        pagina.Excluir = "❌";
                        pagina.InserirDados = "❌";
                    }
                }
                else 
                {
                    Console.Write("Erro ao carregar a página de fechamento de fundo.");
                    pagina.Nome = "Fechamento fundo";
                    pagina.StatusCode = fechamentoFundo.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://custodia.idsf.com.br/home/dashboard");
                }


            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                return pagina;

            }

            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}
