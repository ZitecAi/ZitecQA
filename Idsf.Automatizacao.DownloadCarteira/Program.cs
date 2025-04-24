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
using System.Threading;
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
            while (true)
            {

                Thread.Sleep(6000);


                var carteiraUN = CarteirasDownload.GetCarteira();

                if (!string.IsNullOrEmpty(carteiraUN.IdCarteira))
                {
                    var playwright = await Playwright.CreateAsync();
                    var browser = await playwright.Chromium.LaunchAsync(
                        new BrowserTypeLaunchOptions { Channel = "chrome", Headless = false, SlowMo = 50, Timeout = 0, Args = new List<string>() { "--start-maximized" } });
                    var page = await browser.NewPageAsync();

                    await LoginBritech(page);

                    if (carteiraUN.IdCarteira == "376511022")
                    {
                        var debug = true;
                    }
                    if (carteiraUN.CnpjFundo != null)
                    {
                        if (page.Url == "https://id.britech.com.br/PAS/Login/LoginInit.aspx?ReturnUrl=%2fPAS%2fDefault.aspx" && page.Url == "https://id.britech.com.br/PAS/Login/LoginInit.aspx")
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
                        //pathDropBox == null ? "C:\\Users\\IDMurilloPereira\\ID CTVM Dropbox\\" : pathDropBox;

                        if (string.IsNullOrWhiteSpace(DropBox))
                        {
                            DropBox = "C:\\Users\\caioo\\ID CTVM Dropbox\\";
                        }

                        await GoToComposicaoCarteira(page);
                        await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);
                        await page.WaitForTimeoutAsync(5000);

                        if (await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync() == "26631505")
                        {
                            await page.WaitForTimeoutAsync(5000);
                            await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);
                        }

                        if (await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync() == "26631505")
                        {
                            await page.WaitForTimeoutAsync(5000);
                            await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);
                        }

                        if (carteiraUN.IdCarteira == "20998989")
                        {
                            await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);
                        }

                        var downloadExcel = await DownloadRelatorio(page, "EXCEL");

                        if (downloadExcel != null)
                        {
                            var carteira = GetCarteiraByIdCarteiraBritech(carteiraUN.IdCarteira);
                            string fileName = carteiraUN.IdCarteira + "_" + carteiraUN.Data.ToString("yyyy-MM-dd") + ".xlsx";
                            string pathDropbox = @"\CONTROLADORIA\PORTAL IDSF\RELATORIOS\CARTEIRA\\";
                            string PathToFile = pathDropbox + fileName;// + download.SuggestedFilename;
                            string FullPath = DropBox + PathToFile;

                            if (!Directory.Exists(Path.Combine(DropBox, pathDropbox)))
                                Directory.CreateDirectory(Path.Combine(DropBox, pathDropbox));

                            if (File.Exists(FullPath))
                            {
                                File.Delete(FullPath);
                            }

                            await downloadExcel.SaveAsAsync(FullPath);
                        }

                        await GoToComposicaoCarteira(page);
                        await page.WaitForTimeoutAsync(2000);
                        await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);

                        if (await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync() == "26631505")
                        {
                            await GoToComposicaoCarteira(page);
                            await page.WaitForTimeoutAsync(2000);
                            await PreencherCampos(page, carteiraUN.IdCarteira, carteiraUN.Data);
                        }

                        var download = await DownloadRelatorio(page, "PDF");

                        if (download != null)
                        {
                            var carteira = GetCarteiraByIdCarteiraBritech(carteiraUN.IdCarteira);
                            string fileName = carteiraUN.IdCarteira + "_" + carteiraUN.Data.ToString("yyyy-MM-dd") + ".pdf";
                            string pathDropbox = @"\CONTROLADORIA\PORTAL IDSF\RELATORIOS\CARTEIRA\\";
                            string PathToFile = pathDropbox + fileName;// + download.SuggestedFilename;
                            string FullPath = DropBox + PathToFile;

                            if (!Directory.Exists(Path.Combine(DropBox, pathDropbox)))
                                Directory.CreateDirectory(Path.Combine(DropBox, pathDropbox));

                            if (File.Exists(FullPath))
                            {
                                File.Delete(FullPath);
                            }

                            await download.SaveAsAsync(FullPath);

                            if (File.Exists(FullPath))
                            {
                                //await browser.CloseAsync();

                                var fundo = Repository.GetFundo(carteiraUN.CnpjFundo);

                                string NomeCarteira = fundo.NomeFundo;


                                if (carteira.Count > 0)
                                {
                                    NomeCarteira = carteira[0].Apelido;
                                }

                                if (string.IsNullOrEmpty(fundo.SlackChannelIDOperacionais))
                                {

                                }
                                string ValidAtualString = "0";

                                var carteiraatual = BritechRelatorioPosicaoFechamento(Int32.Parse(carteiraUN.IdCarteira), carteiraUN.Data);

                                if (carteiraatual != null)
                                {
                                    var VAlidCarteira = carteiraatual.Posicoes
                                         .Where(posicao => posicao.Ativo.Contains("VALID"))
                                         .ToList();

                                    for (int x = 0; x < VAlidCarteira.Count; x++)
                                    {
                                        ValidAtualString = VAlidCarteira[x].QtdeTotal.ToString();
                                    }
                                }

                                List<double> valids = GetDiferencaValidCarteira(Int32.Parse(carteiraUN.IdCarteira), carteiraUN.Data);

                                if (carteiraUN.ID_RELATORIO == 0 && File.Exists(FullPath))
                                {
                                    PostMakeSendMsg(fundo.SlackChannelIDOperacionais, carteiraUN.Data.ToString("yyyy-MM-dd"), fundo.NomeFundo, carteiraUN.IdCarteira, NomeCarteira, valids);
                                }

                                Repository.UpdateHoraEnvioRelatorioSlack(carteiraUN.ID);
                                Repository.UpdateDataCarteira(carteiraUN.IdCarteira);
                                var response = new
                                {
                                    Path = PathToFile,
                                    IdCanal = fundo.SlackChannelIDOperacionais
                                };
                                //return response.ToString();
                                CarteirasDownload.UpdateStatus(carteiraUN.IdCarteira, carteiraUN.Data, CarteirasDownload.status.SUCESSO);

                                if (carteiraFundo.EnviarCarteira)
                                {
                                    List<string> listasCarteiras = string.IsNullOrWhiteSpace(carteiraFundo.EmailsCarteira) ? new List<string>() : ((carteiraFundo.EmailsCarteira).Split(';').ToList());
                                    if (listasCarteiras.Count > 0)
                                    {
                                        foreach (var email in listasCarteiras)
                                        {
                                            if (string.IsNullOrWhiteSpace(email))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                Util.SendMailWithAttachment(email + ",controladoria@idsf.com.br", "Composição Carteira",
                                                ConfigurationSettings.AppSettings["EMAIL.BODY"] + " a composição da carteira do Fundo.<br> " + ConfigurationSettings.AppSettings["EMAIL.BODY.ATT"],
                                                FullPath);
                                                //Util.MsgAutomacaoRobo(listaCotas[i].Carteira, data);
                                            }

                                        }
                                    }
                                }

                                if (carteiraUN.ID_RELATORIO != 0)
                                {
                                    ReportBatch.UpdateDsStatus("SUCESSO", carteiraUN.ID_RELATORIO);
                                    ReportBatch.UpdatePath(DropBox + pathDropbox, fileName, carteiraUN.ID_RELATORIO);
                                }
                            }
                            else
                            {
                                CarteirasDownload.UpdateStatus(carteiraUN.IdCarteira, carteiraUN.Data, CarteirasDownload.status.NAO_INICIADO);
                            }
                        }
                        else
                        {
                            CarteirasDownload.UpdateStatus(carteiraUN.IdCarteira, carteiraUN.Data, CarteirasDownload.status.NAO_INICIADO);
                        }

                        if (carteiraUN.IdCarteira == "53645")
                        {
                            var enviar = true;
                        }

                        if (carteiraUN.IdCarteira == "430961")
                        {
                            /* e-mails oficiais:*/
                            string emails = "railda.santos@terrainvestimentos.com.br," +
                                "cotas@terrainvestimentos.com.br," +
                                "middle.fundos@terrainvestimentos.com.br," +
                                "custodiadefundos@terrainvestimentos.com.br," +
                                "carlos.silva@terrainvestimentos.com.br";
                            
                            /* e-mails oficiais:*/
                            string cc = "fundo-luna-fip1-aaaaewjb4gswicv5evtyggwvfq@idgr.slack.com," 
                                //"nileide.abreu @idfip.com.br," +
                                //"guilherme.guimaraes @idfip.com.br," +
                                //"douglas.bomfim @idfip.com.br"
                                +"regulatorio@idfip.com.br";

                            await GoToHistoricoCota(page);
                            string primeiro_dia_mes = "01" + "/" + carteiraUN.Data.ToString("MM") + "/" + carteiraUN.Data.ToString("yyyy");

                            await PreencherCamposHistoricoCota(page, carteiraUN.IdCarteira, DateTime.Parse(primeiro_dia_mes), carteiraUN.Data);

                            var download1 = await page.RunAndWaitForDownloadAsync(async () =>
                            {
                                await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("text=Gerar PDF").ClickAsync();
                            });

                            if (string.IsNullOrWhiteSpace(DropBox))
                            {
                                DropBox = ConfigurationManager.AppSettings["PATH.DROPBOX"];
                            }

                            string filename = download1.SuggestedFilename.Replace(".pdf", "");
                            string arquivo = DropBox + "\\CONTROLADORIA\\PORTAL IDSF\\RELATORIOS\\HISTORICO\\" + filename + carteiraFundo.Nome + carteiraUN.Data.ToString("yyyy-MM-dd") + ".pdf";
                            await download1.SaveAsAsync(arquivo);

                            Util.SendMailWithAttachmentAndCC(emails,
                                cc,
                                "Histórico de Cotas",
                                 ConfigurationSettings.AppSettings["EMAIL.BODY"] + " o Histórico de Cotas da carteira do Fundo.<br> " + ConfigurationSettings.AppSettings["EMAIL.BODY.ATT"],
                                arquivo);
                            //Util.MsgAutomacaoRobo(listaCotas[i].Carteira, data);
                        }

                        if (carteiraFundo.EnviarHistoricoCota && carteiraUN.ID_RELATORIO == 0)
                        {
                            await GoToHistoricoCota(page);
                            await PreencherCamposHistoricoCota(page, carteiraUN.IdCarteira, carteiraUN.Data, carteiraUN.Data);

                            if (await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync() == "26631505")
                            {
                                await page.WaitForTimeoutAsync(2000);
                                await PreencherCamposHistoricoCota(page, carteiraUN.IdCarteira, carteiraUN.Data, carteiraUN.Data);
                            }

                            if (await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("#btnEditCodigo_I").InputValueAsync() != carteiraUN.IdCarteira)
                            {
                                await page.WaitForTimeoutAsync(2000);
                                await PreencherCamposHistoricoCota(page, carteiraUN.IdCarteira, carteiraUN.Data, carteiraUN.Data);
                            }

                            try
                            {
                                var download1 = await page.RunAndWaitForDownloadAsync(async () =>
                                {
                                    await page.FrameLocator("iframe[name=\"iframePrincipal\"]").Locator("text=Gerar PDF").ClickAsync();
                                });

                                if (string.IsNullOrWhiteSpace(DropBox))
                                {
                                    DropBox = ConfigurationManager.AppSettings["PATH.DROPBOX.D"];
                                }

                                string filename = download1.SuggestedFilename.Replace(".pdf", "");
                                string arquivo = DropBox + "\\CONTROLADORIA\\PORTAL IDSF\\RELATORIOS\\HISTORICO\\" + filename + carteiraFundo.Nome + carteiraUN.Data.ToString("yyyy-MM-dd") + ".pdf";
                                await download1.SaveAsAsync(arquivo);

                                Util.SendMailWithAttachment(carteiraFundo.EmailsHistoricoCota + ",controladoria@idsf.com.br", "Histórico de Cotas",
                                                                            ConfigurationSettings.AppSettings["EMAIL.BODY"] + " o Histórico de Cotas da carteira do Fundo.<br>" + ConfigurationSettings.AppSettings["EMAIL.BODY.ATT"],
                                                                            arquivo);

                                //string[] emails = carteiraFundo.EmailsHistoricoCota.Split(',').ToArray();

                                //for (int email = 0; email < emails.Length; email++)
                                //{
                                //}

                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                    await page.CloseAsync();
                }
            }
        }

        public static List<double> GetDiferencaValidCarteira(int idCarteira, DateTime dateTime)
        {
            var carteiraatual = BritechRelatorioPosicaoFechamento(idCarteira, dateTime);
            if (carteiraatual != null)
            {
                DateTime diaAnterior = Util.GetUltimoDiaAnterior(dateTime);
                var ultimaCarteira = BritechRelatorioPosicaoFechamento(idCarteira, diaAnterior);

                var VAlidCarteira = carteiraatual.Posicoes
                    .Where(posicao => posicao.Ativo.Contains("VALID"))
                    .ToList();

                double ValidAnterior = 0;

                if (ultimaCarteira != null)
                {
                    var validAnterior = ultimaCarteira.Posicoes
                        .Where(posicao => posicao.Ativo.Contains("VALID"))
                        .ToList();

                    for (int i = 0; i < validAnterior.Count; i++)
                    {
                        ValidAnterior = validAnterior[i].QtdeTotal;
                    }
                }

                double ValidAtual = 0;
                for (int i = 0; i < VAlidCarteira.Count; i++)
                {
                    ValidAtual = VAlidCarteira[i].QtdeTotal;
                }

                double atualValid = ValidAtual - ValidAnterior;

                return new List<double> { ValidAtual, atualValid };
            }
            else
            {
                return new List<double>();
            }
        }

        static string FormatarValorBrasileiro(double valor)
        {
            // Formata o valor como moeda brasileira
            string valorFormatado = valor.ToString("C2");

            // Substitui o separador de milhar e o separador decimal para o padrão brasileiro
            valorFormatado = valorFormatado.Replace(".", "###").Replace(",", ".").Replace("###", ",");

            // Adiciona o símbolo da moeda
            valorFormatado = "" + valorFormatado;

            return valorFormatado;
        }

        public static RelatorioPosicaoFechamento BritechRelatorioPosicaoFechamento(int idCarteira, DateTime dataCarteira)
        {

            string resultStr;
            string url = "https://id.britech.com.br/ws/api/Common/RelatorioPosicaoFechamento?IdsCarteira=" + idCarteira + "&dataInicial=" + dataCarteira.ToString("yyyy-MM-dd") + "&dataFinal=" + dataCarteira.ToString("yyyy-MM-dd") + "&desconsideraGrossup=true";
            //https://localhost:44330
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
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
                    //RelatorioPosicaoFechamento carteiraResponse = JsonConvert.DeserializeObject<RelatorioPosicaoFechamento>(resultStr);
                    var carteiraResponse = JsonConvert.DeserializeObject<List<RelatorioPosicaoFechamento>>(resultStr);

                    return carteiraResponse[0];

                }
                catch (Exception e)
                {
                    //Utils.Slack.MandarMsgErroGrupoDev(e.Message, "Integracao.Britech.ProcessarCarteira", "Idsf.CadastroCedentes, " + url, e.StackTrace);
                    return null;
                }
            }

        }


        public static void PostMakeSendMsg(string CanalId, string data, string nomeFundo, string idCarteira, string nomeCarteira, List<double> valid)
        {
            string url = "https://hook.us1.make.com/monaya9nwroe9lmywdiplr70t6rh973d?ChannelId=" + CanalId + "&NomeFundo=" + nomeFundo + 
                "&Data=" + data + "&IdCarteira=" + idCarteira + "&NomeCarteira=" + nomeCarteira + "&ValorValid=" + valid[0] + "&diferencaValid=" + valid[1];
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseContent = response.Content.ReadAsStringAsync().Result;

                }
                catch (Exception e)
                {
                    //Utils.Slack.MandarMsgErroGrupoDev(e.Message, "Integracao.Carteira.ComposicaoCarteira", "Idsf.CadastroCedentes", e.StackTrace);
                }
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
