using Microsoft.Playwright;
using PortalIDSFTestes.data.cedentes;
using PortalIDSFTestes.elementos.cedentes;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.cedentes
{
    public class CedentesPage
    {
        private IPage page;
        Utils metodo;
        CedentesElements el = new CedentesElements();
        string caminhoArquivo = @"C:\TempQA\Arquivos\";
        string caminhoCedenteNegativo = @"C:\TempQA\Arquivos\CedentesNegativos\";
        CedentesData data = new CedentesData();

        public string cnpjTest { get; }


        public CedentesPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }


        public async Task ValidarAcentosCedentesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Cedentes");
        }

        public async Task DownloadExcel()
        {
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadExcel, "Validar Download do Excel na página de cedentes");
        }

        public async Task CadastrarCedente(string Cedente)
        {
            string cnpjTest = DataGenerator.Generate(DocumentType.Cnpj);
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await page.Locator(el.InputNovoCedente).SetInputFilesAsync(
            Utils.GetPath() + "54638076000176_52721175000191_N.zip",
            new LocatorSetInputFilesOptions { Timeout = 120_000 });
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem Ação realizada com sucesso presente na tela");

        }
        public async Task ConsultarCedente(string Cedente, Func<Task> custom = null)
        {
            if (custom != null)
            {
                await custom();
                return;
            }
            //consultar
            await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar no input da Barra de pesquisa");
            await metodo.Escrever(el.BarraPesquisaCedentes, Cedente, "Pesquisar nome do arquivo para validar cadastro");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentesCadastrado, Cedente, "Validar Se o nome do arquivo esta presente na tabela");

        }
        public async Task AprovarGestora(string Cedente)
        {
            await ConsultarCedente(Cedente);
            await metodo.Clicar(el.BtnAprovarGestora(Cedente), "Clicar no botão para aprovar gestora do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, data.StatusAprovado, "Escrever observação para aprovação do cadastro");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await metodo.ValidarTextoPresente(CedentesData.MsgStatusAtualizado, "Validar mensagem Ação realizada com sucesso presente na tela na aprovação cadastro");
            await metodo.Clicar(el.BtnFecharMensagemSucesso, "Clicar no botão para fechar a mensagem de sucesso após aprovar gestora");
            await Task.Delay(1000);
            await metodo.ValidarTextoDoElemento(el.TdAprovadoGestora("6"), data.StatusAprovado, "Validar se o status do cedente está como aprovado para gestora");

        }
        public async Task AprovarCompliance(string Cedente)
        {
            await ConsultarCedente(Cedente);
            await metodo.Clicar(el.BtnAprovarCompliance(Cedente), "Clicar no botão para aprovar compliance do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, data.StatusAprovado, "Escrever observação para aprovação do compliance");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await Task.Delay(1000);
            await metodo.ValidarTextoPresente(CedentesData.MsgStatusAtualizado, "Validar mensagem Ação realizada com sucesso presente na tela na aprovação compliance");
            await metodo.Clicar(el.BtnFecharMensagemSucesso, "Clicar no botão para fechar a mensagem de sucesso após aprovar Compliance");
            await metodo.ValidarTextoDoElemento(el.TdAprovados("8"), data.StatusAprovado, "Validar se o status do cedente está como aprovado para compliance");

        }
        public async Task AprovarCadastro(string Cedente)
        {
            await ConsultarCedente(Cedente);
            await metodo.Clicar(el.BtnAprovarCadastro(Cedente), "Clicar no botão para aprovar cadastro do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, data.StatusAprovado, "Escrever observação para aprovação da gestora");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await metodo.ValidarTextoPresente(CedentesData.MsgStatusAtualizado, "Validar mensagem Ação realizada com sucesso presente na tela na aprovação gestora");
            await Task.Delay(1000);
            await metodo.ValidarTextoDoElemento(el.TdAprovados("7"), data.StatusAprovado, "Validar se o status do cedente está como aprovado para cadastro");

        }
        public async Task EnviarContratoMae(string Cedente)
        {
            await ConsultarCedente(Cedente);
            await metodo.Clicar(el.BtnContratoMae(Cedente), "Clicar no botão para enviar contrato mãe .pdf");
            await metodo.Clicar(el.BtnEnviarParaAssinatura, "Clickar no checkbox para desabilitar zisign");
            await metodo.EnviarArquivo(el.InputContratoMae, caminhoArquivo + "Arquivo teste.zip", "Enviar contrato mae no input");
            await metodo.Escrever(el.ObsAtivarContratoMae, data.TextoAtivacaoCedente, "Escrever observação no campo de ativação do modal de contrato mae");
            await metodo.Clicar(el.ButtonAtivacao, "Clicar no botão para ativar cedente");
            await metodo.ValidarTextoPresente(CedentesData.MsgContratoRecebidoSucesso, "Validar mensagem Contrato recebido com sucesso presente na tela");
        }
        public async Task AprovarContratoMae(string Cedente)
        {
            await ConsultarCedente(Cedente);
            await metodo.Clicar(el.ButtonAtivarContratoMae(Cedente), "Clicar no botão para visualizar contrato mãe .pdf");
            await metodo.Clicar(el.ButtonAprovado, "Clicar na opção aprovado");
            await metodo.Escrever(el.ObsContratoMae, data.StatusAprovado, "Escrever observação para aprovação do contrato mãe");
            await metodo.Clicar(el.BtnAtivarCedente, "Clicar no botão para ativar cedente");
            await Task.Delay(1500);
            await metodo.ValidarTextoDoElemento(el.StatusCedente, "Ativo", "Validar se o status do cedente está como ativo");
        }
        public async Task ExcluirCedente(string Cedente)
        {
            await ConsultarCedente(Cedente);
            await metodo.Clicar(el.BtnLixeiraCedentes(Cedente), "Clicar no botão da lixeira para excluir cedente");
            await metodo.Clicar(el.BtnConfirmarExcluir, "Clicar no botão para confirmar exclusão do cedente");
            await metodo.ValidarTextoPresente(CedentesData.MsgCedenteExcluidoSucesso, "Validar mensagem Cedente excluído com sucesso presente na tela");
            //await ConsultarCedente(Cedente, async () =>
            //{
            //    await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar no input da Barra de pesquisa");
            //    await metodo.Escrever(el.BarraPesquisaCedentes, Cedente, "Pesquisar nome do arquivo para validar cadastro");
            //    await metodo.VerificarTextoAusenteNaTabela(page, el.TabelaCedentesCadastrado, Cedente, "Validar Se o nome do arquivo esta presente na tabela");
            //});
        }

        public async Task CadastrarCedenteNegativo(string nomeArquivoNegativo)
        {
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivo(el.InputNovoCedente, Utils.GetPath() + "CedentesNegativos/" + nomeArquivoNegativo, "Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgErroRetornada, "Validar mensagem Erro ao cadastrar Cedente presente na tela");
        }

        public async Task ConsultarCedente()
        {
            await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar na Barra de pesquisa para inserir CPF cedente a ser excluído");
            await metodo.Escrever(el.BarraPesquisaCedentes, data.TextoFundoQA, "Escrever CPF do cedente a ser excluido");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentes, "FUNDO QA FIDC", "Pesquisar Fundo QA do Cedente presente na tabela");
        }


    }
}