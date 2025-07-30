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

        string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";



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
            await metodo.Clicar(el.btnNovaOperacaoCNAB,"Clicar no botão para enviar uma Nova Operação CNAB");
            await metodo.ClicarNoSeletorFundo(el.selectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            var enviarArquivo = await metodo.AtualizarDataEEnviarArquivo(page, caminhoArquivo);
            await metodo.ValidarMsgRetornada(el.msgSucessoRetornada, "Validar Mensagem de Sucesso retornada");
            
        }

        public async Task ConsultarCNABPeloHistoricoImportacoes()
        {
            await metodo.Clicar(el.btnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.barraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.barraPesquisaHistorico,"Copia.txt", "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            await metodo.VerificarElementoPresenteNaTabela(page, el.tabelaHistoricoImportacoes,"Copia.txt", "Verificar se CNAB está presente no histórico de importações ");
        }
        public async Task DownloadValidacaoMovimento()
        {
            await metodo.Clicar(el.btnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.barraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.barraPesquisaHistorico,"Copia.txt", "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.btnDownloadValidacaoMovimento, "Click para baixar relatorio movimento atraves do histórico importações");
            });
            await metodo.ValidarDownloadAsync(download,"Download Validação Movimento", "Validar Download de arquivo validação movimento");
        }
        public async Task DownloadAvalidacaoLayout()
        {
            await metodo.Clicar(el.btnHistoricoImportacoes, "Clicar no botão para abrir modal de histórico de importações");
            await metodo.Clicar(el.barraPesquisaHistorico, "Clicar no campo pesquisar para inserir nome do arquivo CNAB a ser consultado");
            await metodo.Escrever(el.barraPesquisaHistorico,"Copia.txt", "Digitar no campo pesquisar nome do arquivo CNAB a ser consultado");
            await Task.Delay(10000);
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.btnDownloadValidacaoLayout,"Clicar no botão para baixar validação Layout");
            });
            await metodo.ValidarDownloadAsync(download, "Download Validação Layout", "Validar Download de arquivo validação Layout");
        }

        public async Task DownloadExcel()
        {            
                await metodo.BaixarExcelPorIdAsync(page, "Baixar Excel na pagina de operações");            
        }
    }
}
