using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
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
        public static string GetPath()
        {
            ConfigurationManager config = new ConfigurationManager();
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            string path = config["Paths:Arquivo"].ToString();
            return path;
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


        public async Task CadastrarCedente(string nomeFundo)
        {
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivoCedenteNovo(el.InputNovoCedente, GetPath() + "36614123000160_49624866830_N.zip",GetPath() + "36614123000160_49624866830_N.zip" + "\\Kit Cedente",  "Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem Ação realizada com sucesso presente na tela")
                .ContinueWith(async t =>
                {
                    await page.ReloadAsync();
                    await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar no input da Barra de pesquisa");
                    await metodo.Escrever(el.BarraPesquisaCedentes, nomeFundo, "Pesquisar nome do arquivo para validar cadastro");
                    await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentesCadastrado, nomeFundo, "Validar Se o nome do arquivo esta presente na tabela");
                }).Unwrap();
            
        }

        public async Task CadastrarCedenteNegativo(string nomeArquivoNegativo)
        {
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivo(el.InputNovoCedente, GetPath()+ "CedentesNegativos/" + nomeArquivoNegativo, "Enviar arquivo no input para cadastrar novo cedente");
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
            await metodo.Escrever(el.BarraPesquisaCedentes, "FUNDO QA", "Escrever CPF do cedente a ser excluido");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentes, "FUNDO QA FIDC", "Pesquisar Fundo QA do Cedente presente na tabela");
        }


    }
}
