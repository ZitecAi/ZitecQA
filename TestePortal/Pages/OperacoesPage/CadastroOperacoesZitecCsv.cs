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
using Microsoft.Extensions.Configuration;


namespace TestePortal.Pages.OperacoesPage
{
    public class CadastroOperacoesZitecCsv
    {
        public static async Task<(Pagina pagina, Operacoes operacoes)> OperacoesZitecCsv(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {

            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\Temp\Arquivos\CNABz.txt";
            operacoes.ListaErros3 = new List<string>();

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "Operacoes/Operacoes2.0.aspx");

                if (OperacoesZitec.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";


                    Console.Write("Operações Zitec csv: ");
                    pagina.Nome = "Operações Zitec csv";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌") errosTotais++;
                    pagina.Listagem = Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌") errosTotais++;

                    if (nivelLogado == NivelEnum.Master)
                    {

                        string pastaArquivos = "C:/TempQA/Arquivos/";
                        string nomeArquivoOriginal = "arquivoteste_operacoescsv_qa.csv";
                        string caminhoOriginal = Path.Combine(pastaArquivos, nomeArquivoOriginal);
                        string nomeArquivoModificado = ModificarArquivoCsv.ModificarCsv(caminhoOriginal, pastaArquivos);
                        string caminhoModificado = Path.Combine(pastaArquivos, nomeArquivoModificado);

                        operacoes.TipoOperacao3 = "Operacoes Zitec - csv";
                        operacoes.NovoNomeArquivo3 = nomeArquivoModificado;
                        //// Envia o arquivo atualizado para o input
                        //for (int i = 0; i < 2; i++)  // Tenta no máximo 2 vezes (inicial + 1 tentativa)
                        //{

                        await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CSV" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#selectFundoCsv").SelectOptionAsync(new[] { "54638076000176" });
                        await Task.Delay(200);
                        await Page.Locator("#fileEnviarOperacoesCsv").SetInputFilesAsync(new[] { caminhoModificado });
                        await Task.Delay(200);
                        await Page.Locator("#fileEnviarLastro").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "Arquivo teste.zip" });
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste de envio csv");
                        await Task.Delay(200);
                        while (await Page.Locator("#enviarButton").IsVisibleAsync())
                        {
                            await Page.Locator("#enviarButton").ClickAsync();
                            await Task.Delay(1000);
                        }
                        await Task.Delay(1000);

                        var idOperacaoRecebivel = Repository.OperacoesCsv.OperacoesCsvRepository.ObterIdOperacaoRec(nomeArquivoModificado);


                        if (idOperacaoRecebivel != 0)
                        {

                            var idOperacao = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarOperacao(idOperacaoRecebivel);
                            var (recebivelExiste, idRecebivel) = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarRecebivel(idOperacaoRecebivel);
                            var complementoRelExiste = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarRecebivelComplemento(idRecebivel);

                            if (recebivelExiste && complementoRelExiste)
                            {
                                pagina.InserirDados = "✅";
                                operacoes.ArquivoEnviado = "✅";
                                //operacoes.StatusTrocados3 = "❓";
                                //operacoes.AprovacoesRealizadas3 = "❓";
                                //    await Page.GetByLabel("Pesquisar").FillAsync(nomeArquivoModificado);
                                //    var primeiroTr = Page.Locator("#listaCedentes tr").First;
                                //    var primeiroTd = primeiroTr.Locator("td").First;
                                //    await primeiroTd.ClickAsync();
                                //    await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                                //await Page.Locator("button[title='Aprovação Gestora']").ClickAsync();

                                //await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar", Exact = true }).ClickAsync();
                                //    await Task.Delay(3000);
                                //    var status = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarStatus(nomeArquivoModificado);

                                //    if (status == "PI")
                                //    {
                                //        operacoes.AprovacoesRealizadas3 = "✅";
                                //        operacoes.StatusTrocados3 = "✅";
                                //    }
                                //    else
                                //    {
                                //        operacoes.StatusTrocados3 = "❌";
                                //        operacoes.AprovacoesRealizadas3 = "❌";
                                //    }


                            }
                            else
                            {
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                                operacoes.ListaErros3.Add("Operação não foi corretamente para o banco de dados");
                            }


                        }
                        else
                        {

                            await Page.Locator("#enviarButton").ClickAsync();
                            Console.WriteLine("Não foi possível lançar operação");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                            operacoes.ListaErros3.Add("Erro ao lançar operação");

                        }

                    }

