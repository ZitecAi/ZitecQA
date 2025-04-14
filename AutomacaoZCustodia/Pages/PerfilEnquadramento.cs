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
    public class PerfilEnquadramento
    {
        public static async Task<Models.Pagina> PerfilEnquadr(IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();
            try
            {
                var perfilEnqua = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/enquadramento/perfil-enquadramento");

                if (perfilEnqua.Status == 200)
                {

                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Perfil Enquadramento: ");
                    pagina.Nome = "Perfil Enquadramento";
                    pagina.StatusCode = perfilEnqua.Status;
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

                    string inputValue = "teste";
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Novo Perfil").Locator("#input-nomePerfil").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Novo Perfil").Locator("#input-nomePerfil").FillAsync("perfil enquadramento teste");
                    await Task.Delay(400);
                    await Page.GetByLabel("Novo Perfil").Locator("#input-descricaoPerfil").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Novo Perfil").Locator("#input-descricaoPerfil").FillAsync("teste de cadastro");
                    await Task.Delay(400);
                    await Page.GetByLabel("Regras").Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "CHEQUE", Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator(".cdk-overlay-container > div:nth-child(3)").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();

                    var idPErfilEnquad = Repository.PerfilEnquadramentoRepository.ObterIdEnqPerfil("teste de cadastro", "perfil enquadramento teste");

                    if (idPErfilEnquad != 0)
                    {
                        pagina.InserirDados = "✅";
                        var deletarAss = Repository.PerfilEnquadramentoRepository.DeletarRegraAssociado(idPErfilEnquad);
                        var deletarPerfilEnq = Repository.PerfilEnquadramentoRepository.DeletarEnqPerfil(idPErfilEnquad);

                        if (deletarAss && deletarPerfilEnq)
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
                    Console.Write("Erro ao carregar a página de Perfil Enquadramento.");
                    pagina.Nome = "Perfil Enquadramento";
                    pagina.StatusCode = perfilEnqua.Status;
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
