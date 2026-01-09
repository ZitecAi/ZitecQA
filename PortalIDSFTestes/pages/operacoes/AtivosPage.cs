using Allure.NUnit.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using PortalIDSFTestes.data.operacoes;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.operacoes
{
    public class AtivosPage
    {


        private IPage page;
        Utils metodo;
        AtivosElements el = new AtivosElements();
        AtivosData data = new AtivosData();
        public AtivosPage(IPage page, AtivosData data = null)
        {
            this.page = page;
            this.data = data ?? new AtivosData();
            metodo = new Utils(page);
        }
        public static string GetPath()
        {
            var envPath = Environment.GetEnvironmentVariable("PORTAL_PATH");
            ConfigurationManager config = new ConfigurationManager();
            config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            string path = config["Paths:Arquivo"].ToString() ?? envPath;
            return path;
        }

        public async Task ValidarAcentosAtivosPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Ativos");
        }

        public async Task DownloadExcel()
        {
            await metodo.ValidateDownloadAndLength(page, el.BtnExcel, "Validar Download do Excel na página de Ativos");
        }





        public async Task CadastrarAtivo()
        {
            await metodo.Clicar(el.BtnNovoAtivo, "Clicar no Botão Para Cadastrar Um Novo Ativo");
            await Task.Delay(500);
            await metodo.ClicarNoSeletor(el.SelectFundo, data.CnpjFundoZitec, "Selecionar Fundo");
            await metodo.Escrever(el.CampoNomeCedente, data.NomeCedenteTeste, "Escrever Nome Cedente");
            await metodo.Escrever(el.CampoCpfPjCedente, data.CnpjCedenteTeste, "Escrever CPF/CNPJ Cedente");
            await metodo.ClicarNoSeletor(el.SelectTipoContrato, "AtivosImobiliarios", "Selecionar Tipo de Contrato");
            await metodo.Escrever(el.CampoAgencia, data.AgenciaPadrao, "Digitar Agencia");
            await metodo.Escrever(el.CampoConta, data.ContaPadrao, "Digitar Conta");
            await metodo.Escrever(el.CampoRazSocialDestinatario, data.NomeAtivo, "Digitar Razão Social");
            await metodo.Escrever(el.CampoCpfPj, data.CpfAtivoTeste, "Digitar CPF/CNPJ Ativo");
            await metodo.Escrever(el.CampoValor, data.ValorAtivo, "Digitar Valor");
            await metodo.Escrever(el.CampoResumoOperacao, data.ResumoOperacao, "Digitar Resumo da operação");
            await metodo.Clicar(el.CheckBoxTermo, "Aceitar Termos");
            await metodo.Clicar(el.BtnSalvar, "Clicar em Salvar");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem de sucesso visivel na tela ");
        }

        public async Task ConsultarAtivo(Func<Task> customStep = null)
        {
            if (customStep != null)
            {
                await customStep();
                return;
            }
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, data.NomeAtivo, "Clicar na Barra de Pesquisa");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaAtivos, data.NomeAtivo, "Validar se ativo com destinatário Teste NUnit está presente na tabela");
        }

        [AllureStep("Aprovação Gestor")]
        public async Task AprovarGestor()
        {
            await ConsultarAtivo();
            await metodo.Clicar(el.BtnEmAnalise2("1"), "Clicar no botão para abrir modal de situação do gestor");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo gestor");
            await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoAprovacao, "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo gestor");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar texto de sucesso presente na tabela para o usuário");
            await page.ReloadAsync();
            await ConsultarAtivo();
            await metodo.ValidarMsgRetornada(el.StatusTabela("Aguar. Ass.", "1"), "Validar se status na tabela foi alterado para Aguardando Ass.");
        }
        [AllureStep("Aprovação Risco")]
        public async Task AprovarRisco()
        {
            await ConsultarAtivo();
            await metodo.Clicar(el.BtnEmAnalise2("1"), "Clicar no botão para abrir modal de situação do jurídico");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo risco");
            await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoAprovacao, "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo risco");
            await metodo.ValidarTextoPresente(AtivosData.MsgContratoAprovadoSucesso, "Validar mensagem de sucesso presenta ao aprovar por jurídico");
            await page.ReloadAsync();
            await ConsultarAtivo();
            await metodo.ValidarTextoDoElemento(el.StatusTabela("Aprovado", "1"), "Aprovado", "Validar se status de risco na tabela foi alterado para Aprovado.");
        }
        [AllureStep("Aprovação Jurídico")]
        public async Task AprovarJuridico()
        {
            await ConsultarAtivo();
            await metodo.Clicar(el.BtnEmAnalise2("2"), "Clicar no botão para abrir modal de situação do jurídico");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo jurídico");
            await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoAprovacao, "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo jurídico");
            await metodo.ValidarTextoPresente(AtivosData.MsgContratoAprovadoSucesso, "Validar mensagem de sucesso presente ao aprovar por jurídico");
            await page.ReloadAsync();
            await ConsultarAtivo();
            await metodo.ValidarTextoDoElemento(el.StatusTabela("Aprovado", "2"), "Aprovado", "Validar se status de juridico na tabela foi alterado para Aprovado.");

        }

        [AllureStep("Aprovação Cadastro")]
        public async Task AprovarCadastro()
        {
            await ConsultarAtivo();
            await metodo.Clicar(el.BtnEmAnalise2("1"), "Clicar no botão para abrir modal de situação do Cadastro");
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo cadastro");
            await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoAprovacao, "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo cadastro");
            await page.ReloadAsync();
            await ConsultarAtivo();
            await metodo.ValidarTextoDoElemento(el.StatusTabela("Aprovado", "3"), "Aprovado", "Validar se status de cadastro na tabela foi alterado para Aprovado.");
        }


        public async Task DownloadArquivo()
        {
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, data.TextoPesquisaAtivo, "Clicar na Barra de Pesquisa");
            await Task.Delay(1000);
            await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
            await metodo.ValidateDownloadAndLength(page, el.BtnBaixarArquivo, "Baixar Arquivo");
        }


        public async Task ContagemDeAtivosTotais()
        {
            await metodo.CapturarTextoDoElemento(el.TotalAtivos, "capturar e validar o total de ativos na página");
        }

        public async Task ReprovarAtivo(string nivel)
        {
            switch (nivel)
            {
                case "Gestor":
                    await ConsultarAtivo();
                    await metodo.Clicar(el.BtnEmAnalise2("1"), "Clicar no botão para abrir modal de situação do gestor");
                    await metodo.Clicar(el.BtnReprovado, "Clicar no botão para Reprovar pelo gestor");
                    await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoReprovacao, "Digitar Observação para reprovar ativo pelo gestor");
                    await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para Reprovar pelo gestor");
                    await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar texto de sucesso presente na tabela para o usuário");
                    await page.ReloadAsync();
                    await ConsultarAtivo();
                    await metodo.ValidarMsgRetornada(el.StatusTabela("Reprovado ", "1"), "Validar se status na tabela foi alterado para Reprovado");
                    break;
                case "Risco":
                    await ConsultarAtivo();
                    await metodo.Clicar(el.BtnEmAnalise2("1"), "Clicar no botão para abrir modal de situação do jurídico");
                    await metodo.Clicar(el.BtnReprovado, "Clicar no botão para Reprovar pelo risco");
                    await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoReprovacao, "Digitar Observação para reprovar ativo por Risco");
                    await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo risco");
                    await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar texto de sucesso presente na tabela para o usuário");
                    await page.ReloadAsync();
                    await ConsultarAtivo();
                    await metodo.ValidarTextoDoElemento(el.StatusTabela("Reprovado ", "1"), "Reprovado", "Validar se status de risco na tabela foi alterado para Reprovado.");
                    break;
                case "Juridico":
                    await ConsultarAtivo();
                    await metodo.Clicar(el.BtnEmAnalise2("2"), "Clicar no botão para abrir modal de situação do jurídico");
                    await metodo.Clicar(el.BtnReprovado, "Clicar no botão para Reprovar pelo jurídico");
                    await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoAprovacao, "Digitar Observação");
                    await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para reprovar pelo jurídico");
                    await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar texto de sucesso presente na tabela para o usuário");
                    await page.ReloadAsync();
                    await ConsultarAtivo();
                    await metodo.ValidarTextoDoElemento(el.StatusTabela("Reprovado ", "2"), "Reprovado", "Validar se status de juridico na tabela foi alterado para Reprovado.");
                    break;
                case "Cadastro":
                    await ConsultarAtivo();
                    await metodo.Clicar(el.BtnEmAnalise2("1"), "Clicar no botão para abrir modal de situação do Cadastro");
                    await metodo.Clicar(el.BtnReprovado, "Clicar no botão para reprovar pelo cadastro");
                    await metodo.Escrever(el.CampoObservacaoParecer, data.ObservacaoAprovacao, "Digitar Observação");
                    await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para reprovar pelo cadastro");
                    await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar texto de sucesso presente na tabela para o usuário");
                    await page.ReloadAsync();
                    await ConsultarAtivo();
                    await metodo.ValidarTextoDoElemento(el.StatusTabela("Reprovado ", "3"), "Reprovado", "Validar se status de cadastro na tabela foi alterado para Reprovado.");
                    break;
            }
        }
    }
}
