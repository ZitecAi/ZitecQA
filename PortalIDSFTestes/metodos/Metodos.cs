using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;

namespace PortalIDSFTestes.metodos
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
                var elemento = page.Locator(locator);
                await elemento.WaitForAsync();
                await elemento.FillAsync(texto);
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
                var elemento = page.Locator(locator);
                await elemento.WaitForAsync();
                await elemento.ClickAsync();
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar o Elemento: " + locator + " Para Clicar no passo: " + passo);
            }


        }
        public async Task ClicarNoSeletorFundo(string locator, string cpf, string passo)
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

        public async Task ValidarUrl(string urlEsperada, string passo)
        {
            try
            {
                await page.WaitForURLAsync(urlEsperada);
                await Expect(page).ToHaveURLAsync(urlEsperada);
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível validar a URL esperada '{urlEsperada}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }


        public async Task ValidarMsgRetornada(string locator, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(locator, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });
                await Expect(page.Locator(locator)).ToBeVisibleAsync();
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{locator}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }


        public async Task VerificarElementoPresenteNaTabela(IPage page, string seletorTabela, string textoEsperado, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(seletorTabela, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });

                var locator = page.Locator(seletorTabela);
                int count = await locator.CountAsync();

                bool textoEncontrado = false;

                for (int i = 0; i < count; i++)
                {
                    var texto = await locator.Nth(i).InnerTextAsync();

                    if (!string.IsNullOrWhiteSpace(texto) && texto.Contains(textoEsperado, StringComparison.OrdinalIgnoreCase))
                    {
                        textoEncontrado = true;
                        Console.WriteLine($"✅ Texto encontrado: {texto}");
                        break;
                    }
                }

                Assert.IsTrue(textoEncontrado, $"❌ O texto '{textoEsperado}' não foi encontrado no(s) elemento(s) com seletor: {seletorTabela}");
            }
            catch (TimeoutException)
            {
                throw new Exception($"⏰ Tempo esgotado ao aguardar a tabela aparecer no passo: {passo}");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao verificar o texto '{textoEsperado}' na tabela no passo: {passo}.\nDetalhes: {ex.Message}");
            }
        }

        public async Task VerificarTextoAusenteNaTabela(IPage page, string seletorTabela, string textoIndesejado, string passo)
        {
            try
            {
                // Aguarda a tabela estar visível
                await page.WaitForSelectorAsync(seletorTabela, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });

                var locator = page.Locator(seletorTabela);
                int count = await locator.CountAsync();

                bool textoEncontrado = false;

                for (int i = 0; i < count; i++)
                {
                    var texto = await locator.Nth(i).InnerTextAsync();

                    if (!string.IsNullOrWhiteSpace(texto) && texto.Contains(textoIndesejado, StringComparison.OrdinalIgnoreCase))
                    {
                        textoEncontrado = true;
                        Console.WriteLine($"❌ Texto indesejado encontrado: {texto}");
                        break;
                    }
                }

                Assert.IsFalse(textoEncontrado, $"❌ O texto indesejado '{textoIndesejado}' foi encontrado no(s) elemento(s) com seletor: {seletorTabela}");
                Console.WriteLine($"✅ O texto '{textoIndesejado}' não está presente na tabela.");
            }
            catch (TimeoutException)
            {
                throw new Exception($"⏰ Tempo esgotado ao aguardar a tabela aparecer no passo: {passo}");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao verificar ausência do texto '{textoIndesejado}' na tabela no passo: {passo}.\nDetalhes: {ex.Message}");
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
                    var locator = page.Locator($"xpath=//*[contains(normalize-space(text()), '{texto}')]");
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
            catch (Exception ex)
            {
                throw new PlaywrightException("Não foi possível validar acentuação da página no passo: " + passo + "\n" + ex.Message);
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
                    await botaoExcel.ClickAsync();
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

        public async Task<string> AtualizarDataArquivo(string caminhoTemplate, string passo)
        {
            try
            {
                Assert.IsTrue(File.Exists(caminhoTemplate), $"❌ Arquivo de template não encontrado: {caminhoTemplate}");

                var linhas = await File.ReadAllLinesAsync(caminhoTemplate);
                string dataHoje = DateTime.Now.ToString("ddMMyy");
                bool dataSubstituida = false;

                var padraoData = new Regex(@"\b\d{6}\b");

                for (int i = 0; i < linhas.Length; i++)
                {
                    var matches = padraoData.Matches(linhas[i]);

                    foreach (Match match in matches)
                    {
                        if (DateTime.TryParseExact(match.Value, "ddMMyy", null, System.Globalization.DateTimeStyles.None, out _))
                        {
                            linhas[i] = linhas[i].Replace(match.Value, dataHoje);
                            dataSubstituida = true;
                        }
                    }
                }

                Assert.IsTrue(dataSubstituida, "❌ Nenhuma data no padrão 'ddMMyy' encontrada para substituir.");

                string nomeArquivo = $"FundoQA_{DateTime.Now:yyyyMMdd}_{Guid.NewGuid().ToString().Split('-')[0]}.txt";
                string novoCaminho = Path.Combine(Path.GetDirectoryName(caminhoTemplate), nomeArquivo);

                await File.WriteAllLinesAsync(novoCaminho, linhas);
                Assert.IsTrue(File.Exists(novoCaminho), $"❌ O novo arquivo não foi salvo corretamente: {novoCaminho}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Arquivo atualizado salvo como: {novoCaminho}");
                Console.ResetColor();

                return novoCaminho;
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao atualizar o arquivo no passo '{passo}': {ex.Message}", ex);
            }
        }


        public async Task EnviarArquivo(string locator, string caminhoArquivo, string passo)
        {
            try
            {
                Assert.IsTrue(File.Exists(caminhoArquivo), $"❌ Arquivo para envio não encontrado: {caminhoArquivo}");

                await page.Locator(locator).SetInputFilesAsync(caminhoArquivo);
                Console.WriteLine("✅ Arquivo enviado com sucesso");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao enviar o arquivo no passo '{passo}': {ex.Message}", ex);
            }
        }






        public async Task ValidarDownloadAsync(IDownload download, string nomeEsperado, string passo)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadPath, nomeEsperado);

            try
            {
                // Remove arquivo antigo, se existir
                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Salva o download no caminho esperado
                await download.SaveAsAsync(filePath);

                // Valida com NUnit
                Assert.IsTrue(File.Exists(filePath), $"❌ O arquivo esperado '{nomeEsperado}' não foi salvo corretamente.");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Arquivo foi baixado e validado com sucesso.");
                Console.ResetColor();

                // Exclusão opcional
                File.Delete(filePath);
                Console.WriteLine("🗑️ Arquivo excluído após validação.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"❌ Erro ao validar o download do arquivo '{nomeEsperado}': {ex.Message}");
            }
        }

        public async Task<string> AtualizarDataEEnviarArquivo(IPage page, string caminhoArquivo)
        {
            var linhas = File.ReadAllLines(caminhoArquivo);

            // Atualizando data
            string dataAtual = DateTime.Now.ToString("ddMMyy");
            string DataArquivoTemplate = linhas[0].Substring(94, 6);
            string AnteriorData = linhas[0].Substring(0, 94);
            string PosData = linhas[0].Substring(101);
            linhas[0] = linhas[0].Replace("#DATA#", dataAtual);

            // Atualizando num consultoria
            Random random = new Random();
            for (int i = 1; i <= 7; i++)
            {
                string randomNumber = "";
                for (int j = 0; j < 25; j++)
                {
                    randomNumber += random.Next(0, 10).ToString();
                }

                linhas[i] = linhas[i].Replace("#DOC_NUMERO_CONSULTORIA_#", randomNumber);

                string randomNumberNumDoc = "";
                for (int j = 0; j < 10; j++)
                {
                    randomNumberNumDoc += random.Next(0, 10).ToString();
                }

                linhas[i] = linhas[i].Replace("#NUM_DOCU#", randomNumberNumDoc);
            }

            string dataFormatada = DateTime.Now.ToString("yyyyMMdd");

            // Usar GUID para garantir que o nome do arquivo seja único
            string uniqueIdentifier = Guid.NewGuid().ToString().Split('-')[0]; // Pega apenas a primeira parte do GUID
            string novoNomeArquivo = $"FundoQA_{dataFormatada}_{uniqueIdentifier}.txt";
            string novoCaminhoArquivo = Path.Combine(Path.GetDirectoryName(caminhoArquivo), novoNomeArquivo);


            File.WriteAllLines(novoCaminhoArquivo, linhas);

            await page.Locator("#fileEnviarOperacoes").SetInputFilesAsync(new[] { novoCaminhoArquivo });

            Console.WriteLine($"Arquivo {novoNomeArquivo} enviado com sucesso.");

            return novoNomeArquivo;
        }

        private static readonly Random random = new();

        public string ModificarCsv(string caminhoEntrada, string pastaSaida)
        {
            try
            {
                if (!File.Exists(caminhoEntrada))
                {
                    Console.WriteLine("Arquivo CSV não encontrado.");
                    return string.Empty;
                }

                var linhas = File.ReadAllLines(caminhoEntrada);

                if (linhas.Length < 2)
                {
                    Console.WriteLine("Arquivo CSV não contém dados suficientes.");
                    return string.Empty;
                }

                // Gera valores aleatórios para os campos
                string novoNumero = GerarNumeroAleatorio();
                string novoDocumento = GerarNumeroAleatorio();

                // Substitui os placeholders pelos valores gerados
                linhas[1] = linhas[1].Replace("#nudocumento#", novoNumero)
                                     .Replace("#seunumero#", novoDocumento);

                var colunas = linhas[1].Split(';');

                Console.WriteLine($"Linha original: {linhas[1]}");
                Console.WriteLine($"Colunas encontradas: {colunas.Length}");

                if (colunas.Length < 14)
                {
                    Console.WriteLine("A linha não tem colunas suficientes.");
                    return string.Empty;
                }

                // Gera um nome único para o arquivo
                string nomeUnico = $"arquivo_modificado_{random.Next(1000, 9999)}.csv";
                string caminhoCompleto = Path.Combine(pastaSaida, nomeUnico);

                // Salva o arquivo modificado
                File.WriteAllLines(caminhoCompleto, linhas);

                return caminhoCompleto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao modificar o CSV: " + ex.Message);
                return string.Empty;
            }
        }

        private static string GerarNumeroAleatorio()
        {
            int parte1 = random.Next(5000000, 5999999);
            int parte2 = random.Next(10, 99);
            int parte3 = random.Next(2020, 2025);
            int parte4 = random.Next(1, 30);
            int parte5 = random.Next(1000, 9999);

            return $"{parte1}-{parte2}.{parte3}.{parte4}.{parte5}";
        }





    }
}