                    //else if (nivelLogado == NivelEnum.Consultoria)
                    //{
                    //    await Page.ReloadAsync();
                    //    await Page.GetByLabel("Pesquisar").ClickAsync();
                    //    await Task.Delay(200);
                    //    await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo3);
                    //    var primeiroTr = Page.Locator("#listaCedentes tr").First;
                    //    var primeiroTd = primeiroTr.Locator("td").First;
                    //    await primeiroTd.ClickAsync();
                    //    await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Consultoria']").ClickAsync();
                    //    await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar" }).ClickAsync();
                    //    pagina.InserirDados = "❓";
                    //    pagina.Excluir = "❓";
                    //}
                    else if (nivelLogado == NivelEnum.Gestora)

                    {
                        string status = Repository.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo3);
                        var (existe, idArquivo) = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo3);
                        var idOperacaoRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo3);


                        if (status == "PG")
                        {
                            pagina.InserirDados = "❓";
                            Console.WriteLine("O status foi trocado para aguardar a aprovação da gestora");
                            statusTrocados++;
                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(2000);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo3);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Gestora']").ClickAsync();

                            await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar", Exact = true }).ClickAsync();
                            await Task.Delay(4000);
                            string status2 = Repository.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo3);

                            if (status2 == "AC")
                            {
                                statusTrocados++;
                                Console.WriteLine("Todos os status foram trocados corretamente, aprovações realizadas! ");
                                operacoes.AprovacoesRealizadas3 = "✅";
                                operacoes.StatusTrocados3 = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Os status não foram trocados corretamente, aprovações realizadas! ");
                                errosTotais2++;
                                operacoes.ListaErros3.Add("Não foi possível aprovar como gestora");
                                operacoes.AprovacoesRealizadas3 = "❌";
                                operacoes.StatusTrocados3 = "❌";
                            }
                            //apagar pelo botão
                            await Page.ReloadAsync();
                            var cnpj = "54638076000176";
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo3);
                            await Task.Delay(600);
                            var primeiroTr2 = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd2 = primeiroTr2.Locator("td").First;
                            await primeiroTd2.ClickAsync();
                            await Page.EvaluateAsync(
          $"ModalExcluirArquivo('{idArquivo}', '{idOperacaoRecebivel}', '{operacoes.NovoNomeArquivo3}', '{cnpj}');"
      );
                            await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#motivoExcluirArquivo").FillAsync("teste de exclus");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            Console.WriteLine("Botão de apagar operação encontrado.");

                            var apagarOperacao = await Page.GetByText("Arquivo excluído com sucesso!").ElementHandleAsync();

                            if (apagarOperacao != null)
                            {
                                operacoes.OpApagadaBtn = "✅";
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                operacoes.OpApagadaBtn = "❌";
                                pagina.Excluir = "❌";
                            }

                            bool exclusaoRemessa = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo3);
                            bool exclusaoTed = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo3);
                            var idRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo3);
                            var excluirAvalista = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);

                            if (exclusaoRemessa && exclusaoTed && excluirAvalista)

                            {
                                bool excluirOperacao = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo3);

                                if (excluirOperacao)

                                {
                                    Console.WriteLine("Operação excluida com sucesso. ");
                                    pagina.Excluir = "✅";
                                    pagina.InserirDados = "❓";
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluida. ");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros3.Add("Operação não excluída");
                                }

                            }
                            else
                            {

                                Console.WriteLine("Operação não excluida. ");
                                pagina.Excluir = "❌";
                                errosTotais2++;
                                operacoes.ListaErros3.Add("Não foi possível exluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED ");
                            }


                        }
                        else
                        {
                            operacoes.AprovacoesRealizadas3 = "❌";
                            operacoes.StatusTrocados3 = "❌";
                            Console.WriteLine("O status não foi trocado para aguardar a aprovação da gestora");
                            errosTotais2++;
                            operacoes.ListaErros3.Add("Status não foi trocado para aprovação da gestora");
                            bool exclusaoRemessa = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo3);
                            bool exclusaoTed = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo3);

                            if (exclusaoRemessa && exclusaoTed)

                            {

                                bool excluirOperacao = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo3);

                                if (excluirOperacao)

                                {
                                    Console.WriteLine("Operação excluida com sucesso. ");
                                    pagina.Excluir = "✅";
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluida. ");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros3.Add("Operação não excluída");
                                }

                            }

                        }

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
