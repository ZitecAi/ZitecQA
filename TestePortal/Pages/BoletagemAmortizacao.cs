using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestePortal.Pages
{
    public class BoletagemAmortizacao
    {
        public static async Task<Model.Pagina> Amortizacao (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Task.Delay(500);

            try
            {

                var BoletagemAmortizacao = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Boleta/Amortizacao.aspx");

                if (BoletagemAmortizacao.Status == 200)
                {
                    string seletorTabela = "#tabelaAmortizacao";

                    Console.Write("Amortizacao - Boletagem : ");
                    pagina.Nome = "Amortizacao";
                    pagina.StatusCode = BoletagemAmortizacao.Status;
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

                    var apagarBoletagemAmortizacao2 = Repository.BoletagemAmortizacao.BoletagemAmortizacaoRepository.ApagarBoletagemAmortizacao("teste robo", "49624866830");

                   
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Amortização" }).ClickAsync();
                    await Page.Locator("#fundoAmortizacao").SelectOptionAsync(new[] { "54638076000176" });
                    await Page.Locator("#tipoAmortizacao").SelectOptionAsync(new[] { "Parcial" });
                    await Page.Locator("#jurosAmortizacao").SelectOptionAsync(new[] { "PrincipalOnly" });
                    await Page.GetByLabel("Data de Assembleia:*").ClickAsync();
                    await Page.GetByLabel("Data de Assembleia:*").FillAsync("17/04/2025");
                    await Page.GetByLabel("Data Limite:*").ClickAsync();
                    await Page.GetByLabel("Data Limite:*").FillAsync("17/05/2025");
                    await Page.GetByLabel("Nome do Cotista:*").ClickAsync();
                    await Page.GetByLabel("Nome do Cotista:*").FillAsync("teste robo");
                    await Page.GetByLabel("CPF/CNPJ do Cotista:*").ClickAsync();
                    await Page.GetByLabel("CPF/CNPJ do Cotista:*").FillAsync("49624866830");
                    await Page.GetByPlaceholder("0.000,00").ClickAsync();
                    await Page.GetByPlaceholder("0.000,00").FillAsync("10,000");
                    await Page.GetByLabel("Dígito do Banco:*").ClickAsync();
                    await Page.GetByLabel("Dígito do Banco:*").FillAsync("439");
                    await Page.GetByLabel("Agência:*").ClickAsync();
                    await Page.GetByLabel("Agência:*").FillAsync("0001");
                    await Page.GetByLabel("Conta Corrente:*").ClickAsync();
                    await Page.GetByLabel("Conta Corrente:*").FillAsync("58787");
                    await Page.GetByText("Dígito do Banco:* Agência:*").ClickAsync();
                    await Page.GetByLabel("Dígito da CC.:*").ClickAsync();
                    await Page.GetByLabel("Dígito da CC.:*").FillAsync("1");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                    var BoletagemAmortizacaoExiste = Repository.BoletagemAmortizacao.BoletagemAmortizacaoRepository.VerificaExistenciaBoletagemAmortizacao("teste robo", "49624866830");
                    

                    if (BoletagemAmortizacaoExiste)
                    {
                        Console.WriteLine("Amortização adicionada com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarBoletagemAmortizacao = Repository.BoletagemAmortizacao.BoletagemAmortizacaoRepository.ApagarBoletagemAmortizacao("teste robo", "49624866830");

                        if (apagarBoletagemAmortizacao)
                        {
                            Console.WriteLine("Amortização apagada com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Amortização");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Amortização");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Amortizacao no tópico Boletagem ");
                    pagina.Nome = "Amortizacao";
                    pagina.StatusCode = BoletagemAmortizacao.Status;
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
