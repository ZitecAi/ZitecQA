using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestePortalInterno.Pages
{
    public class OperacoesEnviarLastros
    {
        public static async Task<Model.Pagina> EnviarLastros(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var OperacoesEnviarLastros = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Operacoes/EnviarLastros.aspx");

                if (OperacoesEnviarLastros.Status == 200)
                {
                    string seletorTabela = "#tabelaLastros";

                    try
                    {

                        Console.Write("Operações - Enviar Lastros : ");
                        pagina.Nome = "Operações - Enviar Lastros";
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

                        await Page.Locator("i.fas.fa-file-pdf").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "36614123000160" });
                        await Page.Locator("#dataOperacao").FillAsync(dataAtual);
                        await Task.Delay(300);
                        await Page.Locator("#fileEnviarLastros").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste.zip" });
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste jessica");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(100);

                        await Task.Delay(300);
                        var lastroExiste = Repository.Lastros.LastrosRepository.VerificaExistenciaLastros("36614123000160", "teste jessica");

                        if (lastroExiste)
                        {
                            Console.WriteLine("Lastro cadastrado na tabela.");
                            await Task.Delay(300);
                            pagina.InserirDados = "✅";
                            var apagarLastro = Repository.Lastros.LastrosRepository.ApagarLastros("36614123000160", "teste jessica");

                            if (apagarLastro)
                            {
                                Console.WriteLine("Lastro apagado com sucesso");
                                pagina.Excluir = "✅";


                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar lastro ");

                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir lastro");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;

                        }

                    }
                    catch (TimeoutException ex)

                    {
                        Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                        Console.WriteLine($"Exceção: {ex.Message}");
                        errosTotais++;
                        await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/login.aspx");

                    }

                }

                else
                {
                    Console.Write("Erro ao carregar a página de Enviar Lastros no tópico Operações: ");
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
