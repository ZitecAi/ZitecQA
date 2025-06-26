using Microsoft.Playwright;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortalExecutavel.Repository;
using TestePortalExecutavel.Utils;
using TestePortalExecutavel.Model;
using static TestePortalExecutavel.Model.Usuario;

namespace TestePortalExecutavel.Pages
{
    public class ArquivoBaixas
    {
        public static async Task<(Model.Pagina pagina, Operacoes operacoes)> Baixas(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\template.txt";
            operacoes.ListaErros2 = new List<string>();

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
                        var processamentoFundo = Repository.OperacoesZitec.OperacoesZitecRepository.VerificaProcessamentoFundo(9991);

                        if (processamentoFundo)
                        {
                            operacoes.TipoOperacao2 = "Baixas";
                            operacoes.StatusTrocados2 = "❓";
                            operacoes.AprovacoesRealizadas2 = "❓";
                            operacoes.NovoNomeArquivo2 = AtualizarArquivoBaixa.AtualizarDataArquivo(caminhoArquivo);

                            await Page.GetByRole(AriaRole.Button, new() { Name = "Importar Baixa" }).ClickAsync();
                            await Page.Locator("#select_fundo").SelectOptionAsync(new[] { "54638076000176" });
                            string caminhoCompleto = Path.Combine(Program.Config["Paths:Arquivo"], operacoes.NovoNomeArquivo2);
                            await Page.Locator("#fileEnviarBaixas").SetInputFilesAsync(new[] { caminhoCompleto });
                            await Page.Locator("#btnFecharNovoOperacao").ClickAsync();
                            await Task.Delay(35000);
                            var idRecebivel = 14893646;

                            var (existe, idMovimento) = Repository.Baixas.ArquivoBaixas.VerificaMovimento(idRecebivel, 48, 9991);

                            if (existe)
                            {
                                operacoes.ArquivoEnviado = "✅";

                                var apagarBaixa = Repository.Baixas.ArquivoBaixas.ExcluirMovimento(idMovimento);

                                if (apagarBaixa)
                                {
                                    Console.WriteLine("Baixa apagada no banco de dados");

                                }
                                else
                                {
                                    Console.WriteLine("Erro ao apagar baixa");
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Erro ao apagar baixa na tabela");
                                }

                            }
                            else {
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Erro ao enviar arquivo de baixa no portal");
                            }
                        }
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
                if (operacoes.ListaErros2.Count == 0)
                {
                    operacoes.ListaErros2.Add("0");
                }
                return (pagina, operacoes);
            }

            if (operacoes.ListaErros2.Count == 0)
            {
                operacoes.ListaErros2.Add("0");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, operacoes);
        }
    }
}
