using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestePortal.Model.Usuario;
using TestePortal.Model;
using Segment.Model;
using TestePortal.Utils;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml.Drawing.Slicer.Style;

namespace TestePortal.Pages
{
    public class OperacoesConciliacaoExtrato
    {
        public static async Task<(Model.Pagina pagina, Model.Conciliacao conciliacao )> ConciliacaoExtrato (IPage Page,  NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var conciliacao = new Model.Conciliacao();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;

            try
            {
                var ConciliacaoExtrato = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/operacoes/ConciliacaoExtrato.aspx");

                if (ConciliacaoExtrato.Status == 200)
                {
                    string seletorTabela = "#tabelaConciliacao";

                    Console.Write("Concilicação e Extrato  - Operações : ");
                    pagina.Nome = "Conciliação e Extrato - Operações";
                    pagina.StatusCode = ConciliacaoExtrato.Status;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }


                    if (nivelLogado == NivelEnum.Master)
                    {

                        //enviar extrato
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Extrato +" }).ClickAsync();
                        await Page.Locator("#extratoConciliacaoSelect").SelectOptionAsync(new[] { "SANTANDER" });
                        await Page.Locator("#fundosExtrato").SelectOptionAsync(new[] { "36614123000160" });
                        await Page.Locator("#fileExtrato").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Extrato_Santander (1) (1).xlsx" });
                        await Page.Locator("#enviarButtonExtrato").ClickAsync();

                        //conciliar 

                        var idConciliacao = Repository.ConciliacaoExtrato.ConciliacaoRepository.ObterIdConciliacao(11111, 300000, "TAR ENVIO TIT CART COB SIMP-ELETR");

                        var selector = $"[id=\"{idConciliacao}_ConciliarAction\"]";
                        await Page.Locator(selector).ClickAsync();
                        await Page.GetByPlaceholder("R$").ClickAsync();
                        await Page.GetByPlaceholder("R$").FillAsync("R$ 300.000,00");
                        await Page.Locator("#selectTipo").SelectOptionAsync(new[] { "APORTE" });
                        await Page.Locator("#selectMovimento").SelectOptionAsync(new[] { "68" });
                        await Page.Locator("#fileConciliar").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        //conciliação em lote

                        string caminhoExcel = @"C:\Temp\Arquivos\TemplateItem (1).xlsx";
                        var ids = Repository.ConciliacaoExtrato.ConciliacaoRepository.ObterIdsNaoConciliados(11111);


                        bool sucesso = AtualizarExcel.AtualizarExcelComIds(caminhoExcel, ids);

                        if (sucesso)
                        {
                            Console.WriteLine("Excel atualizado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Falha ao atualizar o Excel.");
                        }

                        await Page.GetByRole(AriaRole.Button, new() { Name = "Conciliar em Lote" }).ClickAsync();
                        await Page.Locator("#selectTipoLote").SelectOptionAsync(new[] { "APORTE" });
                        await Page.Locator("#selectMovimentoLote").SelectOptionAsync(new[] { "45" });
                        await Page.Locator("#fileLote").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "TemplateItem (1).xlsx" });
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste ");
                        await Page.Locator("#enviarButtonLote").ClickAsync();
                        ids.Add(idConciliacao);

                        if (ids.Count == 4)
                        {
                            conciliacao.ExtratoEnviado = "✅";


                        }
                        else
                        {
                            conciliacao.ExtratoEnviado = "❌";
                            errosTotais2++;
                            conciliacao.ListaErros.Add("Erro ao enviar extrato");

                        }
                        //aprovação pela custódia 

                        foreach (var id in ids)
                        {

                            var locator = $"button[onclick=\"OpenModalAprovarConciliacao({id})\"]";

                            var button = Page.Locator(locator);
                            if (await button.IsVisibleAsync())
                            {

                                await button.ClickAsync();
                                await Page.Locator("#status").GetByText("Aprovado").ClickAsync();
                                await Page.Locator("#descricaoStatus").FillAsync("teste aprovação");
                                await Page.Locator("#statusButton").ClickAsync();

                            }
                            else
                            {
                                Console.WriteLine($"Botão com ID {id} não encontrado.");

                            }
                        }

                        bool todosConciliados = Repository.ConciliacaoExtrato.ConciliacaoRepository.VerificarIdsConciliados(ids);

                        if (todosConciliados)
                        {
                            Console.WriteLine("Todos os IDs estão conciliados.");
                            conciliacao.AprovacaoEmLote = "✅";
                            conciliacao.AprovacaoModal = "✅";
                            conciliacao.StatusConciliado = "✅";


                        }
                        else
                        {
                            Console.WriteLine("Nem todos os IDs estão conciliados.");
                            conciliacao.AprovacaoEmLote = "❌";
                            conciliacao.AprovacaoModal = "❌";
                            conciliacao.StatusConciliado = "❌";
                            errosTotais2++;
                            conciliacao.ListaErros.Add("Erro ao aprovar, status não foi para conciliado");

                        }

                    }
                  

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Conciliação e Extrato no tópico Operações: ");
                    pagina.Nome = "Conciliação e Extrato - Operações";
                    pagina.StatusCode = ConciliacaoExtrato.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                    conciliacao.AprovacaoEmLote = "❌";
                    conciliacao.AprovacaoModal = "❌";
                    conciliacao.StatusConciliado = "❌";
                    conciliacao.ExtratoEnviado = "❌";
                    errosTotais2 += 4;
                    pagina.TotalErros = errosTotais;
                    conciliacao.TotalErros = errosTotais2;
                    conciliacao.ListaErros.Add("Erro ao carregar página");
                    return (pagina, conciliacao);
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                conciliacao.AprovacaoEmLote = "❌";
                conciliacao.AprovacaoModal = "❌";
                conciliacao.StatusConciliado = "❌";
                conciliacao.ExtratoEnviado = "❌";
                errosTotais2 += 4;
                pagina.TotalErros = errosTotais;
                conciliacao.TotalErros = errosTotais2;
                conciliacao.ListaErros.Add("Erro de timeout");
                if (conciliacao.ListaErros.Count == 0)
                {
                    conciliacao.ListaErros.Add("0");
                }

                return (pagina, conciliacao);
            }

            pagina.TotalErros = errosTotais;
            conciliacao.TotalErros = errosTotais2;
            if (conciliacao.ListaErros.Count == 0)
            {
                conciliacao.ListaErros.Add("0");
            }
            return (pagina, conciliacao);
        }
    }
}
