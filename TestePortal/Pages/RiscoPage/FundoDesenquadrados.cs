using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages.RiscoPage
{
    public class FundoDesenquadrados
    {

        public static async Task<Model.Pagina> FundosDesenquadrados(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;


            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var escrowExterno = await Page.GotoAsync(portalLink + "/Risco/FundosDesenquadrados.aspx");

                if (escrowExterno.Status == 200)
                {

                    String seletorTabela = "#tabelaDesenquadrameento";


                    Console.Write("Risco - Fundos Desenquadrados:  ");
                    pagina.StatusCode = escrowExterno.Status;
                    pagina.Nome = "Risco/Fundos Desenquadrados";
                    listErros.Add("0");
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        //await Page.PauseAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                        await Page.GetByLabel("Default select example").SelectOptionAsync(new[] { "53300608000106" });
                        await Page.GetByLabel("Fundo: *").FillAsync("2025-06-20");
                        await Page.Locator("#MotivoDesenquad").ClickAsync();
                        await Page.Locator("#MotivoDesenquad").FillAsync("Testar Fluxo");
                        await Page.Locator("#planoAcao").ClickAsync();
                        await Page.Locator("#planoAcao").FillAsync("Apenas Teste");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();
                        //await Page.GetByText("Desenquadramento Adicionado!").ClickAsync();


                        var verificarFundoDesenquadradoCriado = await Page.WaitForSelectorAsync("text=Desenquadramento Adicionado!", new PageWaitForSelectorOptions

                        {

                            Timeout = 9000

                        });


                        var verificarExistenciaFundoDesenquadradoCriado = Repository.Risco.FundosDesenquadradosRepository.VerificarExistenciaFundoDesenquadrado("QA","Testar Fluxo");

                        if (verificarExistenciaFundoDesenquadradoCriado)
                        {
                            Console.WriteLine("Fundo Desenquadrado Adicionado com sucesso na Tabela.");
                            pagina.InserirDados = "✅";
                        }

                        var excluirFundoDesenquadrado = Repository.Risco.FundosDesenquadradosRepository.ApagarFundoDesenquadrado("QA", "Testar Fluxo");
                        if (excluirFundoDesenquadrado)
                        {
                            Console.WriteLine("FundoDesenquadrado Excluido com sucesso da Tabela.");
                            pagina.Excluir = "✅";
                        }
                        pagina.Excluir = "?";
                        pagina.Reprovar = "?";
                        pagina.BaixarExcel = "?";





                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                return pagina;

            }

            return pagina;
        }


    }
}
