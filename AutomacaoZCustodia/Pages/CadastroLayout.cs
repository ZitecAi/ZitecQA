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
    public class CadastroLayout
    {

        public static async Task<Models.Pagina> CadastroDeLayout(IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var novoTipoMovimento = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/receivables/layout");

                if (novoTipoMovimento.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro de layout: ");
                    pagina.Nome = "Cadastro de layout";
                    pagina.StatusCode = novoTipoMovimento.Status;
                    pagina.Acentos = Utils.VerificarAcentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    //pagina.Listagem = Utils.VerificarListagem.Listagem(Page, seletorTabela).Result;

                    //if (pagina.Listagem == "❌")
                    //{
                    //    errosTotais++;
                    //}
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Listagem = "❌";

                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    //await Page.PauseAsync();
                    //await Page.GetByLabel("Layout").Locator("path").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "CNAB500 - FIDD" }).ClickAsync();
                    //await Page.GetByLabel("Tipo Recebível").Locator("span").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Duplicata", Exact = true }).ClickAsync();
                    //await Page.GetByLabel("Código de Ocorrência").FillAsync("777");
                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                }
                else
                {

                    Console.Write("Erro ao carregar a página de cadastro de layout.");
                    pagina.Nome = "Cadastro layout";
                    pagina.StatusCode = novoTipoMovimento.Status;
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
