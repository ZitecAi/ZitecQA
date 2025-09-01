using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestePortal.Pages.CadastroPage
{
    public class CadastroPrestServico
    {
        public static async Task<Model.Pagina> PrestServico (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
                var CadastroPrestServico = await Page.GotoAsync( portalLink + "/PrestServicos/Prestservicos.aspx");

                if (CadastroPrestServico.Status == 200)
                {
                    string seletorTabela = "#tabelaPrestadores";

                    Console.Write("Prestação de Serviços - Cadastro: ");
                    pagina.Nome = "Prstação de serviços";
                    pagina.StatusCode = CadastroPrestServico.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();

                    if (pagina.Acentos == "❌")
                    { 
                    errosTotais++;
                    }

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                    var apagarPrestadorServico2 = Repository.PrestadorServico.PrestadorServico.ApagarPrestadorServico("info", "07084370");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();

                    await Task.Delay(300);
                    await Page.Locator("#nameEmpress").ClickAsync();
                    await Page.Locator("#nameEmpress").FillAsync("Kaio");
                    await Page.GetByPlaceholder("Ex: Logística").ClickAsync();
                    await Page.GetByPlaceholder("Ex: Logística").FillAsync("info");
                    await Page.GetByLabel("Tipo do Prestador:").SelectOptionAsync(new[] { "pf" });
                    await Page.Locator("#inputCNPJ").ClickAsync();
                    await Page.Locator("#inputCNPJ").FillAsync("45543915000181");
                    await Page.Locator("#inputAddress").ClickAsync();
                    await Page.Locator("#inputAddress").FillAsync("Av Alexandre Grandisoli");
                    await Page.GetByPlaceholder("São Paulo").ClickAsync();
                    await Page.GetByPlaceholder("São Paulo").FillAsync("S");
                    await Page.GetByPlaceholder("(00) 00000-").ClickAsync();
                    await Page.GetByPlaceholder("(00) 00000-").FillAsync("(11) 96018-3248");
                    await Page.GetByLabel("Estado", new() { Exact = true }).ClickAsync();
                    await Page.GetByLabel("Estado", new() { Exact = true }).FillAsync("SP");
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "CEP: CEP: CEP:" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "CEP: CEP: CEP:" }).FillAsync("06463-260");
                    await Page.GetByLabel("E-mail:", new() { Exact = true }).ClickAsync();
                    await Page.GetByLabel("E-mail:", new() { Exact = true }).FillAsync("jessica.tavares@gmail.com");
                    await Page.Locator("#FilePrestador").SetInputFilesAsync(new[] { TestePortalIDSF.Program.Config["Paths:Arquivo"] + "documentosteste.zip" });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "+ Representante" }).ClickAsync();
                    await Page.GetByLabel("Nome do Representante:").ClickAsync();
                    await Page.GetByLabel("Nome do Representante:").FillAsync("Jaqueline");
                    await Page.GetByLabel("CPF/CNPJ:").ClickAsync();
                    await Page.GetByLabel("CPF/CNPJ:").FillAsync("12741863822");
                    await Page.GetByLabel("Endereço:").ClickAsync();
                    await Page.GetByLabel("Endereço:").FillAsync("Avenida");
                    await Page.GetByLabel("Telefone:").ClickAsync();
                    await Page.GetByLabel("Telefone:").FillAsync("(11) 24576-816");
                    await Page.GetByLabel("Complemento:").ClickAsync();
                    await Page.GetByLabel("Complemento:").FillAsync("casa");
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Cidade: Cidade:", Exact = true }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Cidade: Cidade:", Exact = true }).FillAsync("Guarulhos");
                    await Page.GetByLabel("Estado:", new() { Exact = true }).ClickAsync();
                    await Page.GetByLabel("Estado:", new() { Exact = true }).FillAsync("SP");
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "CEP: CEP:", Exact = true }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "CEP: CEP:", Exact = true }).FillAsync("07084370");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar a Listagem" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "+ Contas Bancarias" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Favorecido:" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Favorecido:" }).FillAsync("Kaio");
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Banco:" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Banco:" }).FillAsync("439");
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Agência:" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Agência:" }).FillAsync("0001");
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Conta:" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Conta:" }).FillAsync("46091");
                    await Page.Locator("#DigitoCc").ClickAsync();
                    await Page.Locator("#DigitoCc").FillAsync("5");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                    await Task.Delay(400);

                    var prestadorServicoExiste = Repository.PrestadorServico.PrestadorServico.VerificaExistenciaPrestadorServico("info", "06463260");
                   
                    if (prestadorServicoExiste)
                    {
                        Console.WriteLine("Prestador de Serviço cadastrado com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarPrestadorServico = Repository.PrestadorServico.PrestadorServico.ApagarPrestadorServico("info", "06463260");

                        if (apagarPrestadorServico)
                        {
                            Console.WriteLine("Prestador de Serviço apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Prestador de Serviço");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Prestador de Serviço");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }


                }
                else
                {
                    Console.Write("Erro ao carregar a página de Prestação de Serviço no tópico Cadastro ");
                    pagina.Nome = "Prestação de serviços";
                    pagina.StatusCode = CadastroPrestServico.Status;
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
