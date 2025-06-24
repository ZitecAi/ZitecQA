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

                    //Fazer Fluxo de inserir dados e criar repository
                    


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
