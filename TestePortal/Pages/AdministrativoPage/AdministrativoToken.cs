using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortal.Model;
using Microsoft.Playwright;
using TestePortal.Utils;

namespace TestePortal.Pages.AdministrativoPage
{
    public class AdministrativoToken
    {

        public static async Task<Pagina> Token(IPage Page)
        {
            var pagina = new Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var PaginaAdministrativoToken = await Page.GotoAsync(portalLink + "/CedentesToken.aspx");

                if (PaginaAdministrativoToken.Status == 200)
                {
                    //Aplicar Correção nesse console apos correção do BreadCrumb
                    //pagina.Nome = "BancoID/Escrow Externo";+-----------+
                    Console.Write("Administrativo - Cedentes ");
                    Console.WriteLine(PaginaAdministrativoToken.Status);
                    pagina.StatusCode = PaginaAdministrativoToken.Status;

                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();
                    pagina.Nome = "Administrativo/Token";
                    pagina.Acentos = Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌") errosTotais++;
                    string seletorTabela = "#tabelaCedentes";

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    pagina.InserirDados = "❓";

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Excluir = "❓";

                    
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

