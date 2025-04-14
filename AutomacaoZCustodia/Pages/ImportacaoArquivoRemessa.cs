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
    public class ImportacaoArquivoRemessa
    {
        public static async Task<Models.Pagina> ImpArquivoRemessa (IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var arquivoRemessa = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/importation/shipping-file");

                if (arquivoRemessa.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Importação Arquivo Remessa: ");
                    pagina.Nome = "Importação Arquivo Remessa";
                    pagina.StatusCode = arquivoRemessa.Status;
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



                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Importar" }).ClickAsync();
                    //await Page.GetByRole(AriaRole.Combobox, new() { Name = "Fundo" }).Locator("span").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Zitec FIDC" }).ClickAsync();
                    //pagina.Listagem = Utils.VerificarListagem.Listagem(Page, seletorTabela).Result;

                    //if (pagina.Listagem == "❌")
                    //{
                    //    errosTotais++;
                    //}
                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Adicione um arquivo..." }).ClickAsync();

                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❌";
                    pagina.Excluir = "❌";

                }
                else
                {
                    Console.Write("Erro ao carregar a página de arquivo remessa.");
                    pagina.Nome = "Arquivo Remessa";
                    pagina.StatusCode = arquivoRemessa.Status;
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
