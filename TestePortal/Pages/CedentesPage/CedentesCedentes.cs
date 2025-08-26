using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Segment.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestePortal.Pages.CedentesPage
{
    public class CedentesCedentes
    {
        bool inserirDados;

        public static async Task<Model.Pagina> CedentesPJ(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();
            pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();


            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var BoletagemCedentes = await Page.GotoAsync(portalLink + "/Cedentes.aspx");

                if (BoletagemCedentes.Status == 200)
                {
                    string seletorTabela = "#tabelaCedentes";

                    Console.Write("Cedentes PJ: ");
                    pagina.Nome = "Cedentes PJ";
                    pagina.StatusCode = BoletagemCedentes.Status;
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
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    var apagarCedente2 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "53300608000106");

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "36614123000160_49624866830_N.zip" });
                    var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                    {
                        Timeout = 10000
                    });
                    bool cedenteExiste = Repository.Cedentes.CedentesRepository.VerificaExistenciaCedente("36614123000160", "49624866830");


                    if (cedenteCadastrado != null && cedenteExiste == true)
                    {
                        Console.WriteLine("Cedente adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "49624866830");
                        if (apagarCedente)
                        {
                            Console.WriteLine("Cedente apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar cedente");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Não foi possível inserir cedente");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }                   
                    









                }


                else
                {
                    Console.Write("Erro ao carregar a página de Cedentes no tópico Boletagem ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = BoletagemCedentes.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                return pagina;
                //await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

        public static async Task<Model.Pagina> CedentesPf(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();
            pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();
            bool cedenteCadastrado;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var BoletagemCedentes = await Page.GotoAsync(portalLink + "/Cedentes.aspx");

                if (BoletagemCedentes.Status == 200)
                {
                    string seletorTabela = "#tabelaCedentes";

                    Console.Write("Cedentes PF: ");
                    pagina.Nome = "Cedentes PF";
                    pagina.StatusCode = BoletagemCedentes.Status;
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
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    var apagarCedente2 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "49624866830");


                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();

                    // Obtém o caminho base do arquivo a partir do App.config
                    string basePath = TestePortalIDSF.Program.Config["Paths:Arquivo"];
                    string fileName = "36614123000160_49624866830_N.zip";
                    string filePath = Path.Combine(basePath, fileName);
                    Console.WriteLine(filePath);

                    Console.WriteLine($"Arquivo gerado: {filePath}");


                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("ERRO: Arquivo não encontrado!");
                        throw new FileNotFoundException("Arquivo não encontrado para upload", filePath);
                    }
                    try
                    {
                        Timeout = 10000
                    });
                    bool cedenteExiste = Repository.Cedentes.CedentesRepository.VerificaExistenciaCedente("36614123000160", "49624866830");


                    if (cedenteCadastrado != null && cedenteExiste == true)
                    {
                        Console.WriteLine("Cedente adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "49624866830");
                        if (apagarCedente)
                        {
                            Console.WriteLine("Cedente apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar cedente");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir cedente");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }


                }






                else
                {
                    Console.Write("Erro ao carregar a página de Cedentes no tópico Cedentes ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = BoletagemCedentes.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel cadastrar cedente");
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                return pagina;
                //await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}
