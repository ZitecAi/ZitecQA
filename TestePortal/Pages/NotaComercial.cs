using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class NotaComercial
    {
        public static async Task<Model.Pagina> NotasComerciais(IPage Page, NivelEnum nivelLogado, IConfiguration config)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var portalLink = config["Links:Portal"];
                var NotaComercial = await Page.GotoAsync(portalLink + "/Operacoes/NotaComercial.aspx");

                if (NotaComercial.Status == 200)
                {
                    string seletorTabela = "#tabelaNotaComercial";

                    try
                    {
                        Console.Write("Nota Comercial: ");
                        Console.WriteLine(NotaComercial.Status);
                        pagina.StatusCode = NotaComercial.Status;
                        pagina.Nome = "Notas Comerciais";
                        pagina.Reprovar = "❓";

                        if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                        {

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
                            pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;
                            if (pagina.BaixarExcel == "❌")
                            {
                                errosTotais++;
                            }
                            var apagarNotaComercial2 = Repository.NotaComercial.NotaComercialRepository.ApagarNotaComercial("54638076000176", "teste nota comercial");
                            var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");
                            await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#Fundos").SelectOptionAsync(new[] { "54638076000176" });
                            await Page.Locator("#Produtos").SelectOptionAsync(new[] { "125" });
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Digite para buscar..." }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Digite para buscar..." }).FillAsync("teste");
                            await Task.Delay(200);
                            await Page.Keyboard.PressAsync("Enter");
                            await Task.Delay(200);
                            await Page.GetByText("CEDENTE TESTE").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#tipo").SelectOptionAsync(new[] { "pix" });
                            await Task.Delay(200);
                            await Page.Locator("#contaLiquidacao").SelectOptionAsync(new[] { "49525875" });
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste nota comercial");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Tab, new() { Name = "Envolvidos" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "   Adicionar envolvido" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#relacionado").SelectOptionAsync(new[] { "1" });
                            await Task.Delay(200);
                            await Page.Locator("#envolvido").SelectOptionAsync(new[] { "51324287896" });
                            await Task.Delay(200);
                            await Page.Locator("#tipoRelacao").SelectOptionAsync(new[] { "empregador" });
                            await Task.Delay(200);
                            await Page.Locator("#formaEnvio").SelectOptionAsync(new[] { "email" });
                            await Task.Delay(200);
                            await Page.Locator("#formaValidacao").SelectOptionAsync(new[] { "assinaturaSelfie" });
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar envolvido", Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "   Adicionar envolvido" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#relacionado").SelectOptionAsync(new[] { "idsf" });
                            await Task.Delay(200);
                            await Page.Locator("#envolvido").SelectOptionAsync(new[] { "46837686828" });
                            await Task.Delay(200);
                            await Page.Locator("#tipoRelacao").SelectOptionAsync(new[] { "cedente" });
                            await Task.Delay(200);
                            await Page.Locator("#formaEnvio").SelectOptionAsync(new[] { "email" });
                            await Task.Delay(200);
                            await Page.Locator("#formaValidacao").SelectOptionAsync(new[] { "biometriaFacial" });
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar envolvido", Exact = true }).ClickAsync();
                            await Page.Locator("#Operacao-tab").ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Valor Solicitado:*" }).FillAsync("10000");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Taxa de Juros:*" }).FillAsync("1");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Duração:*" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Duração:*" }).FillAsync("10");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Carência Amortização:*" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Carência Amortização:*" }).FillAsync("5");
                            await Task.Delay(200);
                            await Page.Locator("#tipoCalculo").SelectOptionAsync(new[] { "bruto" });
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Dia de Vencimento:*" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Dia de Vencimento:*" }).FillAsync("05");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).FillAsync(dataAtual);
                            await Task.Delay(200);
                            await Page.GetByLabel("CORBAN:").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#indexPosFix").SelectOptionAsync(new[] { "CDI" });
                            await Page.GetByRole(AriaRole.Tab, new() { Name = "Documentos" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "   Adicionar documento" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#fileNotaComercial").SetInputFilesAsync(new[] { config["Paths:Arquivo"] + "Arquivo teste 2.pdf" });
                            await Page.Locator("#tipoDocumento").SelectOptionAsync(new[] { "cpf" });
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Atualizar documento" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar mudanças" }).ClickAsync();


                            var notaComercialExiste = Repository.NotaComercial.NotaComercialRepository.VerificaExistenciaNotaComercial("54638076000176", "teste nota comercial");


                            if (notaComercialExiste)
                            {
                                Console.WriteLine("Nota comercial adicionada com sucesso na tabela.");
                                pagina.InserirDados = "✅";
                                var apagarNotaComercial = Repository.NotaComercial.NotaComercialRepository.ApagarNotaComercial("54638076000176", "teste nota comercial");

                                if (apagarNotaComercial)
                                {
                                    Console.WriteLine("Nota comercial apagado com sucesso");
                                    pagina.Excluir = "✅";
                                }
                                else
                                {
                                    Console.WriteLine("Não foi possível apagar Nota comercial");
                                    pagina.Excluir = "❌";
                                    errosTotais++;
                                }

                            }
                            else
                            {
                                Console.WriteLine("Não foi possível inserir Nota comercial");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                            }

                        }
                        else 
                        {
                            pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                            if (pagina.Acentos == "❌")
                            {
                                errosTotais++;
                            }
                            pagina.Listagem = "❓";
                            pagina.BaixarExcel = "❓";
                            pagina.InserirDados = "❓";
                            pagina.Excluir = "❓";

                        }
                    }

                    catch (TimeoutException ex)
                    {
                        Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                        Console.WriteLine($"Exceção: {ex.Message}");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais++;
                        await Page.GotoAsync(portalLink + "/login.aspx");

                    }
                
                
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Notas Comercial ");
                    pagina.Nome = "Notas Comerciais";
                    pagina.StatusCode = NotaComercial.Status;
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
                errosTotais+=2;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
