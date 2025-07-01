using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages.OperacoesPage
{
    public class OperacoesLastros2
    {

        public static async Task<Model.Pagina> EnviarLastros2(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var OperacoesEnviarLastros = await Page.GotoAsync(portalLink + "/Operacoes/Lastros.aspx");

                if (OperacoesEnviarLastros.Status == 200)
                {
                    string seletorTabela = "#tabelaLastros_wrapper";

                    try
                    {

                        Console.Write("Operações - Enviar Lastros 2.0 : ");
                        pagina.Nome = "Operações Enviar Lastros 2.0";
                        pagina.StatusCode = OperacoesEnviarLastros.Status;
                        pagina.BaixarExcel = "❓";
                        pagina.Reprovar = "❓";
                        await Task.Delay(500);
                        pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                        pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                        if (pagina.Listagem == "❌")
                        {
                            errosTotais++;
                        }
                        if (pagina.Acentos == "❌")
                        {
                            errosTotais++;
                        }
                        var dataAtual = DateTime.Now.ToString("yyyy-MM-dd");

                        var apagarLastro2 = Repository.Lastros.LastrosRepository.ApagarLastros("36614123000160", "teste jessica");

                        //await Page.PauseAsync();

                        //await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Lastro" }).ClickAsync();
                        //await Page.Locator("#FundosPDF").SelectOptionAsync(new[] { "36614123000160" });
                        //await Task.Delay(300);
                        //await Page.GetByLabel("Data da Operação:*").FillAsync("2025-06-23");
                        //await Task.Delay(300);
                        //await Page.Locator("#fileEnviarLastrosPDF").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "Arquivo teste.zip" });
                        //await Task.Delay(300);
                        //await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        //await Task.Delay(300);
                        //await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("Observação Teste");
                        //await Task.Delay(300);
                        //await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        
                        

                        //await Task.Delay(300);
                        //var lastroExiste = Repository.Lastros.LastrosRepository.VerificaExistenciaLastros("36614123000160", "Observação Teste");

                        //if (lastroExiste)
                        //{
                        //    Console.WriteLine("Lastro cadastrado na tabela.");
                        //    await Task.Delay(300);
                        //    pagina.InserirDados = "✅";
                        //    var apagarLastro = Repository.Lastros.LastrosRepository.ApagarLastros("36614123000160", "Observação Teste");

                        //    if (apagarLastro)
                        //    {
                        //        Console.WriteLine("Lastro apagado com sucesso");
                        //        pagina.Excluir = "✅";


                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("Não foi possível apagar lastro ");

                        //        pagina.Excluir = "❌";
                        //        errosTotais++;
                        //    }

                        //}
                        //else
                        //{
                        //    Console.WriteLine("Não foi possível inserir lastro");
                        //    pagina.InserirDados = "❌";
                        //    pagina.Excluir = "❌";
                        //    errosTotais += 2;

                        //}

                    }
                    catch (TimeoutException ex)

                    {
                        Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                        Console.WriteLine($"Exceção: {ex.Message}");
                        errosTotais++;
                        await Page.GotoAsync(portalLink + "/login.aspx");

                    }

                }

                else
                {
                    Console.Write("Erro ao carregar a página de Enviar Lastros 2.0 no tópico Operações: ");
                    pagina.Nome = "Operações - Enviar Lastro";
                    pagina.StatusCode = OperacoesEnviarLastros.Status;
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
