using Microsoft.Playwright;
using PortalIDSFTestes.data.notas;
using PortalIDSFTestes.elementos.notas;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.notas
{
    public class PagamentosNotasPage
    {
        private IPage page;
        Utils metodo;
        PagamentosNotasElements el = new PagamentosNotasElements();
        PagamentosNotasData _data;

        public PagamentosNotasPage(IPage page, PagamentosNotasData data = null)
        {
            this.page = page;
            metodo = new Utils(page);
            _data = data ?? new PagamentosNotasData();

        }

        public async Task ValidarAcentosPagamentosNotasPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Pagamentos Notas ");
        }

        public async Task<PagamentosNotasPage> ClicarBtnNovoNotasInternasPage()
        {
            await metodo.Clicar(el.BtnNovo, "Clicar no botão Novo na página de Notas Internas ");
            return this;
        }

        public async Task<PagamentosNotasPage> PreencherFormularioNovaNota()
        {
            var caminhoCompleto = Path.Combine(Utils.GetPath(), "Arquivo teste 2.pdf");
            await metodo.ClicarNoSeletor(el.SelectTipoNota, _data.TipoNotaComissao, "Selecionar tipo de nota");
            await metodo.Escrever(el.ValorNota, _data.ValorNota, "Preencher o campo Valor da Nota");
            await metodo.ClicarNoSeletor(el.SelectFundos, _data.FundoZitec, "Selecionar Fundo Zitec");
            await metodo.ClicarNoSeletor(el.SelectPrestadores, _data.PrestadorGrid, "Selecionar Prestador");
            await ValidarContaBancariaPresente(async () =>
            {
                await metodo.ValidarTextoDoElemento(el.ContaBanco, _data.ContaBancaria, "Validar texto da Conta Bancária retornada");
            });
            await metodo.EnviarArquivo(el.InputEnviarNota, caminhoCompleto, "Enviar Arquivo nota");
            await metodo.Escrever(el.Observacao, PagamentosNotasData.Observacao, "Preencher o campo Observação da Nota");
            return this;
        }
        public async Task<PagamentosNotasPage> ValidarTextoPresente(string mensagem)
        {
            await metodo.ValidarTextoPresente(mensagem, "Validar texto presente na página");
            return this;
        }

        public async Task<PagamentosNotasPage> ClicarBtnEnviarNota()
        {
            await metodo.Clicar(el.BtnEnviarNota, "Clicar no botão Enviar Nota");
            return this;
        }

        public async Task ValidarContaBancariaPresente(Func<Task> ValidarTexto = null)
        {
            await metodo.ValidarElementoHabilitado(el.ContaBanco, "Validar Se prestador retornou conta banco");
        }

        public async Task<PagamentosNotasPage> AbrirModalAprovacao()
        {
            await metodo.Clicar(el.BtnAguardandoAprovacaoCustodia(PagamentosNotasData.Observacao), "Clicar no botão Aguardando Aprovação da Custódia para abrir modal");
            return this;
        }
        public async Task<PagamentosNotasPage> AbrirModalPagamento()
        {
            await metodo.Clicar(el.BtnAguardandoPagamento(PagamentosNotasData.Observacao), "Clicar no botão Aguardando Pagamento para abrir modal");
            return this;
        }
        public async Task<PagamentosNotasPage> ClicarBtnAprovado()
        {
            await metodo.Clicar(el.BtnAprovado, "Clicar no botão Aprovado dentro do modal");
            return this;
        }
        public async Task<PagamentosNotasPage> ClicarBtnReprovado()
        {
            await metodo.Clicar(el.BtnReprovado, "Clicar no botão Reprovado dentro do modal");
            return this;
        }
        public async Task<PagamentosNotasPage> ClicarBtnReprovarNota()
        {
            await metodo.Clicar(el.BtnReprovarNota, "Clicar no botão Reprovar nota ");
            return this;
        }
        public async Task<PagamentosNotasPage> SubmeterStatusNota()
        {
            await metodo.Clicar(el.BtnSubmitStatus, "Clicar no botão para submeter status da nota");
            return this;
        }

        public async Task<PagamentosNotasPage> ConsultarNotaCriada()
        {
            await metodo.Escrever(el.BarraDePesquisa, _data.NomeFundoZitec, "Pesquisar Fundo zitec na tabela");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaNotas, PagamentosNotasData.Observacao, $"Verificar se a Observação {PagamentosNotasData.Observacao} está presente na tabela de notas");
            return this;
        }

        public async Task<PagamentosNotasPage> ValidarStatusNaTabela(string status)
        {
            await metodo.ValidarTextoDoElemento(el.StatusTabela(PagamentosNotasData.Observacao), status, $"Validar se status na tabela esta como {status}");
            return this;
        }

        public async Task<PagamentosNotasPage> InserirObservacaoPagamento()
        {
            await metodo.Escrever(el.ObservacaoPagamento, _data.ObservacaoPagamento, "Inserir mensagem de observação para reprovar pagamento");
            return this;
        }

        public async Task<PagamentosNotasPage> AbrirHistoricoDeEventos()
        {
            await metodo.Clicar(el.BtnHistoricoDeEventos(PagamentosNotasData.Observacao), "Clicar no botão para abrir o histórico de eventos");
            return this;
        }
        public async Task<PagamentosNotasPage> GerarNota()
        {
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadGerarNota(PagamentosNotasData.Observacao), "Clicar no botão Gerar Nota e validar download");
            return this;
        }

        public async Task<PagamentosNotasPage> ValidarEventoNoHistoricoDeEventos(string evento)
        {
            await metodo.ValidarTextoDoElemento(el.TextoHistoricoDeEventos(evento), evento, $"Validar se o evento {evento} está presente no histórico de eventos");
            return this;
        }



    }
}
