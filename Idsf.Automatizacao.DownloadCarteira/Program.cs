using Microsoft.Playwright;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static Idsf.Automatizacao.DownloadCarteira.Program;
using System.Xml.Linq;

namespace Idsf.Automatizacao.DownloadCarteira
{
    class Program
    {
        public double validGlobal;
        public double difValidGlobal;

        public static async Task Main()
        {
            //implementação de Task.WhenAll para executar várias tarefas ao mesmo tempo e automatizar o código

            while (true)
            {
                Thread.Sleep(6000);

                var carteiraUN = CarteirasDownload.GetCarteira();

                if (!string.IsNullOrEmpty(carteiraUN.IdCarteira))
                {

                    // [LOG]: Sucesso em obter Carteira
                    RegistroDeLogs.InserirLogBritechCarteira("GetCarteira", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 1);

                    var playwright = await Playwright.CreateAsync();
                    var browser = await playwright.Chromium.LaunchAsync(
                        new BrowserTypeLaunchOptions
                        {
                            Channel = "chrome",
                            Headless = false,
                            SlowMo = 50,
                            Timeout = 0,
                            Args = new List<string>() { "--start-maximized" }
                        });
                    var page = await browser.NewPageAsync();

                    await LoginBritech(page);

                    RegistroDeLogs.InserirLogBritechCarteira("LoginBritech", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 2);

                    if (carteiraUN.CnpjFundo != null)
                    {
                        if (page.Url == "https://id.britech.com.br/PAS/Login/LoginInit.aspx?ReturnUrl=%2fPAS%2fDefault.aspx" ||
                            page.Url == "https://id.britech.com.br/PAS/Login/LoginInit.aspx")
                        {
                            await LoginBritech(page);
                        }

                        carteiraUN.CnpjFundo = carteiraUN.CnpjFundo.Replace(".", "").Replace("-", "").Replace("/", "");

                        if (carteiraUN.ID_RELATORIO != 0)
                        {
                            ReportBatch.UpdateDsStatus("PROCESSANDO", carteiraUN.ID_RELATORIO);
                        }

                        var carteiraFundo = CarteiraFundo.GetCarteiraByIdCarteira(carteiraUN.IdCarteira);

                        string DropBox = ConfigurationManager.AppSettings["PATH.DROPBOX"];
                        if (string.IsNullOrWhiteSpace(DropBox))
                            DropBox = "C:\\Users\\caioo\\ID CTVM Dropbox\\";

                        await GoToComposicaoCarteira(page);
                        await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);

                        await page.WaitForTimeoutAsync(5000);

                        if (await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync() == "26631505"
                            || carteiraUN.IdCarteira == "20998989")
                        {
                            await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);
                        }

                        // [LOG]: Preencheu os campos da Carteira
                        RegistroDeLogs.InserirLogBritechCarteira("PreencherCamposCarteira", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 3);

                        // Baixar Excel e PDF em paralelo
                        var downloadExcelTask = DownloadRelatorio(page, "EXCEL");
                        var downloadPdfTask = DownloadRelatorio(page, "PDF");

                        await Task.WhenAll(downloadExcelTask, downloadPdfTask);

                        var downloadExcel = downloadExcelTask.Result;
                        var downloadPdf = downloadPdfTask.Result;

                        var tasksToRun = new List<Task>();

                        if (downloadExcel != null)
                        {
                            tasksToRun.Add(Task.Run(async () =>
                            {
                                try
                                {
                                   
                                    var carteira = GetCarteiraByIdCarteiraBritech(carteiraUN.IdCarteira);
                                    string fileName = $"{carteiraUN.IdCarteira}_{carteiraUN.Data:yyyy-MM-dd}.xlsx";
                                    //string pathDropbox = @"\\CONTROLADORIA\\PORTAL IDSF\\RELATORIOS\\CARTEIRA\\";
                                    string pathDropbox = ConfigurationManager.AppSettings["PATH.DROPBOX"];
                                    string fullPath = Path.Combine(pathDropbox, fileName);

                                    if (!Directory.Exists(pathDropbox))
                                    {
                                        Console.WriteLine($"[ERRO]: O caminho de rede não foi encontrado: {pathDropbox}");
                                        return; 
                                    }

                                    Directory.CreateDirectory(pathDropbox); 

                                    if (File.Exists(fullPath)) File.Delete(fullPath);

                                    await downloadExcel.SaveAsAsync(fullPath);

                                    Console.WriteLine($"Arquivo salvo em: {fullPath}");

                                   // [LOG]: Salvou Excel
                                   RegistroDeLogs.InserirLogBritechCarteira("SalvarExcel", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 4);
                                }
                             
                                catch (Exception ex)
                                {
                                    // Captura qualquer outro erro inesperado
                                    Console.WriteLine($"[ERRO]: {ex.Message}");
                                }
                            }));
                        }


                        if (downloadPdf != null)
                        {
                            tasksToRun.Add(Task.Run(async () =>
                            {
                                var carteira = GetCarteiraByIdCarteiraBritech(carteiraUN.IdCarteira);
                                string fileName = $"{carteiraUN.IdCarteira}_{carteiraUN.Data:yyyy-MM-dd}.pdf";
                                //string pathDropbox = @"\\CONTROLADORIA\\PORTAL IDSF\\RELATORIOS\\CARTEIRA\\";
                                string pathDropbox = ConfigurationManager.AppSettings["PATH.DROPBOX"];
                                string fullPath = Path.Combine(DropBox, pathDropbox, fileName);

                                try
                                {
                                    // Verifica se o caminho de rede existe
                                    if (!Directory.Exists(pathDropbox))
                                    {
                                        Console.WriteLine($"[ERRO]: O caminho de rede não foi encontrado: {pathDropbox}");
                                        return; // Impede o código de continuar se o caminho não estiver acessível
                                    }

                                    // Tenta criar o diretório
                                    Directory.CreateDirectory(Path.Combine(DropBox, pathDropbox));

                                    if (File.Exists(fullPath)) File.Delete(fullPath);

                                    await downloadPdf.SaveAsAsync(fullPath);

                                    if (File.Exists(fullPath))
                                    {
                                        var fundo = Repository.GetFundo(carteiraUN.CnpjFundo);
                                        string nomeCarteira = carteira.Count > 0 ? carteira[0].Apelido : fundo.NomeFundo;

                                        var carteiraAtual = BritechRelatorioPosicaoFechamento(Int32.Parse(carteiraUN.IdCarteira), carteiraUN.Data);
                                        var valids = GetDiferencaValidCarteira(Int32.Parse(carteiraUN.IdCarteira), carteiraUN.Data);

                                        RegistroDeLogs.InserirLogBritechCarteira("GoToComposicaoCarteira", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 5);


                                        if (carteiraUN.ID_RELATORIO == 0)
                                        {
                                            PostMakeSendMsg(fundo.SlackChannelIDOperacionais, carteiraUN.Data.ToString("yyyy-MM-dd"), fundo.NomeFundo, carteiraUN.IdCarteira, nomeCarteira, valids);
                                            RegistroDeLogs.InserirLogBritechCarteira("PostMakeSendMsg", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 6);

                                        }


                                        Repository.UpdateHoraEnvioRelatorioSlack(carteiraUN.ID);
                                        Repository.UpdateDataCarteira(carteiraUN.IdCarteira);
                                        CarteirasDownload carteiraDownload = new CarteirasDownload();
                                        CarteirasDownload.UpdateStatus(carteiraUN.IdCarteira, carteiraUN.Data, Status.Sucesso);

                                        if (carteiraFundo.EnviarCarteira)
                                        {
                                            List<string> emails = string.IsNullOrWhiteSpace(carteiraFundo.EmailsCarteira) ? new List<string>() : carteiraFundo.EmailsCarteira.Split(';').ToList();
                                            foreach (var email in emails.Where(email => !string.IsNullOrWhiteSpace(email)))
                                            {
                                                Util.SendMailWithAttachment(email + ",controladoria@idsf.com.br", "Composição Carteira",
                                                    ConfigurationSettings.AppSettings["EMAIL.BODY"] + " a composição da carteira do Fundo.<br> " + ConfigurationSettings.AppSettings["EMAIL.BODY.ATT"],
                                                    fullPath);
                                            }
                                        }

                                        if (carteiraUN.ID_RELATORIO != 0)
                                        {
                                            ReportBatch.UpdateDsStatus("SUCESSO", carteiraUN.ID_RELATORIO);
                                            ReportBatch.UpdatePath(Path.Combine(DropBox, pathDropbox), fileName, carteiraUN.ID_RELATORIO);
                                        }
                                    }
                                    else
                                    {
                                        CarteirasDownload.UpdateStatus(carteiraUN.IdCarteira, carteiraUN.Data, Status.NaoIniciado);
                                    }
                                    // [LOG]: Salvou PDF
                                    RegistroDeLogs.InserirLogBritechCarteira("SalvarPDF", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 7);
                                }
                               
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"[ERRO]: {ex.Message}");
                                }
                            }));
                        }


