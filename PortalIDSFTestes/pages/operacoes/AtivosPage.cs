using Allure.NUnit.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.operacoes
{
    public class AtivosPage
    {
        public static string NomeAleatorio { get; set; } = GenerateNameUnique();

        string NomeAtivo = NomeAleatorio;

        private IPage page;
        Utils metodo;
        AtivosElements el = new AtivosElements();
        public AtivosPage(IPage page)
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

        public async Task ValidarAcentosAtivosPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Ativos");
        }

        public async Task DownloadExcel()
        {
            await metodo.ValidateDownloadAndLength(page, el.BtnExcel, "Validar Download do Excel na página de Ativos");
        }

        public static string GenerateNameUnique()
        {
            try
            {
                Random random = new Random();

                int numeroUnico = random.Next(0, 9999);
                string nomeUnico = $"Teste NUnit {numeroUnico}";

                return nomeUnico;
            }
            catch
            {
                throw new Exception("Erro ao gerar número único para o nome");
            }
        }



        public async Task CadastrarEConsultarAtivo()
        {


            await metodo.Clicar(el.BtnNovoAtivo, "Clicar no Botão Para Cadastrar Um Novo Ativo");
            await Task.Delay(500);
            await metodo.ClicarNoSeletor(el.SelectFundo, "53300608000106", "Selecionar Fundo");
            await metodo.Escrever(el.CampoNomeCedente, "Cedente Teste NUnit", "Escrever Nome Cedente");
            await metodo.Escrever(el.CampoCpfPjCedente, "533.006.080-00106", "Escrever CPF/CNPJ Cedente");
            await metodo.ClicarNoSeletor(el.SelectTipoContrato, "AtivosImobiliarios", "Selecionar Tipo de Contrato");
            await metodo.Escrever(el.CampoAgencia, "0001", "Digitar Agencia");
            await metodo.Escrever(el.CampoConta, "460915", "Digitar Conta");
            await metodo.Escrever(el.CampoRazSocialDestinatario, NomeAleatorio, "Digitar Razão Social");
            await metodo.Escrever(el.CampoCpfPj, "49624866830", "Digitar CPF/CNPJ Ativo");
            await metodo.Escrever(el.CampoValor, "10", "Digitar Valor");
            await metodo.Escrever(el.CampoResumoOperacao, "Teste NUnit", "Digitar Resumo da operação");
            await metodo.Clicar(el.BtnAnexos, "Clicar no Botão Anexos");
            await metodo.EnviarArquivo(el.InputAnexos, GetPath() + "21321321321.pdf", "Enviar Anexo no Input de anexos");
            await metodo.Clicar(el.BtnVoltar, "Clicar no botão voltar após enviar anexo");
            await Task.Delay(150);
            await metodo.Clicar(el.CheckBoxTermo, "Aceitar Termos");
            await metodo.Clicar(el.BtnSalvar, "Clicar em Salvar");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem de sucesso visivel na tela ");
            await Task.Delay(1000);
            await page.ReloadAsync();


            //consultar ativo cadastrado
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, NomeAleatorio, "Clicar na Barra de Pesquisa");
            await Task.Delay(150);
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaAtivos, NomeAleatorio, "Validar se ativo com destinatário Teste NUnit está presente na tabela");



        }
        [AllureStep("Aprovação Gestor")]
        public async Task AprovarGestor()
        {
            //await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            //await metodo.Escrever(el.BarraPesquisa, NomeAtivo, "Clicar na Barra de Pesquisa");
            await Task.Delay(150);
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaAtivos, NomeAtivo, "Validar se ativo com destinatário Teste NUnit está presente na tabela");
            //await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
            ILocator btnGestor = page.Locator("(//button[text()='Análise'])[1]");
            if (await btnGestor.IsVisibleAsync())
            {
                await metodo.Clicar(el.BtnEmAnalise("1"), "Clicar no botão para abrir modal de situação do gestor");
                return;
            }
            else
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmTDGestor("Análise"), "Clicar no botão para abrir modal de situação do Gestor");
            }
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo gestor");
            await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo gestor");
            //await metodo.ValidarTextoPresente("Documentos enviados ao email administrativo@yaaleh.com.br para assinatura, gentileza validar.",
            //"Validar se mensagem de sucesso ao aprovar por gestor esta visivel ao usuário");
            //await Task.Delay(10000);
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar texto de sucesso presente na tabela para o usuário");
            await page.ReloadAsync();
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, NomeAtivo, "Clicar na Barra de Pesquisa");
            await Task.Delay(150);
            await metodo.ValidarTextoDoElemento(el.StatusTabela, "AGUARDANDO ASSINATURAS", "Validar se status na tabela foi alterado para Aguardando Ass.");
        }
        [AllureStep("Aprovação Jurídico")]
        public async Task AprovarJuridico()
        {
            //await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            //await metodo.Escrever(el.BarraPesquisa, NomeAtivo, "Clicar na Barra de Pesquisa");
            await Task.Delay(150);
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaAtivos, NomeAtivo, "Validar se ativo com destinatário Teste NUnit está presente na tabela");
            ILocator btnJuridico = page.Locator("(//th[text()='Jurídico']/ancestor::table//tbody//td)[12]//button[text()='Análise']");
            if (await btnJuridico.IsVisibleAsync())
            {
                await metodo.Clicar(el.BtnJuridico("Análise"), "Clicar no botão para abrir modal de situação do jurídico");
                return;
            }
            else
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmTDJuridico("Análise"), "Clicar no botão para abrir modal de situação do jurídico");
            }


            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo jurídico");
            await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo jurídico");
            await metodo.ValidarTextoPresente("Contrato aprovado com sucesso!", "Validar mensagem de sucesso presenta ao aprovar por jurídico");
            //await page.ReloadAsync();            
            await Task.Delay(150);
            //await metodo.ValidarMsgRetornada(el.BtnEmAnaliseJuridico("Aprovado"), "Validar status de sucesso visivel na tela ");

        }
        [AllureStep("Aprovação Risco")]
        public async Task AprovarRisco()
        {
            await page.ReloadAsync();
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, NomeAtivo, "Clicar na Barra de Pesquisa");
            await Task.Delay(150);
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaAtivos, NomeAtivo, "Validar se ativo com destinatário Teste NUnit está presente na tabela");
            ILocator btnRisco = page.Locator($"(//td[text()='{NomeAtivo}']/ancestor::tr//button[text()='Análise'])[2]");
            if (await btnRisco.IsVisibleAsync())
            {
                await metodo.Clicar(el.BtnEmAnaliseRisco(NomeAtivo), "Clicar no botão para abrir modal de situação do risco");
                return;
            }
            else
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmTDRisco("Análise"), "Clicar no botão para abrir modal de situação do Risco");
            }
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo risco");
            await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo risco");
            await metodo.ValidarTextoPresente("Contrato aprovado com sucesso!", "Validar mensagem de sucesso presenta ao aprovar por jurídico");

        }
        [AllureStep("Aprovação Cadastro")]
        public async Task AprovarCadastro()
        {
            await page.ReloadAsync();
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, NomeAtivo, "Clicar na Barra de Pesquisa");
            await Task.Delay(150);
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaAtivos, NomeAtivo, "Validar se ativo com destinatário Teste NUnit está presente na tabela");
            ILocator btnCadastro = page.Locator("(//th[text()='Jurídico']/ancestor::table//tbody//td)[12]//button[text()='Análise']");
            if (await btnCadastro.IsVisibleAsync())
            {
                await metodo.Clicar(el.BtnEmAnaliseCadastro("Análise"), "Clicar no botão para abrir modal de situação do jurídico");
                return;
            }
            else
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmTDCadastro("Análise"), "Clicar no botão para abrir modal de situação do jurídico");
            }
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo cadastro");
            await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
            await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo cadastro");
            await page.ReloadAsync();
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, NomeAtivo, "Clicar na Barra de Pesquisa");
            await metodo.Clicar(el.PrimeiroTd, "Clicar no primeiro TD para expandir dados");
            await Task.Delay(150);
            await metodo.ValidarMsgRetornada(el.BtnEmTDCadastro("Aprovado"), "Validar status de sucesso visivel na tela ");
        }


        public async Task DownloadArquivo()
        {
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, "Teste NUnit", "Clicar na Barra de Pesquisa");
            await Task.Delay(1000);
            await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
            await metodo.ValidateDownloadAndLength(page, el.BtnBaixarArquivo, "Baixar Arquivo");
        }


        public async Task ContagemDeAtivosTotais()
        {
            await metodo.CapturarTextoDoElemento(el.TotalAtivos, "capturar e validar o total de ativos na página");
        }

    }
}
