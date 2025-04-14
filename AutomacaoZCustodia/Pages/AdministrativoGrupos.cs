using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AutomacaoZCustodia.Pages
{
    public class AdministrativoGrupos
    {

        public static async Task<Models.Pagina> CadastroGrupos(IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var cadastroGrupos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/admin/groups");

                if (cadastroGrupos.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";
                    Console.Write("Administrativo Grupos: ");
                    pagina.Nome = "Administrativo Grupos";
                    pagina.StatusCode = cadastroGrupos.Status;
                    pagina.Acentos = Utils.VerificarAcentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Utils.VerificarListagem.Listagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    // pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Grupo" }).ClickAsync();
                    await Page.GetByLabel("Nome").ClickAsync();
                    await Page.GetByLabel("Nome").FillAsync("Grupo teste");
                    await Page.GetByLabel("Descrição").ClickAsync();
                    await Page.GetByLabel("Descrição").FillAsync("grupo teste");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";



                }
                else 
                {
                    Console.Write("Erro ao carregar a página de Cedentes no tópico Boletagem ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = cadastroGrupos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://custodia.idsf.com.br/home/dashboard");

                }

            }
            catch (Exception ex)
            {

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
