using Microsoft.Playwright;
using PortalIDSFTestes.elementos.cedentes;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.cedentes
{
    public class CedentesPage
    {
        private IPage page;
        Metodos metodo;
        CedentesElements el = new CedentesElements();
        string caminhoArquivo = @"C:\TempQA\Arquivos\36614123000160_49624866830_N.zip";
        string caminhoCedenteNegativo = @"C:\TempQA\Arquivos\CedentesNegativos\";

        public CedentesPage(IPage page) 
        {
            this.page = page;
            metodo = new Metodos(page);
        }   


        public async Task ValidarAcentosCedentesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Cedentes");
        }

        public async Task DownloadExcel()
        {
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnDownloadExcel, "Clicar no botão para baixar Excel");
            });
            await metodo.ValidarDownloadAsync(download, "Download Arquivo Excel", "Validar Download de Excel na Página de cedentes");

        }


        public async Task CadastrarCedente()
        {
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivo(el.InputNovoCedente, caminhoArquivo,"Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem Ação realizada com sucesso presente na tela");
        }

        public async Task CadastrarCedenteNegativo(string nomeArquivoNegativo)
        {
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivo(el.InputNovoCedente, caminhoCedenteNegativo + nomeArquivoNegativo, "Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgErroRetornada, "Validar mensagem Erro ao cadastrar Cedente presente na tela");
        }

        public async Task ExcluirCedente()
        {
            await metodo.Clicar(el.BarraPesquisaCedentes,"Clicar na Barra de pesquisa para inserir CPF cedente a ser excluído");
            await metodo.Escrever(el.BarraPesquisaCedentes, "496.248.668-30", "Escrever CPF do cedente a ser excluido");
            await Task.Delay(500);
            await metodo.Clicar(el.BtnLixeiraCedentes, "Clicar na lixeira para excluir cedente selecionado");
            await metodo.Clicar(el.CampoObservacaoExcluir, "Clicar na lixeira para excluir cedente selecionado");
            await metodo.Escrever(el.CampoObservacaoExcluir,"Teste Excluir Cedente", "Escrever Observação no modal excluir cedente");
            await metodo.Clicar(el.BtnConfirmarExcluir, "Clicar na Botão para confirmar exclusão cedente selecionado");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem presente na tela");
        }

        public async Task ConsultarCedente()
        {
            await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar na Barra de pesquisa para inserir CPF cedente a ser excluído");
            await metodo.Escrever(el.BarraPesquisaCedentes, "496.248.668-30", "Escrever CPF do cedente a ser excluido");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentes, "496.248.668-30","Pesquisar CPF Cedente presente na tabela");
        }


    }
}
