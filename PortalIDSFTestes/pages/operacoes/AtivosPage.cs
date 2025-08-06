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
    public class AtivosPage
    {
        private IPage page;
        Metodos metodo;
        AtivosElements el = new AtivosElements();
        string caminhoArquivo = @"C:\TempQA\Arquivos\";
        public AtivosPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosAtivosPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Ativos");
        }

        public async Task DownloadExcel()
        {
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnExcel, "Clicar no botão para baixar Excel");
            });
            await metodo.ValidarDownloadAsync(download, "Download Validação Layout", "Validar Download de Excel");

        }

        public async Task CadastrarAtivo()
        {
            await metodo.Clicar(el.BtnNovoAtivo,"Clicar no Botão Para Cadastrar Um Novo Ativo");
            await Task.Delay(500);
            await metodo.ClicarNoSeletor(el.SelectFundo, "53300608000106", "Selecionar Fundo");
            await metodo.Escrever(el.CampoNomeCedente, "Cedente Teste NUnit","Escrever Nome Cedente");
            await metodo.Escrever(el.CampoCpfPjCedente, "533.006.080-00106", "Escrever CPF/CNPJ Cedente");
            await metodo.ClicarNoSeletor(el.SelectTipoContrato, "AtivosImobiliarios", "Selecionar Tipo de Contrato");
            await metodo.Escrever(el.CampoAgencia, "0001", "Digitar Agencia");
            await metodo.Escrever(el.CampoConta, "460915", "Digitar Conta");
            await metodo.Escrever(el.CampoRazSocialDestinatario, "Teste NUnit", "Digitar Razão Social");
            await metodo.Escrever(el.CampoCpfPj, "49624866830", "Digitar CPF/CNPJ Ativo");
            await metodo.Escrever(el.CampoValor, "10", "Digitar Valor");
            await metodo.Escrever(el.CampoResumoOperacao, "Teste NUnit", "Digitar Resumo da operação");
            await metodo.Clicar(el.BtnAnexos, "Clicar no Botão Anexos");
            await metodo.EnviarArquivo(el.InputAnexos, caminhoArquivo + "21321321321.pdf", "Enviar Anexo no Input de anexos");
            await metodo.Clicar(el.BtnVoltar, "Clicar no botão voltar após enviar anexo");
            await metodo.Clicar(el.CheckBoxTermo, "Aceitar Termos");
            await metodo.Clicar(el.BtnSalvar, "Clicar em Salvar");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem de sucesso visivel na tela ");            
        }

        public async Task ConsultarAtivo()
        {
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa,"Teste NUnit", "Clicar na Barra de Pesquisa");
            await metodo.VerificarElementoPresenteNaTabela(page,el.TabelaAtivos, "Teste NUnit","Validar se ativo com destinatário Teste NUnit está presente na tabela");
        }




    }
}
