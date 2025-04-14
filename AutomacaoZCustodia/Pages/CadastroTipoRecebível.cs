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
    public class CadastroTipoRecebível
    {
        public static async Task<Models.Pagina> CadastroNvRecebivel(IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var novoRecebivel = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/receivables");

                if (novoRecebivel.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro novo recebível: ");
                    pagina.Nome = "Cadastro novo recebível";
                    pagina.StatusCode = novoRecebivel.Status;
                    pagina.Acentos = Utils.VerificarAcentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    //pagina.Listagem = Utils.VerificarListagem.Listagem(Page, seletorTabela).Result;

                    //if (pagina.Listagem == "❌")
                    //{
                    //    errosTotais++;
                   //
                   //}
                    pagina.Listagem = "❌";
                    pagina.BaixarExcel = "❓";
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Tipo de Recebível" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Tipo Recebível" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Nome Tipo Recebível").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome Tipo Recebível").FillAsync("teste de cadastro");
                    await Task.Delay(400);
                    await Page.GetByText("Dias de Valorização Vencido").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Dias de Valorização Vencido").FillAsync("50");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                    var tipoRecebivelExiste = AutomacaoZCustodia.Repository.TipoRecebivelRepository.VerificarExistenciaTpRec("teste de cadastro");

                    if(tipoRecebivelExiste)
                    {
                        pagina.InserirDados = "✅";

                        var apagarTipoRecebivel = AutomacaoZCustodia.Repository.TipoRecebivelRepository.ApagarTipoRecebivel("teste de cadastro");
                        if (apagarTipoRecebivel == false)
                        {
                            pagina.Excluir = "✅";


                        }
                        else
                        {
                        pagina.Excluir = "❌";



                        }

                    } else
                    {
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";


                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de cadastro de novo recebível.");
                    pagina.Nome = "Cadastro tipo recebível";
                    pagina.StatusCode = novoRecebivel.Status;
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
