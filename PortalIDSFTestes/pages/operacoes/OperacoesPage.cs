using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using PortalIDSFTestes.data.operacoes;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.operacoes
{
    public class OperacoesPage
    {
        private IPage page;
        Utils metodo;
        OperacoesElements el = new OperacoesElements();
        OperacoesData data = new OperacoesData();

        static string NomeNovoArquivo { get; set; }
        public OperacoesPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task ValidarAcentosOperacoesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Operações");
        }

        public static string GetPath()
        {
            var envPath = Environment.GetEnvironmentVariable("PORTAL_PATH");
            ConfigurationManager config = new ConfigurationManager();
            config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
            string path = config["Paths:Arquivo"].ToString() ?? envPath;
            return path;
        }

        public async Task EnviarOperacaoCNAB()
        {
            var amanha = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");

            await metodo.Clicar(el.BtnNovaOperacaoCNAB, "Clicar no botão para enviar uma Nova Operação CNAB");
            await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            NomeNovoArquivo = await metodo.AtualizarDataEEnviarArquivo(page, GetPath() + "CNABz - Copia.txt", "Enviar Arquivo CNAB para teste positivo");
            await metodo.ValidarTextoPresente("Arquivo processado com sucesso!", "Esperar Arquivo Ser Processado para seguir o fluxo");
        }

        public async Task ConsultarOperacaoNaTabela()
        {
            await metodo.Clicar(el.CampoPesquisaTabela, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.CampoPesquisaTabela, NomeNovoArquivo, "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaOperacoes, NomeNovoArquivo, "Verificar se CNAB está presente na Tabela ");
        }
        public async Task ExcluirOperacao()
        {
            await ConsultarOperacaoNaTabela();
            await metodo.Clicar(el.BtnLixeira(NomeNovoArquivo), "Clicar na Lixeira da tabela");
            await metodo.Escrever(el.CampoMotivoExcluirArquivo, data.MotivoExclusaoArquivo, "Escrever motivo da exclusão do arquivo");
            await metodo.Clicar(el.BtnConfirmarExclusao, "Clicar Botão para confirmar excluisão do arquivo");
            await metodo.ValidarTextoPresente("Arquivo excluído com sucesso!", "Validar se mensagem de arquivo excluido com sucesso está visivel!");
        }

        public async Task EnviarOperacaoCNABNegativo(string nomeCnabNegativo)
        {
            await metodo.Clicar(el.BtnNovaOperacaoCNAB, "Clicar no botão para enviar uma Nova Operação CNAB");
            await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            await metodo.EnviarArquivo(el.EnviarOperacaoInput, GetPath() + nomeCnabNegativo, "Enviar Arquivo CNAB negativo");
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
                await metodo.EnviarArquivo(el.EnviarOperacaoInput, GetPath() + nomeCnabNegativo, "Enviar Arquivo CNAB negativo");
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
            string caminhoArquivoCSVparaModificar = GetPath();
            //arquivoteste_operacoescsv_qa.csv
            await metodo.Clicar(el.BtnNovaOperacaoCSV, "Clicar no botão para enviar uma Nova Operação CSV");
            await metodo.ClicarNoSeletor(el.SelectFundoCSV, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            var arquivoComNomeModificado = metodo.ModificarCsv(GetPath() + "TesteOperacaoCSV.csv", caminhoArquivoCSVparaModificar);
            await metodo.EnviarArquivo(el.EnviarOperacaoInputCSV, arquivoComNomeModificado, "Enviar Arquivo CSV no Input");
            var caminhoLastro = GetPath() + "Arquivo teste.zip";
            await metodo.EnviarArquivo(el.InputEnviarLastro, caminhoLastro, "Enviar Lastro no Input");
            await metodo.Escrever(el.CampoObservacao, data.ObservacaoCSV, "Escrever no campo observação ao enviar arquivo CSV");
            await metodo.Clicar(el.BtnEnviarOperacaoCSV, "Clicar no botão para Confirmar Envio uma Nova Operação CSV");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar Mensagem de Sucesso retornada");

        }

        public async Task EnviarOperacaoCSVNegativo(string nomeCsvNegativo)
        {
            await metodo.Clicar(el.BtnNovaOperacaoCSV, "Clicar no botão para enviar uma Nova Operação CSV");
            await metodo.ClicarNoSeletor(el.SelectFundoCSV, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            await metodo.EnviarArquivo(el.EnviarOperacaoInputCSV, GetPath() + nomeCsvNegativo, "Enviar Arquivo CSV no Input");
            var caminhoLastro = GetPath() + "Arquivo teste.zip";
            await metodo.EnviarArquivo(el.InputEnviarLastro, caminhoLastro, "Enviar Lastro no Input");
            await metodo.Escrever(el.CampoObservacao, data.ObservacaoCSVNegativo, "Escrever no campo observação ao enviar arquivo CSV");
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

                await metodo.EnviarArquivo(el.EnviarOperacaoInputCSV, GetPath() + nomeCsv, "Enviar Arquivo CSV no Input");

                var caminhoLastro = GetPath() + "Arquivo teste.zip";
                await metodo.EnviarArquivo(el.InputEnviarLastro, caminhoLastro, "Enviar Lastro no Input");

                await metodo.Escrever(el.CampoObservacao, data.ObservacaoCSVNegativo, "Escrever no campo observação ao enviar arquivo CSV");
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
            await metodo.Escrever(el.BarraPesquisaHistorico, NomeNovoArquivo, "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaHistoricoImportacoes, NomeNovoArquivo, "Verificar se CNAB está presente no histórico de importações ");
        }

        public async Task DownloadValidacaoMovimento_Layout()
        {
            await metodo.Clicar(el.BtnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.BarraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.BarraPesquisaHistorico, data.ExtensaoArquivoPesquisa, "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(15000);
            await metodo.Clicar(el.PrimeiroTdHistorico, "Clicar no primeiro TD que estiver presente para baixar arquivo validação movimento");
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadValidacaoMovimento, "Download Validação Movimento");
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadValidacaoLayout, "Download Validação Layout Alternativo");
        }


        public async Task DownloadExcel()
        {
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadExcel, "Validar Download do Excel na página de Operações");
        }








    }
}
