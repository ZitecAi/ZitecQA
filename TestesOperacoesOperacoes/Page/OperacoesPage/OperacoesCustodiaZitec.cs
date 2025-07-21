using Azure;
using Microsoft.Playwright;
using TesteOperacoesOperacoes.Model;
using TesteOperacoesOperacoes.Repositories;
//using System.Windows.Controls;
using TesteOperacoesOperacoes.Util;
using static TesteOperacoesOperacoes.Model.Usuario;
using static Microsoft.Playwright.Assertions;


namespace TesteOperacoesOperacoes.Pages.OperacoesPage
{
    public class OperacoesCustodiaZitec
    {
        public static async Task<(Pagina pagina, Operacoes operacoes)> OperacoesZitecInterno(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var portalLink = TestesOperacoesOperacoes.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "/Operacoes/Operacoes2.0.aspx ");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec - Interno";
                    pagina.StatusCode = OperacoesZitec.Status;

                    pagina.Acentos = await Acentos.ValidarAcentos(Page) ?? "❌";
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;
                    pagina.BaixarExcel = await Util.Excel.BaixarExcel(Page) ?? "❌";
                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    var processamentoFundo = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificaProcessamentoFundo(9991);

                    if (processamentoFundo)
                    {
                        operacoes.TipoOperacao2 = "Nova Operação - Interno";
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CNAB" }).ClickAsync();
                        await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });

                        // Valida se AtualizarDataEEnviarArquivo NÃO retornou null

                        operacoes.NovoNomeArquivo2 = await AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
                        if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                        {
                            throw new NullReferenceException("AtualizarDataEEnviarArquivo retornou null ou vazio.");
                        }
                        await Task.Delay(300);
                        var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                        await Task.Delay(20000);


