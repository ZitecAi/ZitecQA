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
    public class CadastroLayoutMov
    {
        public static async Task<Models.Pagina> CadastroDeLayoutMov(IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var layoutMov = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/receivables/movement-layout ");
                if (layoutMov.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro de layout mov: ");
                    pagina.Nome = "Cadastro de layout mov";
                    pagina.StatusCode = layoutMov.Status;
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

                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❌";
                    pagina.Excluir = "❌";
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Page.Locator(".mat-mdc-form-field-infix").First.ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "CNAB ATT / FIDC" }).ClickAsync();
                    await Page.GetByLabel("Tipo de Movimento").Locator("span").ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "AQUISIÇÃO", Exact = true }).ClickAsync();
                    await Page.GetByText("Código de Ocorrência").ClickAsync();
                    await Page.GetByLabel("Código de Ocorrência").FillAsync("7777");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();



                }
                else
                {
                    Console.Write("Erro ao carregar a página de cadastro de layout mov.");
                    pagina.Nome = "Cadastro layout";
                    pagina.StatusCode = layoutMov.Status;
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
