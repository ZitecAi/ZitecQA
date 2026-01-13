using Microsoft.Playwright;
using PortalIDSFTestes.data.cadastro;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.cadastro
{
    public class InvestidoresPage
    {

        private readonly IPage page;
        Utils metodo;
        InvestidoresElements el = new InvestidoresElements();
        InvestidoresData _data;

        public InvestidoresPage(IPage page, InvestidoresData data = null)
        {
            this.page = page;
            metodo = new Utils(page);
            _data = data ?? new InvestidoresData();
        }


        public async Task ValidarAcentosInvestidores()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Investidores");
        }

        public async Task<InvestidoresPage> PesquisarCotista()
        {
            await metodo.Escrever(el.TabelaCotistas, _data.NomeCotista, "Pesquisar Cotista na Tabela");
            return this;
        }

        public async Task<InvestidoresPage> ClicarLinkFormulario()
        {
            await metodo.Clicar(el.BotaoLinkFormulario, "Clicar no Botão de Link do Formulário");
            return this;
        }

        public async Task<IPage> AbrirFormularioNovaGuia()
        {
            await page.Context.GrantPermissionsAsync(new[] { "clipboard-read", "clipboard-write" });

            var linkFormulario = await page.EvaluateAsync<string>(
                "navigator.clipboard.readText()");

            var novaPagina = await page.Context.NewPageAsync();

            await novaPagina.AddInitScriptAsync(@"
        document.addEventListener('DOMContentLoaded', () => {
            document.body.style.zoom = '0.50';
        });
    ");

            novaPagina.Dialog += async (_, dialog) => await dialog.AcceptAsync();

            await novaPagina.GotoAsync(linkFormulario);

            return novaPagina;
        }


        public async Task<InvestidoresPage> PreencherDadosDoInvestidor(IPage page)
        {
            await metodo.Escrever(el.CampoNome, _data.NomeCotista, "Preencher o Campo Nome Completo");
            await metodo.ClicarNoSeletor(el.SeletorSexo, _data.Sexo, "Selecionar sexo do investidor");
            await metodo.Escrever(el.DataNascimento, _data.DataNascimento, "Preencher o Campo Data de Nascimento");
            await metodo.Escrever(el.NomePai, _data.NomePai, "Preencher o Campo Nome do Pai");
            await metodo.Escrever(el.NomeMae, _data.NomeMae, "Preencher o Campo Nome da Mãe");
            await metodo.ClicarNoSeletor(el.EstadoCivil, _data.EstadoCivil, "Selecionar Estado Civil do investidor");
            await metodo.ClicarNoSeletor(el.SituacaoLegal, _data.SituacaoLegal, "Selecionar Situação Legal do investidor");
            await metodo.Escrever(el.TelefoneContato, _data.TelefoneContato, "Preencher o Campo Telefone celular");
            await metodo.Escrever(el.MsgEletronica, _data.MsgEletronica, "Preencher o Campo Mensagem Eletrônica");
            await metodo.ClicarNoSeletor(el.TipoDocumento, _data.TipoDocumento, "Setar tipo de documento a ser enviado");
            await metodo.Escrever(el.NumeroDocumento, _data.NumeroDocumento, "Preencher o Campo Número do Documento");
            await metodo.Escrever(el.OrgaoExpedidor, _data.OrgaoExpedidor, "Preencher o Campo Órgão Expedidor");
            await metodo.ClicarNoSeletor(el.PaisExpedidor, _data.PaisExpedidor, "Selecionar País Expedidor do Documento");
            await metodo.ClicarNoSeletor(el.UfExpedidor, _data.UfExpedidor, "Selecionar UF Expedidor do Documento");
            await metodo.ClicarNoSeletor(el.UfNascimento, _data.EstadoNascido, "Selecionar Estado de Nascimento");
            await metodo.ClicarNoSeletor(el.Naturalidade, _data.CidadeNascido, "Selecionar Cidade de Nascimento");
            await metodo.Escrever(el.OcupacaoProf, _data.OcupacaoProfissional, "Preencher o Campo Ocupação Profissional");
            return this;
        }

        public async Task<InvestidoresPage> AvancarEtapaFormulario(string formulario)
        {
            await metodo.Clicar(el.BotaoAvancar, $"Avançar etapa do formulário para o passo {formulario}");
            return this;
        }

        public async Task<InvestidoresPage> PreencherEnderecoDoCotista()
        {
            await metodo.ClicarNoSeletor(el.PaisResidencia, "BRASIL", "Selecionar País de Residência");
            await metodo.Escrever(el.LogradouroResidencia, "AVENIDA TESTE AUTOMATIZADO", "Preencher o Campo Logradouro de Residência");
            return this;
        }

        public async Task<InvestidoresPage> PreencherSituacaoPatrimonial()
        {
            await metodo.Escrever(el.DataSituacaoPatrimonial, "01/06/2024", "Preencher o Campo Data da Situação Patrimonial");
            await metodo.Escrever(el.PatrimoniEstimado, "500000", "Preencher o Campo Patrimônio Estimado");
            return this;
        }

        public async Task<InvestidoresPage> PreencherSuitability()
        {
            await metodo.ClicarNoSeletor(el.MotivoDispensa, _data.MotivoDispensa, "Selecionar Motivo de Dispensa da Suitability");
            await metodo.ClicarNoSeletor(el.PerfilSuitability, _data.PerfilSuitability, "Selecionar Perfil de Suitability");
            return this;
        }

        public async Task<InvestidoresPage> AceitarTermos()
        {
            await metodo.Clicar(el.AceiteTermos, "Aceitar os Termos e Condições");
            return this;
        }

        public async Task<InvestidoresPage> EnviarFormulario()
        {
            await metodo.Clicar(el.BotaoEnviarFormulario, "Clicar no Botão Enviar Formulário");
            return this;
        }

        public async Task<InvestidoresPage> ValidarMensagemLinkCopiado()
        {
            await metodo.ValidarTextoPresente(_data.MensagemLinkCopiado, "Validar Mensagem de link copiado presente na tela");
            return this;
        }

    }
}
