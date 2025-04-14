using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Azure;
using System.Runtime.InteropServices;

namespace AutomacaoZCustodia.Pages
{
    public class CadastroCertificadora
    {
        public static async Task<Models.Pagina> CadastroDeCertificadora(IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var novaCertificadora = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/certifier");

                if (novaCertificadora.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro nova certificadora: ");
                    pagina.Nome = "Cadastro nova certificadora";
                    pagina.StatusCode = novaCertificadora.Status;
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
                    pagina.InserirDados = "❌";
                    pagina.Excluir = "❌";

                  
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Page.GetByText("Nome").ClickAsync();
                    await Page.GetByLabel("Nome").FillAsync("Certificadora teste");
                    await Page.GetByText("CNPJ").ClickAsync();
                    await Page.GetByPlaceholder("/0000-00").ClickAsync();
                    await Page.GetByPlaceholder("/0000-00").FillAsync("14.716.363/0001-32");
                    await Page.GetByText("Endereço").ClickAsync();
                    await Page.GetByLabel("Endereço").FillAsync("teste");
                    await Page.GetByText("Número").ClickAsync();
                    await Page.GetByLabel("Número").FillAsync("123");
                    await Page.GetByText("Complemento").ClickAsync();
                    await Page.GetByLabel("Complemento").FillAsync("teste");
                    await Page.GetByText("Bairro").ClickAsync();
                    await Page.GetByLabel("Bairro").FillAsync("Continental");
                    await Page.GetByLabel("CEP").ClickAsync();
                    await Page.GetByLabel("CEP").FillAsync("07087-370");
                    await Page.GetByLabel("UF").Locator("svg").ClickAsync();
                    await Page.GetByText("São Paulo").ClickAsync();
                    await Page.GetByText("Cidade").ClickAsync();
                    await Page.GetByLabel("Cidade").FillAsync("São Paulo");
                    await Page.GetByText("Iniciais do Arquivo").ClickAsync();
                    await Page.GetByLabel("Iniciais do Arquivo").FillAsync("teste");
                    await Page.GetByText("QName", new() { Exact = true }).ClickAsync();
                    await Page.GetByLabel("QName", new() { Exact = true }).FillAsync("t");
                    await Page.GetByText("URL", new() { Exact = true }).ClickAsync();
                    await Page.GetByLabel("URL", new() { Exact = true }).FillAsync("teste");
                    await Page.GetByText("QName Consulta Nfe").ClickAsync();
                    await Page.GetByLabel("QName Consulta Nfe").FillAsync("Certificadora teste");
                    await Page.GetByText("URL Consulta Nfe").ClickAsync();
                    await Page.GetByLabel("URL Consulta Nfe").FillAsync("Certificadora Teste");
                    await Page.GetByText("QName Cancelamento Operação").ClickAsync();
                    await Page.GetByLabel("QName Cancelamento Operação").FillAsync("Certificadora teste");
                    await Page.GetByText("URL Cancelamento Operação").ClickAsync();
                    await Page.GetByLabel("URL Cancelamento Operação").FillAsync("Certificadora teste");
                    await Page.GetByText("Usuário Cancelamento Operação").ClickAsync();
                    await Page.GetByLabel("Usuário Cancelamento Operação").FillAsync("usuario teste");
                    await Page.GetByText("Senha Cancelamento Operação").ClickAsync();
                    await Page.GetByLabel("Senha Cancelamento Operação").FillAsync("123");
                    await Page.GetByLabel("Tipo Certificadora").Locator("svg").ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Serviço Padrão" }).ClickAsync();
                    await Page.GetByText("QName Retorno CCB Digital").ClickAsync();
                    await Page.GetByLabel("QName Retorno CCB Digital").FillAsync("Certificadora teste");
                    await Page.GetByLabel("URL Retorno CCB Digital").FillAsync("urlteste");
                    await Page.GetByLabel("Envia Dados da Conta de").CheckAsync();
                    await Page.GetByLabel("Tipo Serviço").Locator("svg").ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Serviço Padrão" }).ClickAsync();
                    await Page.GetByText("QName Certificação de Contrato").ClickAsync();
                    await Page.GetByLabel("QName Certificação de Contrato").FillAsync("Certificadora teste");
                    await Page.GetByText("Usuário Certificação de").ClickAsync();
                    await Page.GetByLabel("Usuário Certificação de").FillAsync("usuario teste");
                    await Page.GetByText("URL Certificação de Contrato").ClickAsync();
                    await Page.GetByLabel("URL Certificação de Contrato").FillAsync("teste");
                    await Page.GetByText("Senha Certificação de Contrato").ClickAsync();
                    await Page.GetByLabel("Senha Certificação de Contrato").FillAsync("998877");
                    await Page.GetByText("Dados de Contato").ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Page.GetByText("Nome do Contato").ClickAsync();
                    await Page.GetByLabel("Nome do Contato").FillAsync("contrato teste");
                    await Page.GetByText("Email do Contato").ClickAsync();
                    await Page.GetByLabel("Email do Contato").FillAsync("robo@zitec.ai");
                    await Page.GetByText("Telefone do Contato").ClickAsync();
                    await Page.GetByLabel("Telefone do Contato").FillAsync("(11) 78458-88874");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                }
                else 
                {

                    Console.Write("Erro ao carregar a página de cadastro de certificadora.");
                    pagina.Nome = "Cadastro certificadora";
                    pagina.StatusCode = novaCertificadora.Status;
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
