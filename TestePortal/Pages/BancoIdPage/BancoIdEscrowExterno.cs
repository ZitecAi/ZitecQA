using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestePortal.Model.Usuario;
using TestePortal.Repository.EscrowExterno;

namespace TestePortal.Pages.BancoIdPage
{
    public class BancoIdEscrowExterno
    {
        public static async Task<Model.Pagina> EscrowExterno(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;


            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var escrowExterno = await Page.GotoAsync(portalLink + "/BancoID/EscrowExterno.aspx");

                if (escrowExterno.Status == 200)
                {

                    String seletorTabela = "#tabelaContasEscrows";


                    Console.Write("ContasEscrow - Banco ID: ");
                    pagina.StatusCode = escrowExterno.Status;
                    pagina.Nome = "BancoID/Escrow Externo";
                    listErros.Add("0");
                    pagina.BaixarExcel = "❓";
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
                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        //await Page.PauseAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = " Nova" }).ClickAsync();
                        await Page.GetByLabel("Titular: *").ClickAsync();
                        await Page.GetByLabel("Titular: *").FillAsync("testeQA");
                        await Page.Locator("#titularDestino").ClickAsync();
                        await Page.Locator("#titularDestino").FillAsync("testeQA");
                        await Page.Locator("#cnpjDestino").ClickAsync();
                        await Page.Locator("#cnpjDestino").FillAsync("81602266085");
                        await Page.Locator("#bancoDestino").ClickAsync();
                        await Page.Locator("#bancoDestino").FillAsync("123");
                        await Page.GetByPlaceholder("XXXX", new() { Exact = true }).ClickAsync();
                        await Page.GetByPlaceholder("XXXX", new() { Exact = true }).FillAsync("0062");
                        await Page.GetByPlaceholder("XXXXXXXXXXXX").ClickAsync();
                        await Page.GetByPlaceholder("XXXXXXXXXXXX").FillAsync("21878");
                        await Page.GetByPlaceholder("X", new() { Exact = true }).ClickAsync();
                        await Page.GetByPlaceholder("X", new() { Exact = true }).FillAsync("9");
                        await Page.Locator("#valorDestino").FillAsync("01");
                        await Page.GetByRole(AriaRole.Link, new() { Name = "Enviar" }).ClickAsync();
                        await Page.GetByText("Movimetação enviada com").ClickAsync();
                        //var movimentacaoEscrowCadastrada = await Page.WaitForSelectorAsync("text=Rendimento cadastrado com sucesso!", new PageWaitForSelectorOptions

                        //{

                        //    Timeout = 90000

                        //});


                        var verificarExistenciaMovimentacaoEscrow = Repository.EscrowExterno.EscrowExternoRepository.VerificarExistenciaEscrowExterno("testeQA", "testeQA");

                        if (verificarExistenciaMovimentacaoEscrow)
                        {
                            Console.WriteLine("Movimentação Escrow Adicionada com sucesso na Tabela.");
                            pagina.InserirDados = "✅";
                        }

                        var excluirMovimentacaoEscrow = Repository.EscrowExterno.EscrowExternoRepository.ApagarMovimentacaoEscrowExterno("testeQA", "testeQA");
                        if (excluirMovimentacaoEscrow)
                        {
                            Console.WriteLine("Movimentação Escrow Excluida com sucesso da Tabela.");
                            pagina.Excluir = "✅";
                        }




                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                return pagina;

            }

            return pagina;
        }
    }
}
