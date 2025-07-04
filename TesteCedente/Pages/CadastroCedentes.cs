using Microsoft.Playwright;
using TesteCedente.Utils;

namespace TesteCedente.Pages.CedentesPage
{
    public class CadastroCedentes
    {
        public static async Task<Model.Pagina> CedentesPJ(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();

            try
            {
                var portalLink = AppSettings.Config["Links:Portal"];
                var BoletagemCedentes = await Page.GotoAsync(portalLink + "/Cedentes.aspx");

                if (BoletagemCedentes.Status == 200)
                {
                    string seletorTabela = "#tabelaCedentes";

                    Console.Write("Cedentes PJ: ");
                    pagina.Nome = "Cedentes PJ";
                    pagina.StatusCode = BoletagemCedentes.Status;
                    pagina.Reprovar = "❓";
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

                    var apagarCedente2 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "26038995000173");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_26038995000173_N.zip" });
                    var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                    {

                        Timeout = 90000

                    });

                    if (cedenteCadastrado != null)
                    {

                        await Page.Locator("#btnFecharNovoCedente").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("FUNDO QA");
                        await Task.Delay(600);
                        var primeiroTr = Page.Locator("#tabelaCedentes tbody tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Page.Locator("[id='36614123000160_26038995000173_GESTORA']").ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).FillAsync("Teste de Aprovaçao");
                        await Page.Locator("#modal-parecer").GetByText("Aprovado", new() { Exact = true }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Page.Locator("[id='36614123000160_26038995000173_CADASTRO_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Page.Locator("[id='36614123000160_26038995000173_COMPLIANCE_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        var statusFormalizacao = Repository.Cedentes.CedentesRepository.CedenteEmFormalizacao("36614123000160", "26038995000173");

                        if (statusFormalizacao)
                        {

                            //ativar e aprovar contrato mãe 
                            await Page.Locator(".buttonParecer.btn.btn-success").First.ClickAsync();
                            await Page.Locator("#fileAtivaCedente").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "Arquivo teste 2.pdf" });
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("Teste de ativaç");
                            await Page.Locator("#submitButtonAtivacao").ClickAsync();
                            await Page.GetByRole(AriaRole.Row, new() { Name = "FUNDO QA FIDC Teste robô -" }).GetByRole(AriaRole.Listitem).Nth(1).ClickAsync();
                            await Page.Locator("#botaoStatus label").Filter(new() { HasText = "Aprovado" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste  de aprovaç");
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                            var statusAtivo = Repository.Cedentes.CedentesRepository.CedenteAtivo("36614123000160", "26038995000173");

                            if (statusAtivo)
                            {

                                //verificar cadastro no zCustódia
                                var cedenteCadsZCust = Repository.Cedentes.CedentesRepository.CedenteCadastrodoZCust("26038995000173", "jt@zitec.ai");

                                if (cedenteCadsZCust)
                                {


                                    var kitBaixado = await BaixarKit(Page, "36614123000160", "26038995000173");

                                    if (kitBaixado == true) 
                                    {



                                    }
                                    else 
                                    {
                                    
                                    
                                    }
                                    //clicar no outro botão

                                    await Page.PauseAsync();



                                }
                                else 
                                { 
                                


                                
                                }



                            }
                            else 
                            
                            { 
                            
                            }


                        }
                        var cedenteExiste = Repository.Cedentes.CedentesRepository.VerificaExistenciaCedente("36614123000160", "26038995000173");

                        if (cedenteExiste)
                        {
                        var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "26038995000173");
                            Console.WriteLine("Cedente adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            if (apagarCedente)
                            {
                                Console.WriteLine("Cedente apagado com sucesso");
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
                            Console.WriteLine("Não foi possível inserir cedente");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }
                    }
                    else
                    {
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
                //await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

        public static async Task<Model.Pagina> CedentesPf(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();

            try
            {
                var portalLink = AppSettings.Config["Links:Portal"];
                var BoletagemCedentes = await Page.GotoAsync(portalLink + "/Cedentes.aspx");

                if (BoletagemCedentes.Status == 200)
                {
                    string seletorTabela = "#tabelaCedentes";

                    Console.Write("Cedentes PF: ");
                    pagina.Nome = "Cedentes PF";
                    pagina.StatusCode = BoletagemCedentes.Status;
                    pagina.Reprovar = "❓";
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

                    var apagarCedente2 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "49624866830");

                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    //await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "36614123000160_49624866830_N.zip" });
                    //var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions



                    //{

                    //    Timeout = 90000

                    //});

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();

                    // Obtém o caminho base do arquivo a partir do App.config
                    string basePath = AppSettings.Config["Paths:Arquivo"];
                    string fileName = "36614123000160_49624866830_N.zip";
                    string filePath = Path.Combine(basePath, fileName);
                    Console.WriteLine(filePath);

                    Console.WriteLine($"Arquivo gerado: {filePath}");


                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("ERRO: Arquivo não encontrado!");
                        throw new FileNotFoundException("Arquivo não encontrado para upload", filePath);
                    }

                    await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_49624866830_N.zip" });
                    var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                    {
                        Timeout = 90000
                    });
                    if (cedenteCadastrado != null)
                    {
                        var cedenteExiste = Repository.Cedentes.CedentesRepository.VerificaExistenciaCedente("36614123000160", "49624866830");

                        if (cedenteExiste)
                        {
                            var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "49624866830");
                            Console.WriteLine("Cedente adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            if (apagarCedente)
                            {
                                Console.WriteLine("Cedente apagado com sucesso");
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
                            Console.WriteLine("Não foi possível inserir cedente");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }
                    }
                    else
                    {
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }

                    //pf


                }


                else
                {
                    Console.Write("Erro ao carregar a página de Cedentes no tópico Boletagem ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = BoletagemCedentes.Status;
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
                //await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

        public static async Task<bool> BaixarKit(IPage Page, string fundoCnpj, string cedenteCnpj)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string prefixoNomeArquivo = $"{fundoCnpj}_{cedenteCnpj}_N_";

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator($"button[onclick*='{fundoCnpj}_{cedenteCnpj}']").First.ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 2000
                    });
                });

                string tempFileName = download.SuggestedFilename;
                string fullFilePath = Path.Combine(downloadPath, tempFileName);

                await download.SaveAsAsync(fullFilePath);

                if (File.Exists(fullFilePath) && Path.GetFileName(fullFilePath).StartsWith(prefixoNomeArquivo))
                {
                    Console.WriteLine("Kit cadastral baixado com sucesso.");
                    File.Delete(fullFilePath);
                    return true;
                }

                Console.WriteLine("Arquivo não encontrado ou nome incorreto.");
                return false;
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout ao tentar baixar o kit: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao baixar o kit cadastral: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> BaixarContratoMaeAsync(IPage page, string idBotao)
        {
            try
            {
                // Clica no botão principal que abre o modal
                await page.Locator($"[id=\"{idBotao}\"]").First.ClickAsync();

                // Download do Template Contrato-Mãe
                var download1 = await page.RunAndWaitForDownloadAsync(async () =>
                {
                    var popup = await page.RunAndWaitForPopupAsync(async () =>
                    {
                        await page.GetByText("Template Contrato-Mãe").ClickAsync();
                    });

                    await popup.CloseAsync();
                });

                // Salva o primeiro arquivo
                string path1 = Path.Combine(Path.GetTempPath(), download1.SuggestedFilename);
                await download1.SaveAsAsync(path1);
                bool existe1 = File.Exists(path1);
                if (existe1) File.Delete(path1);

                // Download do Contrato Atual
                var download2 = await page.RunAndWaitForDownloadAsync(async () =>
                {
                    var popup = await page.RunAndWaitForPopupAsync(async () =>
                    {
                        await page.GetByText("Download Contrato Atual").ClickAsync();
                    });

                    await popup.CloseAsync();
                });

                // Salva o segundo arquivo
                string path2 = Path.Combine(Path.GetTempPath(), download2.SuggestedFilename);
                await download2.SaveAsAsync(path2);
                bool existe2 = File.Exists(path2);
                if (existe2) File.Delete(path2);

                // Retorna true apenas se os dois arquivos existirem
                return existe1 && existe2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao baixar contratos: {ex.Message}");
                return false;
            }
        }
    }
}
