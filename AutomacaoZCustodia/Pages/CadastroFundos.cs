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
    public class CadastroFundos
    {
        public static async Task<Models.Pagina> CadastroDeFundos(IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var novoFundo = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/funds");

                if (novoFundo.Status == 200)

                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";
                    Console.Write("Cadastro de fundo: ");
                    pagina.Nome = "Cadastro de fundo";
                    pagina.StatusCode = novoFundo.Status;
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


                    //await Page.PauseAsync();
                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Fundo" }).ClickAsync();
                    //await Page.GetByLabel("Fundo", new() { Exact = true }).ClickAsync();
                    //await Page.GetByLabel("Fundo", new() { Exact = true }).FillAsync("Fundo teste qa");
                    //await Page.Locator("#mat-mdc-form-field-label-4").GetByText("CNPJ").ClickAsync();
                    //await Page.Locator("#input-cnpj").FillAsync("53.300.608/0001-06");
                    //await Page.GetByLabel("Gestor").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "ORIGINADOR QA" }).ClickAsync();
                    //await Page.GetByLabel("Auditor Independete").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "LUGO FLY" }).ClickAsync();
                    //await Page.GetByLabel("Consultoria").Locator("svg").ClickAsync();
                    //await Page.GetByText("MM CONSULTORIA E ASSESSORIA").ClickAsync();
                    //await Page.GetByText("Código ISIN").ClickAsync();
                    //await Page.GetByLabel("Código ISIN").FillAsync("7777");
                    //await Page.GetByLabel("Tipo Fundo").Locator("path").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Renda Fixa", Exact = true }).ClickAsync();
                    //await Page.GetByLabel("Nível Risco").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Baixo" }).ClickAsync();
                    //await Page.GetByText("N° CELIC").ClickAsync();
                    //await Page.GetByLabel("N° CELIC").FillAsync("14");
                    //await Page.GetByText("Registro na CVM").ClickAsync();
                    //await Page.GetByLabel("Registro na CVM").FillAsync("12/12/2020");
                    //await Page.GetByLabel("Lastro", new() { Exact = true }).Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Nubank" }).ClickAsync();
                    //await Page.GetByLabel("Código", new() { Exact = true }).Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "1" }).ClickAsync();
                    //await Page.GetByLabel("Cheque").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "CNAB160 - Retorno de Cheque" }).ClickAsync();
                    //await Page.GetByLabel("Layout").Locator("svg").ClickAsync();
                    //await Page.GetByText("CNAB CUSTÓDIA BRADESCO - REMESSA OCORRÊNCIAS MAINFRAME").ClickAsync();
                    //await Page.GetByText("error_outline Regras").ClickAsync();
                    //await Page.GetByLabel("Nome da Regra").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "teste" }).ClickAsync();
                    //await Page.GetByLabel("Modelo de Precificação").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Por recebível" }).ClickAsync();
                    //await Page.GetByLabel("Aplica-se a").Locator("svg").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Novos Recebíveis" }).ClickAsync();
                    //await Page.GetByText("Término do Relacionamento").ClickAsync();
                    //await Page.GetByLabel("Término do Relacionamento").FillAsync("25/03/2027");
                    //await Page.GetByLabel("Não").CheckAsync();


                    //cnpj 53300608 / 0001 - 06

                }
                else
                {
                    Console.Write("Erro ao carregar a página de cadastro de entidade.");
                    pagina.Nome = "Cadastro entidade";
                    pagina.StatusCode = novoFundo.Status;
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
