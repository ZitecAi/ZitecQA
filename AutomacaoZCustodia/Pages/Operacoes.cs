using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Azure;

namespace AutomacaoZCustodia.Pages
{
    internal class Operacoes
    {

        public static async Task<Models.Pagina> OperacoesCnab(IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            string caminhoArquivo = @"C:\Temp\Arquivos\CNABz.txt";
            await Page.WaitForLoadStateAsync();


            try
            {
                var cadastroOperacoes = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/operations");

                if (cadastroOperacoes.Status == 200)

                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Operacoes: ");
                    pagina.Nome = "Operacoes";
                    pagina.StatusCode = cadastroOperacoes.Status;
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
                    // pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    //await Page.GetByLabel("Fundo").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "FUNDO QA" }).ClickAsync();
                    //await Utils.AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);

                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";


                   

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operacoes");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = cadastroOperacoes.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://custodia.idsf.com.br/home/dashboard");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
