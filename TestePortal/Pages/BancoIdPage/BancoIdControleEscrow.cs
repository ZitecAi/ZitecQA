using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortal.Utils;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages.BancoIdPage
{
    public class BancoIdControleEscrow
    {
        public static async Task<Model.Pagina> ControleEscrowExterno(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;


            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var escrowExterno = await Page.GotoAsync(portalLink + "/BancoID/ControleEscrow.aspx");

                if (escrowExterno.Status == 200)
                {

                    String seletorTabela = "#tabelaUsuariosExternos";


                    Console.Write("ContasEscrow - Banco ID: ");
                    pagina.StatusCode = escrowExterno.Status;
                    pagina.Nome = "BancoID/Escrow Externo";
                    listErros.Add("0");
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.Reprovar = "❓";
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
                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        //await Page.PauseAsync();

                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("qazitec01@gmail.com");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                        await Page.Locator("#bancoNewConta").ClickAsync();
                        await Page.Locator("#bancoNewConta").FillAsync("123");
                        await Page.Locator("#agenciaNewConta").ClickAsync();
                        await Page.Locator("#agenciaNewConta").FillAsync("1234");
                        await Page.Locator("#contaNewConta").ClickAsync();
                        await Page.Locator("#contaNewConta").FillAsync("123456");
                        await Page.Locator("#digcontaNewConta").ClickAsync();
                        await Page.Locator("#digcontaNewConta").FillAsync("7");
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Adicione pessoas..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Adicione pessoas..." }).FillAsync("qazitec01@gmail.com");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "  Nova Conta" }).ClickAsync();                        
                        //await Page.GetByText("Conta cadastrada com sucesso!").ClickAsync();
                        var contaEscrowCriada = await Page.WaitForSelectorAsync("text=Conta cadastrada com sucesso!", new PageWaitForSelectorOptions

                        {

                           Timeout = 90000

                        });


                        var verificarExistenciaContaEscrow = Repository.ControleEscrow.ControleEscrowRepository.VerificarExistenciaContaEscrow("qazitec01@gmail.com");

                        if (verificarExistenciaContaEscrow)
                        {
                            Console.WriteLine("Conta Controle Escrow Adicionada com sucesso na Tabela.");
                            pagina.InserirDados = "✅";
                        }

                        var excluirMovimentacaoEscrow = Repository.ControleEscrow.ControleEscrowRepository.ApagarContaControleEscrow("qazitec01@gmail.com");
                        if (excluirMovimentacaoEscrow)
                        {
                            Console.WriteLine("Conta Controle Escrow Excluida com sucesso da Tabela.");
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
