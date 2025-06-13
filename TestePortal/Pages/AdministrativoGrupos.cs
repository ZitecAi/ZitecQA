using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestePortal.Model;

namespace TestePortal.Pages
{
    public class AdministrativoGrupos
    {
        public static async Task<Model.Pagina> Grupos(IPage page, IConfiguration config)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                    var portalLink = config["Links:Portal"];
                var PaginaAdministrativoGrupos = await page.GotoAsync(portalLink + "/Permissoes/GrupoPermissoes.aspx");

                if (PaginaAdministrativoGrupos.Status == 200)
                {
                    Console.Write("Administrativo - Grupos: ");
                    Console.WriteLine(PaginaAdministrativoGrupos.Status);

                    pagina.StatusCode = PaginaAdministrativoGrupos.Status;
                    pagina.Nome = "Administrativo Grupos";
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Grupos no tópico Administrativo ");
                    Console.WriteLine(PaginaAdministrativoGrupos.Status);
                    pagina.Nome = "Administrativo Grupos";
                    pagina.StatusCode = PaginaAdministrativoGrupos.Status;
                    errosTotais++;
                    await page.GotoAsync(portalLink + "/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
            }

            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
