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
    public class AdministrativoBancos
    {
        public static async Task<Models.Pagina> CadastroBancos(IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var fechamentoFundo = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/admin/banks");

                if (fechamentoFundo.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro bancos: ");
                    pagina.Nome = "Cadastro bancos";
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

                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    //await Page.GetByLabel("Nome").ClickAsync();
                    //await Page.GetByLabel("Nome").FillAsync("banco teste");
                    //await Page.GetByText("Número").ClickAsync();
                    //await Page.GetByLabel("Número").FillAsync("123");
                    //await Page.GetByLabel("Descrição").ClickAsync();
                    //await Page.GetByLabel("Descrição").FillAsync("testee");
                    //await Page.GetByLabel("Aceita Pix").ClickAsync();
                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";

                }
                else
                {

                    Console.Write("Erro ao carregar a página de cadastro de bancos.");
                    pagina.Nome = "Cadastro bancos";
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
