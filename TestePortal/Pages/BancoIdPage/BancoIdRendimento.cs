using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestePortal.Model.Usuario;
namespace TestePortal.Pages.BancoIdPage
{
    public class BancoIdRendimento
    {
        public static async Task<Model.Pagina> Rendimento(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;


            try

            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var BancoIdRendimento = await Page.GotoAsync(portalLink + "/BancoID/Rendimento.aspx");

                if (BancoIdRendimento.Status == 200)
                {

                    String seletorTabela = "#tabelaRendimento";


                    Console.Write("Rendimentos - Banco ID: ");
                    pagina.StatusCode = BancoIdRendimento.Status;
                    pagina.Nome = "BancoID/Rendimento";
                    listErros.Add("0");
                    pagina.BaixarExcel = Utils.Excel.BaixarExcelRendimentoPorId(Page).Result;
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();
                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }
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
                    pagina.InserirDados = "❓";
                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        //await Page.PauseAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                        await Page.Locator("#dataLiquid i").ClickAsync();
                        //await Page.GetByRole(AriaRole.Cell, new() { Name = "18", Exact = true }).ClickAsync();
                        await Page.GetByLabel("Valor:*").ClickAsync();
                        await Page.GetByLabel("Valor:*").FillAsync("R$2000");
                        await Page.GetByLabel("Fundo:*").SelectOptionAsync(new[] { "08621199000187" });
                        await Page.Locator("#lstCarteiras").SelectOptionAsync(new[] { "255603" });
                        await Page.GetByLabel("Cotista:*").SelectOptionAsync(new[] { "27078529" });
                        await Page.Locator("#codBancoInput").ClickAsync();
                        await Page.Locator("#codBancoInput").FillAsync("123");
                        await Page.Locator("#agenciaInput").ClickAsync();
                        await Page.Locator("#agenciaInput").FillAsync("1234");
                        await Page.Locator("#contaCorrenteInput").ClickAsync();
                        await Page.Locator("#contaCorrenteInput").FillAsync("1234567");
                        await Page.Locator("#digitoInput").ClickAsync();
                        await Page.Locator("#digitoInput").FillAsync("8");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                        var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Rendimento cadastrado com sucesso!", new PageWaitForSelectorOptions

                        {

                            Timeout = 90000

                        });




                        var verificarExistenciaRendimento = Repository.Rendimento.RendimentoRepository.VerificarExistenciaRendimento("08.621.199/0001-87", "123456 - 7");

                        if (verificarExistenciaRendimento)
                        {
                            Console.WriteLine("Rendimento Adicionado com sucesso na Tabela.");
                            pagina.InserirDados = "✅";

                        }

                        var apagarRendimento = Repository.Rendimento.RendimentoRepository.ApagarRendimento("08.621.199/0001-87", "123455 - 6");

                        if (apagarRendimento)
                        {
                            Console.WriteLine("Rendimento apagado com sucesso da Tabela.");
                            pagina.Excluir = "✅";
                        }

                        var apagarEventoRendimento = Repository.Rendimento.RendimentoRepository.ApagarEventoRendimento("qazitec01@gmail.com");

                        if (apagarEventoRendimento)
                        {
                            Console.WriteLine("Evento Rendimento apagado com sucesso da Tabela.");
                            pagina.Excluir = "✅";
                        }
                        // Adicionar Função para apagar da tabela TB_EVENTO_RENDIMENTO também, passando o Email de usuario QA 


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


