using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Model;
using static TestePortal.Model.Usuario;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages.BancoIdPage
{
    public class BancoIdContasEscrow
    {

        public static async Task<Pagina> ContasEscrow (IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var PaginaContasEscrow = await Page.GotoAsync( portalLink + "/Escrow/Escrows.aspx");
                if (PaginaContasEscrow.Status == 200)
                {

                    Console.Write("Banco Id - Contas Escrow: ");
                    pagina.StatusCode = PaginaContasEscrow.Status;
                    pagina.Nome = "Contas Escrow";
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Excluir = "❌";
                    pagina.InserirDados = "❌";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    string seletorTabela = "#tabelaContasEscrows";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora)
                    {
                        var apagarContasEscrow2 = Repository.ContasEscrows.ContasEscrows.ApagarContasEscrow("teste robo", "titular teste");
                        await Page.GetByRole(AriaRole.Button, new() { Name = " Nova" }).ClickAsync();
                        await Page.Locator("#fundoBanco").SelectOptionAsync(new[] { "54638076000176" });
                        await Task.Delay(300);
                        await Page.Locator("#contratoBanco").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#contratoBanco").FillAsync("teste robo");
                        await Task.Delay(300);
                        await Page.Locator("#titularBanco").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#titularBanco").FillAsync("titular teste");
                        await Task.Delay(300);
                        await Page.Locator("#cnpjBanco").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#cnpjBanco").FillAsync("49624866830");
                        await Task.Delay(300);
                        await Page.GetByLabel("Modo de Pagamento: *").SelectOptionAsync(new[] { "PIX" });
                        await Task.Delay(300);
                        await Page.Locator("#titularDestino").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#titularDestino").FillAsync("titular teste");
                        await Task.Delay(300);
                        await Page.Locator("#cnpjDestino").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#cnpjDestino").FillAsync("49624866830");
                        await Task.Delay(300);
                        await Page.Locator("#valorDestino").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#valorDestino").FillAsync("R$100");
                        await Task.Delay(300);
                        await Page.Locator("#inputPix").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#inputPix").FillAsync("robo@zitec.ai");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        //var contaCadastrado = await Page.WaitForSelectorAsync("text=Cedente Cadastrado com Sucesso", new PageWaitForSelectorOptions

                        //{

                        //    Timeout = 20000

                        //}); //verificar a mensagem ants de continuar

                        var contasEcrowExiste = Repository.ContasEscrows.ContasEscrows.VerificaExistenciaContasEscrow("teste robo", "titular teste");
                        await Task.Delay(700);

                        if (contasEcrowExiste)
                        {
                            Console.WriteLine("Conta Escrow adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            var apagarContasEscrow = Repository.ContasEscrows.ContasEscrows.ApagarContasEscrow("teste robo", "titular teste");
                            if (apagarContasEscrow)
                            {
                                Console.WriteLine("Conta Escrow apagada com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Conta Escrow");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Conta Escrow");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    else {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Contas Escrow no tópico Banco ID ");
                    Console.WriteLine(PaginaContasEscrow.Status);
                    pagina.Nome = "Contas Escrow";                 
                    pagina.StatusCode = PaginaContasEscrow.Status;
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
