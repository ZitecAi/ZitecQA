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
    public class EnquadramentoPdd
    {
        public static async Task<Models.Pagina> EnquadrPdd (IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try

            {
                var EnquadramentoPdd = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/enquadramento/pdd");

                if (EnquadramentoPdd.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Enquadramento pdd: ");
                    pagina.Nome = "Enquadraamento pdd";
                    pagina.StatusCode = EnquadramentoPdd.Status;
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

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Page.GetByLabel("Nome").ClickAsync();
                    await Page.GetByLabel("Nome").FillAsync("Teste qa pdd");
                    await Page.GetByLabel("Sim").CheckAsync();
                    await Page.GetByRole(AriaRole.Tab, new() { Name = "Faixas do PDD" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo", Exact = true }).ClickAsync();
                    await Page.GetByLabel("Data de início").ClickAsync();
                    await Page.GetByLabel("Data de início").FillAsync("30/01/2025");
                    await Page.GetByLabel("Data de término").ClickAsync();
                    await Page.GetByLabel("Data de término").FillAsync("31/01/2025");
                    await Page.GetByText("Número Mínimo de Dias").ClickAsync();
                    await Page.GetByLabel("Número Mínimo de Dias").FillAsync("55");
                    await Page.GetByLabel("Número Máximo de Dias").FillAsync("100");
                    await Page.Locator("#mat-mdc-form-field-label-12 span").ClickAsync();
                    await Page.GetByLabel("Percentual Apropriado").FillAsync("10");
                    await Page.GetByLabel("Risco de Atraso").Locator("svg").ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "A" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                    var enquadramentoPddExiste = AutomacaoZCustodia.Repository.EnquadramentoPddRepository.VerificarPdd("Teste qa pdd");

                    if (enquadramentoPddExiste)
                    {
                        pagina.InserirDados = "✅";

                        var apagarEnqPdd = AutomacaoZCustodia.Repository.EnquadramentoPddRepository.DeletarPdd("Teste qa pdd");

                        if (apagarEnqPdd)
                        {
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            pagina.Excluir = "❌";
                        }


                    }
                    else
                    {
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";


                    }


                }
                else
                {
                    Console.Write("Erro ao carregar a página de cadastro de bancos.");
                    pagina.Nome = "Cadastro bancos";
                    pagina.StatusCode = EnquadramentoPdd.Status;
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
