using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
//using System.Windows.Controls;
using TestePortalInterno.Utils;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using static TestePortalInterno.Model.Usuario;
//using TestePortalInterno.Repositorys.OperacoesZitec;
using TestePortalInterno.Model;

namespace TestePortalInterno.Pages
{
    public class OperacoesCustodiaZitec
    {
        public static async Task<(Model.Pagina pagina, Operacoes operacoes)> OperacoesZitec(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Model.Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var OperacoesZitec = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Operacoes/OperacoesZitec.aspx");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = await Utils.Acentos.ValidarAcentos(Page) ?? "❌";
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Utils.Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;

                    if (nivelLogado == NivelEnum.Master)
                    {
                        var processamentoFundo = Repositorys.OperacoesZitec.VerificaProcessamentoFundo(9991);

                        if (processamentoFundo)
                        {
                            operacoes.TipoOperacao2 = "Nova Operação - CNAB";
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CNAB" }).ClickAsync();
                            await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });

                            // Valida se AtualizarDataEEnviarArquivo NÃO retornou null
                            operacoes.NovoNomeArquivo2 = await Utils.AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
                            if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                            {
                                throw new NullReferenceException("AtualizarDataEEnviarArquivo retornou null ou vazio.");
                            }

                            await Task.Delay(500);

                            var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                            await Task.Delay(25000);

                            if (CadastroOperacoes != null)
                            {
                                var (existe, idOperacao) = Repositorys.OperacoesZitec.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo2);

                                await Task.Delay(600);

                                if (existe)
                                {
                                    Console.WriteLine("Operação lançada.");
                                    pagina.InserirDados = "✅";
                                }
                                else
                                {
                                    Console.WriteLine("Não foi possível lançar operação");
                                    pagina.InserirDados = "❌";
                                    pagina.Excluir = "❌";
                                    errosTotais += 2;
                                    operacoes.ListaErros2.Add("Erro ao lançar operação");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("O fundo não está na data atual");
                            errosTotais2++;
                            operacoes.ListaErros2.Add("Não foi possível processar o fundo para a data de hoje");
                        }
                    }
                    else if (nivelLogado == NivelEnum.Consultoria)
                    {
                        await Page.ReloadAsync();

                        if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                            throw new NullReferenceException("NovoNomeArquivo2 está null ou vazio no fluxo de Consultoria.");

                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);

                        var primeiroTr = Page.Locator("#listaCedentes tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;

                        await primeiroTd.ClickAsync();
                        await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Consultoria']").ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar" }).ClickAsync();

                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                    else if (nivelLogado == NivelEnum.Gestora)
                    {
                        if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                            throw new NullReferenceException("NovoNomeArquivo2 está null ou vazio no fluxo da Gestora.");

                        string status = Repositorys.OperacoesZitec.VerificarStatus(operacoes.NovoNomeArquivo2);

                        if (status == "PG")
                        {
                            pagina.InserirDados = "❓";
                            Console.WriteLine("O status foi trocado para aguardar a aprovação da gestora");
                            statusTrocados++;

                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(2000);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);

                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;

                            await primeiroTd.ClickAsync();
                            await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Gestora']").ClickAsync();

                            await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar", Exact = true }).ClickAsync();
                            await Task.Delay(5000);

                            string status2 = Repositorys.OperacoesZitec.VerificarStatus(operacoes.NovoNomeArquivo2);

                            if (status2 == "PI")
                            {
                                statusTrocados++;
                                Console.WriteLine("Todos os status foram trocados corretamente, aprovações realizadas! ");
                            }
                            else
                            {
                                Console.WriteLine("Os status não foram trocados corretamente, aprovações realizadas! ");
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Aprovações realizadas, mas status não foi trocado no banco de dados");
                            }

                            if (statusTrocados == 2)
                            {
                                operacoes.StatusTrocados2 = "✅";
                                operacoes.AprovacoesRealizadas2 = "✅";
                            }
                            else
                            {
                                operacoes.StatusTrocados2 = "❌";
                                operacoes.AprovacoesRealizadas2 = "❌";
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Status PI não encontrado");
                            }

                            bool exclusaoRemessa = Repositorys.OperacoesZitec.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                            bool exclusaoTed = Repositorys.OperacoesZitec.ExcluirTbTed(operacoes.NovoNomeArquivo2);
                            var idRecebivel = Repositorys.OperacoesZitec.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo2);
                            var excluirAvalista = Repositorys.OperacoesZitec.ExcluirAvalista(idRecebivel);
                            var excluirCertificadora = Repositorys.OperacoesZitec.ExcluirOperacaoCertificadora(idRecebivel);

                            if (exclusaoRemessa && exclusaoTed && excluirAvalista)
                            {
                                bool excluirOperacao = Repositorys.OperacoesZitec.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                if (excluirOperacao)
                                {
                                    Console.WriteLine("Operação excluída com sucesso.");
                                    pagina.Excluir = "✅";
                                    pagina.InserirDados = "❓";
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Operação não excluída");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Operação não excluída.");
                                pagina.Excluir = "❌";
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Não foi possível excluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED");
                            }
                        }
                        else
                        {
                            Console.WriteLine("O status não foi trocado para aguardar a aprovação da gestora");
                            errosTotais2++;
                            operacoes.ListaErros2.Add("Status não foi trocado para aprovação da gestora");

                            bool exclusaoRemessa = Repositorys.OperacoesZitec.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                            bool exclusaoTed = Repositorys.OperacoesZitec.ExcluirTbTed(operacoes.NovoNomeArquivo2);

                            if (exclusaoRemessa && exclusaoTed)
                            {
                                bool excluirOperacao = Repositorys.OperacoesZitec.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                if (excluirOperacao)
                                {
                                    Console.WriteLine("Operação excluída com sucesso.");
                                    pagina.Excluir = "✅";
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Operação não excluída");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operações no tópico Operações: ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesZitec?.Status ?? 0;
                    errosTotais += 2;
                    operacoes.ListaErros2.Add("Erro ao carregar a página de operações");
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");

                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                errosTotais2++;
                operacoes.ListaErros2.Add("Erro de timeout");
                operacoes.totalErros2 = errosTotais2;
                pagina.TotalErros = errosTotais;
                return (pagina, operacoes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                operacoes.ListaErros2.Add($"Exceção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros2 = errosTotais2;
            }

            pagina.TotalErros = errosTotais;
            if (operacoes.ListaErros2.Count == 0)
            {
                operacoes.ListaErros2.Add("0");
            }

            return (pagina, operacoes);
        }
    }
}
