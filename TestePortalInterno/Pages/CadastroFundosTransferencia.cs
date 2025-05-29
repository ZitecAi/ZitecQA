using Microsoft.Playwright;
using System.Configuration;
using static TestePortalInterno.Model.Usuario;

namespace TestePortalInterno.Pages
{
    public class CadastroFundosTransferencia
    {

        public static async Task<Model.Pagina> FundosTransf(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var CadastroFundosTransferencia = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/FundosTransferencia.aspx");

                if (CadastroFundosTransferencia.Status == 200)
                {
                    string seletorTabela = "#tabelaUsuarios";

                    Console.Write("Fundos De Transferencia - Cadastro: ");
                    pagina.Nome = "Fundos De Transferencia";
                    pagina.StatusCode = CadastroFundosTransferencia.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    if (nivelLogado == NivelEnum.Master)

                    {
                        var apagarFundoTransferencia2 = Repositorys.FundoTransferencia.ApagarFundoTransferencia("16695922000109", "QA teste");
                        await Task.Delay(600);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Fundo de Transferência" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#NomeFundo").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#NomeFundo").FillAsync("QA teste");
                        await Task.Delay(200);
                        await Page.GetByPlaceholder("/0000-00").ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByPlaceholder("/0000-00").FillAsync("16.695.922/0001-09");
                        await Task.Delay(200);
                        await Page.Locator("#Gestora").SelectOptionAsync(new[] { "16007398000128" });
                        await Task.Delay(200);
                        await Page.Locator("#CoGestora").SelectOptionAsync(new[] { "21046086000163" });
                        await Task.Delay(200);
                        await Page.Locator("#Consultora").SelectOptionAsync(new[] { "11578970000195" });
                        await Task.Delay(200);
                        await Page.Locator("#CoConsultora").SelectOptionAsync(new[] { "26452257000178" });
                        await Task.Delay(200);
                        await Page.Locator("#antigoAdministrador").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#antigoAdministrador").FillAsync("Administrador");
                        await Task.Delay(200);
                        await Page.Locator("#tipoFundo").SelectOptionAsync(new[] { "FIDC" });
                        await Task.Delay(200);
                        await Page.Locator("#tipoInvestidor").SelectOptionAsync(new[] { "PROFISSIONAL" });
                        await Task.Delay(200);
                        await Page.Locator("#mercadoFundo").SelectOptionAsync(new[] { "ABERTURA" });
                        await Task.Delay(200);
                        await Page.Locator("#agenteCobranca").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#agenteCobranca").FillAsync("Cobrança");
                        await Task.Delay(200);
                        await Page.Locator("#antigaGestora").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#antigaGestora").FillAsync("antiga");
                        await Task.Delay(200);
                        await Page.Locator("#antigaConsultoria").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#antigaConsultoria").FillAsync("Consultoria");
                        await Task.Delay(200);
                        await Page.Locator("#FileArchives").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "documentosteste.zip" });
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                        var fundoTransferenciaExiste = Repositorys.FundoTransferencia.VerificaExistenciaFundoTransferencia("16695922000109", "QA teste");

                        if (fundoTransferenciaExiste)
                        {
                            Console.WriteLine("Fundo de Transferencia adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarFundoTransferencia = Repositorys.FundoTransferencia.ApagarFundoTransferencia("16695922000109", "QA teste");

                            if (apagarFundoTransferencia)
                            {
                                Console.WriteLine("Fundo de Transferencia apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Fundo de Transferencia");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir fundo de Transferencia");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }
                    }
                    else
                    {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Fundos De Transferencia no tópico Cadastro ");
                    pagina.Nome = "Fundos de Transferência";
                    pagina.StatusCode = CadastroFundosTransferencia.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
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
