using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.Configuration;

namespace TestePortalInterno.Pages
{
    public class BancoIdRendimento
    {

        public static async Task<Model.Pagina> Rendimento (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var PaginaAdministrativoRendimento = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Rendimento.aspx");
                if (PaginaAdministrativoRendimento.Status == 200)
                {
                    // await Listagem.VerificarListagem(Page, seletorTabela); outra forma de chamar o método

                    Console.Write("Administrativo - tabelaCedentes: ");
                    pagina.Nome = "Administrativo rendimento";
                    pagina.StatusCode = PaginaAdministrativoRendimento.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    string seletorTabela = "#tabelaRendimento";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de rendimento no tópico Administrativo ");
                    Console.WriteLine(PaginaAdministrativoRendimento.Status);
                    listErros.Add($"Erro {PaginaAdministrativoRendimento.Status} ao carregar a página de rendimento no tópico Administrativo ");
                    pagina.Nome = "Administrativo rendimento";
                    pagina.StatusCode = PaginaAdministrativoRendimento.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                    pagina.TotalErros = errosTotais;
                    return pagina;
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

    
