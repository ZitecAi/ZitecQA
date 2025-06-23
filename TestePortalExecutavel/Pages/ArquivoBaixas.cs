using Microsoft.Playwright;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortalExecutavel.Repository;
using TestePortalExecutavel.Utils;
using static TestePortalExecutavel.Model.Usuario;

namespace TestePortalExecutavel.Pages
{
    public class ArquivoBaixas
    {
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxoDeCadastro)> Baixas(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            int errosTotais2 = 0;

            try
            {
                var portalLink = Program.Config["Links:Portal"];

                var OperacoesBaixas = await Page.GotoAsync(portalLink + "/Operacoes/ArquivosBaixa.aspx");

                if (OperacoesBaixas.Status == 200)
                {

                    string seletorTabela = "#tabelaBaixas";
                    Console.Write("Operações - Baixas: ");
                    pagina.Nome = "Operações - Baixas";
                    pagina.StatusCode = OperacoesBaixas.Status;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (nivelLogado == NivelEnum.Master )
                    {


                    }




                }
                else
                {
                    Console.Write("Erro ao carregar a página de Baixas no tópico Operações ");
                    pagina.Nome = "Operações - Baixas";
                    pagina.StatusCode = OperacoesBaixas.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                if (fluxoDeCadastros.ListaErros.Count == 0)
                {
                    fluxoDeCadastros.ListaErros.Add("0");
                }
                return (pagina, fluxoDeCadastros);
            }

            if (fluxoDeCadastros.ListaErros.Count == 0)
            {
                fluxoDeCadastros.ListaErros.Add("0");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, fluxoDeCadastros);
        }


    }
}
