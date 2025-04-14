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
    public class CadastroSacado
    {
        public static async Task<Models.Pagina> CadastroDeSacado(IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();


            try
            {
                var novoSacado = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/drawees");

                if (novoSacado.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro novo sacado: ");
                    pagina.Nome = "Cadastro novo sacado";
                    pagina.StatusCode = novoSacado.Status;
                    pagina.Acentos = Utils.VerificarAcentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    //pagina.Listagem = Utils.VerificarListagem.Listagem(Page, seletorTabela).Result;

                    //if (pagina.Listagem == "❌")
                    //{
                    //    errosTotais++;
                    //}
                    pagina.Listagem = "❌";
                    pagina.BaixarExcel = "❓";

                    var (sacadoCadastrado2, idSacado2) = Repository.SacadoRepository.VerificaExistenciaSacado("49624866830", "robo@zitec.ai");
                    var idRepresentante2 = Repository.SacadoRepository.ObterIdRepresentante(idSacado2);
                    var deletarAsscRep2 = Repository.SacadoRepository.DeletarAssociacaoSacadoRepresentante(idSacado2);
                    var deletarRepresentante2 = Repository.SacadoRepository.DeletarRepresentante(idRepresentante2);
                    var apagarSacado2 = Repository.SacadoRepository.ApagarSacado("49624866830", "robo@zitec.ai");

                    await Page.GetByLabel("Fundo").Locator("svg").ClickAsync();

                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Zitec FIDC" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Zitec FIDC" }).ClickAsync();
                    await Task.Delay(400);
                    //await Page.PauseAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Sacado" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Nome").ScrollIntoViewIfNeededAsync();
                    await Page.GetByText("Nome").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Nome").FillAsync("sacado teste");
                    await Task.Delay(400);
                    await Page.GetByText("Email", new() { Exact = true }).ScrollIntoViewIfNeededAsync();
                    await Page.GetByText("Email", new() { Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Email").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Email").FillAsync("robo@zitec.ai");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Radio, new() { Name = "CPF" }).ScrollIntoViewIfNeededAsync();
                    await Page.GetByRole(AriaRole.Radio, new() { Name = "CPF" }).CheckAsync();
                    await Task.Delay(400);
                    await Page.GetByPlaceholder("-00").ScrollIntoViewIfNeededAsync();
                    await Page.GetByPlaceholder("-00").FillAsync("496.248.668-30");
                    await Task.Delay(400);
                    await Page.GetByText("Início de relacionamento").ScrollIntoViewIfNeededAsync();
                    await Page.GetByText("Início de relacionamento").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Início de relacionamento").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Início de relacionamento").FillAsync("12/12/2023");
                    await Task.Delay(400);
                    await Page.GetByText("Término de relacionamento").ScrollIntoViewIfNeededAsync();
                    await Page.GetByText("Término de relacionamento").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Término de relacionamento").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Término de relacionamento").FillAsync("12/12/2050");
                    string inputValue = "5000000";
                    await Task.Delay(400);
                    char[] charArr = inputValue.ToCharArray();
                    for (int ii = 0; ii < charArr.Length; ii++)
                    {
                        await Page.GetByLabel("Faturamento Anual").ScrollIntoViewIfNeededAsync();
                        await Page.GetByLabel("Faturamento Anual").PressAsync(charArr[ii].ToString());
                    }
                    await Task.Delay(400);
                    await Page.GetByText("Conglomerado Econômico").ScrollIntoViewIfNeededAsync();
                    await Page.GetByText("Conglomerado Econômico").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Conglomerado Econômico").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Conglomerado Econômico").FillAsync("teste");
                    await Task.Delay(400);
                    await Page.GetByLabel("Porte").Locator("svg").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Porte").Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Microempreendedor Individual").ScrollIntoViewIfNeededAsync();
                    await Page.GetByText("Microempreendedor Individual").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Classificação de Risco").Locator("svg").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Classificação de Risco").Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Alto" }).ScrollIntoViewIfNeededAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Alto" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Tipo de Sociedade").Locator("svg").ScrollIntoViewIfNeededAsync();
                    await Page.GetByLabel("Tipo de Sociedade").Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "LTDA", Exact = true }).ScrollIntoViewIfNeededAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "LTDA", Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-radio-5-input").WaitForAsync();
                    await Page.Locator("#mat-radio-5-input").ClickAsync(new() { Force = true });
                    await Task.Delay(400);
                    await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");

                    await Page.GetByText("Endereço").WaitForAsync();
                    await Page.GetByText("Endereço").ClickAsync(new() { Force = true });
                    await Task.Delay(400);
                    await Page.GetByLabel("Endereço").WaitForAsync();
                    await Page.GetByLabel("Endereço").FillAsync("Avevida teste");
                    await Task.Delay(400);

                    await Page.GetByText("Número").WaitForAsync();
                    await Page.GetByText("Número").ClickAsync(new() { Force = true });
                    await Task.Delay(400);
                    await Page.GetByLabel("Número").WaitForAsync();
                    await Page.GetByLabel("Número").FillAsync("321");
                    await Task.Delay(400);

                    await Page.GetByText("Complemento").WaitForAsync();
                    await Page.GetByText("Complemento").ClickAsync(new() { Force = true });
                    await Task.Delay(400);
                    await Page.GetByLabel("Complemento").WaitForAsync();
                    await Page.GetByLabel("Complemento").FillAsync("teste");
                    await Task.Delay(400);

                    await Page.GetByText("CEP").WaitForAsync();
                    await Page.GetByText("CEP").ClickAsync(new() { Force = true });
                    await Task.Delay(400);
                    await Page.GetByLabel("CEP").WaitForAsync();
                    await Page.GetByLabel("CEP").FillAsync("07084-370");
                    await Task.Delay(400);

                    await Page.GetByText("Bairro").WaitForAsync();
                    await Page.GetByText("Bairro").ClickAsync(new() { Force = true });
                    await Page.GetByLabel("Bairro").WaitForAsync();
                    await Page.GetByLabel("Bairro").FillAsync("teste");
                    await Task.Delay(400);

                    await Page.GetByText("Cidade").WaitForAsync();
                    await Page.GetByText("Cidade").ClickAsync(new() { Force = true });
                    await Page.GetByLabel("Cidade").WaitForAsync();
                    await Page.GetByLabel("Cidade").FillAsync("cidade teste");
                    await Task.Delay(400);

                    await Page.GetByLabel("UF").Locator("svg").WaitForAsync();
                    await Page.GetByLabel("UF").Locator("svg").ClickAsync(new() { Force = true });
                    await Page.GetByRole(AriaRole.Option, new() { Name = "São Paulo" }).WaitForAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "São Paulo" }).ClickAsync();
                    await Task.Delay(400);

                    await Page.GetByText("Telefone(DDD)").WaitForAsync();
                    await Page.GetByText("Telefone(DDD)").ClickAsync(new() { Force = true });
                    await Page.GetByLabel("Telefone(DDD)").WaitForAsync();
                    await Page.GetByLabel("Telefone(DDD)").FillAsync("(11)94856-3214");
                    await Task.Delay(400);
                    await Page.GetByText("Fax(DDD)").WaitForAsync();
                    await Page.GetByText("Fax(DDD)").ClickAsync(new() { Force = true });
                    await Page.GetByLabel("Fax(DDD)").WaitForAsync();
                    await Page.GetByLabel("Fax(DDD)").FillAsync("(11)97845-2477");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo", Exact = true }).WaitForAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo", Exact = true }).ClickAsync(new() { Force = true });
                    await Page.GetByLabel("Novo Representante").Locator("#input-nome").ClickAsync();
                    await Page.GetByLabel("Novo Representante").Locator("#input-nome").FillAsync("Representante teste");
                    await Page.GetByLabel("Novo Representante").Locator("#input-email").ClickAsync();
                    await Page.GetByLabel("Novo Representante").Locator("#input-email").FillAsync("robo@zitec.ai");
                    await Page.Locator(".mat-mdc-dialog-content > app-form-grid > .row > .col-md-4 > app-input > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix").ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "CPF" }).FillAsync("496.248.668-30");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    var botaoSalvar = Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" });
                    await botaoSalvar.WaitForAsync(); // Espera o botão existir no DOM
                    await botaoSalvar.ScrollIntoViewIfNeededAsync(); // Rolagem automática
                    await Task.Delay(500); // Garante tempo para animações carregarem
                    await botaoSalvar.ClickAsync();

                    await Task.Delay(30000);
                    var (sacadoCadastrado, idSacado) = Repository.SacadoRepository.VerificaExistenciaSacado("49624866830", "robo@zitec.ai");
                    var idRepresentante = Repository.SacadoRepository.ObterIdRepresentante(idSacado);

                    if (sacadoCadastrado)
                    {
                        Console.WriteLine("Sacado cadastrado");
                        pagina.InserirDados = "✅";
                        try
                        {
                            var deletarAsscRep = Repository.SacadoRepository.DeletarAssociacaoSacadoRepresentante(idSacado);
                            var deletarRepresentante = Repository.SacadoRepository.DeletarRepresentante(idRepresentante);
                            var apagarSacado = Repository.SacadoRepository.ApagarSacado("49624866830", "robo@zitec.ai");

                            if (apagarSacado || deletarRepresentante || deletarAsscRep)
                            {
                                pagina.Excluir = "✅";
                                Console.WriteLine("Sacado excluido");
                            }
                            else
                            {
                                pagina.Excluir = "❌";
                                Console.WriteLine("erro ao apagar Sacado");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro ao apagar Sacados! Mensagem: " + e.Message);
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                            pagina.TotalErros = errosTotais;
                            return pagina;
                        }
                    }
                    else
                    {
                        pagina.InserirDados = "❌";
                        Console.WriteLine("Sacado não cadastrado");
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de cadastro de sacado.");
                    pagina.Nome = "Cadastro sacado";
                    pagina.StatusCode = novoSacado.Status;
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
