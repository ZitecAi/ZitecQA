using Azure;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AutomacaoZCustodia.Pages
{
    public class CadastroEntidade
    {

        public static async Task<Models.Pagina> CadastroDeEntidade(IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {

                var novaEntidade = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/entities");
                if (novaEntidade.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";
                    Console.Write("Cadastro nova entidade: ");
                    pagina.Nome = "Cadastro nova entidade";
                    pagina.StatusCode = novaEntidade.Status;
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
                    await Task.Delay(400);
                    await Page.Locator("#mat-mdc-form-field-label-0 span").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").FillAsync("Entidade teste");
                    await Task.Delay(400);
                    await Page.GetByText("Data do Contrato").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Data do Contrato").FillAsync("23/01/2025");
                    await Task.Delay(400);
                    await Page.GetByLabel("CPF").CheckAsync();
                    await Task.Delay(400);
                    await Page.GetByPlaceholder("-00").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByPlaceholder("-00").FillAsync("496.248.668-30");
                    await Task.Delay(400);
                    await Page.Locator("div:nth-child(5) > app-input > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Email").FillAsync("robo@zitec.ai");
                    await Task.Delay(400);
                    await Page.GetByText("Endereço").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Endereço").FillAsync("Avenida teste");
                    await Task.Delay(400);
                    await Page.GetByText("Número").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Número").FillAsync("123");
                    await Task.Delay(400);
                    await Page.GetByText("CEP").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("CEP").FillAsync("07084-370");
                    await Task.Delay(400);
                    await Page.GetByText("Bairro").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Bairro").FillAsync("luz");
                    await Task.Delay(400);
                    await Page.Locator("div:nth-child(12) > app-input > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Cidade").FillAsync("S");
                    await Task.Delay(400);
                    await Page.GetByLabel("Estado", new() { Exact = true }).Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "São Paulo" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Telefone").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Telefone").FillAsync("(11) 24576-8164");
                    await Task.Delay(400);
                    await Page.GetByLabel("Administrador").CheckAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Conta Corrente Consultoria").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Banco").Locator("span").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Banco do Brasil S.A." }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Nº Agência (Sem dígito)").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nº Agência (Sem dígito)").FillAsync("001");
                    await Task.Delay(400);
                    await Page.GetByText("Conta Corrente", new() { Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Conta Corrente", new() { Exact = true }).FillAsync("578499");
                    await Task.Delay(400);
                    await Page.GetByLabel("Descrição").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Descrição").FillAsync("teste");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Representantes").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").FillAsync("Representante teste");
                    await Page.GetByLabel("Email").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Email").FillAsync("robo@zitec.ai");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Radio, new() { Name = "CPF" }).CheckAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-mdc-form-field-label-42").GetByText("CPF").ClickAsync();
                    await Page.GetByPlaceholder("-00").FillAsync("496.248.668-30");
                    await Task.Delay(400);
                    await Page.GetByLabel("Telefone").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Telefone").FillAsync("(11) 9785-46214");
                    await Task.Delay(400);
                    await Page.Locator("#mat-radio-9-input").CheckAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-radio-12-input").CheckAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-radio-15-input").CheckAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-radio-18-input").CheckAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Partes Relacionadas").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-mdc-form-field-label-46 span").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").FillAsync("teste");
                    await Task.Delay(400);
                    await Page.Locator("div:nth-child(2) > app-input > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix").ClickAsync();
                    await Page.GetByPlaceholder("-00").FillAsync("496.248.668-30");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                    var idPessoa = 1432;
                    //var idPessoa = Repository.EntidadeRepository.ObterIdPessoa("Entidade teste", "robo@zitec.ai");
                    var idRepresentante = Repository.EntidadeRepository.ObterIdRepresentante("robo@zitec.ai", "49624866830");

                    if (idPessoa != 0 && idRepresentante != 0)
                    {
                        pagina.InserirDados = "✅";

                        var deletarAssociado = Repository.EntidadeRepository.DeletarAssociacoesDoRepresentante(idRepresentante);
                        var deletarRepresentante = Repository.EntidadeRepository.DeletarRepresentante("robo@zitec.ai", "49624866830");
                        var deletarAssPessoa = Repository.EntidadeRepository.DeletarAssociacoesDaPessoa(idPessoa);
                        var deletarContaCorrente = Repository.EntidadeRepository.DeletarContaCorrente(idPessoa);
                        var deletarParteRelacionada = Repository.EntidadeRepository.DeletarParteRelacionada(idPessoa);
                        var deletarEntidade = Repository.EntidadeRepository.DeletarPessoa("Entidade teste", "robo@zitec.ai");

                        if (deletarEntidade)
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

                    Console.Write("Erro ao carregar a página de cadastro de entidade.");
                    pagina.Nome = "Cadastro entidade";
                    pagina.StatusCode = novaEntidade.Status;
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
