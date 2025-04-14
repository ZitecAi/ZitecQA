using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    public class OperacoesOperacoes
    {

        public static async Task<(Model.Pagina pagina, Operacoes operacoes)> Operacoes(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Model.Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0; 
            string caminhoArquivo = @"C:\Temp\Arquivos\CNABz.txt";
      
            #region Portal
            try
            {
                var OperacoesOperacoes = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Operacoes/Operacoes.aspx");

                if (OperacoesOperacoes.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações - Operações : ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesOperacoes.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌") errosTotais++;

                    if (nivelLogado == NivelEnum.Master)
                    {
                        await Page.Locator("[aria-controls='divTabelaCedentes'] span:has-text('Nova Operação +')").ClickAsync();

                        await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "36614123000160" });
                        operacoes.NovoNomeArquivo = await Utils.AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);

                        await Task.Delay(500);
                        var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                        await Task.Delay(25000);

                        if (CadastroOperacoes != null)
                        {
                            var operacoesExiste = Repository.Operacoes.OperacoesRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo);

                            await Task.Delay(600);

                            if (operacoesExiste)
                            {
                                Console.WriteLine("Operação lançada.");
                                pagina.InserirDados = "✅";
                                pagina.Excluir = "❓";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                            }

                        }
                    }
                    else if (nivelLogado == NivelEnum.Consultoria)

                    {
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Task.Delay(2000);
                        await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo);
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
                        string status = Repository.Operacoes.OperacoesRepository.VerificarStatus(operacoes.NovoNomeArquivo);

                        if (status == "PG")
                        {
                            pagina.InserirDados = "❓";
                            Console.WriteLine("O status foi trocado para aguardar a aprovação da gestora");
                            statusTrocados++;
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(2000);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Gestora']").ClickAsync();

                            await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar", Exact = true }).ClickAsync();
                            await Task.Delay(4000);
                            string status2 = Repository.Operacoes.OperacoesRepository.VerificarStatus(operacoes.NovoNomeArquivo);

                            if (status2 == "PI")
                            {
                               
                                statusTrocados++;
                                Console.WriteLine("Todos os status foram trocados corretamente, aprovações realizadas! ");

                            }
                            else
                            {
                                Console.WriteLine("Os status não foram trocados corretamente, aprovações realizadas! ");

                            }

                            if (statusTrocados == 2)
                            {
                                operacoes.StatusTrocados = "✅";
                                operacoes.AprovacoesRealizadas = "✅";
                            }
                            else
                            {
                                operacoes.StatusTrocados = "❌";
                                operacoes.AprovacoesRealizadas = "❌";
                                errosTotais2++;
                                operacoes.ListaErros.Add("Nem todos os status foram trocados corretamente (PG ou PI)");
                            }

                            bool exclusaoRemessa = OperacoesRepository.ExcluirRemessa(operacoes.NovoNomeArquivo);
                            bool exclusaoTed = OperacoesRepository.ExcluirTbTed(operacoes.NovoNomeArquivo);

                            if (exclusaoRemessa && exclusaoTed)

                            {

                                bool excluirOperacao = OperacoesRepository.ExcluirOperacao(operacoes.NovoNomeArquivo);

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
                                    operacoes.ListaErros.Add("Operação não excluída");
                                }

                            }
                            else
                            {

                                Console.WriteLine("Operação não excluida. ");
                                pagina.Excluir = "❌";
                                errosTotais2++;
                                operacoes.ListaErros.Add("Não foi possível exluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED ");
                            }

                            
                        }
                        else
                        {
                            Console.WriteLine("O status não foi trocado para aguardar a aprovação da gestora");
                            errosTotais2++;
                            operacoes.ListaErros.Add("Status não foi trocado para aprovação da gestora");
                            bool exclusaoRemessa = OperacoesRepository.ExcluirRemessa(operacoes.NovoNomeArquivo);
                            bool exclusaoTed = OperacoesRepository.ExcluirTbTed(operacoes.NovoNomeArquivo);

                            if (exclusaoRemessa && exclusaoTed)

                            {

                                bool excluirOperacao = OperacoesRepository.ExcluirOperacao(operacoes.NovoNomeArquivo);

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
                                    operacoes.ListaErros.Add("Operação não excluída");
                                }

                            }

                        }

                    }
                    
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operações no tópico Operações: ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesOperacoes.Status;
                    errosTotais += 2;
                    operacoes.ListaErros.Add("Erro ao carregar a página de operações");
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }


            }
            catch (TimeoutException ex)
            {

                if (operacoes.ListaErros.Count == 0)
                {
                    operacoes.ListaErros.Add("0");
                }
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                operacoes.ListaErros.Add("Erro de timeout");
                errosTotais2++;
                operacoes.totalErros = errosTotais2;
                pagina.TotalErros = errosTotais;
                return (pagina, operacoes);
            }
            catch (Exception ex)
            
            {

                if (operacoes.ListaErros.Count == 0)
                {
                    operacoes.ListaErros.Add("0");
                }
                Console.WriteLine($"exceção: {ex}");
                operacoes.ListaErros.Add($"Execeção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros = errosTotais2;

            }
            #endregion
            //#region API
            //try
            //{
            //    string tokenFundoApi = "5LGMATOTBCE6IR49HKS3MX25L23K1O";

            //    var caminhoArquivo = "C:\\Caminho\\Do\\Arquivo\\Modelo.txt";

            //    // Chama a função para gerar o arquivo ZIP e obter a Base64
            //    var (nomeArquivoZip, arquivoBase64) = await TestePortal.Utils.AtualizarTxt.AtualizarDataEConverterParaBase64(caminhoArquivo);

            //    // Construir o JSON para a requisição
            //    var data = new
            //    {
            //        CnpjFundo = "42196346000157",
            //        NomeArquivo = nomeArquivoZip,
            //        File = arquivoBase64
            //    };

            //    // Serializa o JSON
            //    var jsonContent = JsonConvert.SerializeObject(data);
            //    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            //    // Token para a requisição
            //    string token = "A35JC4V08LD3O0M43AL03Y6HFK5Q10";

            //    // Configura o cliente HTTP com o token
            //    using (var client = new HttpClient())
            //    {
            //        client.DefaultRequestHeaders.Add("token", token);

            //        // Envia a requisição POST
            //        var response = await client.PostAsync("https://prod.idsf.com.br/api/operation", content);

            //        // Processa a resposta
            //        var responseContent = await response.Content.ReadAsStringAsync();
            //        Console.WriteLine($"Status Code: {response.StatusCode}");
            //        Console.WriteLine($"Response: {responseContent}");

            //    }
            //catch (Exception)
            //{

            //}
            //#endregion
            if (operacoes.ListaErros.Count == 0)
            {
                operacoes.ListaErros.Add("0");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, operacoes);
        }
    }
}
