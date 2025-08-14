using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TestePortal.Pages.RelatoriosPage
{
    public class RelatorioFundos
    {
        public static async Task<Model.Pagina> Fundos (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var RelatorioFundos = await Page.GotoAsync(portalLink + "/Relatorios/Fundos.aspx");

                if (RelatorioFundos.Status == 200)
                {
                    Console.Write("Fundos  - Relatorios : ");
                    pagina.Nome = "Fundos - Relatorios";
                    pagina.StatusCode = RelatorioFundos.Status;
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    //await Page.PauseAsync();
                    await Page.Locator("#btnComposicaoCarteira").ClickAsync();
                    await Page.Locator("#listFundos").SelectOptionAsync(new[] { "361" });
                    await Page.GetByLabel("Composição de Carteira").Locator("i").ClickAsync();
                    await Page.GetByRole(AriaRole.Cell, new() { Name = "23" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Carregar" }).ClickAsync();
                    await Page.GetByText("Seu relatório foi enviado").ClickAsync();
                    
                    var contaEscrowCriada = await Page.WaitForSelectorAsync("text=Seu relatório foi enviado", new PageWaitForSelectorOptions
                    {

                        Timeout = 90000

                    });
                }
                else
                {
                    Console.Write("Erro ao carregar o relatório de fundos: ");
                    pagina.Nome = "Fundos - Relatorios";
                    pagina.StatusCode = RelatorioFundos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
