using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Playwright;
using Segment.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Utils;
using static TestePortalInterno.Model.Usuario;

namespace TestePortalInterno.Pages
{
    public class CadastroFundos
    {
        public static async Task<Model.Pagina> Fundos(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var CadastroFundos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Fundos.aspx");

                if (CadastroFundos.Status == 200)
                {
                    string seletorTabela = "#tabelaFundos";

                    Console.Write("Fundos - Cadastro: ");
                    pagina.StatusCode = CadastroFundos.Status;
                    pagina.Nome = "Fundos";
                    listErros.Add("0");
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    if (nivelLogado == NivelEnum.Master)
                    {


                        var apagarFundo2 = Repository.Fundos.FundosRepository.ApagarFundo("45543915000181", "teste QA");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Fundo" }).ClickAsync();
                        await Page.Locator("#Nome").FillAsync("teste QA");
                        await Page.GetByPlaceholder("/0000-00").ClickAsync();
                        await Page.GetByPlaceholder("/0000-00").FillAsync("45543915000181");
                        await Page.Locator("#tipoProcessamento").SelectOptionAsync(new[] { "zitec" });
                        await Page.Locator("#ambienteSelect").SelectOptionAsync(new[] { "custodia" });
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                        await Task.Delay(1500);


                        var fundoExiste = Repository.Fundos.FundosRepository.VerificaExistenciaFundo("45543915000181", "teste QA");

                        if (fundoExiste)
                        {
                            Console.WriteLine("Fundo adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarFundo = Repository.Fundos.FundosRepository.ApagarFundo("45543915000181", "teste QA");

                            if (apagarFundo)
                            {
                                Console.WriteLine("Fundo apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Fundo");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir fundo");
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
                    Console.Write("Erro ao carregar a página de Fundos no tópico Cadastro ");
                    pagina.Nome = "Fundos";
                    errosTotais++;
                    pagina.StatusCode = CadastroFundos.Status;
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
