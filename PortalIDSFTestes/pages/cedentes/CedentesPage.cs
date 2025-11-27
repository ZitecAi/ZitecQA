using Allure.NUnit.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
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

        public string cnpjTest { get; }


        public CedentesPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
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
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadExcel, "Validar Download do Excel na página de cedentes");
        }

        [AllureStep("Cadastrar Cedente")]
        public async Task CadastrarCedente(string Cedente)
        {
            string cnpjTest = DataGenerator.Generate(DocumentType.Cnpj);
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivoCedenteNovo(el.InputNovoCedente, GetPath() + "36614123000160_21465218000191_N.zip", GetPath() + "36614123000160_21465218000191_N.zip" + "\\Kit Cedente", cnpjTest, "Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem Ação realizada com sucesso presente na tela");
            await page.ReloadAsync();
            //consultar
            await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar no input da Barra de pesquisa");
            await metodo.Escrever(el.BarraPesquisaCedentes, Cedente, "Pesquisar nome do arquivo para validar cadastro");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaCedentesCadastrado, Cedente, "Validar Se o nome do arquivo esta presente na tabela");
            await Task.Delay(1000);


        }
        [AllureStep("Aprovar Gestora do Cedente")]
        public async Task AprovarGestora(string Cedente)
        {
            await metodo.Clicar(el.BtnAprovarGestora(Cedente), "Clicar no botão para aprovar gestora do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, "Aprovado", "Escrever observação para aprovação do cadastro");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await metodo.ValidarTextoPresente("Status atualizado!", "Validar mensagem Ação realizada com sucesso presente na tela na aprovação cadastro");
            await metodo.Clicar(el.BtnFecharMensagemSucesso, "Clicar no botão para fechar a mensagem de sucesso após aprovar gestora");
            await Task.Delay(1000);
            await metodo.ValidarTextoDoElemento(el.TdAprovadoGestora("6"), "Aprovado", "Validar se o status do cedente está como aprovado para gestora");

        }
        [AllureStep("Aprovar Compliance do Cedente")]
        public async Task AprovarCompliance(string Cedente)
        {
            await metodo.Clicar(el.BtnAprovarCompliance(Cedente), "Clicar no botão para aprovar compliance do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, "Aprovado", "Escrever observação para aprovação do compliance");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await Task.Delay(1000);
            await metodo.ValidarTextoPresente("Status atualizado!", "Validar mensagem Ação realizada com sucesso presente na tela na aprovação compliance");
            await metodo.Clicar(el.BtnFecharMensagemSucesso, "Clicar no botão para fechar a mensagem de sucesso após aprovar Compliance");
            await metodo.ValidarTextoDoElemento(el.TdAprovados("8"), "Aprovado", "Validar se o status do cedente está como aprovado para compliance");

        }
        [AllureStep("Aprovar Cadastro do Cedente")]
        public async Task AprovarCadastro(string Cedente)
        {
            await metodo.Clicar(el.BtnAprovarCadastro(Cedente), "Clicar no botão para aprovar cadastro do cedente");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para selecionar aprovado");
            await metodo.Escrever(el.Obs, "Aprovado", "Escrever observação para aprovação da gestora");
            await metodo.Clicar(el.BtnEnviarParecerDepartamento, "Clicar no botão para enviar parecer do departamento");
            await metodo.ValidarTextoPresente("Status atualizado!", "Validar mensagem Ação realizada com sucesso presente na tela na aprovação gestora");
            await Task.Delay(1000);
            await metodo.ValidarTextoDoElemento(el.TdAprovados("7"), "Aprovado", "Validar se o status do cedente está como aprovado para cadastro");

        }
        [AllureStep("Enviar Contrato Mãe do Cedente")]
        public async Task EnviarContratoMae(string Cedente)
        {
            //ativar cedente
            await metodo.Clicar(el.BtnContratoMae(Cedente), "Clicar no botão para enviar contrato mãe .pdf");
            await metodo.Clicar(el.CheckBoxAssinante, "Clicar no Checkbox para selecionar assinante");
            await metodo.Clicar(el.BotaoEnviarAssinante, "Clicar no botão para enviar assinantes");
            await metodo.ValidarTextoPresente("Documento enviado para a assinatura digital.", "Validar mensagem Documento enviado para a assinatura digital. presente na tela");

        }
        //[AllureStep("Aprovar Contrato Mãe do Cedente")]
        //public async Task AprovarContratoMae(string Cedente)
        //{
        //    await Task.Delay(500);
        //    await metodo.Clicar(el.ButtonAtivarContratoMae(Cedente), "Clicar no botão para visualizar contrato mãe .pdf");
        //    await metodo.Clicar(el.ButtonAprovado, "Clicar na opção aprovado");
        //    await metodo.Escrever(el.ObsContratoMae, "Aprovado", "Escrever observação para aprovação do contrato mãe");
        //    await metodo.Clicar(el.BtnAtivarCedente, "Clicar no botão para ativar cedente");
        //    await Task.Delay(1500);
        //    await metodo.ValidarTextoDoElemento(el.StatusCedente, "Ativo", "Validar se o status do cedente está como ativo");
        //}

        public async Task CadastrarCedenteNegativo(string nomeArquivoNegativo)
        {
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivo(el.InputNovoCedente, GetPath() + "CedentesNegativos/" + nomeArquivoNegativo, "Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgErroRetornada, "Validar mensagem Erro ao cadastrar Cedente presente na tela");
        }

        public async Task ExcluirCedente()
        {
            await metodo.Clicar(el.BarraPesquisaCedentes, "Clicar na Barra de pesquisa para inserir CPF cedente a ser excluído");
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
