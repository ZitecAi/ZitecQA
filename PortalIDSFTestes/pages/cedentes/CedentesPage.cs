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
        string caminhoArquivo = @"C:\TempQA\Arquivos\";
        string caminhoCedenteNegativo = @"C:\TempQA\Arquivos\CedentesNegativos\";

        public string cnpjTest { get; }


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


        public async Task CadastrarCedente(string Cedente)
        {
            string cnpjTest = DataGenerator.Generate(DocumentType.Cnpj);
            string cnpjCedente = "21.465.218/0001-91";
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivoCedenteNovo(el.InputNovoCedente, GetPath() + "36614123000160_21465218000191_N.zip", GetPath() + "36614123000160_21465218000191_N.zip" + "\\Kit Cedente", cnpjTest,  "Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem Ação realizada com sucesso presente na tela");
            await page.ReloadAsync();
            //consultar
            await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar no input da Barra de pesquisa");
            await metodo.Escrever(el.BarraPesquisaCedentes, cnpjCedente, "Pesquisar nome do arquivo para validar cadastro");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentesCadastrado, cnpjCedente, "Validar Se o nome do arquivo esta presente na tabela");
            //aprovações
            await metodo.Clicar(el.BtnAprovarGestora(Cedente), "Clicar no botão para aprovar cadastro do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, "Aprovado", "Escrever observação para aprovação do cadastro");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await metodo.ValidarTextoPresente("Ação Status atualizado!", "Validar mensagem Ação realizada com sucesso presente na tela na aprovação cadastro");
            await Task.Delay(500);
            await metodo.Clicar(el.BtnAprovarCadastro(Cedente), "Clicar no botão para aprovar gestora do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, "Aprovado", "Escrever observação para aprovação da gestora");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await metodo.ValidarTextoPresente("Ação Status atualizado!", "Validar mensagem Ação realizada com sucesso presente na tela na aprovação gestora");
            await Task.Delay(500);
            await metodo.Clicar(el.BtnAprovarCompliance(Cedente), "Clicar no botão para aprovar compliance do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, "Aprovado", "Escrever observação para aprovação do compliance");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await Task.Delay(500);
            await metodo.ValidarTextoPresente("Ação Status atualizado!", "Validar mensagem Ação realizada com sucesso presente na tela na aprovação compliance");
            await Task.Delay(500);
            await metodo.ValidarTextoDoElemento(el.TdAprovados("6"),"Aprovado", "Validar se o status do cedente está como aprovado para cadastro");
            await metodo.ValidarTextoDoElemento(el.TdAprovados("7"),"Aprovado", "Validar se o status do cedente está como aprovado para gestora");
            await metodo.ValidarTextoDoElemento(el.TdAprovados("8"),"Aprovado", "Validar se o status do cedente está como aprovado para compliance");
            //ativar cedente
            await metodo.Clicar(el.BtnContratoMae(cnpjCedente), "Clicar no botão para enviar contrato mãe .pdf");
            await metodo.EnviarArquivo(el.InputContratoMae, caminhoArquivo + "Arquivo teste.zip", "Enviar contrato mae no input");
            await metodo.Escrever(el.ObsAtivarContratoMae, "Teste ativação cedente", "Escrever observação no campo de ativação do modal de contrato mae");
            await metodo.Clicar(el.ButtonAtivacao, "Clicar no botão para ativar cedente");
            await metodo.ValidarTextoPresente("Contrato recebido com sucesso, em breve ele será ativado", "Validar mensagem Contrato recebido com sucesso presente na tela");


            //await metodo.Clicar(el.BtnReprovarCedente(cnpjCedente), $"Reprovar cedente do cnpj {cnpjCedente}");
            //await metodo.Escrever(el.ObsReprovar, "Aprovado", "Escrever observação para aprovação do compliance");
            //await metodo.Clicar(el.ButtonReprovar, "Clicar no botão para reprovar cedente");
            //await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar no input da Barra de pesquisa");
            //await metodo.Escrever(el.BarraPesquisaCedentes, cnpjCedente, "Pesquisar nome do arquivo para validar cadastro");
            //await metodo.ValidarTextoDoElemento(el.TdReprovado, "Reprovado", "Validar se o status do cedente está como aprovado para compliance");

            await Task.Delay(500);
            await metodo.Clicar(el.ButtonAtivarContratoMae(cnpjCedente), "Clicar no botão para visualizar contrato mãe .pdf");
            await metodo.Clicar(el.ButtonAprovado, "Clicar na opção aprovado");
            await metodo.Escrever(el.ObsContratoMae, "Aprovado", "Escrever observação para aprovação do contrato mãe");
            await metodo.Clicar(el.BtnAtivarCedente, "Clicar no botão para ativar cedente");
            await metodo.ValidarTextoDoElemento(el.TdAprovados("4"), "Ativo", "Validar se o status do cedente está como ativo");

            //reprovar
            //excluir
            //await Task.Delay(500);
            //await metodo.Clicar(el.BtnLixeiraCedentes, "Clicar na lixeira para excluir cedente selecionado");
            //await metodo.Clicar(el.CampoObservacaoExcluir, "Clicar na lixeira para excluir cedente selecionado");
            //await metodo.Escrever(el.CampoObservacaoExcluir, "Teste Excluir Cedente", "Escrever Observação no modal excluir cedente");
            //await metodo.Clicar(el.BtnConfirmarExcluir, "Clicar na Botão para confirmar exclusão cedente selecionado");
            //await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem presente na tela");

            //Fazer Fluxo de Exclusão de cedente após ativo, excluir do custódia e portal

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
            
        }

        public async Task ConsultarCedente()
        {
            await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar na Barra de pesquisa para inserir CPF cedente a ser excluído");
            await metodo.Escrever(el.BarraPesquisaCedentes, "FUNDO QA", "Escrever CPF do cedente a ser excluido");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentes, "FUNDO QA FIDC", "Pesquisar Fundo QA do Cedente presente na tabela");
        }


    }
}
