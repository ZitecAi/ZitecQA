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

namespace TesteOperacoesOperacoes.Util
{
    public static class EnviarCsvNegativo
    {

        

        public static async Task<string> EnviarAquivoCvsNegativo(IPage page, string fileName, string TestName)
        {

            string TestesNegativos = "";

            try
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CSV" }).ClickAsync();
                await Task.Delay(200);
                await page.Locator("#selectFundoCsv").SelectOptionAsync(new[] { "54638076000176" });
                await Task.Delay(200);
                await page.Locator("#fileEnviarOperacoesCsv").SetInputFilesAsync(new[] { TestesOperacoesOperacoes.Program.Config["Paths:Arquivo"] + fileName });
                await Task.Delay(200);
                await page.Locator("#fileEnviarLastro").SetInputFilesAsync(new[] { TestesOperacoesOperacoes.Program.Config["Paths:Arquivo"] + "Arquivo teste.zip" });
                await Task.Delay(200);
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                await Task.Delay(200);
                await page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync($"teste negativo de envio de Operação com Arquivo com {TestName} ");
                await Task.Delay(200);
                await page.Locator("#enviarButton").ClickAsync();
                await Task.Delay(200);
                await page.Locator("#btnFecharNovoOperacaoCsv").ClickAsync();
                await page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(fileName);
                await page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();

                bool arquivopresentenatabela = true;
                if (arquivopresentenatabela)
                {
                    var linhas = await page.Locator("#divTabelaCedentes").AllAsync();
                    bool textoEncontrado;

                    foreach (var linha in linhas)
                    {
                        string textolinha = await linha.InnerTextAsync();

                        if (textolinha.Contains("testenegativocnpjoriginadorembranco - copia.csv"))
                        {
                            textoEncontrado = true;
                            Console.WriteLine($"texto encontrado: {textolinha} na tabela.");
                            TestesNegativos = "❌";
                            break;
                        }
                        else
                        {
                            textoEncontrado = false;
                            Console.WriteLine($"arquivo com {TestName} não foi aceito na tabela como esperado!");
                            TestesNegativos = "✅";
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"TimeoutException: O arquivo envio de arquivo não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                TestesNegativos = "❌";
            }
            return TestesNegativos;
        }

    }
}
