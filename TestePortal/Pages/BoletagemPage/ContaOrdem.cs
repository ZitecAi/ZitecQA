using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages.BoletagemPage
{
    public class ContaOrdem
    {

        public static async Task<Model.Pagina> ContaEOrdem(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Task.Delay(500);

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var controleCapital = await Page.GotoAsync(portalLink + "/Boleta/ContaOrdem.aspx");

                if (controleCapital.Status == 200)
                {
                    string seletorTabela = "#tabelaContaOrdem";

                    Console.Write("Conta e Ordem - Boletagem : ");
                    pagina.Nome = "Conta e Ordem";
                    pagina.StatusCode = controleCapital.Status;
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.BaixarExcel = "❓";

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    //await Page.PauseAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                    await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });
                    await Page.Locator("#selectCarteira").SelectOptionAsync(new[] { "791348654" });
                    await Page.Locator("#inputDistribuidor").ClickAsync();
                    await Page.Locator("#inputDistribuidor").FillAsync("TesteQA");
                    //await Page.GetByLabel("Novo Registro de Conta/Ordem").Locator("i").ClickAsync();
                    await Page.Locator("#inputEnviarOrdem").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "caixa_CONTAORDEM (1).csv" });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Importar" }).ClickAsync();
                    //await Page.GetByText("Ordem enviada com sucesso!").ClickAsync();
                    var contaEscrowCriada = await Page.WaitForSelectorAsync("text=Ordem enviada com sucesso!", new PageWaitForSelectorOptions
                    {

                        Timeout = 90000

                    });

                    
                    var contaOrdemExiste = Repository.BoletagemContaOrdem.BoletagemContaOrdemRepository.VerificaExistenciaBoletagemContaOrdem("qazitec01@gmail.com", "TesteQA");

                    if (contaOrdemExiste)
                    {
                        Console.WriteLine("Conta Ordem adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarContaOrdem = Repository.BoletagemContaOrdem.BoletagemContaOrdemRepository.ApagarBoletagemContaOrdem("qazitec01@gmail.com", "TesteQA");

                        if (apagarContaOrdem)
                        {
                            Console.WriteLine("Conta Ordem apagado com sucesso");
                            var apagarAporteBoletagemContaOrdem = Repository.BoletagemContaOrdem.BoletagemContaOrdemRepository.ApagarAporteBoletagemContaOrdem(2929);
                            if (apagarAporteBoletagemContaOrdem)
                            {
                                Console.WriteLine("Aporte Conta Ordem apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possivel apagar Aporte da conta ordem");
                                pagina.Excluir = "❌";
                            }
                                Console.WriteLine("Conta Ordem apagado com sucesso");
                            var apagarResgateBoletagemContaOrdem = Repository.BoletagemContaOrdem.BoletagemContaOrdemRepository.ApagarResgateBoletagemContaOrdem("qazitec01@gmail.com");
                            if (apagarAporteBoletagemContaOrdem)
                            {
                                Console.WriteLine("Resgate Conta Ordem apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possivel apagar Resgate da conta ordem");
                                pagina.Excluir = "❌";
                            }
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Conta Ordem");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }
                        


                    }
                    else
                    {
                        Console.Write("Erro ao carregar a página de Conta e Ordem no tópico Boletagem ");
                        pagina.Nome = "Conta e Ordem";
                        pagina.StatusCode = controleCapital.Status;
                        errosTotais++;
                        await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("exceção lançada");
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