                        if (CadastroOperacoes != null)
                        {
                            var (existe, idArquivo) = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo2);
                            var idOperacaoRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo2);

                            //await Page.ReloadAsync();

                            if (existe)
                            {
                                Console.WriteLine("Operação lançada.");
                                pagina.InserirDados = "✅";
                                operacoes.ArquivoEnviado = "✅";
                                operacoes.AprovacoesRealizadas2 = "❓";
                                operacoes.StatusTrocados2 = "❓";
                            }
                            else
                            {
                                operacoes.ArquivoEnviado = "❌";
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                                operacoes.ListaErros2.Add("Erro ao lançar operação");
                            }



                            await Page.ReloadAsync();
                            //await Page.PauseAsync();




                            //Alterar status antes de reprovar para habilitar checkbox
                            try
                            {
                                await Page.GetByLabel("Pesquisar").ClickAsync();
                                await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);
                                var primeiroItem = Page.Locator("#listaCedentes").First;
                                //var primeiroTd2 = primeiroTr2.Locator("[class='OperacaoSelecionado']").First;
                                var primeiroItemTd2 = primeiroItem.Locator("(//tbody[@id='listaCedentes']//tr//td[@class='dtr-control'])[1]");
                                await primeiroItemTd2.ClickAsync();
                                await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                                await Page.Locator("#statusPosOp").SelectOptionAsync(new[] { "PG" });
                                await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                                await Task.Delay(25000);
                                await Page.ReloadAsync();
                                RegistroTestesPositivos.Registrar("CTPC-01 Deve alterar status para Aguardando aprovação do Gestor", true);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Não foi possivel alterar status para: Aguardando aprovação do Gestor {ex.Message}");
                                RegistroTestesPositivos.Registrar("CTPC-01 Deve alterar status para Aguardando aprovação do Gestor", false);

                            }




                            #region Deve Validar Redirecionamento para download relatorio cessão e validar atraves da URL
                            try
                            {
                                await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                                await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(operacoes.NovoNomeArquivo2);
                                await Page.Locator("(//input[@type='checkbox'])[1]").ClickAsync();
                                var page1 = await Page.RunAndWaitForPopupAsync(async () =>
                                {
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                                });
                                await TesteOperacoesOperacoes.Util.Excel.ValidarAberturaDeNovaAbaAsync(page1);
                                Console.WriteLine("Download Relatorio cessão baixado e validado com sucesso!");
                                await page1.CloseAsync();
                                RegistroTestesPositivos.Registrar("CTPC-02 Deve Validar Redirecionamento para download relatorio cessão e validar atraves da URL", true);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Não foi possivel baixar relatorio cessão  motivo: {ex.Message}");
                                RegistroTestesPositivos.Registrar("CTPC-02 Deve Validar Redirecionamento para download relatorio cessão e validar atraves da URL", false);
                            }

                            #endregion
                            #region Deve validar Download de arquivo remessa
                            try
                            {
                                var page2 = await Page.RunAndWaitForPopupAsync(async () =>
                                {
                                    var download = await Page.RunAndWaitForDownloadAsync(async () =>
                                    {
                                        await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                                    });

                                    await TesteOperacoesOperacoes.Util.Excel.ValidarDownloadAsync(download, "Download Arquivo remessa");
                                    Console.WriteLine("Download arquivo remessa baixado e validado com sucesso!");
                                    RegistroTestesPositivos.Registrar("CTPC-03 Deve validar Download de arquivo remessa", true);
                                });
                                await page2.CloseAsync();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Não foi possivel baixar arquivo Remessa motivo: {ex.Message}");
                                RegistroTestesPositivos.Registrar("CTPC-03 Deve validar Download de arquivo remessa", false);

                            }

                            #endregion

                            #region Deve validar histórico de eventos
                            //await Page.PauseAsync();
                            await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                            await Page.GetByText("Histórico de Eventos × Timeline Arquivo InseridoUpload de Operaçãoqazitec01@").ClickAsync();
                            await Page.GetByRole(AriaRole.Heading, new() { Name = "Histórico de Eventos" }).ClickAsync();
                            try
                            {
                                await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Histórico de Eventos" })).ToBeVisibleAsync();
                                RegistroTestesPositivos.Registrar("CTPC-04 Deve validar histórico de eventos", true);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Não foi possivel Encontrar histórico de eventos motivo: {ex.Message}");
                                RegistroTestesPositivos.Registrar("CTPC-04 Deve validar histórico de eventos", false);
                            }
                            #endregion

                            //#region Deve consultar arquivo CNAB enviado pelo histórico
                            //await Page.ReloadAsync(); 
                            //await Page.GetByRole(AriaRole.Button, new() { Name = "Histórico" }).ClickAsync();
                            //await Task.Delay(5000);
                            //await Page.Locator("#tabelaHistorico_filter").GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                            //await Page.Locator("#tabelaHistorico_filter").GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(operacoes.NovoNomeArquivo2);
                            ////await Page.Locator("#tabelaHistorico_wrapper div").Filter(new() { HasText = "Nome ArquivoStatusData" }).Nth(1).ClickAsync();
                            //try
                            //{
                            //    await Task.Delay(10000);
                            //    await Expect(Page.Locator("#listaHistorico", new()
                            //    {
                            //        HasTextString = operacoes.NovoNomeArquivo2
                            //    })).ToBeVisibleAsync();
                            //}
                            //catch (Exception ex)
                            //{
                            //    Console.WriteLine($"Não foi possivel consultar arquivo CNAB pelo histórico, Motivo: {ex}");
                            //}                          
                            //#endregion

                            //#region Deve Validar Download Validação Movimento
                            //await Page.Locator("//tbody[@id='listaHistorico']//td[@class='dtr-control']").ClickAsync();
                            //try
                            //{
                            //    var downloadValMovimento = await Page.RunAndWaitForDownloadAsync(async () =>
                            //    {
                            //        await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                            //    });
                            //    await TesteOperacoesOperacoes.Util.Excel.ValidarDownloadAsync(downloadValMovimento, "Download Validação Movimento");
                            //}
                            //catch (Exception ex)
                            //{
                            //    Console.WriteLine($"Não foi possivel fazer Download da validação movimento: Motivo {ex.Message}");
                            //}
                            //#endregion

                            //#region Deve Validar Download Validação Layout
                            //try
                            //{
                            //    var downloadValLayout = await Page.RunAndWaitForDownloadAsync(async () =>
                            //    {
                            //        await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                            //    });
                            //    await TesteOperacoesOperacoes.Util.Excel.ValidarDownloadAsync(downloadValLayout, "Download Validação Layout");
                            //}
                            //catch (Exception ex)
                            //{
                            //    Console.WriteLine($"Não foi possivel fazer Download da validação Layout: Motivo {ex.Message}");
                            //}
                            //#endregion

                            #region Reprovar Lote

                            await Page.ReloadAsync();
                            try
                            {
                                await Page.GetByLabel("Pesquisar").ClickAsync();
                                await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);
                                var primeiroTr2 = Page.Locator("#listaCedentes").First;
                                var primeiroTd2 = primeiroTr2.Locator("[class='OperacaoSelecionado']").First;
                                //var primeiroTd2 = primeiroTr2.Locator("(//tbody[@id='listaCedentes']//tr//td[@class='dtr-control'])[1]");
                                await primeiroTd2.ClickAsync();
                                await Page.GetByRole(AriaRole.Button, new() { Name = "Reprovar Lote" }).ClickAsync();
                                await Page.Locator("#MotivoLote").ClickAsync();
                                await Page.Locator("#MotivoLote").FillAsync("teste reprovação");
                                await Page.GetByRole(AriaRole.Button, new() { Name = "Reprovar em Lote" }).ClickAsync();
                                await Page.GetByText("Operação reprovada com").ClickAsync();
                                var reprovarOperacao = await Page.GetByText("Operação reprovada com sucesso!").ElementHandleAsync();
                                RegistroTestesPositivos.Registrar("CTPC-05 Deve Reprovar Lote", true);
                                if (reprovarOperacao != null)
                                {
                                    pagina.Reprovar = "✅";
                                }

                            }
                            catch
                            {
                                RegistroTestesPositivos.Registrar("CTPC-05 Deve Reprovar Lote", false);
                            }

                            #endregion

                            #region Excluir operação
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);
                            await Task.Delay(600);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            var cnpj = "54638076000176";

                            //var button = Page.Locator($"button[onclick=\"ModalExcluirArquivo('{idArquivo}','{idOperacaoRecebivel}','{operacoes.NovoNomeArquivo2}','{cnpj}')\"]").First.ClickAsync();
                            //await Page.PauseAsync();
                            //await Page.EvaluateAsync($"""ModalExcluirArquivo('{idArquivo}','{idOperacaoRecebivel}','{operacoes.NovoNomeArquivo2}','{cnpj}');""");
                            //await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            //await Task.Delay(200);
                            //await Page.Locator("#motivoExcluirArquivo").FillAsync("teste de exclus");
                            //await Task.Delay(200);
                            //await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                            await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            await Page.Locator("#motivoExcluirArquivo").FillAsync("Teste Exclusão");
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            Console.WriteLine("Botão de apagar operação encontrado.");
                            var mensagens = new[]
                            {
                                "Solicitação recebida com sucesso!",
                                "Arquivo excluído com sucesso!"
                            };

                            bool mensagemEncontrada = false;

                            foreach (var mensagem in mensagens)
                            {
                                var locator = Page.GetByText(mensagem);
                                var elemento = await locator.ElementHandleAsync(); // espera no máximo 1s

                                if (elemento != null)
                                {
                                    mensagemEncontrada = true;
                                    break; // encontrou um, ignora o resto
                                }
                            }

                            if (mensagemEncontrada)
                            {
                                operacoes.OpApagadaBtn = "✅";
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                operacoes.OpApagadaBtn = "❌";

                                bool exclusaoRemessa = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                                bool exclusaoTed = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo2);
                                var idRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo2);
                                var excluirAvalista = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);
                                var excluirCertificadora = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacaoCertificadora(idRecebivel);

                                if (exclusaoRemessa && exclusaoTed && excluirAvalista)
                                {
                                    bool excluirOperacao = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                    if (excluirOperacao)
                                    {
                                        Console.WriteLine("Operação excluída com sucesso pelo banco.");
                                        pagina.Excluir = "✅";

                                    }
                                    else
                                    {
                                        Console.WriteLine("Operação não excluída.");
                                        pagina.Excluir = "❌";
                                        errosTotais2++;
                                        operacoes.ListaErros2.Add("Operação não excluída");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Não foi possível excluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED");
                                }
                            }
                            #endregion
                        }
                    }


                    else
                    {
                        Console.WriteLine("O fundo não está na data atual");
                        errosTotais2++;
                        operacoes.ListaErros2.Add("Não foi possível processar o fundo para a data de hoje");
                    }

                    //caminho arquvos inseridos, pendente criar relatório para cnab positivo e negativo, e ajustar o que tiver com problemas, ou reordernar o fluxo

                    #region Testes Negativos



                    #region CTNC-01 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado1 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-01_CnpjOriginadorEmBranco.txt", "CTN-01 CNAB CnpjOriginadorEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-01 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-01 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorEmBranco", false);
                    }

                    #endregion

                    #region CTNC-02 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorInvalido13Char
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado2 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-02_CnpjOriginadorInvalido13Char.txt", "CTN-02 CNAB CnpjOriginadorInvalido13Char");
                        RegistroTestesNegativos.Registrar("CTNC-02 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorInvalido13Char", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-02 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorInvalido13Char", false);
                    }
                    #endregion

                    #region CTNC-03 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorInvalido15Char
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado3 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-03_CnpjOriginadorInvalido15Char.txt", "CTN-03 CNAB CnpjOriginadorInvalido15Char");
                        RegistroTestesNegativos.Registrar("CTNC-03 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorInvalido15Char", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-03 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjOriginadorInvalido15Char", false);
                    }
                    #endregion

                    #region CTNC-04 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado4 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-04_NomeCedenteEmBranco.txt", "CTN-04 CNAB NomeCedenteEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-04 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-04 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteEmBranco", false);
                    }
                    #endregion

                    #region CTNC-05 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteInexistente
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado5 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-05_NomeCedenteInexistente.txt", "CTN-05 CNAB NomeCedenteInexistente");
                        RegistroTestesNegativos.Registrar("CTNC-05 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteInexistente", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-05 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteInexistente", false);
                    }
                    #endregion

                    #region CTNC-06 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteInvalido
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado6 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-06_NomeCedenteInvalido.txt", "CTN-06 CNAB NomeCedenteInvalido");
                        RegistroTestesNegativos.Registrar("CTNC-06 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteInvalido", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-06 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeCedenteInvalido", false);
                    }
                    #endregion

                    #region CTNC-07 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado7 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-07_CnpjCedenteEmBranco.txt", "CTN-07 CNAB CnpjCedenteEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-07 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-07 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteEmBranco", false);
                    }
                    #endregion

                    #region CTNC-08 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteInvalido13Char
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado8 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-08_CnpjCedenteInvalido13Char.txt", "CTN-08 CNAB CnpjCedenteInvalido13Char");
                        RegistroTestesNegativos.Registrar("CTNC-08 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteInvalido13Char", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-08 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteInvalido13Char", false);
                    }
                    #endregion

                    #region CTNC-09 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteInvalido15Char
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado9 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-09_CnpjCedenteInvalido15Char.txt", "CTN-09 CNAB CnpjCedenteInvalido15Char");
                        RegistroTestesNegativos.Registrar("CTNC-09 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteInvalido15Char", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-09 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjCedenteInvalido15Char", false);
                    }
                    #endregion

                    #region CTNC-10 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado10 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-10_NomeSacadoEmBranco.txt", "CTN-10 CNAB NomeSacadoEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-10 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-10 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoEmBranco", false);
                    }
                    #endregion

                    #region CTNC-11 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoInexistente
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado11 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-11_NomeSacadoInexistente.txt", "CTN-11 CNAB NomeSacadoInexistente");
                        RegistroTestesNegativos.Registrar("CTNC-11 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoInexistente", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-11 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoInexistente", false);
                    }
                    #endregion

                    #region CTNC-12 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoInvalido
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado12 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-12_NomeSacadoInvalido.txt", "CTN-12 CNAB NomeSacadoInvalido");
                        RegistroTestesNegativos.Registrar("CTNC-12 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoInvalido", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-12 Não deve Aceitar Envio de Operação CNAB com Arquivo com NomeSacadoInvalido", false);
                    }
                    #endregion

                    #region CTNC-13 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado13 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-13_CnpjSacadoEmBranco.txt", "CTN-13 CNAB CnpjSacadoEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-13 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-13 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoEmBranco", false);
                    }
                    #endregion

                    #region CTNC-14 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoInvalido13Char
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado14 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-14_CnpjSacadoInvalido13Char.txt", "CTN-14 CNAB CnpjSacadoInvalido13Char");
                        RegistroTestesNegativos.Registrar("CTNC-14 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoInvalido13Char", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-14 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoInvalido13Char", false);
                    }
                    #endregion

                    #region CTNC-15 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoInvalido15Char
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado15 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-15_CnpjSacadoInvalido15Char.txt", "CTN-15 CNAB CnpjSacadoInvalido15Char");
                        RegistroTestesNegativos.Registrar("CTNC-15 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoInvalido15Char", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-15 Não deve Aceitar Envio de Operação CNAB com Arquivo com CnpjSacadoInvalido15Char", false);
                    }
                    #endregion

                    #region CTNC-16 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencFormatoInv
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado16 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-16_DataVencimentoFormatoInválido.txt", "CTN-16 CNAB DataVencFormatoInv");
                        RegistroTestesNegativos.Registrar("CTNC-16 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencFormatoInv", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-16 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencFormatoInv", false);
                    }
                    #endregion

                    #region CTNC-17 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado17 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-17_DataVencimentoEmBranco.txt", "CTN-17 CNAB DataVencEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-17 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-17 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencEmBranco", false);
                    }
                    #endregion

                    #region CTNC-18 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencPassado
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado18 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-18_DataVencimentoNoPassado.txt", "CTN-18 CNAB DataVencPassado");
                        RegistroTestesNegativos.Registrar("CTNC-18 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencPassado", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-18 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataVencPassado", false);
                    }
                    #endregion

                    #region CTNC-19 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataEmisEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado19 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-19_DataEmissãoEmBranco.txt", "CTN-19 CNAB DataEmisEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-19 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataEmisEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-19 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataEmisEmBranco", false);
                    }
                    #endregion

                    #region CTNC-20 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataEmissPassado
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado20 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-20_DataEmissãoNoPassado.txt", "CTN-20 CNAB DataEmissPassado");
                        RegistroTestesNegativos.Registrar("CTNC-20 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataEmissPassado", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-20 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataEmissPassado", false);
                    }
                    #endregion

                    #region CTNC-21 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqFormatoInv
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado21 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-21_DataAquisiçãoFormatoInválido.txt", "CTN-21 CNAB DataAqFormatoInv");
                        RegistroTestesNegativos.Registrar("CTNC-21 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqFormatoInv", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-21 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqFormatoInv", false);
                    }
                    #endregion

                    #region CTNC-22 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado22 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-22_DataAquisiçãoEmBranco.txt", "CTN-22 CNAB DataAqEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-22 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-22 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqEmBranco", false);
                    }
                    #endregion

                    #region CTNC-23 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqPassado
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado23 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-23_DataAquisiçãoNoPassado.txt", "CTN-23 CNAB DataAqPassado");
                        RegistroTestesNegativos.Registrar("CTNC-23 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqPassado", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-23 Não deve Aceitar Envio de Operação CNAB com Arquivo com DataAqPassado", false);
                    }
                    #endregion

                    #region CTNC-24 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado24 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-24_NumeroDocumentoEmBranco.txt", "CTN-24 CNAB NuDocEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-24 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-24 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocEmBranco", false);
                    }
                    #endregion

                    #region CTNC-25 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocInexistente
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado25 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-25_NumeroDocumentoInexistente.txt", "CTN-25 CNAB NuDocInexistente");
                        RegistroTestesNegativos.Registrar("CTNC-25 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocInexistente", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-25 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocInexistente", false);
                    }
                    #endregion

                    #region CTNC-26 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocInvalido
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado26 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-26_NumeroDocumentoInvalido.txt", "CTN-26 CNAB NuDocInvalido");
                        RegistroTestesNegativos.Registrar("CTNC-26 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocInvalido", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-26 Não deve Aceitar Envio de Operação CNAB com Arquivo com NuDocInvalido", false);
                    }
                    #endregion

                    #region CTNC-27 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroEmBranco
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado27 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-27_SeuNumeroEmBranco.txt", "CTN-27 CNAB SeuNumeroEmBranco");
                        RegistroTestesNegativos.Registrar("CTNC-27 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroEmBranco", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-27 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroEmBranco", false);
                    }
                    #endregion

                    #region CTNC-28 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroInexistente
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado28 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-28_SeuNumeroInexistente.txt", "CTN-28 CNAB SeuNumeroInexistente");
                        RegistroTestesNegativos.Registrar("CTNC-28 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroInexistente", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-28 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroInexistente", false);
                    }
                    #endregion

                    #region CTNC-29 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroInvalido
                    await Page.ReloadAsync();
                    try
                    {
                        var resultado29 = await EnviarArquivoOperacaoNegativo.EnviarArquivoCnabNegativo(Page, "CTN-29_SeuNumeroInvalido.txt", "CTN-29 CNAB SeuNumeroInvalido");
                        RegistroTestesNegativos.Registrar("CTNC-29 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroInvalido", true);
                    }
                    catch
                    {
                        RegistroTestesNegativos.Registrar("CTNC-29 Não deve Aceitar Envio de Operação CNAB com Arquivo com SeuNumeroInvalido", false);
                    }
                    #endregion


                    //#region CTN-30 Não deve Aceitar Envio de Operação CNAB Com Arquivo em Formato .pdf
                    //await Page.ReloadAsync();
                    //var resultado30 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCSVPdfNegativo(Page, "CTN-30 CNAB teste.pdf");
                    //#endregion

                    #endregion

                    pagina.TotalErros = errosTotais;
                    if (operacoes.ListaErros2.Count == 0)
                    {
                        operacoes.ListaErros2.Add("0");
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operações no tópico Operações: ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesZitec?.Status ?? 0;
                    errosTotais += 2;
                    operacoes.ListaErros2.Add("Erro ao carregar a página de operações");
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                operacoes.ListaErros2.Add($"Exceção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros2 = errosTotais2;
            }

            pagina.TotalErros = errosTotais;
            if (operacoes.ListaErros2.Count == 0)
            {
                operacoes.ListaErros2.Add("0");
            }

            return (pagina, operacoes);
        }

        public static async Task<(Pagina pagina, Operacoes operacoes)> OperacoesZitecConsultoria(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var portalLink = TestesOperacoesOperacoes.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "/Operacoes/Operacoes2.0.aspx ");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec - Consultoria";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = await Acentos.ValidarAcentos(Page) ?? "❌";
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;

                    var processamentoFundo = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificaProcessamentoFundo(9991);

                    if (processamentoFundo)
                    {
                        operacoes.TipoOperacao2 = "Nova Operação - Consultoria";
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CNAB" }).ClickAsync();
                        await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });

                        // Valida se AtualizarDataEEnviarArquivo NÃO retornou null
                        operacoes.NovoNomeArquivo2 = await AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
                        if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                        {
                            throw new NullReferenceException("AtualizarDataEEnviarArquivo retornou null ou vazio.");
                        }

                        await Task.Delay(500);

                        var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                        await Task.Delay(25000);


                        if (CadastroOperacoes != null)
                        {

                            var (existe, idArquivo) = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo2);
                            var idOperacaoRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo2);
                            await Page.ReloadAsync();

                            if (existe)
                            {
                                Console.WriteLine("Operação lançada.");
                                pagina.InserirDados = "✅";
                                operacoes.ArquivoEnviado = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                                operacoes.ListaErros2.Add("Erro ao lançar operação");
                                operacoes.ArquivoEnviado = "❌" +
                                    "";
                            }

                            //await Page.ReloadAsync();

                            if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                                throw new NullReferenceException("NovoNomeArquivo2 está null ou vazio no fluxo de Consultoria.");
                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Consultoria']").ClickAsync();
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar" }).ClickAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("CEDENTE TESTE");
                            await Task.Delay(600);
                            var primeiroTr2 = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd2.ClickAsync();
                            var cnpj = "54638076000176";

                            var statusTrocado = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo2);
                            if (statusTrocado == "PG")
                            {
                                operacoes.StatusTrocados2 = "✅";
                                operacoes.AprovacoesRealizadas2 = "✅";
                            }
                            else
                            {
                                operacoes.StatusTrocados2 = "❌";
                                operacoes.AprovacoesRealizadas2 = "❌";
                            }
                            //var button = Page.Locator($"button[onclick=\"ModalExcluirArquivo('{idArquivo}','{idOperacaoRecebivel}','{operacoes.NovoNomeArquivo2}','{cnpj}')\"]").First.ClickAsync();
                            await Page.EvaluateAsync($"""
    ModalExcluirArquivo('{idArquivo}','{idOperacaoRecebivel}','{operacoes.NovoNomeArquivo2}','{cnpj}');
""");
                            await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#motivoExcluirArquivo").FillAsync("teste de exclusão");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            Console.WriteLine("Botão de apagar operação encontrado.");

                            var apagarOperacao = await Page.GetByText("Arquivo excluído com sucesso!").ElementHandleAsync();

                            if (apagarOperacao != null)
                            {
                                operacoes.OpApagadaBtn = "✅";
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                operacoes.OpApagadaBtn = "❌";
                                operacoes.OpApagadaBtn = "❌";

                                bool exclusaoRemessa = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                                bool exclusaoTed = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo2);
                                var idRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo2);
                                var excluirAvalista = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);
                                var excluirCertificadora = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacaoCertificadora(idRecebivel);

                                if (exclusaoRemessa && exclusaoTed && excluirAvalista)
                                {
                                    bool excluirOperacao = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                    if (excluirOperacao)
                                    {
                                        Console.WriteLine("Operação excluída com sucesso pelo banco.");
                                        pagina.Excluir = "✅";

                                    }
                                    else
                                    {
                                        Console.WriteLine("Operação não excluída.");
                                        pagina.Excluir = "❌";
                                        errosTotais2++;
                                        operacoes.ListaErros2.Add("Operação não excluída");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Não foi possível excluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("O fundo não está na data atual");
                        errosTotais2++;
                        operacoes.ListaErros2.Add("Não foi possível processar o fundo para a data de hoje");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                operacoes.ListaErros2.Add($"Exceção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros2 = errosTotais2;
            }

            pagina.TotalErros = errosTotais;
            if (operacoes.ListaErros2.Count == 0)
            {
                operacoes.ListaErros2.Add("0");
            }

            return (pagina, operacoes);
        }


        public static async Task<(Pagina pagina, Operacoes operacoes)> OperacoesZiteGestora(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var portalLink = TestesOperacoesOperacoes.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "/Operacoes/Operacoes2.0.aspx ");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec - Gestora";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = await Acentos.ValidarAcentos(Page) ?? "❌";
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;


                    var processamentoFundo = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificaProcessamentoFundo(9991);

                    if (processamentoFundo)
                    {

                        operacoes.TipoOperacao2 = "Nova Operação - Gestora";
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CNAB" }).ClickAsync();
                        await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "54638076000176" });

                        // Valida se AtualizarDataEEnviarArquivo NÃO retornou null
                        operacoes.NovoNomeArquivo2 = await AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
                        if (string.IsNullOrEmpty(operacoes.NovoNomeArquivo2))
                        {
                            throw new NullReferenceException("AtualizarDataEEnviarArquivo retornou null ou vazio.");
                        }

                        await Task.Delay(500);
                        var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                        await Task.Delay(15000);

                        if (CadastroOperacoes != null)
                        {

                            var (existe, idArquivo) = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo2);
                            var idOperacaoRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo2);

                            await Page.ReloadAsync();

                            if (existe)
                            {
                                Console.WriteLine("Operação lançada.");
                                pagina.InserirDados = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                                operacoes.ListaErros2.Add("Erro ao lançar operação");
                            }
                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo2);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Consultoria']").ClickAsync();
                            await Task.Delay(300);
                            while (await Page.Locator("#aprovaOpConsultoriaBtn").IsVisibleAsync())
                            {
                                await Page.Locator("#aprovaOpConsultoriaBtn").ClickAsync();
                                await Task.Delay(1000);
                            }

                            string status2 = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo2);
                            await Task.Delay(200);

                            if (status2 == "PG")
                            {
                                operacoes.StatusTrocados2 = "✅";
                                operacoes.AprovacoesRealizadas2 = "✅";
                                statusTrocados++;
                                Console.WriteLine("Todos os status foram trocados corretamente, aprovações realizadas! ");
                            }
                            else
                            {
                                operacoes.StatusTrocados2 = "❌";
                                operacoes.AprovacoesRealizadas2 = "❌";
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Status PI não encontrado");
                                Console.WriteLine("Os status não foram trocados corretamente, aprovações realizadas! ");
                                errosTotais2++;
                                operacoes.ListaErros2.Add("Aprovações realizadas, mas status não foi trocado no banco de dados");
                            }

                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("CEDENTE TESTE");
                            await Task.Delay(600);
                            var primeiroTr2 = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await primeiroTd2.ClickAsync();
                            var cnpj = "54638076000176";

                            //var button = Page.Locator($"button[onclick=\"ModalExcluirArquivo('{idArquivo}','{idOperacaoRecebivel}','{operacoes.NovoNomeArquivo2}','{cnpj}')\"]").First.ClickAsync();
                            await Page.EvaluateAsync($"""
                            ModalExcluirArquivo('{idArquivo}','{idOperacaoRecebivel}','{operacoes.NovoNomeArquivo2}','{cnpj}');
                            """);
                            await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#motivoExcluirArquivo").FillAsync("teste de exclusão");
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            Console.WriteLine("Botão de apagar operação encontrado.");

                            var apagarOperacao = await Page.GetByText("Arquivo excluído com sucesso!").ElementHandleAsync();

                            if (apagarOperacao != null)
                            {
                                operacoes.OpApagadaBtn = "✅";
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                operacoes.OpApagadaBtn = "❌";
                                pagina.Excluir = "❌";
                                bool exclusaoRemessa = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo2);
                                bool exclusaoTed = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo2);
                                var idRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo2);
                                var excluirAvalista = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);
                                var excluirCertificadora = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacaoCertificadora(idRecebivel);

                                if (exclusaoRemessa && exclusaoTed && excluirAvalista)
                                {
                                    bool excluirOperacao = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo2);

                                    if (excluirOperacao)
                                    {
                                        Console.WriteLine("Operação excluída com sucesso pelo banco.");
                                        pagina.Excluir = "✅";

                                    }
                                    else
                                    {
                                        Console.WriteLine("Operação não excluída.");
                                        pagina.Excluir = "❌";
                                        errosTotais2++;
                                        operacoes.ListaErros2.Add("Operação não excluída");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluída.");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros2.Add("Não foi possível excluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED");
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("O fundo não está na data atual");
                            errosTotais2++;
                            operacoes.ListaErros2.Add("Não foi possível processar o fundo para a data de hoje");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                operacoes.ListaErros2.Add($"Exceção lançada. ");
                errosTotais2++;
                operacoes.totalErros2 = errosTotais2;
            }
            pagina.TotalErros = errosTotais;
            if (operacoes.ListaErros2.Count == 0)
            {
                operacoes.ListaErros2.Add("0");
            }

            return (pagina, operacoes);
        }
    }
}
