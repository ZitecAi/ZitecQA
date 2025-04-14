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
    public class AdministrativoRamoAtividade
    {
        public static async Task<Models.Pagina> RamoAtividade (IPage Page) 
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var ramoAtividade = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/admin/branch-activity");

                if (ramoAtividade.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro ramo de atividade: ");
                    pagina.Nome = "Cadastro ramo de atividade";
                    pagina.StatusCode = ramoAtividade.Status;
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
                    await Task.Delay(400);
                    await Page.Locator(".mat-mdc-form-field-infix").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").FillAsync("Ramo de atividade teste");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                 



                    var cadRamoAtividade = AutomacaoZCustodia.Repository.RamoAtividadeRepository.VerificarRamoAtividade("Ramo de atividade teste");

                    if (cadRamoAtividade)
                    {

                        pagina.InserirDados = "✅";
                       
                        var apagarRamoAtividade = AutomacaoZCustodia.Repository.RamoAtividadeRepository.ApagarRamoAtividade("Ramo de atividade teste");

                        if (apagarRamoAtividade == false)
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
                    pagina.StatusCode = ramoAtividade.Status;
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
