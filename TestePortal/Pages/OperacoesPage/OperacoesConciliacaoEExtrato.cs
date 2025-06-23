using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages.OperacoesPage
{
    public class OperacoesConciliacaoEExtrato
    {
        public static async Task<Model.Pagina> ConciliacaoExtrato(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;


            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var escrowExterno = await Page.GotoAsync(portalLink + "/operacoes/ConciliacaoExtrato.aspx");

                if (escrowExterno.Status == 200)
                {

                    String seletorTabela = "#tabelaConciliacao";


                    Console.Write("Conciliação e Extrato - Operações: ");
                    pagina.StatusCode = escrowExterno.Status;
                    pagina.Nome = "Operações/Conciliação E Extrato";
                    listErros.Add("0");
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;
                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.Reprovar = "❓";
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    //if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    //{
                    //    //await Page.PauseAsync();
                    //    //Testar manualmente e Adicionar Metodos De query nessa Page

                    //    //await Page.GetByText("Conta cadastrada com sucesso!").ClickAsync();
                    //    var contaEscrowCriada = await Page.WaitForSelectorAsync("text=Conta cadastrada com sucesso!", new PageWaitForSelectorOptions

                    //    {

                    //        Timeout = 90000

                    //    });


                    //    var verificarExistenciaContaEscrow = Repository.ControleEscrow.ControleEscrowRepository.VerificarExistenciaContaEscrow("qazitec01@gmail.com");

                    //    if (verificarExistenciaContaEscrow)
                    //    {
                    //        Console.WriteLine("Conta Controle Escrow Adicionada com sucesso na Tabela.");
                    //        pagina.InserirDados = "✅";
                    //    }

                    //    var excluirMovimentacaoEscrow = Repository.ControleEscrow.ControleEscrowRepository.ApagarContaControleEscrow("qazitec01@gmail.com");
                    //    if (excluirMovimentacaoEscrow)
                    //    {
                    //        Console.WriteLine("Conta Controle Escrow Excluida com sucesso da Tabela.");
                    //        pagina.Excluir = "✅";
                    //    }




                    //}

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
