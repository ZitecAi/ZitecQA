using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages.NotasPage
{
    public class NotasPagamentos
    {
        public static async Task<Model.Pagina> Pagamentos(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();
            await Page.WaitForLoadStateAsync();
            await Page.WaitForTimeoutAsync(1000);

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var NotasPagamentos = await Page.GotoAsync(portalLink + "/Notas/PagamentosNotas.aspx");
                if (NotasPagamentos.Status == 200)
                {
                    string seletorTabela = "#tabelaNotas";

                    Console.Write("Pagamentos - Notas : ");
                    pagina.Nome = "Pagamentos";
                    pagina.StatusCode = NotasPagamentos.Status;
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();
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

                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        var apagarNotaPagamento2 = Repository.NotaPagamento.NotaPagamentoRepository.ApagarNotaPagamento("36614123000160", "teste jessica");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                        var data = new DateTime(2024, 8, 28).ToString("yyyy-MM-dd");
                        await Page.Locator("#agendamentoFiltro").FillAsync(data);
                        await Page.Locator("#data").FillAsync("12/12/2025");
                        await Page.Locator("#tipoNota").SelectOptionAsync(new[] { "ASSEMBLEIA" });
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "36614123000160" });
                        await Page.GetByPlaceholder("0000,00").FillAsync("100");                        
                        await Page.Locator("#Prestadores").SelectOptionAsync(new SelectOptionValue { Label = "ZIELO TECNOLOGIA LTDA" });
                        await Page.Locator("#filePagamentosNotas").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "21321321321.pdf" });
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste jessica");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(700);


                        var notaPagamentoExiste = Repository.NotaPagamento.NotaPagamentoRepository.VerificaExistenciaNotaPagamento("36614123000160", "teste jessica");

                        if (notaPagamentoExiste)
                        {
                            Console.WriteLine("Notas pagamento adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarNotaPagamento = Repository.NotaPagamento.NotaPagamentoRepository.ApagarNotaPagamento("36614123000160", "teste jessica");

                            if (apagarNotaPagamento)
                            {
                                Console.WriteLine("Notas pagamento apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar pagamento nota");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir pagamento nota");
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
                    Console.Write("Erro ao carregar a página de Pagamentos no tópico Notas ");
                    pagina.Nome = "Pagamentos";
                    pagina.StatusCode = NotasPagamentos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }


            }
            catch
            {
                throw new Exception();
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
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
