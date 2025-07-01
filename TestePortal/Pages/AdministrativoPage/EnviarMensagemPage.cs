using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestePortal.Model.Usuario;
using TestePortal.Model;


namespace TestePortal.Pages.AdministrativoPage
{
    internal class EnviarMensagemPage
    {

        public static async Task<Pagina> EnviarMensagem(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var PaginaAdministrativoToken = await Page.GotoAsync(portalLink + "/EnviarMensagem.aspx");

                if (PaginaAdministrativoToken.Status == 200)
                {
                    
                    pagina.Nome = "Administrativo/Enviar Mensagem";
                    Console.Write("Administrativo - Enviar Mensagem ");
                    Console.WriteLine(PaginaAdministrativoToken.Status);
                    pagina.StatusCode = PaginaAdministrativoToken.Status;



                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = "?";
                    pagina.BaixarExcel = "?";
                    pagina.Reprovar = "?";
                    pagina.Excluir = "?";
                    pagina.InserirDados = "?";                 


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
