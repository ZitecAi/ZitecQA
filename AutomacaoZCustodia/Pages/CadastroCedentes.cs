using Azure;
using Microsoft.Identity.Client;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutomacaoZCustodia.Pages
{
    public class CadastroCedentes
    {
        public static async Task<Models.Pagina> Cedentes(IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var BoletagemCedentes = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/assignors");

                if (BoletagemCedentes.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cedentes: ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = BoletagemCedentes.Status;
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
                    // pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    var idCedente2 = Repository.CedenteRepository.ObterIdCedente("49624866830", "Cedente teste", 9991);
                    var idRepresentante2 = Repository.CedenteRepository.ObterIdRepresentante(idCedente2);
                    var apagarRepresentante2 = Repository.CedenteRepository.ApagarAssoc(idRepresentante2);
                    var apagarEntidade2 = Repository.CedenteRepository.ApagarEntidadeLigada(idCedente2);
                    var apagarContaBancaria2 = Repository.CedenteRepository.ApagarContaCorrente(idCedente2);
                    var apagarAvalista2 = Repository.CedenteRepository.ApagarAvalista(idCedente2);
                    var apagarcedente2 = Repository.CedenteRepository.ApagarCedente("49624866830", "Cedente teste", 9991);

                    if (apagarcedente2)
                    {
                        Console.WriteLine("cedente apagado com sucesso");
                       
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível apagar cedente");
                    }

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Menuitem, new() { Name = "Formulário de Cedentes" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Fundo").Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Zitec FIDC" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Nome").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").FillAsync("Cedente teste");
                    await Task.Delay(400);
                    await Page.GetByLabel("CPF").CheckAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-mdc-form-field-label-66").GetByText("CPF").ClickAsync();
                    await Page.GetByPlaceholder("000.000.000-").FillAsync("496.248.668-30");
                    await Task.Delay(400);
                    await Page.GetByText("Inscrição Municipal").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Inscrição Municipal").FillAsync("12345678912");
                    await Task.Delay(400);
                    await Page.GetByLabel("Ramo de Atividade").Locator("path").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "DEV", Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Porte").Locator("path").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Microempreendedor Individual" }).DblClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Email", new() { Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Email").FillAsync("jejehtavv@gmail.com");
                    await Task.Delay(400);
                    await Page.GetByLabel("Tipo de Sociedade").Locator("path").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "OUTROS" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-radio-5-input").ClickAsync();
                    await Page.GetByLabel("Classificação de Risco").Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Médio" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Início de Relacionamento").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Início de Relacionamento").FillAsync("01/01/2023");
                    await Task.Delay(400);
                    await Page.GetByText("Término de Relacionamento").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Término de Relacionamento").FillAsync("01/01/2030");
                    await Task.Delay(400);
                    string inputValue = "300000";
                    char[] charArr = inputValue.ToCharArray();
                    for (int ii = 0; ii < charArr.Length; ii++)
                    {
                        await Page.GetByLabel("Faturamento Anual").PressAsync(charArr[ii].ToString());
                    }
                    await Page.Locator("#mat-input-7").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("CEP").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("CEP").FillAsync("07084-370");
                    await Task.Delay(400);
                    await Page.GetByText("Número", new() { Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Número", new() { Exact = true }).FillAsync("901");
                    await Task.Delay(400);
                    await Page.GetByText("Telefone(DDD)").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Telefone(DDD)").FillAsync("(11)96018-3248");
                    await Task.Delay(400);
                    await Page.GetByText("Conglomerado Econômico").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Conglomerado Econômico").FillAsync("financeiro");
                    await Task.Delay(400);
                    await Page.GetByText("Número mínimo de assinaturas").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Número mínimo de assinaturas").FillAsync("2");
                    await Task.Delay(400);
                    string inputValue2 = "400000";
                    char[] charArr2 = inputValue2.ToCharArray();
                    for (int ii = 0; ii < charArr2.Length; ii++)
                    {
                        await Page.GetByLabel("Limite de Crédito").PressAsync(charArr2[ii].ToString());
                    }
                    await Task.Delay(400);
                    await Page.GetByLabel("Tipo de Controle Cedente").Locator("path").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "- Público Federal" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Data do Cadastro").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Data do Cadastro").FillAsync("12/12/2022");
                    await Task.Delay(400);
                    await Task.Delay(400);
                    //await Page.GetByText("Data última alteração").ClickAsync();
                    //await Task.Delay(400);
                    //await Page.GetByLabel("Data última alteração").FillAsync("12/12/2022");
                    //await Task.Delay(400);
                    //await Page.GetByText("Código CNAE").ClickAsync();
                    //await Task.Delay(400);
                    //await Page.GetByLabel("Código CNAE").FillAsync("444");
                    await Task.Delay(400);
                    await Page.GetByLabel("Tipo Chave Pix").Locator("svg").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "E-mail" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Chave Pix", new() { Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Chave Pix", new() { Exact = true }).FillAsync("a@email.com");
                    await Task.Delay(400);
                    await Page.GetByLabel("Coobrigação").Locator("svg").ClickAsync();
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Ambos" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Tab, new() { Name = "Dados da Conta" }).Locator("span").Nth(1).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo", Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator(".mat-mdc-form-field-infix").First.ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Option, new() { Name = "Banco do Brasil S.A." }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Número Agência").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Número Agência").FillAsync("0001");
                    await Task.Delay(400);
                    await Page.Locator("#mat-mdc-form-field-label-72").GetByText("Dígito").ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-input-26").FillAsync("7");
                    await Task.Delay(400);
                    await Page.GetByText("Conta Corrente").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Conta Corrente").FillAsync("7854774");
                    await Task.Delay(400);
                    await Page.Locator("#mat-mdc-form-field-label-76").GetByText("Dígito").ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator("#mat-input-28").FillAsync("3");
                    await Task.Delay(400);
                    await Page.Locator(".col-md-6 > app-input > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Descrição").FillAsync("teste de cadastro");
                    await Task.Delay(400);
                    await Page.Locator("#mat-radio-26-input").CheckAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Representante").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo", Exact = true }).ClickAsync();
                    await Page.Locator("mat-form-field").Filter(new() { HasText = "Nome" }).GetByRole(AriaRole.Textbox).ClickAsync();
                    await Task.Delay(400);
                    await Page.Locator("mat-form-field").Filter(new() { HasText = "Nome" }).GetByRole(AriaRole.Textbox).FillAsync("representante teste");
                    await Task.Delay(400);
                    await Page.Locator("div:nth-child(2) > app-input > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("E-mail").FillAsync("representante@gmail.com");
                    await Task.Delay(400);
                    await Page.Locator("#mat-mdc-form-field-label-84").GetByText("CPF").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByPlaceholder("-00").FillAsync("496.248.668-30");
                    await Task.Delay(400);
                    await Page.GetByText("Telefone").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Telefone").FillAsync("(11)97845-6847");
                    await Page.GetByRole(AriaRole.Radio, new() { Name = "CPF" }).CheckAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Tab, new() { Name = "Avalista" }).Locator("span").Nth(1).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo", Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").FillAsync("avalista teste");
                    await Task.Delay(400);
                    await Page.GetByText("E-mail").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("E-mail").FillAsync("avalista@gmail.com");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Radio, new() { Name = "CPF" }).CheckAsync();
                    await Page.Locator("#mat-mdc-form-field-label-92").GetByText("CPF").ClickAsync();
                    await Page.GetByPlaceholder("-00").FillAsync("496.248.668-30");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByText("Partes Relacionadas").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo", Exact = true }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByLabel("Nome").FillAsync(" parte relacionada teste");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Radio, new() { Name = "CPF" }).CheckAsync();
                    await Task.Delay(400);
                    //await Page.GetByPlaceholder("-00").ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByPlaceholder("-00").FillAsync("496.248.668-30");
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();
                    await Task.Delay(400);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();




                    var cedenteExiste = Repository.CedenteRepository.VerificaExistenciaCedente("49624866830", "Cedente teste", 9991);


                    if (cedenteExiste)
                    {
                        pagina.InserirDados = "✅";

                        var idCedente = Repository.CedenteRepository.ObterIdCedente("49624866830", "Cedente teste", 9991);
                        var idRepresentante = Repository.CedenteRepository.ObterIdRepresentante(idCedente);
                        var apagarRepresentante = Repository.CedenteRepository.ApagarAssoc(idRepresentante);
                        var apagarEntidade = Repository.CedenteRepository.ApagarEntidadeLigada(idCedente);
                        var apagarContaBancaria = Repository.CedenteRepository.ApagarContaCorrente(idCedente);
                        var apagarAvalista = Repository.CedenteRepository.ApagarAvalista(idCedente);
                        var apagarcedente = Repository.CedenteRepository.ApagarCedente("49624866830", "Cedente teste", 9991);

                        if (apagarcedente)
                        {
                            Console.WriteLine("cedente apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar cedente");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("não foi possível inserir cedente");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }


                }
                else
                {
                    Console.Write("Erro ao carregar a página de Cedentes no tópico Boletagem ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = BoletagemCedentes.Status;
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
                //await Page.GotoAsync("https://custodia.idsf.com.br/home/dashboard");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}
