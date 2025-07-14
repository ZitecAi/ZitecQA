using Azure;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

using TesteOperacoesOperacoes.Model;
using TesteOperacoesOperacoes.Util;
using static TesteOperacoesOperacoes.Model.Usuario;


namespace TesteOperacoesOperacoes.Pages.OperacoesPage
{
    public class CadastroOperacoesZitecCsv
    {

        public static async Task<(Pagina pagina, Operacoes operacoes, List<TesteNegativoResultado> resultadosNegativos)> OperacoesZitecCsv(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {

            var pagina = new Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\Temp\Arquivos\CNABz.txt";
            operacoes.ListaErros3 = new List<string>();
            List<TesteNegativoResultado> resultadosTestesNegativos = new();


            try
            {
                var portalLink = TestesOperacoesOperacoes.Program.Config["Links:Portal"];
                var OperacoesZitec = await Page.GotoAsync(portalLink + "Operacoes/Operacoes2.0.aspx");

                if (OperacoesZitec.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";


                    Console.Write("Operações Zitec csv: ");
                    pagina.Nome = "Operações Zitec csv";
                    pagina.StatusCode = OperacoesZitec.Status;                    
                    pagina.Acentos = Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌") errosTotais++;
                    pagina.Listagem = TesteOperacoesOperacoes.Util.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌") errosTotais++;
                    pagina.BaixarExcel = Util.Excel.BaixarExcelPorId(Page).Result;
                    if (pagina.BaixarExcel == "❌") errosTotais++;

                    string pastaArquivos = "C:/TempQA/Arquivos/";
                    string nomeArquivoOriginal = "arquivoteste_operacoescsv_qa.csv";
                    string caminhoOriginal = Path.Combine(pastaArquivos, nomeArquivoOriginal);
                    string nomeArquivoModificado = TesteOperacoesOperacoes.Util.ModificarArquivoCsv.ModificarCsv(caminhoOriginal, pastaArquivos);
                    string caminhoModificado = Path.Combine(pastaArquivos, nomeArquivoModificado);

                    operacoes.TipoOperacao3 = "Operacoes Zitec - csv";
                    operacoes.NovoNomeArquivo3 = nomeArquivoModificado;

                    if (nivelLogado == NivelEnum.Master)
                    {

                        //string pastaArquivos = "C:/TempQA/Arquivos/";
                        //string nomeArquivoOriginal = "arquivoteste_operacoescsv_qa.csv";
                        //string caminhoOriginal = Path.Combine(pastaArquivos, nomeArquivoOriginal);
                        //string nomeArquivoModificado = TesteOperacoesOperacoes.Util.ModificarArquivoCsv.ModificarCsv(caminhoOriginal, pastaArquivos);
                        //string caminhoModificado = Path.Combine(pastaArquivos, nomeArquivoModificado);

                        operacoes.TipoOperacao3 = "Operacoes Zitec - csv";
                        operacoes.NovoNomeArquivo3 = nomeArquivoModificado;
                        // Envia o arquivo atualizado para o input
                        for (int i = 0; i < 2; i++)  // Tenta no máximo 2 vezes (inicial + 1 tentativa)
                        {
                            #region Deve enviar uma operação Csv
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação - CSV" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#selectFundoCsv").SelectOptionAsync(new[] { "54638076000176" });
                            await Task.Delay(200);
                            await Page.Locator("#fileEnviarOperacoesCsv").SetInputFilesAsync(new[] { caminhoModificado });
                            await Task.Delay(200);
                            await Page.Locator("#fileEnviarLastro").SetInputFilesAsync(new[] { TestesOperacoesOperacoes.Program.Config["Paths:Arquivo"] + "Arquivo teste.zip" });
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                            await Task.Delay(200);
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste de envio csv");
                            await Task.Delay(200);
                            //while (await Page.Locator("#enviarButton").IsVisibleAsync())
                            //{
                            //    await Page.Locator("#enviarButton").ClickAsync();
                            //    await Task.Delay(1000);
                            //}
                            await Page.Locator("#enviarButton").ClickAsync();
                            await Task.Delay(5000);

                            ILocator xSelector = Page.Locator("#btnFecharNovoOperacaoCsv");
                            if (await xSelector.IsVisibleAsync())
                            {
                                await xSelector.ClickAsync();
                                break;
                            }
                            #endregion



                            #region Deve Consultar Operação CSV pela Tabela
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(nomeArquivoModificado);
                            bool arquivopresentenatabela = true;
                            if (arquivopresentenatabela)
                            {
                                var linhas = await Page.Locator("#divTabelaCedentes").AllAsync();
                                bool textoEncontrado = false;

                                foreach (var linha in linhas)
                                {
                                    string textolinha = await linha.InnerTextAsync();

                                    if (textolinha.Contains("CEDENTE TESTE"))
                                    {
                                        textoEncontrado = true;
                                        Console.WriteLine($"texto encontrado: {textolinha} na tabela, Filtragem Funcionando corretamente");
                                        //adicionar errosnegativos++
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Não foi possivel encontrar o Arquivo na tabela");
                                    }
                                }
                            }
                            #endregion

                            #region Deve Baixar Arquivo Remessa
                            
                            await Page.ReloadAsync();
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync("CEDENTE TESTE");
                            await Page.Locator("#listaCedentes input[type='checkbox']").First.CheckAsync(); 
                            

                            var page1 = await Page.RunAndWaitForPopupAsync(async () =>
                            {
                                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                                {
                                    await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                                    
                                });
                                try
                                {
                                    await Util.Excel.ValidarDownloadAsync(download, "Arquivo Remessa CSV CEDENTE TESTE");
                                    Console.WriteLine("Arquivo Remessa Baixado com Sucesso.");
                                }
                                catch
                                {
                                    throw new Exception("Não foi possivel baixar Arquivo Remessa.");
                                }
                                
                                
                            });

                            #region Deve Alterar Status da Operação
                            #region Reprovado pelo custodiante
                            await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                            await Page.Locator("#statusPosOp").SelectOptionAsync(new[] { "RI" });
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            await Task.Delay(15000);
                            await Page.ReloadAsync();
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(nomeArquivoModificado);
                            await Page.Locator(".dtr-control").First.ClickAsync();                            
                            try
                            {
                                string textoProcurado = "Reprovado pelo custodiante";
                                // Espera a tabela aparecer
                                await  Page.Locator(seletorTabela).WaitForAsync();

                                // Captura todas as linhas da tabela
                                var linhas = await Page.Locator($"{seletorTabela} tr").AllAsync();

                                bool textoEncontrado = false;

                                foreach (var linha in linhas)
                                {
                                    string textoLinha = await linha.InnerTextAsync();
                                    if (textoLinha.Contains(textoProcurado))
                                    {
                                        textoEncontrado = true;
                                        Console.WriteLine("Texto Reprovado pelo custodiante Presente na tabela como esperado.");
                                        break;
                                    }
                                }

                                

                            }
                            catch
                            {
                                throw new Exception();
                                Console.WriteLine("Nâo foi possivel encontrar o Texto comprovando alteração de status da operação para Reprovado pelo custodiante.");
                            }
                            #endregion

                            #region Pago pelo Banco Cobrador
                            await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                            await Page.Locator("#statusPosOp").SelectOptionAsync(new[] { "PB" });
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            await Task.Delay(15000);
                            await Page.ReloadAsync();
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(nomeArquivoModificado);
                            await Page.Locator(".dtr-control").First.ClickAsync();
                            try
                            {
                                string textoProcurado = "Pago pelo Banco Cobrador";
                                // Espera a tabela aparecer
                                await Page.Locator(seletorTabela).WaitForAsync();

                                // Captura todas as linhas da tabela
                                var linhas = await Page.Locator($"{seletorTabela} tr").AllAsync();

                                bool textoEncontrado = false;

                                foreach (var linha in linhas)
                                {
                                    string textoLinha = await linha.InnerTextAsync();
                                    if (textoLinha.Contains(textoProcurado))
                                    {
                                        textoEncontrado = true;
                                        Console.WriteLine("Texto Pago pelo Banco Cobrador Presente na tabela como esperado.");
                                        break;
                                    }
                                }



                            }
                            catch
                            {
                                throw new Exception();
                                Console.WriteLine("Nâo foi possivel encontrar o Texto comprovando alteração de status da operação para Pago pelo Banco Cobrador.");
                            }
                            #endregion



                            #endregion



                            #endregion

                            var idOperacaoRecebivel = Repositories.OperacoesCsv.OperacoesCsvRepository.ObterIdOperacaoRec(nomeArquivoModificado);


                            if (idOperacaoRecebivel != 0)
                            {

                                var idOperacao = Repositories.OperacoesCsv.OperacoesCsvRepository.VerificarOperacao(idOperacaoRecebivel);
                                var (recebivelExiste, idRecebivel) = Repositories.OperacoesCsv.OperacoesCsvRepository.VerificarRecebivel(idOperacaoRecebivel);
                                var complementoRelExiste = Repositories.OperacoesCsv.OperacoesCsvRepository.VerificarRecebivelComplemento(idRecebivel);

                                if (recebivelExiste && complementoRelExiste)
                                {
                                    pagina.InserirDados = "✅";
                                    operacoes.ArquivoEnviado = "✅";
                                    //operacoes.StatusTrocados3 = "❓";
                                    //operacoes.AprovacoesRealizadas3 = "❓";
                                    //    await Page.GetByLabel("Pesquisar").FillAsync(nomeArquivoModificado);
                                    //    var primeiroTr = Page.Locator("#listaCedentes tr").First;
                                    //    var primeiroTd = primeiroTr.Locator("td").First;
                                    //    await primeiroTd.ClickAsync();
                                    //    await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                                    //await Page.Locator("button[title='Aprovação Gestora']").ClickAsync();

                                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar", Exact = true }).ClickAsync();
                                    //    await Task.Delay(3000);
                                    //    var status = Repository.OperacoesCsv.OperacoesCsvRepository.VerificarStatus(nomeArquivoModificado);

                                    //    if (status == "PI")
                                    //    {
                                    //        operacoes.AprovacoesRealizadas3 = "✅";
                                    //        operacoes.StatusTrocados3 = "✅";
                                    //    }
                                    //    else
                                    //    {
                                    //        operacoes.StatusTrocados3 = "❌";
                                    //        operacoes.AprovacoesRealizadas3 = "❌";
                                    //    }


                                }
                                else
                                {
                                    Console.WriteLine("Não foi possível lançar operação");
                                    pagina.InserirDados = "❌";
                                    pagina.Excluir = "❌";
                                    errosTotais += 2;
                                    operacoes.ListaErros3.Add("Operação não foi corretamente para o banco de dados");
                                }


                            }
                            else
                            {
                                await Page.Locator("#enviarButton").ClickAsync();
                                Console.WriteLine("Não foi possível lançar operação");
                                pagina.InserirDados = "❌";
                                pagina.Excluir = "❌";
                                errosTotais += 2;
                                operacoes.ListaErros3.Add("Erro ao lançar operação");

                            }
                        }
                        #region Testes Negativos

                        #region CTN-01 Não deve Aceitar Envio de Operação Com Arquivo em Formato .pdf
                        await Page.ReloadAsync();
                        var resultado1 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarArquivoPdfNegativo(Page, "CTN-01 teste.pdf");
                        resultadosTestesNegativos.Add(resultado1);
                        #endregion

                        #region CTN-02 Não deve Aceitar Envio de Operação com Arquivo com CnpjOriginadorEmBranco
                        await Page.ReloadAsync();
                        var resultado2 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjOriginadorEmBranco - Copia.csv", "CTN-02 CnpjOriginadorEmBranco");
                        resultadosTestesNegativos.Add(resultado2);
                        #endregion

                        #region CTN-03 Não deve Aceitar Envio de Operação com Arquivo com CnpjOriginadorInvalido13Char
                        await Page.ReloadAsync();
                        var resultado3 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjOriginadorInvalido13Char.csv", "CTN-03 CnpjOriginadorInvalido13Char");
                        resultadosTestesNegativos.Add(resultado3);
                        #endregion

                        #region CTN-04 Não deve Aceitar Envio de Operação com Arquivo com CnpjOriginadorInvalido15Char
                        await Page.ReloadAsync();
                        var resultado4 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjOriginadorInvalido15Char.csv", "CTN-04 CnpjOriginadorInvalido15Char");
                        resultadosTestesNegativos.Add(resultado4);
                        #endregion

                        #region CTN-05 Não deve Aceitar Envio de Operação com Arquivo com NomeCedenteEmBranco
                        await Page.ReloadAsync();
                        var resultado5 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNomeCedenteEmBranco.csv", "CTN-05 NomeCedenteEmBranco");
                        resultadosTestesNegativos.Add(resultado5);
                        #endregion

                        #region CTN-06 Não deve Aceitar Envio de Operação com Arquivo com NomeCedenteInexistente
                        await Page.ReloadAsync();
                        var resultado6 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNomeCedenteInexistente.csv", "CTN-06 NomeCedenteInexistente");
                        resultadosTestesNegativos.Add(resultado6);
                        #endregion

                        #region CTN-07 Não deve Aceitar Envio de Operação com Arquivo com NomeCedenteInvalido
                        await Page.ReloadAsync();
                        var resultado7 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNomeCedenteInvalido.csv", "CTN-07 NomeCedenteInvalido");
                        resultadosTestesNegativos.Add(resultado7);
                        #endregion

                        #region CTN-08 Não deve Aceitar Envio de Operação com Arquivo com CnpjCedenteEmBranco
                        await Page.ReloadAsync();
                        var resultado8 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjCedenteEmBranco.csv", "CTN-08 CnpjCedenteEmBranco");
                        resultadosTestesNegativos.Add(resultado8);
                        #endregion

                        #region CTN-09 Não deve Aceitar Envio de Operação com Arquivo com CnpjCedenteInvalido13Char
                        await Page.ReloadAsync();
                        var resultado9 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjCedenteInvalido13Char.csv", "CTN-09 CnpjCedenteInvalido13Char");
                        resultadosTestesNegativos.Add(resultado9);
                        #endregion

                        #region CTN-10 Não deve Aceitar Envio de Operação com Arquivo com CnpjCedenteInvalido15Char
                        await Page.ReloadAsync();
                        var resultado10 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjCedenteInvalido15Char.csv", "CTN-10 CnpjCedenteInvalido15Char");
                        resultadosTestesNegativos.Add(resultado10);
                        #endregion

                        #region CTN-11 Não deve Aceitar Envio de Operação com Arquivo com NomeSacadoEmBranco
                        await Page.ReloadAsync();
                        var resultado11 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNomeSacadoEmBranco.csv", "CTN-11 NomeSacadoEmBranco");
                        resultadosTestesNegativos.Add(resultado11);
                        #endregion

                        #region CTN-12 Não deve Aceitar Envio de Operação com Arquivo com NomeSacadoInexistente
                        await Page.ReloadAsync();
                        var resultado12 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNomeSacadoInexistente.csv", "CTN-12 NomeSacadoInexistente");
                        resultadosTestesNegativos.Add(resultado12);
                        #endregion

                        #region CTN-13 Não deve Aceitar Envio de Operação com Arquivo com NomeSacadoInvalido
                        await Page.ReloadAsync();
                        var resultado13 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNomeSacadoInvalido.csv", "CTN-13 NomeSacadoInvalido");
                        resultadosTestesNegativos.Add(resultado13);
                        #endregion

                        #region CTN-14 Não deve Aceitar Envio de Operação com Arquivo com CnpjSacadoEmBranco
                        await Page.ReloadAsync();
                        var resultado14 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjSacadoEmBranco.csv", "CTN-14 CnpjSacadoEmBranco");
                        resultadosTestesNegativos.Add(resultado14);
                        #endregion

                        #region CTN-15 Não deve Aceitar Envio de Operação com Arquivo com CnpjSacadoInvalido13Char
                        await Page.ReloadAsync();
                        var resultado15 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjSacadoInvalido13Char.csv", "CTN-15 CnpjSacadoInvalido13Char");
                        resultadosTestesNegativos.Add(resultado15);
                        #endregion

                        #region CTN-16 Não deve Aceitar Envio de Operação com Arquivo com CnpjSacadoInvalido15Char
                        await Page.ReloadAsync();
                        var resultado16 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoCnpjSacadoInvalido15Char.csv", "CTN-16 CnpjSacadoInvalido15Char");
                        resultadosTestesNegativos.Add(resultado16);
                        #endregion

                        #region CTN-17 Não deve Aceitar Envio de Operação com Arquivo com DataVencFormatoInv
                        await Page.ReloadAsync();
                        var resultado17 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataVencFormatoInv.csv", "CTN-17 DataVencFormatoInv");
                        resultadosTestesNegativos.Add(resultado17);
                        #endregion

                        #region CTN-18 Não deve Aceitar Envio de Operação com Arquivo com DataVencEmBranco
                        await Page.ReloadAsync();
                        var resultado18 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataVencEmBranco.csv", "CTN-18 DataVencEmBranco");
                        resultadosTestesNegativos.Add(resultado18);
                        #endregion

                        #region CTN-19 Não deve Aceitar Envio de Operação com Arquivo com DataVencPassado
                        await Page.ReloadAsync();
                        var resultado19 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataVencPassado.csv", "CTN-19 DataVencPassado");
                        resultadosTestesNegativos.Add(resultado19);
                        #endregion

                        #region CTN-20 Não deve Aceitar Envio de Operação com Arquivo com DataEmisEmBranco
                        await Page.ReloadAsync();
                        var resultado20 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataEmisEmBranco.csv", "CTN-20 DataEmisEmBranco");
                        resultadosTestesNegativos.Add(resultado20);
                        #endregion

                        #region CTN-21 Não deve Aceitar Envio de Operação com Arquivo com DataEmissPassado
                        await Page.ReloadAsync();
                        var resultado21 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataEmissPassado.csv", "CTN-21 DataEmissPassado");
                        resultadosTestesNegativos.Add(resultado21);
                        #endregion

                        #region CTN-22 Não deve Aceitar Envio de Operação com Arquivo com DataAqFormatoInv
                        await Page.ReloadAsync();
                        var resultado22 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataAqFormatoInv.csv", "CTN-22 DataAqFormatoInv");
                        resultadosTestesNegativos.Add(resultado22);
                        #endregion

                        #region CTN-23 Não deve Aceitar Envio de Operação com Arquivo com DataAqEmBranco
                        await Page.ReloadAsync();
                        var resultado23 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataAqEmBranco.csv", "CTN-23 DataAqEmBranco");
                        resultadosTestesNegativos.Add(resultado23);
                        #endregion

                        #region CTN-24 Não deve Aceitar Envio de Operação com Arquivo com DataAqPassado
                        await Page.ReloadAsync();
                        var resultado24 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoDataAqPassado.csv", "CTN-24 DataAqPassado");
                        resultadosTestesNegativos.Add(resultado24);
                        #endregion

                        #region CTN-25 Não deve Aceitar Envio de Operação com Arquivo com NuDocEmBranco
                        await Page.ReloadAsync();
                        var resultado25 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNuDocEmBranco.csv", "CTN-25 NuDocEmBranco");
                        resultadosTestesNegativos.Add(resultado25);
                        #endregion

                        #region CTN-26 Não deve Aceitar Envio de Operação com Arquivo com NuDocInexistente
                        await Page.ReloadAsync();
                        var resultado26 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNuDocInexistente.csv", "CTN-26 NuDocInexistente");
                        resultadosTestesNegativos.Add(resultado26);
                        #endregion

                        #region CTN-27 Não deve Aceitar Envio de Operação com Arquivo com NuDocInvalido
                        await Page.ReloadAsync();
                        var resultado27 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoNuDocInvalido.csv", "CTN-27 NuDocInvalido");
                        resultadosTestesNegativos.Add(resultado27);
                        #endregion

                        #region CTN-28 Não deve Aceitar Envio de Operação com Arquivo com SeuNumeroEmBranco
                        await Page.ReloadAsync();
                        var resultado28 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoSeuNumeroEmBranco.csv", "CTN-28 SeuNumeroEmBranco");
                        resultadosTestesNegativos.Add(resultado28);
                        #endregion

                        #region CTN-29 Não deve Aceitar Envio de Operação com Arquivo com SeuNumeroInexistente
                        await Page.ReloadAsync();
                        var resultado29 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoSeuNumeroInexistente.csv", "CTN-29 SeuNumeroInexistente");
                        resultadosTestesNegativos.Add(resultado29);
                        #endregion

                        #region CTN-30 Não deve Aceitar Envio de Operação com Arquivo com SeuNumeroInvalido
                        await Page.ReloadAsync();
                        var resultado30 = await TesteOperacoesOperacoes.Util.EnviarCsvNegativo.EnviarAquivoCvsNegativo(Page, "TesteNegativoSeuNumeroInvalido.csv", "CTN-30 SeuNumeroInvalido");
                        resultadosTestesNegativos.Add(resultado30);
                        #endregion

                        #endregion


                    }

                    else if (nivelLogado == NivelEnum.Consultoria)
                    {
                        await Page.ReloadAsync();
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo3);
                        var primeiroTr = Page.Locator("#listaCedentes tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Consultoria']").ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar" }).ClickAsync();
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                    else if (nivelLogado == NivelEnum.Gestora)
                    {
                        string status = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo3);
                        var (existe, idArquivo) = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificaExistenciaOperacao(operacoes.NovoNomeArquivo3);
                        var idOperacaoRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOpRec(operacoes.NovoNomeArquivo3);


                        if (status == "PG")
                        {
                            pagina.InserirDados = "❓";
                            Console.WriteLine("O status foi trocado para aguardar a aprovação da gestora");
                            statusTrocados++;
                            await Page.ReloadAsync();
                            await Task.Delay(2000);
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(2000);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo3);
                            var primeiroTr = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            await Page.Locator("span.dtr-title:has-text('Ações') >> xpath=.. >> button[title='Aprovação Gestora']").ClickAsync();

                            await Page.GetByRole(AriaRole.Button, new() { Name = "Aprovar", Exact = true }).ClickAsync();
                            await Task.Delay(4000);
                            string status2 = Repositories.OperacoesZitec.OperacoesZitecRepository.VerificarStatus(operacoes.NovoNomeArquivo3);

                            if (status2 == "AC")
                            {
                                statusTrocados++;
                                Console.WriteLine("Todos os status foram trocados corretamente, aprovações realizadas! ");
                                operacoes.AprovacoesRealizadas3 = "✅";
                                operacoes.StatusTrocados3 = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Os status não foram trocados corretamente, aprovações realizadas! ");
                                errosTotais2++;
                                operacoes.ListaErros3.Add("Não foi possível aprovar como gestora");
                                operacoes.AprovacoesRealizadas3 = "❌";
                                operacoes.StatusTrocados3 = "❌";
                            }
                            //apagar pelo botão
                            await Page.ReloadAsync();
                            var cnpj = "54638076000176";
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync(operacoes.NovoNomeArquivo3);
                            await Task.Delay(600);
                            var primeiroTr2 = Page.Locator("#listaCedentes tr").First;
                            var primeiroTd2 = primeiroTr2.Locator("td").First;
                            await primeiroTd2.ClickAsync();
                            await Page.EvaluateAsync($"""ModalExcluirArquivo('{idArquivo}','{idOperacaoRecebivel}','{operacoes.NovoNomeArquivo3}','{cnpj}');""");
                            await Page.Locator("#motivoExcluirArquivo").ClickAsync();
                            await Task.Delay(200);
                            await Page.Locator("#motivoExcluirArquivo").FillAsync("teste de exclus");
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
                            }

                            bool exclusaoRemessa = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo3);
                            bool exclusaoTed = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo3);
                            var idRecebivel = Repositories.OperacoesZitec.OperacoesZitecRepository.ObterIdOperacaoRecebivel(operacoes.NovoNomeArquivo3);
                            var excluirAvalista = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirAvalista(idRecebivel);

