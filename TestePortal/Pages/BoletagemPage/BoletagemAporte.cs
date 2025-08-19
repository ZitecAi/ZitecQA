using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Segment.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Microsoft.Playwright.Assertions;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages.BoletagemPage
{
    public class BoletagemAporte
    {
        public static async Task<Model.Pagina> Aporte(IPage Page, NivelEnum nivelLogado)
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
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.BaixarExcel = await Utils.Excel.BaixarExcel(Page);

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        var apagarBoletagemAporte2 = Repository.BoletagemAporte.BoletagemAporteRepository.ApagarBoletagemAporte("Jessica Tavares", "COTAFIXA");
                        await Page.Locator("//button[text()='Novo +']").ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).FillAsync("30/08/2024");
                        await Page.Locator("#ValorAporte").ClickAsync();
                        await Page.Locator("#ValorAporte").FillAsync("R$10");
                        await Page.Locator("#CPFCotista").ClickAsync();
                        await Page.Locator("#CPFCotista").FillAsync("49624866830");
                        await Page.Locator("#NomeCotista").ClickAsync();
                        await Page.Locator("#NomeCotista").FillAsync("Jessica Tavares");
                        await Page.Locator("#tipoCota").SelectOptionAsync("COTAFIXA");
                        await Page.Locator("#valorCota").FillAsync("100");
                        await Page.Locator("#Fundos").SelectOptionAsync("54638076000176");
                        await Page.Locator("#fileBoleta").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "documentosteste.zip" });
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(500);
                        var msgPresente = Expect(Page.GetByText("Boleta recebida com sucesso!")).ToBeVisibleAsync();




                        var BoletagemAporteExiste = Repository.BoletagemAporte.BoletagemAporteRepository.VerificaExistenciaBoletagemAporte("Jessica Tavares", "COTAFIXA");

                        if (BoletagemAporteExiste && msgPresente != null)
                        {
                            Console.WriteLine("Boleta adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            await Page.Locator("#tabelaBoletas_filter").ClickAsync();

                            await Page.Locator("//div[@id='tabelaBoletas_filter']//input").FillAsync("Zitec Tecnologia");
                            await Page.Locator("//button[@title='Remover Boleta']").ClickAsync();
                            await Page.Locator("//button[text()='Excluir']").ClickAsync();
                            var apagarBtn = Expect(Page.GetByText("Boleta excluída com sucesso.")).ToBeVisibleAsync();
                            await Page.Locator("//div[@id='tabelaBoletas_filter']//input").FillAsync("Zitec Tecnologia");

                            string tabela = "#tabelaBoletas";

                            await Page.WaitForSelectorAsync(tabela, new PageWaitForSelectorOptions
                            {
                                State = WaitForSelectorState.Visible
                            });

                            var locator = Page.Locator(tabela);
                            int count = await locator.CountAsync();

                            bool textoEncontrado;

                            for (int i = 0; i < count; i++)
                            {
                                var texto = await locator.Nth(i).InnerTextAsync();

                                if (!string.IsNullOrWhiteSpace(texto) && texto.Contains("Zitec Tecnologia", StringComparison.OrdinalIgnoreCase))
                                {
                                    textoEncontrado = true;
                                    Console.WriteLine($"❌ Texto indesejado encontrado: {texto}");
                                    pagina.Excluir = "❌";
                                    break;
                                }
                            }

                            if (apagarBtn != null)
                            {
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                try
                                {

                                    var apagarBoletagemAporte = Repository.BoletagemAporte.BoletagemAporteRepository.ApagarBoletagemAporte("Jessica Tavares", "COTAFIXA");
                                    Console.WriteLine("Boleta apagado pelo banco");
                                    pagina.Excluir = "❌";
                                }
                                catch
                                {
                                    Console.WriteLine("Não foi possível apagar Boleta");
                                    pagina.Excluir = "❌";
                                    errosTotais++;
                                }
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
