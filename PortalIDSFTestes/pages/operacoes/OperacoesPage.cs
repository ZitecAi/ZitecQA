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
            await metodo.Clicar(el.BtnNovaOperacaoCNAB,"Clicar no botão para enviar uma Nova Operação CNAB");
            await metodo.ClicarNoSeletorFundo(el.SelectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            var enviarArquivo = await metodo.AtualizarDataEEnviarArquivo(page, caminhoArquivoCNAB);
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar Mensagem de Sucesso retornada");
            
        }

        public async Task EnviarOperacaoCSV()
        {
            string caminhoArquivoCSVparaModificar = @"C:\TempQA\Arquivos";

            await metodo.Clicar(el.BtnNovaOperacaoCSV, "Clicar no botão para enviar uma Nova Operação CSV");
            await metodo.ClicarNoSeletorFundo(el.SelectFundoCSV, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            var arquivoComNomeModificado = metodo.ModificarCsv(caminhoArquivoCSV, caminhoArquivoCSVparaModificar);
            await metodo.EnviarArquivo(el.EnviarOperacaoInputCSV, arquivoComNomeModificado, "Enviar Arquivo CSV no Input");
            var caminhoLastro = @"C:\TempQA\Arquivos\Arquivo teste.zip";
            await metodo.EnviarArquivo(el.InputEnviarLastro, caminhoLastro, "Enviar Lastro no Input");
            await metodo.Escrever(el.CampoObservacao, "Teste de Arquivo CSV", "Escrever no campo observação ao enviar arquivo CSV");
            await metodo.Clicar(el.BtnEnviarOperacaoCSV, "Clicar no botão para Confirmar Envio uma Nova Operação CSV");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar Mensagem de Sucesso retornada");

        }

        public async Task ConsultarCNABPeloHistoricoImportacoes()
        {
            await metodo.Clicar(el.BtnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.BarraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.BarraPesquisaHistorico,".rem", "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            await metodo.VerificarTextoAusenteNaTabela(page, el.TabelaHistoricoImportacoes,"Copia.txt", "Verificar se CNAB está presente no histórico de importações ");
        }
        public async Task DownloadValidacaoMovimento_Layout()
        {
            await metodo.Clicar(el.BtnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.BarraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.BarraPesquisaHistorico,".rem", "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            await metodo.Clicar(el.PrimeiroTdHistorico, "Clicar no primeiro TD que estiver presente para baixar arquivo validação movimento");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnDownloadValidacaoMovimento, "Click para baixar relatorio movimento atraves do histórico importações");
            });
            await metodo.ValidarDownloadAsync(download,"Download Validação Movimento", "Validar Download de arquivo validação movimento");
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
