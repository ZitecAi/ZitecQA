using Microsoft.Playwright;
//using System.Windows.Controls;
using System.Configuration;
using TestePortalGestora.Model;
using TestePortalGestora.Utils;
using static TestePortalGestora.Model.Usuario;


namespace TestePortalGestora.Pages
{
    public class CadastroOperacoesZitecCsv
    {
        public static async Task<(Model.Pagina pagina, Model.Operacoes operacoes)> OperacoesZitecCsv(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {

            var pagina = new Model.Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\Temp\Arquivos\CNABz.txt";
            operacoes.ListaErros3 = new List<string>();

            try
            {
                var OperacoesZitec = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "Operacoes/Operacoes2.0.aspx");

                if (OperacoesZitec.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";


                    Console.Write("Operações Zitec csv: ");
                    pagina.Nome = "Operações Zitec csv";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌") errosTotais++;
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌") errosTotais++;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operações no tópico Operações: ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesZitec.Status;
                    errosTotais += 2;
                    operacoes.ListaErros3.Add("Erro ao carregar a página de operações");
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (operacoes.ListaErros3.Count == 0)
                {
                    operacoes.ListaErros3.Add("0");
                }
                Console.WriteLine($"exceção: {ex}");
                operacoes.ListaErros3.Add($"Execeção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros3 = errosTotais2;
            }
            if (operacoes.ListaErros3.Count == 0)
            {
                operacoes.ListaErros3.Add("0");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, operacoes);
        }
    }
}
