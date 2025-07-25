using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;

namespace PortaIDSFTestes.metodos
{
    public class Metodos
    {
        private readonly IPage page;

        public Metodos(IPage page)
        {
            this.page = page;
        }

        public async Task Escrever(string locator, string texto, string passo)
        {
            try
            {
                await page.Locator(locator).FillAsync(texto);
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar o Elemento: " + locator + " Para escrever no passo: " + passo);
            }


        }

        public async Task Clicar(string locator, string passo)
        {
            try
            {
                await page.Locator(locator).ClickAsync();
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar o Elemento: " + locator + " Para Clicar no passo: " + passo);
            }


        }
        public async Task ClicarNoSeletorFundo(string locator,string cpf, string passo)
        {
            try
            {
                await page.Locator(locator).SelectOptionAsync(new[] { cpf });
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar o Elemento: " + locator + " Para Clicar no passo: " + passo);
            }


        }

        public async Task validarUrl(string urlEsperada, string passo)
        {
            try
            {
                await Expect(page).ToHaveURLAsync(urlEsperada);
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar a Url: " + urlEsperada + " Para Validar no passo: " + passo);
            }


        }

        public async Task validarMsgRetornada(string locator, string passo)
        {
            try
            {
                await Expect(page.Locator(locator)).ToBeVisibleAsync();
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar a Url: " + locator + " Para Validar no passo: " + passo);
            }


        }

        public async Task ValidarAcentosAsync(IPage page, string passo)
        {
            try
            {
                string[] textosInvalidos = { "Ã£", "Ã§", "Ü", "Ã¡", "Ã©", "Ã¨", "Ã§Ã£", "Ã§Ã" };
                var errosEncontrados = new List<string>();

                foreach (var texto in textosInvalidos)
                {
                    var locator = page.Locator($"xpath=//*[normalize-space(text()) = '{texto}']");
                    int count = await locator.CountAsync();

                    if (count > 0)
                    {
                        errosEncontrados.Add($"❌ Texto inválido '{texto}' encontrado {count} vez(es).");
                    }
                }

                if (errosEncontrados.Any())
                {
                    Assert.Fail("Erros de acentuação encontrados:\n" + string.Join("\n", errosEncontrados));
                }
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel validar acentuação da Pagina no Passo: " + passo);
            }
        }

        public async Task BaixarExcelPorIdAsync(IPage page, string passo)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);

            try
            {
                // Inicia e aguarda o download ao clicar no botão
                var download = await page.RunAndWaitForDownloadAsync(async () =>
                {
                    var botaoExcel = page.Locator("#BtnBaixarExcel");
                    await Expect(botaoExcel).ToBeVisibleAsync();
                    await botaoExcel.ClickAsync(new LocatorClickOptions { Timeout = 3000 });
                });

                // Exclui se já existir o arquivo antigo
                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Salva o novo download
                await download.SaveAsAsync(filePath);

                // ✅ Assert para garantir que o arquivo foi salvo
                Assert.IsTrue(File.Exists(filePath), "❌ O arquivo Excel não foi baixado corretamente.");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Arquivo Excel baixado com sucesso.");
                Console.ResetColor();

                // Exclui após validação (opcional)
                File.Delete(filePath);
            }
            catch (TimeoutException ex)
            {
                Assert.Fail($"❌ Timeout: O download não foi concluído no tempo esperado.\nDetalhes: {ex.Message}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"❌ Erro ao realizar o download do Excel.\nDetalhes: {ex.Message}");
                throw new Exception("Não foi possivel baixar Excel no passo: " + passo + ex.Message);
            }
        }

        public async Task AtualizarEEnviarArquivo(string caminhoTemplate, string locator, string passo)
        {
            try
            {
                // Verifica se o arquivo existe
                Assert.IsTrue(File.Exists(caminhoTemplate), $"❌ Arquivo de template não encontrado: {caminhoTemplate}");

                // Lê o conteúdo do arquivo
                var linhas = File.ReadAllLines(caminhoTemplate);
                string dataAtual = DateTime.Now.ToString("ddMMyy");
                bool marcadorSubstituido = false;

                // Substitui o marcador #DATA#
                for (int i = 0; i < linhas.Length; i++)
                {
                    if (linhas[i].Contains("#DATA#"))
                    {
                        linhas[i] = linhas[i].Replace("#DATA#", dataAtual);
                        marcadorSubstituido = true;
                    }
                }

                Assert.IsTrue(marcadorSubstituido, "❌ Nenhum marcador '#DATA#' encontrado no arquivo de template.");

                // Cria novo nome de arquivo
                string dataArquivo = DateTime.Now.ToString("yyyyMMdd");
                string idUnico = Guid.NewGuid().ToString().Split('-')[0];
                string novoNomeArquivo = $"FundoQA_{dataArquivo}_{idUnico}.txt";
                string novoCaminho = Path.Combine(Path.GetDirectoryName(caminhoTemplate), novoNomeArquivo);

                // Salva novo arquivo
                File.WriteAllLines(novoCaminho, linhas);
                Assert.IsTrue(File.Exists(novoCaminho), $"❌ O novo arquivo não foi salvo corretamente: {novoCaminho}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Arquivo atualizado salvo como: {novoCaminho}");
                Console.ResetColor();

                // Envia o arquivo via Playwright
                await page.Locator(locator).SetInputFilesAsync(novoCaminho);
                Console.WriteLine("✅ Arquivo enviado com sucesso");

            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao atualizar/enviar arquivo no passo '{passo}': {ex.Message}", ex);
            }
        }

       
        

    }
}

