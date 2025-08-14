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
    public class OperacoesCustodiaZitec
    {
        public static async Task<(Pagina pagina, Operacoes operacoes)> OperacoesZitecInterno(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "/Operacoes/Operacoes2.0.aspx ");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec - Interno";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = await Acentos.ValidarAcentos(Page) ?? "❌";
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();

                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;

                    var processamentoFundo = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaProcessamentoFundo(9991);

                    if (processamentoFundo)
                    {
                        operacoes.TipoOperacao2 = "Nova Operação - Interno";
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CNAB" }).ClickAsync();
                        await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });

                        // Valida se AtualizarDataEEnviarArquivo NÃO retornou null

                        operacoes.NovoNomeArquivo2 = await AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
                        if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                        {
                            throw new NullReferenceException("AtualizarDataEEnviarArquivo retornou null ou vazio.");
                        }
                        await Task.Delay(300);
                        var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                        await Task.Delay(25000);


                        if (CadastroOperacoes != null)
                        {
                            var (existe, idArquivo) = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo2);
                            var idOperacaoRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo2);

                            await Page.ReloadAsync();

                            if (existe)
                            {
                                Console.WriteLine("Operação lançada.");
                                pagina.InserirDados = "✅";
                                operacoes.ArquivoEnviado = "✅";
                                operacoes.AprovacoesRealizadas2 = "❓";
                                operacoes.StatusTrocados2 = "❓";
                            }
                            else
                            {
                                operacoes.ArquivoEnviado = "❌";
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                                operacoes.ListaErros2.Add("Erro ao lançar operação");
                            }

                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("CEDENTE TESTE");
                            await Task.Delay(600);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            var cnpj = "54638076000176";
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

                                bool exclusaoRemessa = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                                bool exclusaoTed = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo2);
                                var idRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo2);
                                var excluirAvalista = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);
                                var excluirCertificadora = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacaoCertificadora(idRecebivel);

                                if (exclusaoRemessa && exclusaoTed && excluirAvalista)
                                {
                                    bool excluirOperacao = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                    if (excluirOperacao)
                                    {
                                        Console.WriteLine("Operação excluída com sucesso pelo banco.");
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
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Não foi possível excluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("O fundo não está na data atual");
                        errosTotais2++;
                        operacoes.ListaErros2.Add("Não foi possível processar o fundo para a data de hoje");
                    }

                    pagina.TotalErros = errosTotais;
                    if (operacoes.ListaErros2.Count == 0)
                    {
                        operacoes.ListaErros2.Add("0");
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

        public static async Task<(Pagina pagina, Operacoes operacoes)> OperacoesZitecConsultoria(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "/Operacoes/Operacoes2.0.aspx ");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec - Consultoria";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = await Acentos.ValidarAcentos(Page) ?? "❌";
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;

                    var processamentoFundo = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaProcessamentoFundo(9991);

                    if (processamentoFundo)
                    {
                        operacoes.TipoOperacao2 = "Nova Operação - Consultoria";
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CNAB" }).ClickAsync();
                        await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });

                        // Valida se AtualizarDataEEnviarArquivo NÃO retornou null
                        operacoes.NovoNomeArquivo2 = await AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
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

                            var (existe, idArquivo) = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo2);
                            var idOperacaoRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo2);
                            await Page.ReloadAsync();

                            if (existe)
                            {
                                Console.WriteLine("Operação lançada.");
                                pagina.InserirDados = "✅";
                                operacoes.ArquivoEnviado = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                                operacoes.ListaErros2.Add("Erro ao lançar operação");
                                operacoes.ArquivoEnviado = "❌" +
                                    "";
                            }

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
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("CEDENTE TESTE");
                            await Task.Delay(600);
                            var primeiroTr2 = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd2.ClickAsync();
                            var cnpj = "54638076000176";

                            var statusTrocado = Repository.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo2);
                            if (statusTrocado == "PG")
                            {
                                operacoes.StatusTrocados2 = "✅";
                                operacoes.AprovacoesRealizadas2 = "✅";
                            }
                            else
                            {
                                operacoes.StatusTrocados2 = "❌";
                                operacoes.AprovacoesRealizadas2 = "❌";
                            }
                            await Page.EvaluateAsync(
    $"ModalExcluirArquivo('{idArquivo}', '{idOperacaoRecebivel}', '{operacoes.NovoNomeArquivo3}', '{cnpj}');"
);
                            await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#motivoExcluirArquivo").FillAsync("teste de exclusão");
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
                                operacoes.OpApagadaBtn = "❌";

                                bool exclusaoRemessa = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                                bool exclusaoTed = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo2);
                                var idRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo2);
                                var excluirAvalista = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);
                                var excluirCertificadora = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacaoCertificadora(idRecebivel);

                                if (exclusaoRemessa && exclusaoTed && excluirAvalista)
                                {
                                    bool excluirOperacao = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                    if (excluirOperacao)
                                    {
                                        Console.WriteLine("Operação excluída com sucesso pelo banco.");
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
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Não foi possível excluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED");
                                }
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


        public static async Task<(Pagina pagina, Operacoes operacoes)> OperacoesZiteGestora(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "/Operacoes/Operacoes2.0.aspx ");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec - Gestora";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = await Acentos.ValidarAcentos(Page) ?? "❌";
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;


                    var processamentoFundo = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaProcessamentoFundo(9991);

                    if (processamentoFundo)
                    {

                        operacoes.TipoOperacao2 = "Nova Operação - Gestora";
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CNAB" }).ClickAsync();
                        await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });

                        // Valida se AtualizarDataEEnviarArquivo NÃO retornou null
                        operacoes.NovoNomeArquivo2 = await AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
                        if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                        {
                            throw new NullReferenceException("AtualizarDataEEnviarArquivo retornou null ou vazio.");
                        }

                        await Task.Delay(500);
                        var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                        await Task.Delay(15000);

                        if (CadastroOperacoes != null)
                        {

                            var (existe, idArquivo) = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo2);
                            var idOperacaoRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo2);

                            await Page.ReloadAsync();

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
                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Consultoria']").ClickAsync();
                            await Task.Delay(300);
                            while (await Page.Locator("#aprovaOpConsultoriaBtn").IsVisibleAsync())
                            {
                                await Page.Locator("#aprovaOpConsultoriaBtn").ClickAsync();
                                await Task.Delay(1000);
                            }

                            string status2 = Repository.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo2);
                            await Task.Delay(200);

                            if (status2 == "PG")
                            {
                                operacoes.StatusTrocados2 = "✅";
                                operacoes.AprovacoesRealizadas2 = "✅";
                                statusTrocados++;
                                Console.WriteLine("Todos os status foram trocados corretamente, aprovações realizadas! ");
                            }
                            else
                            {
                                operacoes.StatusTrocados2 = "❌";
                                operacoes.AprovacoesRealizadas2 = "❌";
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Status PI não encontrado");
                                Console.WriteLine("Os status não foram trocados corretamente, aprovações realizadas! ");
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Aprovações realizadas, mas status não foi trocado no banco de dados");
                            }

                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("CEDENTE TESTE");
                            await Task.Delay(600);
                            var primeiroTr2 = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await primeiroTd2.ClickAsync();
                            var cnpj = "54638076000176";
                            await Page.EvaluateAsync(
                                $"ModalExcluirArquivo('{idArquivo}', '{idOperacaoRecebivel}', '{operacoes.NovoNomeArquivo3}', '{cnpj}');"
                            );
                            await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#motivoExcluirArquivo").FillAsync("teste de exclusão");
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
                                bool exclusaoRemessa = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                                bool exclusaoTed = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo2);
                                var idRecebivel = Repository.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo2);
                                var excluirAvalista = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);
                                var excluirCertificadora = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacaoCertificadora(idRecebivel);

                                if (exclusaoRemessa && exclusaoTed && excluirAvalista)
                                {
                                    bool excluirOperacao = Repository.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                    if (excluirOperacao)
                                    {
                                        Console.WriteLine("Operação excluída com sucesso pelo banco.");
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
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Não foi possível excluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED");
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                operacoes.ListaErros2.Add($"Exceção lançada. ");
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
