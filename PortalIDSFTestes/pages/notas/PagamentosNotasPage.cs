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
            await metodo.ClicarNoSeletor(el.SelectTipoNota, _data.TipoNota, "Selecionar tipo de nota");
            await metodo.Escrever(el.ValorNota, _data.ValorNota, "Preencher o campo Valor da Nota");
            await metodo.ClicarNoSeletor(el.SelectFundos, _data.FundoZitec, "Selecionar Fundo Zitec");
            await metodo.ClicarNoSeletor(el.SelectPrestadores, _data.PrestadorGrid, "Selecionar Prestador");
            await ValidarContaBancariaPresente(async () =>
            {
                await metodo.ValidarTextoDoElemento(el.ContaBanco, _data.ContaBancaria, "Validar texto da Conta Bancária retornada");
            });
            await metodo.EnviarArquivo(el.InputEnviarNota, caminhoCompleto, "Enviar Arquivo nota");
            await metodo.Escrever(el.Observacao, _data.Observacao, "Preencher o campo Observação da Nota");
            return this;
        }
        public async Task<PagamentosNotasPage> ValidarTextoPresente()
        {
            await metodo.ValidarTextoPresente(_data.MsgSucessoNotaEnviada, "Validar texto presente na página");
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


    }
}
