using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages.BoletagemPage
{
    public class BoletagemControleCapital
    {

        public static async Task<Model.Pagina> ControleCapital(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Task.Delay(500);

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var controleCapital = await Page.GotoAsync( portalLink + "/Boleta/ControleCapital.aspx");

                if (controleCapital.Status == 200)
                {
                    string seletorTabela = "#tabelaControle";

                    Console.Write("Controle Capital - Boletagem : ");
                    pagina.Nome = "Controle Capital";
                    pagina.StatusCode = controleCapital.Status;
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
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

                    pagina.BaixarExcel = Utils.Excel.BaixarExcelPorIdControleCapital(Page).Result;
                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }


                }
                else
                {
                    Console.Write("Erro ao carregar a página de Controle Capital no tópico Boletagem ");
                    pagina.Nome = "Controle Capital";
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
