using Microsoft.Playwright;
using System.Runtime.InteropServices;
using TesteCedente.Model;
using TesteCedente.Utils;

namespace TesteCedente.Pages.CedentesPage
{
    public class CadastroCedentes
    {
        public static async Task<(Model.Pagina, Model.Cedente)> CedentesPJ(IPage Page)
        {
            var pagina = new Model.Pagina();
            var cedente = new Model.Cedente();
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

                    cedente.TipoCedente = "Cedente PJ";
                    var apagarCedente2 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "26038995000173");
                    var apagarCedenteZCustodia = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("26038995000173");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_26038995000173_N.zip" });
                    var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                    {

                        Timeout = 90000

                    });

                    if (cedenteCadastrado != null)
                    {
                        cedente.ArquivoEnviado = "✅";
                        await Page.Locator("#btnFecharNovoCedente").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("FUNDO QA");
                        await Task.Delay(200);
                        var primeiroTr = Page.Locator("#tabelaCedentes tbody tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Page.Locator("[id='36614123000160_26038995000173_GESTORA']").ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).FillAsync("Teste de Aprovaçao");
                        await Page.Locator("#modal-parecer").GetByText("Aprovado", new() { Exact = true }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_26038995000173_CADASTRO_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_26038995000173_COMPLIANCE_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(300);

                        var statusFormalizacao = Repository.Cedentes.CedentesRepository.CedenteEmFormalizacao("36614123000160", "26038995000173");

                        if (statusFormalizacao)
                        {
                            cedente.AprovacaoDasAreas = "✅";
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
                            await Task.Delay(300);

                            var statusAtivo = Repository.Cedentes.CedentesRepository.CedenteAtivo("36614123000160", "26038995000173");

                            if (statusAtivo)
                            {
                                cedente.AtivacaoCedente = "✅";
                                var cedenteCadsZCust = Repository.Cedentes.CedentesRepository.CedenteCadastrodoZCust("26038995000173", "jt@zitec.ai");

                                if (cedenteCadsZCust)
                                {
                                    cedente.InsertZCustodia = "✅";
                                    pagina.InserirDados = "✅";

                                    var kitBaixado = await BaixarKit(Page, "36614123000160", "26038995000173");

                                    if (kitBaixado == true)
                                    {
                                        cedente.BtnBaixarKit = "✅";
                                    }
                                    else
                                    {
                                        cedente.BtnBaixarKit = "❌";

                                    }
                                    //clicar no outro botão

                                    var btnContratoMae = await BaixarContratoMaeAsync(Page, "36614123000160_26038995000173");

                                    if (btnContratoMae)
                                    {
                                        cedente.BtnBaixarContratoMae = "✅";
                                    }
                                    else
                                    {
                                        cedente.BtnContratoMaeEFormalizacao = "❌";
                                    }

                                    var btnHistEvent = await VerificarHistoricoDeEventosAsync(Page, "36614123000160_26038995000173");
                                    if (btnHistEvent)
                                    {
                                        cedente.BtnHistoricoEventos = "✅";
                                    }
                                    else
                                    {
                                        cedente.BtnHistoricoEventos = "❌";
                                    }
                                    var baixarArquivoContratoMae = await BaixarArquivoContratoMae(Page, "36614123000160_26038995000173");
                                    if (baixarArquivoContratoMae)
                                    {
                                        cedente.BtnContratoMaeEFormalizacao = "✅";

                                    }
                                    else
                                    {
                                        cedente.BtnContratoMaeEFormalizacao = "❌";

                                    }

                                    //atualização de kit
                                    await Page.GetByRole(AriaRole.Row, new() { Name = "FUNDO QA FIDC Teste robô - CNPJ: 26.038.995/0001-73" }).Locator("[id=\"0\"]").ClickAsync();
                                    await Page.Locator("#fileAtualizarKit").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_26038995000173_A.zip" });
                                    var cedenteAtualizado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                                    {

                                        Timeout = 140000

                                    });

                                    await Page.Locator("#btnFecharAtualizarKit").ClickAsync();
                                    await Page.Locator("[id='36614123000160_26038995000173_CADASTRO_not_analysed']").Nth(0).ClickAsync();
                                    await Page.Locator("#modal-parecer").GetByText("Aprovado", new() { Exact = true }).ClickAsync();
                                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                                    await Task.Delay(2000);

                                    var cedenteAtualizadoBdd = Repository.Cedentes.CedentesRepository.VerificaAtualizacaoCedente("Teste de atualizacao", "testeatualizacao@gmail.com");
                                    if (cedenteAtualizadoBdd)
                                    {
                                        cedente.FluxoAtualizacaoDeKit = "✅";
                                        cedente.BtnAtualizarKit = "✅";
                                        var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "26038995000173");
                                        var apagarCedenteZCustodia2 = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("26038995000173");
                                        var emails = new List<string> { "jt@zitec.ai" };
                                        var apagarRepresentantes = Repository.Cedentes.CedentesRepository.ExcluirRepresentantesPorEmail(emails);

                                        if (apagarCedente && apagarCedenteZCustodia2)
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
                                        cedente.FluxoAtualizacaoDeKit = "❌";
                                    }
                                }
                                else
                                {
                                    cedente.InsertZCustodia = "❌";
                                    pagina.InserirDados = "❌";

                                }
                            }
                            else
                            {
                                cedente.AtivacaoCedente = "✅";
                            }
                        }
                        else
                        {
                            cedente.AprovacaoDasAreas = "❌";
                        }
                    }
                    else
                    {
                        cedente.ArquivoEnviado = "❌";
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }

