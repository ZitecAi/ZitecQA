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
    class RelatorioCadastro
    {
        public static async Task<Model.Pagina> Cadastro (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var RelatorioCadastro = await Page.GotoAsync(portalLink + "/Relatorios/Cadastros.aspx");

                if (RelatorioCadastro.Status == 200)
                {
                    Console.Write("Cadastro  - Relatorios : ");
                    pagina.Nome = "Cadastro - Relatorios";
                    pagina.StatusCode = RelatorioCadastro.Status; 
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
                }
                else
                {
                    Console.Write("Erro ao carregar o relatório de cadastros: ");
                    pagina.Nome = "Cadastro - Relatorios";
                    pagina.StatusCode = RelatorioCadastro.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            } catch (TimeoutException ex) {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
