using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.operacoes
{
    public class OperacoesPage
    {
        private IPage page;
        Metodos metodo;
        OperacoesElements el = new OperacoesElements();

        string caminhoArquivoCNAB = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
        string caminhoArquivoCSV = @"C:\TempQA\Arquivos\arquivoteste_operacoescsv_qa.csv";
        string caminhoArquivoCnabNegativo = @"C:\TempQA\Arquivos\";
        string caminhoArquivoCSVNegativo = @"C:\TempQA\Arquivos\";
        string nomeNovoArquivo { get; set; }
        public OperacoesPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosOperacoesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Operações");
        }

        public async Task EnviarOperacaoCNAB()
        {
            await metodo.Clicar(el.BtnNovaOperacaoCNAB, "Clicar no botão para enviar uma Nova Operação CNAB");
            await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            nomeNovoArquivo = await metodo.AtualizarDataEEnviarArquivo(page, caminhoArquivoCNAB, "Enviar Arquivo CNAB para teste positivo");
            await metodo.ValidarMensagemPorTextoAsync(el.MsgSucessoRetornada, "Arquivo processado com sucesso", "Validar Mensagem de Sucesso retornada");
            await metodo.EsperarTextoPresente("Arquivo processado com sucesso!","Esperar Arquivo Ser Processado para seguir o fluxo");
            //Consultar
            await Task.Delay(7000);
            await page.ReloadAsync();
            await metodo.Clicar(el.CampoPesquisaTabela, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.CampoPesquisaTabela, nomeNovoArquivo, "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await metodo.Clicar(el.PrimeiroTdTabela, "Clicar no primeiro TD da tabela");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaOperacoes, nomeNovoArquivo, "Verificar se CNAB está presente na Tabela ");
            //Excluir
            await metodo.Clicar(el.BtnLixeira, "Clicar na Lixeira da tabela");
            await metodo.Escrever(el.CampoMotivoExcluirArquivo, "Teste Exclusão", "Escrever motivo da exclusão do arquivo");
            await metodo.Clicar(el.BtnConfirmarExclusao, "Clicar Botão para confirmar excluisão do arquivo");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar se mensagem de arquivo excluido com sucesso está visivel!");




        }
        public async Task EnviarOperacaoCNABNegativo(string nomeCnabNegativo)
        {
            await metodo.Clicar(el.BtnNovaOperacaoCNAB, "Clicar no botão para enviar uma Nova Operação CNAB");
            await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            await metodo.EnviarArquivo(el.EnviarOperacaoInput, caminhoArquivoCnabNegativo + nomeCnabNegativo, "Enviar Arquivo CNAB negativo");
            //await metodo.AtualizarDataEEnviarArquivo(page, caminhoArquivoCnabNegativo + nomeCnabNegativo,"Enviar Arquivo CNAB para teste negativo");
            await metodo.Clicar(el.BtnFecharModalOperacaoCnab, "Clicar no 'X' para fechar modal de nova operacao cnab");
            await metodo.Clicar(el.BarraPesquisaTabela, "Clicar na barra de pesquisa");
            await metodo.Escrever(el.BarraPesquisaTabela, nomeCnabNegativo, "Escrever na barra de pesquisa");
            await metodo.VerificarTextoAusenteNaTabela(page, el.TabelaOperacoes, nomeCnabNegativo, "Validar que o Arquivo " + nomeCnabNegativo + " não foi aceito na Tabela");
        }

        public async Task EnviarOperacoesCNABNegativo(params string[] nomesCnabNegativo)
        {
            foreach (var nomeCnabNegativo in nomesCnabNegativo)
            {
                await metodo.Clicar(el.BtnNovaOperacaoCNAB, "Clicar no botão para enviar uma Nova Operação CNAB");
                await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
                await metodo.EnviarArquivo(el.EnviarOperacaoInput, caminhoArquivoCnabNegativo + nomeCnabNegativo, "Enviar Arquivo CNAB negativo");
                //await metodo.AtualizarDataEEnviarArquivo(page, caminhoArquivoCnabNegativo + nomeCnabNegativo,"Enviar Arquivo CNAB para teste negativo");
                await metodo.Clicar(el.BtnFecharModalOperacaoCnab, "Clicar no 'X' para fechar modal de nova operacao cnab");

                await metodo.Clicar(el.BarraPesquisaTabela, "Clicar na barra de pesquisa");

                // opcional: limpe a busca, caso o app preserve o texto após reloads
                // await metodo.Limpar(el.TabelaOperacoes);

                await metodo.Escrever(el.BarraPesquisaTabela, nomeCnabNegativo, "Digitar o nome do arquivo na pesquisa");
                await metodo.VerificarTextoAusenteNaTabela(page, el.TabelaOperacoes, nomeCnabNegativo, "Validar que o Arquivo " + nomeCnabNegativo + " não foi aceito na Tabela");

                // volta ao início para o próximo arquivo
                await page.ReloadAsync(new() { WaitUntil = WaitUntilState.NetworkIdle });
                await page.WaitForSelectorAsync(el.BtnNovaOperacaoCNAB, new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
            }
        }



        public async Task EnviarOperacaoCSV()
        {
            string caminhoArquivoCSVparaModificar = @"C:\TempQA\Arquivos";

            await metodo.Clicar(el.BtnNovaOperacaoCSV, "Clicar no botão para enviar uma Nova Operação CSV");
            await metodo.ClicarNoSeletor(el.SelectFundoCSV, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            var arquivoComNomeModificado = metodo.ModificarCsv(caminhoArquivoCSV, caminhoArquivoCSVparaModificar);
            await metodo.EnviarArquivo(el.EnviarOperacaoInputCSV, arquivoComNomeModificado, "Enviar Arquivo CSV no Input");
            var caminhoLastro = @"C:\TempQA\Arquivos\Arquivo teste.zip";
            await metodo.EnviarArquivo(el.InputEnviarLastro, caminhoLastro, "Enviar Lastro no Input");
            await metodo.Escrever(el.CampoObservacao, "Teste de Arquivo CSV", "Escrever no campo observação ao enviar arquivo CSV");
            await metodo.Clicar(el.BtnEnviarOperacaoCSV, "Clicar no botão para Confirmar Envio uma Nova Operação CSV");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar Mensagem de Sucesso retornada");

        }

        public async Task EnviarOperacaoCSVNegativo(string nomeCsvNegativo)
        {
            await metodo.Clicar(el.BtnNovaOperacaoCSV, "Clicar no botão para enviar uma Nova Operação CSV");
            await metodo.ClicarNoSeletor(el.SelectFundoCSV, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            await metodo.EnviarArquivo(el.EnviarOperacaoInputCSV, caminhoArquivoCSVNegativo + nomeCsvNegativo, "Enviar Arquivo CSV no Input");
            var caminhoLastro = @"C:\TempQA\Arquivos\Arquivo teste.zip";
            await metodo.EnviarArquivo(el.InputEnviarLastro, caminhoLastro, "Enviar Lastro no Input");
            await metodo.Escrever(el.CampoObservacao, "Teste de Arquivo CSV Negativo", "Escrever no campo observação ao enviar arquivo CSV");
            await metodo.Clicar(el.BtnEnviarOperacaoCSV, "Clicar no botão para Confirmar Envio uma Nova Operação CSV");
            await metodo.Clicar(el.BarraPesquisaTabela, "Clicar na barra de pesquisa");
            await metodo.Escrever(el.BarraPesquisaTabela, nomeCsvNegativo, "Digitar o nome do arquivo na pesquisa");
            await metodo.VerificarTextoAusenteNaTabela(page, el.TabelaOperacoes, nomeCsvNegativo, "Validar que o Arquivo " + nomeCsvNegativo + " não foi aceito na Tabela");
        }

        public async Task EnviarOperacoesCSVNegativo(params string[] nomesCsvNegativos)
        {
            foreach (var nomeCsv in nomesCsvNegativos)
            {
                await metodo.Clicar(el.BtnNovaOperacaoCSV, "Clicar no botão para enviar uma Nova Operação CSV");
                await metodo.ClicarNoSeletor(el.SelectFundoCSV, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");

                await metodo.EnviarArquivo(el.EnviarOperacaoInputCSV, caminhoArquivoCSVNegativo + nomeCsv, "Enviar Arquivo CSV no Input");

                var caminhoLastro = @"C:\TempQA\Arquivos\Arquivo teste.zip";
                await metodo.EnviarArquivo(el.InputEnviarLastro, caminhoLastro, "Enviar Lastro no Input");

                await metodo.Escrever(el.CampoObservacao, "Teste de Arquivo CSV Negativo", "Escrever no campo observação ao enviar arquivo CSV");
                await metodo.Clicar(el.BtnEnviarOperacaoCSV, "Clicar no botão para Confirmar Envio uma Nova Operação CSV");
                await metodo.Clicar(el.BtnFecharModalOperacaoCsv, "Fechar modal csv");


                // Pesquisa e valida ausência
                await metodo.Clicar(el.BarraPesquisaTabela, "Clicar na barra de pesquisa");
                // (opcional) limpa caso tenha texto anterior
                await page.FillAsync(el.BarraPesquisaTabela, "");
                await metodo.Escrever(el.BarraPesquisaTabela, nomeCsv, "Digitar o nome do arquivo na pesquisa");

                await metodo.VerificarTextoAusenteNaTabela(
                    page,
                    el.TabelaOperacoes,
                    nomeCsv,
                    "Validar que o Arquivo " + nomeCsv + " não foi aceito na Tabela"
                );

                // Volta ao início para o próximo arquivo
                await page.ReloadAsync(new() { WaitUntil = WaitUntilState.NetworkIdle });
                await page.WaitForSelectorAsync(el.BtnNovaOperacaoCSV, new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
            }
        }



        public async Task ConsultarCNABPeloHistoricoImportacoes()
        {
            await metodo.Clicar(el.BtnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.BarraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.BarraPesquisaHistorico, nomeNovoArquivo, "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaHistoricoImportacoes, nomeNovoArquivo, "Verificar se CNAB está presente no histórico de importações ");
        }

        public async Task DownloadValidacaoMovimento_Layout()
        {
            await metodo.Clicar(el.BtnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.BarraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.BarraPesquisaHistorico, ".rem", "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            await metodo.Clicar(el.PrimeiroTdHistorico, "Clicar no primeiro TD que estiver presente para baixar arquivo validação movimento");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnDownloadValidacaoMovimento, "Click para baixar relatorio movimento atraves do histórico importações");
            });
            await metodo.ValidarDownloadAsync(download, "Download Validação Movimento", "Validar Download de arquivo validação movimento");
            var download1 = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnDownloadValidacaoLayout, "Clicar no botão para baixar validação Layout");
            });
            await metodo.ValidarDownloadAsync(download1, "Download Validação Layout", "Validar Download de arquivo validação Layout");
        }


        public async Task DownloadExcel()
        {
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnDownloadExcel, "Clicar no botão para baixar Excel");
            });
            await metodo.ValidarDownloadAsync(download, "Download Validação Layout", "Validar Download de Excel");

        }








    }
}
