using Azure;
using Microsoft.Playwright;
using TesteOperacoesOperacoes.Model;
using TesteOperacoesOperacoes.Util;
using static Microsoft.Playwright.Assertions;
using static TesteOperacoesOperacoes.Model.Usuario;
using static TesteOperacoesOperacoes.Util.RegistroTestesPositivos;


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

                        //operacoes.TipoOperacao3 = "Operacoes Zitec - csv";
                        //operacoes.NovoNomeArquivo3 = nomeArquivoModificado;
                        //Envia o arquivo atualizado para o input
                        //for (int i = 0; i < 2; i++)  // Tenta no máximo 2 vezes (inicial + 1 tentativa)
                        //{
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
                            //break;
                        }
                        #endregion



                        #region Deve Consultar Operação CSV pela Tabela
                        await Task.Delay(2000);
                        await Page.ReloadAsync();
                        await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(nomeArquivoModificado);


                        try
                        {
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
                                        break;


                                    }

                                }
                                RegistroTestesPositivos.Registrar("CTP-01 Deve Consultar Operação CSV pela Tabela", textoEncontrado);

                            }
                        }
                        catch
                        {
                            Console.WriteLine("texto NÃO encontrado na tabela");
                            RegistroTestesPositivos.Registrar("CTP-01 Deve Consultar Operação CSV pela Tabela", false);
                        }

                        #endregion

                        #region Deve Baixar Arquivo Remessa
                        await Task.Delay(2000);
                        await Page.ReloadAsync();
                        await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Searchbox, new() { Name = "Pesquisar" }).FillAsync(nomeArquivoModificado);
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
                                RegistroTestesPositivos.Registrar("CTP-02 Deve Baixar Arquivo Remessa", true);
                            }
                            catch
                            {
                                //throw new Exception("Não foi possivel baixar Arquivo Remessa.");
                                Console.WriteLine("Download Arquivo Remessa Falhou.");
                                RegistroTestesPositivos.Registrar("CTP-02 Deve Baixar Arquivo Remessa", false);
                            }


                        });

                        #region Deve Alterar Status da Operação
                        #region Reprovado pelo custodiante
                        await Task.Delay(2000);
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
                                    Console.WriteLine("Texto Reprovado pelo custodiante Presente na tabela como esperado.");
                                    break;
                                }

                            }

                            RegistroTestesPositivos.Registrar("CTP-03 Deve Alterar Status da Operação para Reprovado pelo custodiante", textoEncontrado);


                        }
                        catch
                        {
                            //throw new Exception();
                            Console.WriteLine("Nâo foi possivel encontrar o Texto comprovando alteração de status da operação para Reprovado pelo custodiante.");
                            RegistroTestesPositivos.Registrar("CTP-03 Deve Alterar Status da Operação para Reprovado pelo custodiante", false);
                        }
                        #endregion

                        #region Pago pelo Banco Cobrador
                        await Task.Delay(2000);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                        await Page.Locator("#statusPosOp").SelectOptionAsync(new[] { "PB" });
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                        await Task.Delay(18000);
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
                            RegistroTestesPositivos.Registrar("CTP-04 Deve Alterar Status da Operação para Pago pelo Banco Cobrador", textoEncontrado);



                        }
                        catch
                        {
                            //throw new Exception("Nâo foi possivel encontrar o Texto comprovando alteração de status da operação para Pago pelo Banco Cobrador.");
                            Console.WriteLine("Texto Pago pelo Banco Cobrador NÃO encontrado na tabela.");
                            RegistroTestesPositivos.Registrar("CTP-04 Deve Alterar Status da Operação para Pago pelo Banco Cobrador", false);
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

                        #region Testes Negativos

                        #region CTN-01
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado1 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCSVPdfNegativo(Page, "CTN-01 teste.pdf");
                            resultadosTestesNegativos.Add(resultado1);
                            RegistroTestesNegativos.Registrar("CTN-01 Arquivo PDF inválido", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-01 Arquivo PDF inválido", false); }
                        #endregion

                        #region CTN-02
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado2 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjOriginadorEmBranco - Copia.csv", "CTN-02 CnpjOriginadorEmBranco");
                            resultadosTestesNegativos.Add(resultado2);
                            RegistroTestesNegativos.Registrar("CTN-02 CnpjOriginadorEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-02 CnpjOriginadorEmBranco", false); }
                        #endregion

                        #region CTN-03
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado3 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjOriginadorInvalido13Char.csv", "CTN-03 CnpjOriginadorInvalido13Char");
                            resultadosTestesNegativos.Add(resultado3);
                            RegistroTestesNegativos.Registrar("CTN-03 CnpjOriginadorInvalido13Char", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-03 CnpjOriginadorInvalido13Char", false); }
                        #endregion

                        #region CTN-04
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado4 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjOriginadorInvalido15Char.csv", "CTN-04 CnpjOriginadorInvalido15Char");
                            resultadosTestesNegativos.Add(resultado4);
                            RegistroTestesNegativos.Registrar("CTN-04 CnpjOriginadorInvalido15Char", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-04 CnpjOriginadorInvalido15Char", false); }
                        #endregion

                        #region CTN-05
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado5 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNomeCedenteEmBranco.csv", "CTN-05 NomeCedenteEmBranco");
                            resultadosTestesNegativos.Add(resultado5);
                            RegistroTestesNegativos.Registrar("CTN-05 NomeCedenteEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-05 NomeCedenteEmBranco", false); }
                        #endregion

                        #region CTN-06
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado6 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNomeCedenteInexistente.csv", "CTN-06 NomeCedenteInexistente");
                            resultadosTestesNegativos.Add(resultado6);
                            RegistroTestesNegativos.Registrar("CTN-06 NomeCedenteInexistente", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-06 NomeCedenteInexistente", false); }
                        #endregion

                        #region CTN-07
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado7 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNomeCedenteInvalido.csv", "CTN-07 NomeCedenteInvalido");
                            resultadosTestesNegativos.Add(resultado7);
                            RegistroTestesNegativos.Registrar("CTN-07 NomeCedenteInvalido", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-07 NomeCedenteInvalido", false); }
                        #endregion

                        #region CTN-08
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado8 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjCedenteEmBranco.csv", "CTN-08 CnpjCedenteEmBranco");
                            resultadosTestesNegativos.Add(resultado8);
                            RegistroTestesNegativos.Registrar("CTN-08 CnpjCedenteEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-08 CnpjCedenteEmBranco", false); }
                        #endregion

                        #region CTN-09
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado9 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjCedenteInvalido13Char.csv", "CTN-09 CnpjCedenteInvalido13Char");
                            resultadosTestesNegativos.Add(resultado9);
                            RegistroTestesNegativos.Registrar("CTN-09 CnpjCedenteInvalido13Char", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-09 CnpjCedenteInvalido13Char", false); }
                        #endregion

                        #region CTN-10
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado10 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjCedenteInvalido15Char.csv", "CTN-10 CnpjCedenteInvalido15Char");
                            resultadosTestesNegativos.Add(resultado10);
                            RegistroTestesNegativos.Registrar("CTN-10 CnpjCedenteInvalido15Char", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-10 CnpjCedenteInvalido15Char", false); }
                        #endregion

                        #region CTN-11
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado11 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNomeSacadoEmBranco.csv", "CTN-11 NomeSacadoEmBranco");
                            resultadosTestesNegativos.Add(resultado11);
                            RegistroTestesNegativos.Registrar("CTN-11 NomeSacadoEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-11 NomeSacadoEmBranco", false); }
                        #endregion

                        #region CTN-12
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado12 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNomeSacadoInexistente.csv", "CTN-12 NomeSacadoInexistente");
                            resultadosTestesNegativos.Add(resultado12);
                            RegistroTestesNegativos.Registrar("CTN-12 NomeSacadoInexistente", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-12 NomeSacadoInexistente", false); }
                        #endregion

                        #region CTN-13
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado13 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNomeSacadoInvalido.csv", "CTN-13 NomeSacadoInvalido");
                            resultadosTestesNegativos.Add(resultado13);
                            RegistroTestesNegativos.Registrar("CTN-13 NomeSacadoInvalido", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-13 NomeSacadoInvalido", false); }
                        #endregion

                        #region CTN-14
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado14 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjSacadoEmBranco.csv", "CTN-14 CnpjSacadoEmBranco");
                            resultadosTestesNegativos.Add(resultado14);
                            RegistroTestesNegativos.Registrar("CTN-14 CnpjSacadoEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-14 CnpjSacadoEmBranco", false); }
                        #endregion

                        #region CTN-15
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado15 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjSacadoInvalido13Char.csv", "CTN-15 CnpjSacadoInvalido13Char");
                            resultadosTestesNegativos.Add(resultado15);
                            RegistroTestesNegativos.Registrar("CTN-15 CnpjSacadoInvalido13Char", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-15 CnpjSacadoInvalido13Char", false); }
                        #endregion

                        #region CTN-16
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado16 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoCnpjSacadoInvalido15Char.csv", "CTN-16 CnpjSacadoInvalido15Char");
                            resultadosTestesNegativos.Add(resultado16);
                            RegistroTestesNegativos.Registrar("CTN-16 CnpjSacadoInvalido15Char", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-16 CnpjSacadoInvalido15Char", false); }
                        #endregion

                        #region CTN-17
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado17 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataVencFormatoInv.csv", "CTN-17 DataVencFormatoInv");
                            resultadosTestesNegativos.Add(resultado17);
                            RegistroTestesNegativos.Registrar("CTN-17 DataVencFormatoInv", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-17 DataVencFormatoInv", false); }
                        #endregion

                        #region CTN-18
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado18 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataVencEmBranco.csv", "CTN-18 DataVencEmBranco");
                            resultadosTestesNegativos.Add(resultado18);
                            RegistroTestesNegativos.Registrar("CTN-18 DataVencEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-18 DataVencEmBranco", false); }
                        #endregion

                        #region CTN-19
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado19 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataVencPassado.csv", "CTN-19 DataVencPassado");
                            resultadosTestesNegativos.Add(resultado19);
                            RegistroTestesNegativos.Registrar("CTN-19 DataVencPassado", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-19 DataVencPassado", false); }
                        #endregion

                        #region CTN-20
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado20 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataEmisEmBranco.csv", "CTN-20 DataEmisEmBranco");
                            resultadosTestesNegativos.Add(resultado20);
                            RegistroTestesNegativos.Registrar("CTN-20 DataEmisEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-20 DataEmisEmBranco", false); }
                        #endregion

                        #region CTN-21
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado21 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataEmissPassado.csv", "CTN-21 DataEmissPassado");
                            resultadosTestesNegativos.Add(resultado21);
                            RegistroTestesNegativos.Registrar("CTN-21 DataEmissPassado", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-21 DataEmissPassado", false); }
                        #endregion

                        #region CTN-22
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado22 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataAqFormatoInv.csv", "CTN-22 DataAqFormatoInv");
                            resultadosTestesNegativos.Add(resultado22);
                            RegistroTestesNegativos.Registrar("CTN-22 DataAqFormatoInv", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-22 DataAqFormatoInv", false); }
                        #endregion

                        #region CTN-23
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado23 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataAqEmBranco.csv", "CTN-23 DataAqEmBranco");
                            resultadosTestesNegativos.Add(resultado23);
                            RegistroTestesNegativos.Registrar("CTN-23 DataAqEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-23 DataAqEmBranco", false); }
                        #endregion

                        #region CTN-24
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado24 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoDataAqPassado.csv", "CTN-24 DataAqPassado");
                            resultadosTestesNegativos.Add(resultado24);
                            RegistroTestesNegativos.Registrar("CTN-24 DataAqPassado", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-24 DataAqPassado", false); }
                        #endregion

                        #region CTN-25
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado25 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNuDocEmBranco.csv", "CTN-25 NuDocEmBranco");
                            resultadosTestesNegativos.Add(resultado25);
                            RegistroTestesNegativos.Registrar("CTN-25 NuDocEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-25 NuDocEmBranco", false); }
                        #endregion

                        #region CTN-26
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado26 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNuDocInexistente.csv", "CTN-26 NuDocInexistente");
                            resultadosTestesNegativos.Add(resultado26);
                            RegistroTestesNegativos.Registrar("CTN-26 NuDocInexistente", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-26 NuDocInexistente", false); }
                        #endregion

                        #region CTN-27
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado27 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoNuDocInvalido.csv", "CTN-27 NuDocInvalido");
                            resultadosTestesNegativos.Add(resultado27);
                            RegistroTestesNegativos.Registrar("CTN-27 NuDocInvalido", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-27 NuDocInvalido", false); }
                        #endregion

                        #region CTN-28
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado28 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoSeuNumeroEmBranco.csv", "CTN-28 SeuNumeroEmBranco");
                            resultadosTestesNegativos.Add(resultado28);
                            RegistroTestesNegativos.Registrar("CTN-28 SeuNumeroEmBranco", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-28 SeuNumeroEmBranco", false); }
                        #endregion

                        #region CTN-29
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado29 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoSeuNumeroInexistente.csv", "CTN-29 SeuNumeroInexistente");
                            resultadosTestesNegativos.Add(resultado29);
                            RegistroTestesNegativos.Registrar("CTN-29 SeuNumeroInexistente", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-29 SeuNumeroInexistente", false); }
                        #endregion

                        #region CTN-30
                        await Page.ReloadAsync();
                        try
                        {
                            var resultado30 = await TesteOperacoesOperacoes.Util.EnviarArquivoOperacaoNegativo.EnviarArquivoCsvNegativo(Page, "TesteNegativoSeuNumeroInvalido.csv", "CTN-30 SeuNumeroInvalido");
                            resultadosTestesNegativos.Add(resultado30);
                            RegistroTestesNegativos.Registrar("CTN-30 SeuNumeroInvalido", true);
                        }
                        catch { RegistroTestesNegativos.Registrar("CTN-30 SeuNumeroInvalido", false); }
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