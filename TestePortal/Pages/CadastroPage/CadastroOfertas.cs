using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages.CadastroPage
{
    public class CadastroOfertas
    {
        public static async Task<Model.Pagina> Ofertas (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var CadastroOfertas = await Page.GotoAsync(portalLink + "/Carteira/Ofertas.aspx");

                if (CadastroOfertas.Status == 200)
                {
                    string seletorTabela = "#tabelaOfertas";

                    Console.Write("Ofertas - Cadastro: ");
                    pagina.Nome = "Ofertas";
                    pagina.StatusCode = CadastroOfertas.Status;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();
                    if (pagina.Listagem == "❌")
                    { 
                    errosTotais++;
                    }
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
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Ofertas no tópico Cadastro ");
                    pagina.Nome = "Ofertas";
                    pagina.StatusCode = CadastroOfertas.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
