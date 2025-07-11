using Azure;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteOperacoesOperacoes.Model;
using TestesOperacoesOperacoes;
using Program = TestesOperacoesOperacoes.Program;
using static Microsoft.Playwright.Assertions;
namespace TesteOperacoesOperacoes.Util
{
    public static class EnviarCsvNegativo
    {



        public static async Task<TesteNegativoResultado> EnviarAquivoCvsNegativo(IPage page, string fileName, string testName)
        {
            string resultado = "❌"; // default como falha
            try
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CSV" }).ClickAsync();
                await Task.Delay(200);
                await page.Locator("#selectFundoCsv").SelectOptionAsync("54638076000176");
                await Task.Delay(200);
                await page.Locator("#fileEnviarOperacoesCsv").SetInputFilesAsync(TestesOperacoesOperacoes.Program.Config["Paths:Arquivo"] + fileName);
                await Task.Delay(200);
                await page.Locator("#fileEnviarLastro").SetInputFilesAsync(TestesOperacoesOperacoes.Program.Config["Paths:Arquivo"] + "Arquivo teste.zip");
                await Task.Delay(200);
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync($"teste negativo de envio com {testName}");
                await Task.Delay(200);
                await page.Locator("#enviarButton").ClickAsync();
                await Task.Delay(200);
                await page.Locator("#btnFecharNovoOperacaoCsv").ClickAsync();
                await page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(fileName);

                var linhas = await page.Locator("#divTabelaCedentes").AllAsync();

                foreach (var linha in linhas)
                {
                    string textoLinha = await linha.InnerTextAsync();

                    if (textoLinha.Contains(fileName))
                    {
                        Console.WriteLine($"[❌] Arquivo {fileName} foi aceito na tabela, mas não deveria.");
                        resultado = "❌";
                        return new TesteNegativoResultado { IdDoTeste = testName, Resultado = resultado };
                    }
                }

                // Caso ele NÃO esteja na tabela, o teste negativo passou (funcionou)
                Console.WriteLine($"[✅] Arquivo {fileName} não está na tabela, teste negativo passou.");
                resultado = "✅";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] Exceção ao enviar {fileName}: {ex.Message}");
                resultado = "❌";
            }

            return new TesteNegativoResultado
            {
                IdDoTeste = testName,
                Resultado = resultado
            };



        }



        public static async Task<TesteNegativoResultado> EnviarArquivoPdfNegativo(IPage Page, string idTeste)
        {
            try
            {
                await Page.ReloadAsync();
                await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CSV" }).ClickAsync();
                await Task.Delay(200);
                await Page.Locator("#selectFundoCsv").SelectOptionAsync(new[] { "54638076000176" });
                await Task.Delay(200);
                await Page.Locator("#fileEnviarOperacoesCsv").SetInputFilesAsync(new[] { Program.Config["Paths:Arquivo"] + "teste.pdf" });
                await Task.Delay(200);
                await Page.Locator("#fileEnviarLastro").SetInputFilesAsync(new[] { Program.Config["Paths:Arquivo"] + "Arquivo teste.zip" });
                await Task.Delay(200);
                await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste negativo de envio pdf");
                await Task.Delay(200);
                await Page.Locator("#enviarButton").ClickAsync();

                await Expect(Page.GetByText("Arquivo CSV inválido: teste.pdf. Apenas arquivos .csv são permitidos.")).ToBeVisibleAsync();

                return new TesteNegativoResultado
                {
                    IdDoTeste = idTeste,
                    Resultado = "✅"
                };
            }
            catch (Exception ex)
            {
                return new TesteNegativoResultado
                {
                    IdDoTeste = idTeste,
                    Resultado = $"Falha ao validar rejeição do PDF: {ex.Message}"
                };
            }
        }
    }





}