                    //cadastro de cedente com representante que assina isoladamente 

                    var apagarCedente3 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "19890232000190");
                    var apagarCedenteZCustodia3 = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("19890232000190");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_19890232000190_N.zip" });
                    var cedenteCadastrado3 = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                    {

                        Timeout = 90000

                    });

                    if (cedenteCadastrado3 != null)
                    {
                        await Page.Locator("#btnFecharNovoCedente").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("FUNDO QA");
                        await Task.Delay(200);
                        var primeiroTr = Page.Locator("#tabelaCedentes tbody tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Page.Locator("[id='36614123000160_19890232000190_GESTORA']").ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).FillAsync("Teste de Aprovaçao");
                        await Page.Locator("#modal-parecer").GetByText("Aprovado", new() { Exact = true }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_19890232000190_CADASTRO_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_19890232000190_COMPLIANCE_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(300);

                        var statusFormalizacao = Repository.Cedentes.CedentesRepository.CedenteEmFormalizacao("36614123000160", "19890232000190");

                        if (statusFormalizacao)
                        {
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
                            await Task.Delay(300);

                            var statusAtivo = Repository.Cedentes.CedentesRepository.CedenteAtivo("36614123000160", "19890232000190");

                            if (statusAtivo)
                            {
                                var emails = new List<string> { "jt@zitec.ai", "bot@gmail.com", "bot2@gmail.com" };
                                var representantesCadastrados = Repository.Cedentes.CedentesRepository.RepresentantesComEmailsCadastrados(emails);
                                var repAssIso = Repository.Cedentes.CedentesRepository.RepresentanteAssinaIso("jt@zitec.ai");

                                if (representantesCadastrados & repAssIso)
                                {
                                    cedente.FluxoRepresentanteAssIso = "✅";
                                    Console.WriteLine("Todos os e-mails estão cadastrados.");
                                    await Page.Locator(".buttonParecer.btn.btn-danger").First.ClickAsync();
                                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste de reprovação");
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                                    await Task.Delay(500);
                                    var cedenteReprovado = Repository.Cedentes.CedentesRepository.CedenteReprovado("36614123000160", "19890232000190");

                                    if (cedenteReprovado)
                                    {
                                        cedente.ReprovarCedente = "✅";
                                        pagina.Reprovar = "✅";
                                        cedente.BtnReprovarCedente = "✅";
                                    }
                                    else
                                    {
                                        cedente.ReprovarCedente = "❌";
                                        pagina.Reprovar = "❌";
                                        cedente.BtnReprovarCedente = "❌";

                                    }
                                    var apagarRepresentantes = Repository.Cedentes.CedentesRepository.ExcluirRepresentantesPorEmail(emails);
                                    var apagarCedente4 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "19890232000190");
                                    var apagarCedenteZCustodia4 = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("19890232000190");

                                }
                                else
                                {
                                    Console.WriteLine("Nem todos os e-mails foram encontrados.");
                                    cedente.FluxoRepresentanteAssIso = "❌";
                                }
                            }
                            else
                            {
                                Console.WriteLine("Status não está como ativo no banco de dados");
                            }
                        }
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
                return (pagina, cedente);
                //await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, cedente);
        }

        public static async Task<(Model.Pagina, Model.Cedente)> CedentesPf(IPage Page)
        {
            var pagina = new Model.Pagina();
            var cedente = new Model.Cedente();
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


                    cedente.TipoCedente = "Cedente PF";
                    var apagarCedente2 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "71011834090");
                    var apagarCedenteZCustodia = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("71011834090");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#fileAtualizarKit").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_71011834090_N.zip" });
                    var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions

                    {

                        Timeout = 90000

                    });

                    if (cedenteCadastrado != null)
                    {
                        cedente.ArquivoEnviado = "✅";
                        await Page.Locator("#btnFecharNovoCedente").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("FUNDO QA");
                        await Task.Delay(200);
                        var primeiroTr = Page.Locator("#tabelaCedentes tbody tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Page.Locator("[id='36614123000160_71011834090_GESTORA']").ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).FillAsync("Teste de Aprovaçao");
                        await Page.Locator("#modal-parecer").GetByText("Aprovado", new() { Exact = true }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_71011834090_CADASTRO_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_71011834090_COMPLIANCE_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(300);

                        var statusFormalizacao = Repository.Cedentes.CedentesRepository.CedenteEmFormalizacao("36614123000160", "71011834090");

                        if (statusFormalizacao)
                        {
                            cedente.AprovacaoDasAreas = "✅";
                            await Page.Locator(".buttonParecer.btn.btn-success").First.ClickAsync();
                            await Page.Locator("#fileAtivaCedente").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "Arquivo teste 2.pdf" });
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("Teste de ativaç");
                            await Page.Locator("#submitButtonAtivacao").ClickAsync();
                            await Page.GetByRole(AriaRole.Row, new() { Name = "FUNDO QA FIDC Teste de" }).GetByRole(AriaRole.Listitem).Nth(1).ClickAsync();
                            await Page.Locator("#botaoStatus label").Filter(new() { HasText = "Aprovado" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste  de aprovaç");
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                            await Task.Delay(3000);

                            var statusAtivo = Repository.Cedentes.CedentesRepository.CedenteAtivo("36614123000160", "71011834090");

                            if (statusAtivo)
                            {
                                cedente.AtivacaoCedente = "✅";
                                var cedenteCadsZCust = Repository.Cedentes.CedentesRepository.CedenteCadastrodoZCust("71011834090", "robo@zitec.ai");

                                if (cedenteCadsZCust)
                                {
                                    cedente.InsertZCustodia = "✅";
                                    pagina.InserirDados = "✅";

                                    var kitBaixado = await BaixarKit(Page, "36614123000160", "71011834090");

                                    if (kitBaixado == true)
                                    {
                                        cedente.BtnBaixarKit = "✅";
                                    }
                                    else
                                    {
                                        cedente.BtnBaixarKit = "❌";

                                    }
                                    //clicar no outro botão

                                    var btnContratoMae = await BaixarContratoMaeAsync(Page, "36614123000160_71011834090");

                                    if (btnContratoMae)
                                    {
                                        cedente.BtnBaixarContratoMae = "✅";
                                    }
                                    else
                                    {
                                        cedente.BtnContratoMaeEFormalizacao = "❌";
                                    }

                                    var btnHistEvent = await VerificarHistoricoDeEventosAsync(Page, "36614123000160_71011834090");
                                    if (btnHistEvent)
                                    {
                                        cedente.BtnHistoricoEventos = "✅";
                                    }
                                    else
                                    {
                                        cedente.BtnHistoricoEventos = "❌";
                                    }
                                    //cedente n está liberado a opção no portal
                                    cedente.BtnContratoMaeEFormalizacao = "❓";
                                    //var baixarArquivoContratoMae = await BaixarArquivoContratoMae(Page, "36614123000160_71011834090");
                                    //if (baixarArquivoContratoMae)
                                    //{
                                    //    cedente.BtnContratoMaeEFormalizacao = "✅";

                                    //}
                                    //else
                                    //{
                                    //    cedente.BtnContratoMaeEFormalizacao = "❌";

                                    //}

                                    //atualização de kit

                                    await Page.GetByRole(AriaRole.Row, new() { Name = "FUNDO QA FIDC Teste de" }).Locator("[id=\"0\"]").ClickAsync();
                                    await Page.Locator("#fileAtualizarKit").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_71011834090_A.zip" });
                                    var cedenteAtualizado = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                                    {

                                        Timeout = 140000

                                    });

                                    await Page.Locator("#btnFecharAtualizarKit").ClickAsync();
                                    await Page.Locator("[id='36614123000160_71011834090_CADASTRO_not_analysed']").Nth(0).ClickAsync();
                                    await Page.Locator("#modal-parecer").GetByText("Aprovado", new() { Exact = true }).ClickAsync();
                                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                                    await Task.Delay(2000);

                                    var cedenteAtualizadoBdd = Repository.Cedentes.CedentesRepository.CedenteCadastrodoZCust("71011834090", "testeatualizacao@gmail.com");
                                    if (cedenteAtualizadoBdd)
                                    {
                                        cedente.FluxoAtualizacaoDeKit = "✅";
                                        cedente.BtnAtualizarKit = "✅";
                                        var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "71011834090");
                                        var apagarCedenteZCustodia2 = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("71011834090");
                                        var emails = new List<string> { "jt@zitec.ai" };
                                        var apagarRepresentantes = Repository.Cedentes.CedentesRepository.ExcluirRepresentantesPorEmail(emails);

                                        if (apagarCedente && apagarCedenteZCustodia2)
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
                                        cedente.FluxoAtualizacaoDeKit = "❌";
                                    }
                                }
                                else
                                {
                                    cedente.InsertZCustodia = "❌";
                                    pagina.InserirDados = "❌";

                                }
                            }
                            else
                            {
                                cedente.AtivacaoCedente = "✅";
                            }
                        }
                        else
                        {
                            cedente.AprovacaoDasAreas = "❌";
                        }
                    }
                    else
                    {
                        cedente.ArquivoEnviado = "❌";
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }
                    //cadastro de cedente com representante que assina isoladamente 

                    var apagarCedente3 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "19890232000190");
                    var apagarCedenteZCustodia3 = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("19890232000190");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { AppSettings.Config["Paths:Arquivo"] + "36614123000160_19890232000190_N.zip" });
                    var cedenteCadastrado3 = await Page.WaitForSelectorAsync("text=Ação Executada com Sucesso", new PageWaitForSelectorOptions
                    {

                        Timeout = 90000

                    });

                    if (cedenteCadastrado3 != null)
                    {
                        await Page.Locator("#btnFecharNovoCedente").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("FUNDO QA");
                        await Task.Delay(200);
                        var primeiroTr = Page.Locator("#tabelaCedentes tbody tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Page.Locator("[id='36614123000160_19890232000190_GESTORA']").ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).FillAsync("Teste de Aprovaçao");
                        await Page.Locator("#modal-parecer").GetByText("Aprovado", new() { Exact = true }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_19890232000190_CADASTRO_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("[id='36614123000160_19890232000190_COMPLIANCE_not_analysed']").Nth(0).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                        await Task.Delay(300);

                        var statusFormalizacao = Repository.Cedentes.CedentesRepository.CedenteEmFormalizacao("36614123000160", "19890232000190");

                        if (statusFormalizacao)
                        {
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
                            await Task.Delay(300);

                            var statusAtivo = Repository.Cedentes.CedentesRepository.CedenteAtivo("36614123000160", "19890232000190");

                            if (statusAtivo)
                            {
                                var emails = new List<string> { "jt@zitec.ai", "bot@gmail.com", "bot2@gmail.com" };
                                var cedendeCadastrado = Repository.Cedentes.CedentesRepository.RepresentantesComEmailsCadastrados(emails);
                                var repAssIso = Repository.Cedentes.CedentesRepository.RepresentanteAssinaIso("jt@zitec.ai");

                                if (cedendeCadastrado & repAssIso)
                                {
                                    cedente.FluxoRepresentanteAssIso = "✅";
                                    Console.WriteLine("Todos os e-mails estão cadastrados.");

                                    //fluxo de reprovação clicando no botão e vendo se o status trocou no banco de dados
                                    await Page.Locator(".buttonParecer.btn.btn-danger").First.ClickAsync();
                                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                                    await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste de reprovação");
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();
                                    await Task.Delay(500);

                                    var cedenteReprovado = Repository.Cedentes.CedentesRepository.CedenteReprovado("36614123000160", "19890232000190");

                                    if (cedenteReprovado)
                                    {
                                        cedente.ReprovarCedente = "✅";
                                        pagina.Reprovar = "✅";
                                        cedente.BtnReprovarCedente = "✅";
                                    }
                                    else
                                    {
                                        cedente.ReprovarCedente = "❌";
                                        pagina.Reprovar = "❌";
                                        cedente.BtnReprovarCedente = "❌";
                                    }
                                    var apagarRepresentantes = Repository.Cedentes.CedentesRepository.ExcluirRepresentantesPorEmail(emails);
                                    var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "71011834090");
                                    var apagarCedenteZCustodia2 = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("71011834090");
                                    var apagarCedente4 = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "19890232000190");
                                    var apagarCedenteZCustodia4 = Repository.Cedentes.CedentesRepository.ExcluirCedenteZCustodia("19890232000190");
                                }
                                else
                                {
                                    Console.WriteLine("Nem todos os e-mails foram encontrados.");
                                    cedente.FluxoRepresentanteAssIso = "❌";
                                }
                            }
                            else
                            {
                                Console.WriteLine("Status não está como ativo no banco de dados");
                            }
                        }
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
            catch (Exception ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                return (pagina, cedente);
                //await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, cedente);
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
                await page.Locator($"[id=\"{idBotao}\"]").First.ClickAsync();

                var download1 = await page.RunAndWaitForDownloadAsync(async () =>
                {
                    var popup = await page.RunAndWaitForPopupAsync(async () =>
                    {
                        await page.GetByText("Template Contrato-Mãe").ClickAsync();
                    });

                    await popup.CloseAsync();
                });

                string path1 = Path.Combine(Path.GetTempPath(), download1.SuggestedFilename);
                await download1.SaveAsAsync(path1);
                bool existe1 = File.Exists(path1);
                if (existe1) File.Delete(path1);
                var popup2 = await page.RunAndWaitForPopupAsync(async () =>
                {
                    await page.GetByText("Download Contrato Atual").ClickAsync();
                });

                await popup2.WaitForLoadStateAsync(LoadState.Load);

                var url = popup2.Url;
                bool urlValida = url.Contains("ContratoFormalizacao.ashx") && url.Contains("Fundo=") && url.Contains("Cedente=");
                await popup2.CloseAsync();
                await page.Locator("#btnFecharModalDownloadContraoMae").ClickAsync();
                return existe1 && urlValida;

            }
            catch (Exception ex)
            {
                await page.Locator("#btnFecharModalDownloadContraoMae").ClickAsync();
                Console.WriteLine($"Erro ao baixar/verificar contratos: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> VerificarHistoricoDeEventosAsync(IPage page, string idBotao)
        {
            try
            {
                var botaoHistorico = page.Locator($"button[id='{idBotao}'][title='Histórico de Eventos']");
                await botaoHistorico.ClickAsync();

                var modal = page.Locator("#modal-xl");
                await modal.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 5000 });

                Console.WriteLine("✅ Modal de Histórico abriu corretamente.");

                var elementosEmail = modal.Locator(":text('qazitec01@gmail.com')");
                int count = await elementosEmail.CountAsync();

                for (int i = 0; i < count; i++)
                {
                    var item = elementosEmail.Nth(i);
                    if (await item.IsVisibleAsync())
                    {
                        Console.WriteLine("✅ Pelo menos um 'qazitec01@gmail.com' visível no modal.");
                        await page.ReloadAsync();
                        return true;
                    }
                }

                Console.WriteLine("❌ Nenhum 'qazitec01@gmail.com' visível no modal.");
                return false;
            }
            catch (TimeoutException)
            {
                Console.WriteLine("❌ Modal de Histórico **não abriu** no tempo esperado.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao tentar abrir/verificar modal de Histórico: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> BaixarArquivoContratoMae(IPage page, string idBotao)
        {
            try
            {

                await page.Locator($"button[id='{idBotao}'][title='Contrato Mãe']").ClickAsync();

                var linkDownload = page.Locator("a#linkContratoMae");
                await linkDownload.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 5000 });

                var download = await page.RunAndWaitForDownloadAsync(async () =>
                {
                    await linkDownload.ClickAsync();
                });

                string caminho = Path.Combine(Path.GetTempPath(), download.SuggestedFilename);
                await download.SaveAsAsync(caminho);

                bool arquivoExiste = File.Exists(caminho);
                if (arquivoExiste) File.Delete(caminho);

                await page.Locator("#btnFecharParecerContratoMae").ClickAsync();

                return arquivoExiste;
            }
            catch (Exception ex)
            {

                try
                {
                    await page.Locator("#btnFecharParecerContratoMae").ClickAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }

                Console.WriteLine($"❌ Erro ao baixar contrato mãe: {ex.Message}");
                return false;
            }
        }
    }
}
