using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static TestePortal.Model.Usuario;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages.BoletagemPage
{
    public class BoletagemAporte
    {
        public static async Task<Model.Pagina> Aporte (IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var BoletagemAporte = await Page.GotoAsync(portalLink + "/Boleta/Boleta.aspx");

                if (BoletagemAporte.Status == 200)
                {
                    string seletorTabela = "#tabelaBoletas";
                    // string seletorBotao = "#exportarButton";


                    Console.Write("Aporte - Boletagem : ");
                    pagina.Nome = "Aporte";
                    pagina.StatusCode = BoletagemAporte.Status;
                    pagina.Reprovar = "❓";
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
                    pagina.BaixarExcel = "❌";

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }// botão duplicado na página.  //exportarButton

                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        var apagarBoletagemAporte2 = Repository.BoletagemAporte.BoletagemAporteRepository.ApagarBoletagemAporte("teste de cadastro", "COTAFIXA");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).FillAsync("09/07/2025");
                        await Page.Locator("#ValorAporte").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#ValorAporte").FillAsync("R$1.0000");
                        await Task.Delay(200);
                        await Page.Locator("#CPFCotista").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#CPFCotista").FillAsync("118.385.290-80");
                        await Task.Delay(200);
                        await Page.Locator("#NomeCotista").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#NomeCotista").FillAsync("teste de cadastro");
                        await Task.Delay(200);
                        await Page.Locator("#tipoCota").SelectOptionAsync(new[] { "COTAFIXA" });
                        await Task.Delay(200);
                        await Page.Locator("#valorCota").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#valorCota").FillAsync("20");
                        await Task.Delay(200);
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "54638076000176" });
                        await Task.Delay(200);
                        await Page.Locator("#fileBoleta").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "documentosteste.zip" });
                      
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(500);

                        var BoletagemAporteExiste = Repository.BoletagemAporte.BoletagemAporteRepository.VerificaExistenciaBoletagemAporte("teste de cadastro", "COTAFIXA");

                        if (BoletagemAporteExiste)
                        {
                            Console.WriteLine("Boleta adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarBoletagemAporte = Repository.BoletagemAporte.BoletagemAporteRepository.ApagarBoletagemAporte("teste de cadastro", "COTAFIXA");

                            if (apagarBoletagemAporte)
                            {
                                Console.WriteLine("Boleta apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Boleta");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Boleta");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    else
                    {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Aporte no tópico Boletagem ");
                    pagina.Nome = "Aporte";
                    pagina.StatusCode = BoletagemAporte.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }

            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
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
