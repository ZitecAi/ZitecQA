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
    public class AdministrativoAlterarDtFundo
    {
        public static async Task<Models.Pagina> AlterarDtFundo(IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var alteracaoDtFundo = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/funds/date-alteration");
                //await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                if (alteracaoDtFundo.Status == 200)
                { 
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Alteração dt fundo: ");
                    pagina.Nome = "Alteração dt fundo";
                    pagina.StatusCode = alteracaoDtFundo.Status;
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

                    DateTime dataAlteracao = AutomacaoZCustodia.Utils.ObterListaFeriados.ObterProximoDiaUtil(DateTime.Now.AddDays(-1));
                    string dataFormatada = dataAlteracao.ToString("dd/MM/yyyy");
                    await Task.Delay(400);
                    await Page.GetByPlaceholder("Nome").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByPlaceholder("Nome").FillAsync("Zitec");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Row, new() { Name = "/0001-76 Zitec FIDC" }).GetByLabel("").CheckAsync();
                    await Page.GetByLabel("Regredir Para").FillAsync(dataFormatada);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Alterar" }).ClickAsync();
                    await Task.Delay(40000);
                    var dataAlterada = AutomacaoZCustodia.Repository.FechamentoFundoRepository.VerificarDataFundo(9991, dataAlteracao);

                    if (dataAlterada)
                    {
                        pagina.InserirDados = "✅";
                        pagina.Excluir = "❓";
                        Console.WriteLine("Data alterada");

                    }
                    else 
                    {

                        var dataAlterada2 = AutomacaoZCustodia.Repository.FechamentoFundoRepository.VerificarDataFundo(9991, dataAlteracao);

                        if (dataAlterada)
                        {
                            pagina.InserirDados = "✅";
                            pagina.Excluir = "❓";
                            Console.WriteLine("Data alterada");

                        }
                        else
                        {
                            Console.WriteLine("erro ao alterar data");
                            pagina.Excluir = "❌";
                            pagina.InserirDados = "❌";
                            Console.WriteLine("erro ao alterar data");
                        }

                    }


                }
                else
                {
                    Console.Write("Erro ao carregar a página de alteração data fundo");
                    pagina.Nome = "Alteração data fundo";
                    pagina.StatusCode = alteracaoDtFundo.Status;
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
