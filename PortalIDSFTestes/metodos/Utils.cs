using Allure.NUnit.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using OfficeOpenXml;
using System.IO.Compression;
using static Microsoft.Playwright.Assertions;

namespace PortalIDSFTestes.metodos
{

    public class Utils
    {
        private readonly IPage page;

        public Utils(IPage page)
        {
            this.page = page;
        }
        [AllureStep("Escrever - no passo: {passo}")]
        public async Task Escrever(string locator, string texto, string passo)
        {
            try
            {

                var element = page.Locator(locator);
                await Expect(element).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 90000 });
                await Expect(element).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 90000 });
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await element.FocusAsync();
                await element.FillAsync(texto, new LocatorFillOptions { Timeout = 90000 });
            }
            catch (PlaywrightException ex)
            {
                throw new PlaywrightException("Não foi possivel Escrever no Elemento: " + locator + " no passo: " + passo + $"Detalhes {ex.Message}");
            }


        }
        public async Task EscreverCredenciais(string locator, string texto, string passo)
        {
            try
            {
                var element = page.Locator(locator);
                await Expect(element).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 90000 });
                await Expect(element).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 90000 });
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await element.FocusAsync();
                await element.FillAsync(texto);
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar o Elemento: " + locator + " Para escrever no passo: " + passo);
            }
        }
        [AllureStep("Clicar - no passo: {passo}")]
        public async Task Clicar(string locator, string passo)
        {
            try
            {
                var element = page.Locator(locator);

                await Expect(element).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 90000 });
                await Expect(element).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 90000 });
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                await element.ClickAsync(new LocatorClickOptions
                {
                    Timeout = 60000
                });
            }
            catch (PlaywrightException ex)
            {
                throw new PlaywrightException("Não foi possivel Clicar no Elemento: " + locator + " no passo: " + passo + $"Detalhes {ex.Message}");
            }


        }
        [AllureStep("Clicar - no passo: {passo}")]
        public async Task Clicar(ILocator locator, string passo)
        {
            try
            {
                var element = locator;

                await Expect(element).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 90000 });
                await Expect(element).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 90000 });
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                await element.ClickAsync(new LocatorClickOptions
                {
                    Timeout = 60000
                });
            }
            catch (PlaywrightException ex)
            {
                throw new PlaywrightException("Não foi possivel Clicar no Elemento: " + locator + " no passo: " + passo + $"Detalhes {ex.Message}");
            }


        }
        [AllureStep("Limpar - no passo: {passo}")]
        public async Task Limpar(string locator, string passo)
        {
            try
            {
                var element = page.Locator(locator);

                await Expect(element).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 90000 });
                await Expect(element).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 90000 });
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                await element.ClearAsync(new LocatorClearOptions
                {
                    Timeout = 60000
                });
            }
            catch (PlaywrightException ex)
            {
                throw new PlaywrightException("Não foi possivel Limpar o Elemento: " + locator + " no passo: " + passo + $"Detalhes {ex.Message}");
            }


        }
        [AllureStep("Clicar no seletor - no passo: {passo}")]
        public async Task ClicarNoSeletor(string locator, string option, string passo)
        {
            try
            {
                await page.Locator(locator).SelectOptionAsync(new[] { option });
            }
            catch
            {
                throw new PlaywrightException("Não foi possivel Encontrar o Elemento: " + locator + " Para Clicar no passo: " + passo);
            }
        }
        [AllureStep("Validar URL - no passo: {passo}")]
        public async Task ValidarUrl(string urlEsperada, string passo)
        {
            try
            {
                await page.WaitForURLAsync(urlEsperada);
                if (urlEsperada == "https://portal.idsf.com.br/home.aspx#")
                {
                    await Expect(page).ToHaveURLAsync(urlEsperada);
                }
                if (urlEsperada == "https://portal-staging.idsf.com.br/Home.aspx")
                {
                    await Expect(page).ToHaveURLAsync(urlEsperada);
                }
                if (urlEsperada == "https://portal-dev.idsf.com.br/Home.aspx")
                {
                    await Expect(page).ToHaveURLAsync(urlEsperada);
                }
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível validar a URL esperada '{urlEsperada}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }

        [AllureStep("Validar mensagem retornada - no passo: {passo}")]
        public async Task ValidarMsgRetornada(string locator, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(locator, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });
                string textoRetornado = page.Locator(locator).InnerTextAsync().Result;
                Console.WriteLine(textoRetornado);
                await Expect(page.Locator(locator)).ToBeVisibleAsync(new() { Timeout = 90000 }); ;
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{locator}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }
        [AllureStep("Validar mensagem por texto - no passo: {passo}")]
        public async Task ValidarMensagemPorTextoAsync(string locator, string textoEsperado, string passo)
        {
            try
            {
                // filtra pelo texto dentro do próprio locator (resolve conflitos de mesma classe)
                var alvo = page.Locator(locator).Filter(new LocatorFilterOptions
                {
                    HasTextString = textoEsperado
                });

                // garante que existe exatamente 1 elemento com esse texto
                await Expect(alvo).ToHaveCountAsync(1);

                // valida que o elemento contém o texto esperado (espera visibilidade por padrão)
                await Expect(alvo.First).ToContainTextAsync(textoEsperado);

                Console.WriteLine($"✅ '{textoEsperado}' encontrado em '{locator}' (único match).");
            }
            catch (Exception ex)
            {
                // diagnóstico extra: lista o que foi encontrado naquele locator
                try
                {
                    var encontrados = await page.Locator(locator).AllInnerTextsAsync();
                    var joined = string.Join(" | ", encontrados.Select(t => t.Trim()));
                    throw new Exception(
                        $"❌ Conflito/ausência ao validar texto '{textoEsperado}' em '{locator}'. " +
                        $"Textos encontrados: [{joined}]. Detalhe: {ex.Message}", ex);
                }
                catch
                {
                    throw new Exception(
                        $"❌ Erro ao validar texto '{textoEsperado}' em '{locator}', no passo: " + passo);
                }
            }
        }
        [AllureStep("Esperar texto presente - no passo: {passo}")]
        public async Task EsperarTextoPresente(string textoEsperado, string passo)
        {
            try
            {
                await page.GetByText(textoEsperado)
                  .WaitForAsync(new LocatorWaitForOptions
                  {
                      State = WaitForSelectorState.Visible
                  });
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{textoEsperado}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }
        [AllureStep("Validar texto presente - no passo: {passo}")]
        public async Task ValidarTextoPresente(string textoEsperado, string passo)
        {
            try
            {
                await Expect(page.GetByText(textoEsperado)).ToBeVisibleAsync(new() { Timeout = 90000 }); ;
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{textoEsperado}' no passo: '{passo}'. Detalhes: " + ex.Message);
            }
        }
        [AllureStep("Validar texto presente - no passo: {passo}")]
        public async Task ValidarTextoPresente(ILocator locator, string passo)
        {
            try
            {
                await Expect(locator).ToBeVisibleAsync(new() { Timeout = 90000 }); ;
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{locator}' no passo: '{passo}'. Detalhes: " + ex.Message);
            }
        }
        [AllureStep("Validar texto do elemento - no passo: {passo}")]
        public async Task ValidarTextoDoElemento(string locator, string textoEsperado, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(locator, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });
                await Expect(page.Locator(locator)).ToHaveTextAsync(textoEsperado);
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{textoEsperado}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }
        [AllureStep("Validar texto do elemento - no passo: {passo}")]
        public async Task ValidarTextoCapturado(string locator, string textoEsperado, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(locator, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });
                var textoCapturado = await page.Locator(locator).InnerTextAsync();
                Assert.That(textoCapturado, Is.EqualTo(textoEsperado));
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{textoEsperado}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }
        [AllureStep("Validar texto do elemento - no passo: {passo}")]
        public async Task<string> CapturarTexto(string locator, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(locator, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });
                var textoCapturado = await page.Locator(locator).InnerTextAsync();
                return textoCapturado;
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{locator}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }
        [AllureStep("Capturar texto do elemento - no passo: {passo}")]
        public async Task<string> CapturarTextoDoElemento(string locator, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(locator, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible
                });
                string texto = await page.Locator(locator).InnerTextAsync();

                int quantidade = Convert.ToInt16(texto);

                if (quantidade < 100)
                {
                    Assert.Fail(passo + " - Quantidade de ativos menor que 100: " + quantidade);
                }
                Console.WriteLine($"✅ Texto capturado, Quantidade de ativos: {texto}");
                return texto;
            }
            catch (Exception ex)
            {
                throw new PlaywrightException($"❌ Não foi possível encontrar/validar o elemento '{locator}' no passo: '{passo}'. Detalhes: {ex.Message}");
            }
        }

        [AllureStep("Verificar elemento presente na tabela - no passo: {passo}")]
        public async Task VerificarElementoPresenteNaTabela(IPage page, string seletorTabela, string textoEsperado, string passo)
        {
            try
            {
                await page.WaitForSelectorAsync(seletorTabela, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 90000
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
        [AllureStep("Verificar texto ausente na tabela - no passo: {passo}")]
        public async Task VerificarTextoAusenteNaTabela(IPage page, string seletorTabela, string textoDesejado, string passo)
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

                    if (!string.IsNullOrWhiteSpace(texto) && texto.Contains(textoDesejado, StringComparison.OrdinalIgnoreCase))
                    {
                        textoEncontrado = true;
                        Console.WriteLine($"❌ Texto indesejado encontrado: {texto}");
                        break;
                    }
                }

                Assert.IsFalse(textoEncontrado, $"❌ O texto indesejado '{textoDesejado}' foi encontrado no(s) elemento(s) com seletor: {seletorTabela}");
                Console.WriteLine($"✅ O texto '{textoDesejado}' não está presente na tabela.");
            }
            catch (TimeoutException)
            {
                throw new Exception($"⏰ Tempo esgotado ao aguardar a tabela aparecer no passo: {passo}");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao verificar ausência do texto '{textoDesejado}' na tabela no passo: {passo}.\nDetalhes: {ex.Message}");
            }
        }




        [AllureStep("Validar acentuação - no passo: {passo}")]
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
                        errosEncontrados.Add($"❌ Texto inválido encontrado: {texto}");
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

        [AllureStep("Baixar Excel por ID - no passo: {passo}")]
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
        [AllureStep("Atualizar data no arquivo - no passo: {passo}")]
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

        [AllureStep("Enviar arquivo - no passo: {passo}")]
        public async Task EnviarArquivo(string locator, string caminhoArquivo, string passo)
        {
            try
            {
                Assert.IsTrue(File.Exists(caminhoArquivo), $"❌ Arquivo para envio não encontrado: {caminhoArquivo}");
                await page.Locator(locator).SetInputFilesAsync(caminhoArquivo, new LocatorSetInputFilesOptions { Timeout = 120_000 });
                Console.WriteLine("✅ Arquivo enviado com sucesso");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao enviar o arquivo no passo '{passo}': {ex.Message}", ex);
            }
        }


        [AllureStep("Enviar arquivo cedente novo - no passo: {passo}")]
        public async Task EnviarArquivoCedenteNovo(string locator, string caminhoArquivo, string caminhoExcel, string cnpjMassa, string passo)
        {
            try
            {
                Assert.IsTrue(File.Exists(caminhoArquivo), $"Arquivo para envio não encontrado: {caminhoArquivo}");

                // EPPlus precisa disso
                ExcelPackage.License.SetNonCommercialOrganization("Zitec");


                string entryPath = "Kit Cedente/Ficha Cedente - ID CTVM.xlsx";

                using (var zip = ZipFile.Open(caminhoArquivo, ZipArchiveMode.Update))
                {
                    var entry = zip.GetEntry(entryPath);
                    if (entry == null)
                        throw new FileNotFoundException($"Excel não encontrado dentro do zip: {entryPath}");

                    using var ms = new MemoryStream();
                    using (var entryStream = entry.Open())
                        entryStream.CopyTo(ms);

                    ms.Position = 0;
                    var random = new Random();
                    string cnpjAleatorio = cnpjMassa;

                    using (var package = new ExcelPackage(ms))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var plan = package.Workbook.Worksheets[i];
                            plan.Cells[3, 3].Value = cnpjAleatorio;
                        }
                        package.Save();
                    }

                    entry.Delete(); // remove o antigo
                    var newEntry = zip.CreateEntry(entryPath);
                    ms.Position = 0;
                    using (var newStream = newEntry.Open())
                        ms.CopyTo(newStream);
                }

                await page.Locator(locator).SetInputFilesAsync(
                    caminhoArquivo,
                    new LocatorSetInputFilesOptions { Timeout = 120_000 });

                Console.WriteLine("✅ Arquivo enviado com sucesso");
            }
            catch (Exception)
            {
                throw new Exception($"❌ Erro ao enviar o arquivo no passo '{passo}'");
            }
        }
        [AllureStep("Enviar arquivo com nome atualizado - no passo: {passo}")]
        public async Task<string> EnviarArquivoNomeAtualizado(string locator, string caminhoArquivo, string passo)
        {
            try
            {
                Assert.IsTrue(File.Exists(caminhoArquivo),
                    $"❌ Arquivo para envio não encontrado: {caminhoArquivo}");

                var dir = Path.GetDirectoryName(caminhoArquivo) ?? Directory.GetCurrentDirectory();
                var baseName = Path.GetFileNameWithoutExtension(caminhoArquivo);
                var ext = Path.GetExtension(caminhoArquivo) ?? string.Empty;

                // Sanitiza o nome base (remove chars inválidos para nomes de arquivo)
                var invalid = Path.GetInvalidFileNameChars();
                var safeBase = new string(baseName.Where(c => !invalid.Contains(c)).ToArray());
                if (string.IsNullOrWhiteSpace(safeBase)) safeBase = "arquivo";

                // Sufixo aleatório: timestamp + 6 chars do GUID
                string NovoNome()
                {
                    var guid6 = Guid.NewGuid().ToString("N").Substring(0, 6); // <- aqui está a correção
                    return $"{safeBase}_{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{guid6}{ext}";
                }

                // Gera nome único
                string novoNome, novoCaminho;
                do
                {
                    novoNome = NovoNome();
                    novoCaminho = Path.Combine(dir, novoNome);
                } while (File.Exists(novoCaminho));

                // Cria o novo arquivo com o mesmo conteúdo
                File.Copy(caminhoArquivo, novoCaminho, overwrite: false);

                // Envia o novo arquivo
                await page.Locator(locator).SetInputFilesAsync(novoCaminho);

                Console.WriteLine($"✅ Arquivo '{novoNome}' enviado com sucesso ({passo}).");
                return novoNome; // retorna apenas o nome, como você pediu
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao enviar o arquivo no passo '{passo}': {ex.Message}", ex);
            }
        }






        [AllureStep("Validar download do arquivo - no passo: {passo}")]
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
        [AllureStep("Atualizar data e enviar arquivo - no passo: {passo}")]
        public async Task<string> AtualizarDataEEnviarArquivo(IPage page, string caminhoArquivo, string passo)
        {
            try
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
                for (int i = 1; i <= 2; i++)
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
                string novoNomeArquivo = $"CnabZitec_{dataFormatada}_{uniqueIdentifier}.txt";
                string novoCaminhoArquivo = Path.Combine(Path.GetDirectoryName(caminhoArquivo), novoNomeArquivo);


                File.WriteAllLines(novoCaminhoArquivo, linhas);

                await page.Locator("#fileEnviarOperacoes").SetInputFilesAsync(new[] { novoCaminhoArquivo });

                Console.WriteLine($"Arquivo {novoNomeArquivo} enviado com sucesso.");

                return novoNomeArquivo;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possivel Encontrar Arquivo " + caminhoArquivo + " No passo: " + passo + " Mensagem: " + e.Message);
            }
        }


        private static readonly Random random = new();
        [AllureStep("Modificar CSV - no passo: {passo}")]
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

        public async Task ValidarElementoHabilitado(string locator, string passo)
        {
            try
            {
                var element = page.Locator(locator);
                await Expect(element).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 90000 });
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await Expect(element).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 90000 });
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possivel validar elemento habilitado: {locator}, no passo: {passo}" + ex.Message);
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

        [AllureStep("Validar download e tamanho do arquivo - no passo: {step}")]
        public async Task ValidateDownloadAndLength(IPage page, string locatorClickDownload, string step, string downloadsDir = null)
        {
            try
            {
                downloadsDir ??= Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads"
                );

                var download = await page.RunAndWaitForDownloadAsync(
                    async () =>
                    {
                        var element = page.Locator(locatorClickDownload);
                        await element.WaitForAsync(new LocatorWaitForOptions { Timeout = 90000 });
                        await element.ClickAsync();
                    },
                    new PageRunAndWaitForDownloadOptions { Timeout = 120000 }
                );

                var fileName = download.SuggestedFilename;
                var finalPath = Path.Combine(downloadsDir, fileName);

                if (File.Exists(finalPath))
                    File.Delete(finalPath);

                await download.SaveAsAsync(finalPath);

                Assert.That(File.Exists(finalPath), $"❌ Arquivo '{fileName}' não foi salvo.");
                var info = new FileInfo(finalPath);
                Assert.That(info.Length, Is.GreaterThan(0), $"❌ Arquivo '{fileName}' está vazio (0 bytes).");

                Console.WriteLine($"✅ Download ok: '{fileName}' | {info.Length} bytes.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"❌ Erro ao validar download no passo '{step}': {ex.Message}");
            }
        }

        [AllureStep("Validar qualquer um dos textos presentes - no passo: {passo}")]
        public async Task ValidarQualquerTextoPresente(params string[] textosPossiveis)
        {
            try
            {
                foreach (var texto in textosPossiveis)
                {
                    try
                    {
                        await Expect(page.GetByText(texto)).ToBeVisibleAsync(new() { Timeout = 5000 });
                        Console.WriteLine($"✅ Texto encontrado: '{texto}'");
                        return;
                    }
                    catch
                    {
                        continue;
                    }
                }
                throw new Exception($"❌ Nenhum dos textos esperados foi encontrado: {string.Join(", ", textosPossiveis)}");
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Erro ao validar textos no passo: {ex.Message}");
            }
        }

        public async Task ValidarPossiveisTextosDeUmElemento(string locator, string[] textosPossiveis, string passo)
        {
            try
            {
                var textoCapturado = (await page.Locator(locator).InnerTextAsync()).Trim();

                foreach (var textoEsperado in textosPossiveis)
                {

                    if (textoCapturado.Equals(textoEsperado))
                    {
                        Console.WriteLine($"✅ Texto validado com sucesso: {textoCapturado}");
                        return;
                    }

                    else
                    {
                        continue;
                    }
                }

                Assert.Fail($"Nenhum dos textos esperados foi encontrado. Texto na tela: '{textoCapturado}'. Esperados: {string.Join(", ", textosPossiveis)}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro crítico no passo '{passo}': {ex.Message}");
            }
        }


        public static string GetPath()
        {
            var envPath = Environment.GetEnvironmentVariable("PORTAL_PATH");
            ConfigurationManager config = new ConfigurationManager();
            config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            string path = config["Paths:Arquivo"] ?? envPath;
            return path;
        }



    }


}

