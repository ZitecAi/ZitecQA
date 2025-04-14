using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
//using System.Windows.Controls;
using TestePortal.Utils;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using static TestePortal.Model.Usuario;
using TestePortal.Repository.Operacoes;
using TestePortal.Model;


namespace TestePortal.Pages
{
    public class CadastroOperacoesZitecCsv
    {
        public static async Task<(Model.Pagina pagina, Model.Operacoes operacoes)> OperacoesZitecCsv(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {

            var pagina = new Model.Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\Temp\Arquivos\CNABz.txt";
            operacoes.ListaErros3 = new List<string>();

            try
            {
                var OperacoesZitec = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "Operacoes/Operacoes2.0.aspx");

                if (OperacoesZitec.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";


                    Console.Write("Operações Zitec csv: ");
                    pagina.Nome = "Operações Zitec csv";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌") errosTotais++;
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌") errosTotais++;

                    if (nivelLogado == NivelEnum.Master)
                    {

                        string pastaArquivos = "C:/Temp/Arquivos/";
                        string nomeArquivoOriginal = "arquivoteste_operacoescsv_qa.csv";
                        string caminhoOriginal = Path.Combine(pastaArquivos, nomeArquivoOriginal);
                        string nomeArquivoModificado = ModificarArquivoCsv.ModificarCsv(caminhoOriginal, pastaArquivos);
                        string caminhoModificado = Path.Combine(pastaArquivos, nomeArquivoModificado);

                        operacoes.TipoOperacao3 = "Operacoes Zitec - csv";
                        // Envia o arquivo atualizado para o input
                        for (int i = 0; i < 2; i++)  // Tenta no máximo 2 vezes (inicial + 1 tentativa)
                        {

                            await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CSV" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#selectFundoCsv").SelectOptionAsync(new[] { "54638076000176" });
                            await Task.Delay(200);
                            await Page.Locator("#fileEnviarOperacoesCsv").SetInputFilesAsync(new[] { caminhoModificado });
                            await Task.Delay(200);
                            await Page.Locator("#fileEnviarLastro").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste.zip" });
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste de envio csv");
                            await Task.Delay(200);
                            await Page.Locator("#enviarButton").ClickAsync();
                            await Task.Delay(3000);

                            var idOperacaoRecebivel = Repository.OperacoesCsv.OperacoesCsvRepository.ObterIdOperacaoRec(nomeArquivoModificado);

                            if (idOperacaoRecebivel != 0)
                            {

                                var idOperacao = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarOperacao(idOperacaoRecebivel);
                                var (recebivelExiste, idRecebivel) = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarRecebivel(idOperacaoRecebivel);
                                var complementoRelExiste = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarRecebivelComplemento(idRecebivel);

                                if (recebivelExiste && complementoRelExiste)
                                {
                                    pagina.InserirDados = "✅";
                                    operacoes.StatusTrocados3 = "❓";
                                    operacoes.AprovacoesRealizadas3 = "❓";
                                    await Page.GetByLabel("Pesquisar").FillAsync(nomeArquivoModificado);
                                    var primeiroTr = Page.Locator("#listaCedentes tr").First;
                                    var primeiroTd = primeiroTr.Locator("td").First;
                                    await primeiroTd.ClickAsync();
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar", Exact = true }).ClickAsync();
                                    await Task.Delay(3000);

                                    var status = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarStatus(nomeArquivoModificado);

                                    if (status == "PI")
                                    {
                                        operacoes.AprovacoesRealizadas3 = "✅";
                                        operacoes.StatusTrocados3 = "✅";
                                    }
                                    else
                                    {
                                        operacoes.StatusTrocados3 = "❌";
                                        operacoes.AprovacoesRealizadas3 = "❌";
                                    }

                                    var deletarRebComp = Repository.OperacoesCsv.OperacoesCsvRepository.DeletarRecebivelComplemento(idRecebivel);
                                    var deletarArvLastro = Repository.OperacoesCsv.OperacoesCsvRepository.DeletarRecebivelLastro(idRecebivel);
                                    var deletarRecebivel = Repository.OperacoesCsv.OperacoesCsvRepository.DeletarRecebivel(idOperacaoRecebivel);
                                    var deletarOperacao = Repository.OperacoesCsv.OperacoesCsvRepository.DeletarOperacao(idOperacaoRecebivel);
                                    var deletarOpRec = Repository.OperacoesCsv.OperacoesCsvRepository.DeletarOperacaoRecebivel(idOperacaoRecebivel);

                                    if (deletarRebComp && deletarArvLastro && deletarRecebivel && deletarOperacao && deletarOpRec)
                                    {
                                        pagina.Excluir = "✅";

                                    }
                                    else
                                    {
                                        pagina.Excluir = "❌";
                                        errosTotais2++;

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Não foi possível lançar operação");
                                    pagina.InserirDados = "❌";
                                    pagina.Excluir = "❌";
                                    errosTotais += 2;
                                    operacoes.ListaErros3.Add("Operação não foi corretamente para o banco de dados");
                                }

                                break;
                            }
                            else
                            {
                                if (i == 2)
                                {
                                    Console.WriteLine("Não foi possível lançar operação");
                                    pagina.InserirDados = "❌";
                                    pagina.Excluir = "❌";
                                    errosTotais += 2;
                                    operacoes.ListaErros2.Add("Erro ao lançar operação");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Write("Erro ao carregar a página de Operações csv no tópico Operações: ");
                        pagina.Nome = "Operações zitec csv";
                        pagina.StatusCode = OperacoesZitec.Status;
                        errosTotais += 2;
                        operacoes.ListaErros3.Add("Erro ao carregar a página de operações");

                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operações no tópico Operações: ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesZitec.Status;
                    errosTotais += 2;
                    operacoes.ListaErros3.Add("Erro ao carregar a página de operações");
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (operacoes.ListaErros3.Count == 0)
                {
                    operacoes.ListaErros3.Add("0");
                }
                Console.WriteLine($"exceção: {ex}");
                operacoes.ListaErros3.Add($"Execeção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros3 = errosTotais2;
            }
            if (operacoes.ListaErros3.Count == 0)
            {
                operacoes.ListaErros3.Add("0");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, operacoes);
        }
    }
}