                            if (exclusaoRemessa && exclusaoTed && excluirAvalista)

                            {
                                bool excluirOperacao = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo3);

                                if (excluirOperacao)

                                {
                                    Console.WriteLine("Operação excluida com sucesso. ");
                                    pagina.Excluir = "✅";
                                    pagina.InserirDados = "❓";
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluida. ");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros3.Add("Operação não excluída");
                                }

                            }
                            else
                            {

                                Console.WriteLine("Operação não excluida. ");
                                pagina.Excluir = "❌";
                                errosTotais2++;
                                operacoes.ListaErros3.Add("Não foi possível exluir operação nas tabelas: TB_STG_REMESSA e dbo.TB_TED ");
                            }


                        }
                        else
                        {
                            operacoes.AprovacoesRealizadas3 = "❌";
                            operacoes.StatusTrocados3 = "❌";
                            Console.WriteLine("O status não foi trocado para aguardar a aprovação da gestora");
                            errosTotais2++;
                            operacoes.ListaErros3.Add("Status não foi trocado para aprovação da gestora");
                            bool exclusaoRemessa = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirRemessa(operacoes.NovoNomeArquivo3);
                            bool exclusaoTed = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirTbTed(operacoes.NovoNomeArquivo3);

                            if (exclusaoRemessa && exclusaoTed)

                            {

                                bool excluirOperacao = Repositories.OperacoesZitec.OperacoesZitecRepository.ExcluirOperacao(operacoes.NovoNomeArquivo3);

                                if (excluirOperacao)

                                {
                                    Console.WriteLine("Operação excluida com sucesso. ");
                                    pagina.Excluir = "✅";
                                }
                                else
                                {
                                    Console.WriteLine("Operação não excluida. ");
                                    pagina.Excluir = "❌";
                                    errosTotais2++;
                                    operacoes.ListaErros3.Add("Operação não excluída");
                                }

                            }

                        }

                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operações no tópico Operações: ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesZitec.Status;
                    errosTotais += 2;
                    operacoes.ListaErros3.Add("Erro ao carregar a página de operações");
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (operacoes.ListaErros3.Count == 0)
                {
                    operacoes.ListaErros3.Add("0");
                }
                Console.WriteLine($"exceção: {ex}");
                operacoes.ListaErros3.Add($"Execeção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros3 = errosTotais2;
            }
            if (operacoes.ListaErros3.Count == 0)
            {
                operacoes.ListaErros3.Add("0");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, operacoes, resultadosTestesNegativos);
        }
    }
}
