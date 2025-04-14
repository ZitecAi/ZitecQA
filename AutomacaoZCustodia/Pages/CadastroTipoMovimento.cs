using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Azure;

namespace AutomacaoZCustodia.Pages
{
    public class CadastroTipoMovimento
    {
        public static async Task<Models.Pagina> CadastroDeTipoMovimento (IPage Page)
        {


            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {

                var novoTipoMovimento = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/receivables/movement-type");

                if (novoTipoMovimento.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro novo tipo movimento: ");
                    pagina.Nome = "Cadastro novo tipo movimentol";
                    pagina.StatusCode = novoTipoMovimento.Status;
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
                    pagina.BaixarExcel = "❓";

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Tipo Movimento" }).ClickAsync();
                    await Page.GetByLabel("Tipo Movimento").Locator("svg").ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "BAIXA", Exact = true }).ClickAsync();
                    await Page.GetByLabel("Nome").FillAsync("teste de cadastro");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();


                    var tipoMovExiste = AutomacaoZCustodia.Repository.TipoMovimentoRepository.VerificarExistenciaTpMov("teste de cadastro", "B");
                    if (tipoMovExiste)
                    {
                        pagina.InserirDados = "✅";

                        var apagarTipoMov = AutomacaoZCustodia.Repository.TipoMovimentoRepository.ApagarTpMov("teste da cadastro", "B");

                        if (apagarTipoMov == false)
                        {
                            pagina.Excluir = "✅";


                        }
                        else
                        {
                            pagina.Excluir = "❌";

                        }

                    }
                    else
                    {
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";

                    }

                }
                else
                {

                    Console.Write("Erro ao carregar a página de cadastro de tipo movimento.");
                    pagina.Nome = "Cadastro tipo movimentol";
                    pagina.StatusCode = novoTipoMovimento.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://custodia.idsf.com.br/home/dashboard");



                }

            }
            catch (Exception ex) {

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