                        try
                        {
                            await Task.WhenAll(tasksToRun);
                        }
                        catch (Exception ex)
                        {
                            
                            Console.WriteLine($"[ERRO NA EXECUÇÃO DE TAREFAS]: {ex.Message}");
                        }

                        if (carteiraFundo.EnviarHistoricoCota && carteiraUN.ID_RELATORIO == 0)
                        {
                            await GoToHistoricoCota(page);
                            await PreencherCamposHistoricoCota(page, carteiraUN.IdCarteira, carteiraUN.Data, carteiraUN.Data);

                            try
                            {
                                var downloadHistorico = await page.RunAndWaitForDownloadAsync(async () =>
                                {
                                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("text=Gerar PDF").ClickAsync();
                                });

                                if (string.IsNullOrWhiteSpace(DropBox))
                                    DropBox = ConfigurationManager.AppSettings["PATH.DROPBOX.D"];

                                string filename = downloadHistorico.SuggestedFilename.Replace(".pdf", "");
                                string arquivo = Path.Combine(DropBox, "CONTROLADORIA", "PORTAL IDSF", "RELATORIOS", "HISTORICO", filename + carteiraFundo.Nome + carteiraUN.Data.ToString("yyyy-MM-dd") + ".pdf");
                                await downloadHistorico.SaveAsAsync(arquivo);

                                Util.SendMailWithAttachment(carteiraFundo.EmailsHistoricoCota + ",controladoria@idsf.com.br", "Histórico de Cotas",
                                    ConfigurationSettings.AppSettings["EMAIL.BODY"] + " o Histórico de Cotas da carteira do Fundo.<br>" + ConfigurationSettings.AppSettings["EMAIL.BODY.ATT"],
                                    arquivo);

                                // [LOG]: Histórico de cotas baixado
                                RegistroDeLogs.InserirLogBritechCarteira("DownloadHistoricoCota", int.Parse(carteiraUN.IdCarteira), carteiraUN.Data, 8);

                            }
                            catch (Exception)
                            {
                                // handle exception
                            }
                        }
                    }
                    await page.CloseAsync();
                }
            }

        } 



        public static List<double> GetDiferencaValidCarteira(int idCarteira, DateTime dateTime)
        {
            var carteiraAtual = BritechRelatorioPosicaoFechamento(idCarteira, dateTime);
            if (carteiraAtual == null)
                return new List<double>();

            var diaAnterior = Util.GetUltimoDiaAnterior(dateTime);
            var ultimaCarteira = BritechRelatorioPosicaoFechamento(idCarteira, diaAnterior);

            double validAtual = carteiraAtual.Posicoes
                .Where(p => p.Ativo.Contains("VALID"))
                .Sum(p => p.QtdeTotal);

            double validAnterior = 0;
            if (ultimaCarteira != null)
            {
                validAnterior = ultimaCarteira.Posicoes
                    .Where(p => p.Ativo.Contains("VALID"))
                    .Sum(p => p.QtdeTotal);
            }

            double diferenca = validAtual - validAnterior;

            return new List<double> { validAtual, diferenca };
        }


        static string FormatarValorBrasileiro(double valor)
        {
            return valor.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
        }


        public static RelatorioPosicaoFechamento BritechRelatorioPosicaoFechamento(int idCarteira, DateTime dataCarteira)
        {
            string url = $"https://id.britech.com.br/ws/api/Common/RelatorioPosicaoFechamento?IdsCarteira={idCarteira}&dataInicial={dataCarteira:yyyy-MM-dd}&dataFinal={dataCarteira:yyyy-MM-dd}&desconsideraGrossup=true";

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "GET";

                var authString = $"{ConfigurationManager.AppSettings["BRITECH.USERNAME"]}:{ConfigurationManager.AppSettings["BRITECH.PASSWORD"]}";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString)));

                var response = request.GetResponseAsync().Result;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEndAsync().Result;
                    var carteiras = JsonConvert.DeserializeObject<List<RelatorioPosicaoFechamento>>(result);
                    return carteiras?.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }



        public static void PostMakeSendMsg(string canalId, string data, string nomeFundo, string idCarteira, string nomeCarteira, List<double> valid)
        {
            string url = $"https://hook.us1.make.com/monaya9nwroe9lmywdiplr70t6rh973d?ChannelId={canalId}&NomeFundo={nomeFundo}&Data={data}&IdCarteira={idCarteira}&NomeCarteira={nomeCarteira}&ValorValid={valid[0]}&diferencaValid={valid[1]}";

            try
            {

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    var response = client.SendAsync(request).Result;
                    var _ = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception)
            {
                // Log ou tratamento de erro
            }
        }


        public static async Task LoginBritech(IPage page)
        {
            try
            {
                if (page != null)
                {
                    await page.GotoAsync("https://id.britech.com.br/PAS/Login/LoginInit.aspx");
                    await page.GetByLabel("Login:").ClickAsync();
                    await page.GetByLabel("Login:").PressAsync("CapsLock");
                    await page.GetByLabel("Login:").FillAsync("M");
                    await page.GetByLabel("Login:").PressAsync("CapsLock");
                    await page.GetByLabel("Login:").FillAsync("Murillo ");
                    await page.GetByLabel("Login:").PressAsync("CapsLock");
                    await page.GetByLabel("Login:").FillAsync("Murillo P");
                    await page.GetByLabel("Login:").PressAsync("CapsLock");
                    await page.GetByLabel("Login:").FillAsync("Murillo Pereira");
                    await page.GetByLabel("Login:").PressAsync("Tab");
                    await page.GetByLabel("Senha:").FillAsync("idsf2022");
                    await page.GetByLabel("Senha:").PressAsync("Enter");
                }
                else
                {
                    await LoginBritech((IPage)await Playwright.CreateAsync());
                }
            }
            catch (Exception)
            {
                await LoginBritech((IPage)await Playwright.CreateAsync());
            }

        }

        public static async Task GoToComposicaoCarteira(IPage page)
        {
            try
            {
                await page.WaitForTimeoutAsync(5000);
                await page.GotoAsync("https://id.britech.com.br/PAS/Default.aspx");
                //var reportsButton = await page.QuerySelectorAsync("button:contains('Relatórios')"); 
                await page.HoverAsync(selector: "text='Relatórios'");
                await page.HoverAsync(selector: "#ASPxMenu1_DXI13i0_T");
                await page.ClickAsync(selector: "text='Composição Carteira'");
                // Localizar o elemento com o ID "#ASPxMenu1_DXME13_ div" que contém o texto "Carteira" e clicar nele 
                //var carteiraElement = await page.QuerySelectorAsync("#ASPxMenu1_DXME13_ div:has-text('Carteira')"); 
                //await carteiraElement.ClickAsync(); 

                // Encontrar o link com o texto "Composição Carteira" e clicar nele 
                //var composicaoCarteiraLink = await page.QuerySelectorAsync("a:has-text('Composição Carteira')"); 
                //await composicaoCarteiraLink.ClickAsync(); 



            }
            catch (Exception)
            {
                await GoToComposicaoCarteira(page);
            }
        }




        public static async Task GoToHistoricoCota(IPage page)
        {
            try
            {
                await page.WaitForTimeoutAsync(2000);
                await page.GotoAsync("https://id.britech.com.br/PAS/Default.aspx");
                //var reportsButton = await page.QuerySelectorAsync("button:contains('Relatórios')"); 
                await page.HoverAsync(selector: "text='Relatórios'");

                await page.HoverAsync(selector: "#ASPxMenu1_DXI13i0_T");

                await page.ClickAsync(selector: "text='Histórico de Cota'");

            }
            catch (Exception)
            {
                await GoToHistoricoCota(page);
            }
        }


        public static async Task PreencherCampos(IPage page, string IdCarteira, DateTime Data)
        {
            try
            {
                await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").FillAsync(IdCarteira);
                var chk_id = await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync();
                while (chk_id != IdCarteira)
                {
                    await page.WaitForTimeoutAsync(500);
                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").ClickAsync();
                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").PressAsync("Control+A+Delete");
                    char[] carteiraChar = IdCarteira.ToCharArray();

                    for (int ii = 0; ii < carteiraChar.Length; ii++)
                    {
                        await page.WaitForTimeoutAsync(200);
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").PressAsync(carteiraChar[ii].ToString());
                    }
                    chk_id = await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync();
                }

                bool dataImportada = false;

                while (!dataImportada)
                {
                    try
                    {
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textData_I").ClickAsync();
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textData_I").PressAsync("Home");

                        char[] charArr = Data.ToString("ddMMyyyy").ToCharArray();
                        for (int ii = 0; ii < charArr.Length; ii++)
                        {
                            await page.WaitForTimeoutAsync(200);
                            await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textData_I").PressAsync(charArr[ii].ToString());
                        }

                        dataImportada = await verificaDataImportada(page, Data, "#textData_I");

                        if (!dataImportada)
                        {
                            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss") + " Erro na data do Relatório, tentando novamente...");
                            await page.WaitForTimeoutAsync(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao tentar preencher a data: {ex.Message}. Tentando novamente...");
                        await page.WaitForTimeoutAsync(1000);
                    }
                }


                List<string> cotaAbertura = new List<string> { "50269", "50191107", "45019184", "50191107", "44680449" };
                if (cotaAbertura.Contains(IdCarteira))
                {

                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").ClickAsync();

                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").ClickAsync();

                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").FillAsync("");

                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").PressAsync("CapsLock");

                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").FillAsync("A");

                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").PressAsync("CapsLock");

                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").FillAsync("Abertura");




                    //await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").ClickAsync();
                    //await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#dropTipoRelatorio_I").FillAsync();

                }


                // await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textData_I").FillAsync(Data.ToString("dd/MM/yyyy")); 
            }
            catch (TimeoutException)
            {
                await GoToComposicaoCarteira(page);
                await PreencherCampos(page, IdCarteira, Data);
            }
        }



        private static async Task<bool> verificaDataImportada(IPage page, DateTime expectedDate, string campoData)
        {
            var dateValue = await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator(campoData).InputValueAsync();
            DateTime parsedDate;

            if (DateTime.TryParse(dateValue, out parsedDate))
            {
                return parsedDate.ToString("ddMMyyyy") == expectedDate.ToString("ddMMyyyy");
            }

            return false;
        }

        public static async Task PreencherCamposHistoricoCota(IPage page, string IdCarteira, DateTime DataIn, DateTime DataFim)
        {
            try
            {
                await page.WaitForTimeoutAsync(200);
                await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").FillAsync(IdCarteira);
                //Carteira
                var inserted_data = await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync();
                while (inserted_data != IdCarteira)
                {
                    await page.WaitForTimeoutAsync(500);
                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").ClickAsync();
                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").PressAsync("Control+A+Delete");

                    char[] charCad = IdCarteira.ToCharArray();
                    for (int ii = 0; ii < charCad.Length; ii++)
                    {
                        await page.WaitForTimeoutAsync(200);
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").PressAsync(charCad[ii].ToString());
                    }
                    inserted_data = await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync();
                }

                bool dataImportada = false;

                while (!dataImportada)
                {
                    try
                    {
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textDataInicio_I").ClickAsync();
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textDataInicio_I").PressAsync("Home");

                        char[] charArr = DataIn.ToString("ddMMyyyy").ToCharArray();
                        for (int ii = 0; ii < charArr.Length; ii++)
                        {
                            await page.WaitForTimeoutAsync(200);
                            await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textDataInicio_I").PressAsync(charArr[ii].ToString());
                        }

                        dataImportada = await verificaDataImportada(page, DataIn, "#textDataInicio_I");

                        if (!dataImportada)
                        {
                            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss") + " Erro na data de início do Histórico de Cotas, tentando novamente...");
                            await page.WaitForTimeoutAsync(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao tentar preencher a data: {ex.Message}. Tentando novamente...");
                        await page.WaitForTimeoutAsync(1000);
                    }
                }

                dataImportada = false;

                while (!dataImportada)
                {
                    try
                    {
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textDataFim_I").ClickAsync();
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textDataFim_I").PressAsync("Home");

                        char[] charArr = DataFim.ToString("ddMMyyyy").ToCharArray();
                        for (int ii = 0; ii < charArr.Length; ii++)
                        {
                            await page.WaitForTimeoutAsync(200);
                            await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#textDataFim_I").PressAsync(charArr[ii].ToString());
                        }

                        dataImportada = await verificaDataImportada(page, DataFim, "#textDataFim_I");

                        if (!dataImportada)
                        {
                            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss") + " Erro na data final do Histórico de Cotas, tentando novamente...");
                            await page.WaitForTimeoutAsync(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao tentar preencher a data: {ex.Message}. Tentando novamente...");
                        await page.WaitForTimeoutAsync(1000);
                    }
                }
            }
            catch (TimeoutException)
            {
                await GoToHistoricoCota(page);
                await PreencherCamposHistoricoCota(page, IdCarteira, DataIn, DataFim);
            }
        }


        public static async Task<IDownload> DownloadRelatorio(IPage page, string TipoRelatorio)
        {
            try
            {
                if (TipoRelatorio == "EXCEL")
                {
                    return await page.RunAndWaitForDownloadAsync(async () =>
                    {
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("text=Gerar Excel").First.ClickAsync();
                    });
                }
                else
                {
                    return await page.RunAndWaitForDownloadAsync(async () =>
                    {
                        await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("text=Gerar PDF").ClickAsync();
                    });
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Carteira> GetCarteiraByIdCarteiraBritech(string idCarteira)
        {
            List<Carteira> ListaCarteiras = new List<Carteira>();
            try
            {
                string resultStr = "";
                string ApiIDSF = ConfigurationManager.AppSettings["BRITECH.API"].ToString();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiIDSF + "/Fundo/BuscaCarteira?idCarteira=" + idCarteira);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                var authenticationString = $"" + ConfigurationManager.AppSettings["BRITECH.USERNAME"] + ":" + ConfigurationManager.AppSettings["BRITECH.PASSWORD"];
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                httpWebRequest.Headers.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                var response = httpWebRequest.GetResponseAsync().Result;

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEndAsync();
                    resultStr = result.Result;
                }
                ListaCarteiras = JsonConvert.DeserializeObject<List<Carteira>>(resultStr);

            }
            catch (Exception e)
            {
                //Service.LogErrorService.AddFromExceptionSemContexto(e, Model.LogError.Action.PORTAL_CEDENTE_ERROR_GET_INFO_CNPJ);
                return null;
            }
            return ListaCarteiras;
        }

        public class Carteira
        {
            public int IdCarteira { get; set; }
            public string Nome { get; set; }
            public string Apelido { get; set; }
            public int TipoCota { get; set; }
            public int TipoCarteira { get; set; }
            public int StatusAtivo { get; set; }
            public int IdIndiceBenchmark { get; set; }
            public int CasasDecimaisCota { get; set; }
            public int CasasDecimaisQuantidade { get; set; }
            public string TruncaCota { get; set; }
            public string TruncaQuantidade { get; set; }
            public string TruncaFinanceiro { get; set; }
            public double CotaInicial { get; set; }
            public double ValorMinimoAplicacao { get; set; }
            public double ValorMinimoResgate { get; set; }
            public double ValorMinimoSaldo { get; set; }
            public double ValorMaximoAplicacao { get; set; }
            public double ValorMinimoInicial { get; set; }
            public int TipoRentabilidade { get; set; }
            public int TipoCusto { get; set; }
            public int DiasCotizacaoAplicacao { get; set; }
            public int DiasCotizacaoResgate { get; set; }
            public int DiasLiquidacaoAplicacao { get; set; }
            public int DiasLiquidacaoResgate { get; set; }
            public int TipoTributacao { get; set; }
            public string CalculaIOF { get; set; }
            public int DiasAniversario { get; set; }
            public object HorarioInicio { get; set; }
            public object HorarioFim { get; set; }
            public object HorarioLimiteCotizacao { get; set; }
            public int IdCategoria { get; set; }
            public int IdSubCategoria { get; set; }
            public int IdAgenteAdministrador { get; set; }
            public int IdAgenteGestor { get; set; }
            public string CobraTaxaFiscalizacaoCVM { get; set; }
            public object TipoCVM { get; set; }
            public DateTime DataInicioCota { get; set; }
            public string CalculaEnquadra { get; set; }
            public int ContagemDiasConversaoResgate { get; set; }
            public int ProjecaoIRResgate { get; set; }
            public object PublicoAlvo { get; set; }
            public object Objetivo { get; set; }
            public int IdAgenteCustodiante { get; set; }
            public object TextoLivre1 { get; set; }
            public object TextoLivre2 { get; set; }
            public object TextoLivre3 { get; set; }
            public object TextoLivre4 { get; set; }
            public int TipoVisaoFundo { get; set; }
            public int IdEstrategia { get; set; }
            public object CodigoAnbid { get; set; }
            public int ContagemPrazoIOF { get; set; }
            public int PrioridadeOperacao { get; set; }
            public object CodigoCDA { get; set; }
            public int CompensacaoPrejuizo { get; set; }
            public object DiasConfidencialidade { get; set; }
            public object CodigoFDIC { get; set; }
            public int TipoCalculoRetorno { get; set; }
            public object IdGrauRisco { get; set; }
            public string CodigoIsin { get; set; }
            public string CodigoCetip { get; set; }
            public string CodigoBloomberg { get; set; }
            public object HorarioInicioResgate { get; set; }
            public object HorarioFimResgate { get; set; }
            public object IdGrupoEconomico { get; set; }
            public string FundoExclusivo { get; set; }
            public object OrdemRelatorio { get; set; }
            public string LiberaAplicacao { get; set; }
            public string LiberaResgate { get; set; }
            public string ProcAutomatico { get; set; }
            public object PerfilRisco { get; set; }
            public string ExplodeCotasDeFundos { get; set; }
            public string ComeCotasEntreRegatesConversao { get; set; }
            public object CategoriaAnbima { get; set; }
            public object Contratante { get; set; }
            public string Corretora { get; set; }
            public string CodigoSTI { get; set; }
            public int Rendimento { get; set; }
            public string ExportaGalgo { get; set; }
            public string InfluenciaGestorLocalCvm { get; set; }
            public string InvestimentoColetivoCvm { get; set; }
            public string ApenasInvestidorProfissional { get; set; }
            public string ApenasInvestidorQualificado { get; set; }
            public string RealizaOfertaSubscricao { get; set; }
            public int TipoFundo { get; set; }
            public object IdTipoTemplate { get; set; }
            public object DiasUteisContagemAposConversaoIOF { get; set; }
            public object DataInicioContIOF { get; set; }
            public object DataFimContIOF { get; set; }
            public string CalculaPrazoMedio { get; set; }
            public int DiasAposResgateIR { get; set; }
            public int ProjecaoIRComeCotas { get; set; }
            public int DiasAposComeCotasIR { get; set; }
            public string RealizaCompensacaoDePrejuizo { get; set; }
            public string BuscaCotaAnterior { get; set; }
            public int NumeroDiasBuscaCota { get; set; }
            public double BandaVariacao { get; set; }
            public string ExcecaoRegraTxAdm { get; set; }
            public string RebateImpactaPL { get; set; }
            public string FIE { get; set; }
            public int ContaPrzIOFVirtual { get; set; }
            public int ProjecaoIOFResgate { get; set; }
            public int DiasAposResgateIOF { get; set; }
            public object CodigoConsolidacaoExterno { get; set; }
            public object CodigoBDS { get; set; }
            public string TipoVisualizacaoResgCotista { get; set; }
            public object IdGrupoPerfilMTM { get; set; }
            public int CalculaMTM { get; set; }
            public string PossuiResgateAutomatico { get; set; }
            public bool DistribuicaoProventos { get; set; }
            public DateTime DataConstituicaoFundo { get; set; }
            public int TipoAnbima { get; set; }
            public string FdoClasseSerie { get; set; }
            public object AliquotaEspecificaIR { get; set; }
            public object PoliticaInvestimentos { get; set; }
            public object IdCarteiraExterna { get; set; }
        }
    }
}
