using Microsoft.Playwright;
using PortalIDSFTestes.data.boletagem;
using PortalIDSFTestes.elementos.Boletagem;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.boletagem
{
    public class AportePage
    {
        private readonly IPage page;
        Utils metodo;
        AporteElements el = new AporteElements();
        AporteData data = new AporteData();

        public AportePage(IPage page, AporteData data = null)
        {
            this.page = page;
            this.data = data ?? new AporteData();
            metodo = new Utils(page);

        }



        public async Task ValidarAcentosAporte()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Aporte");
        }

        public async Task ConsultarAporte()
        {
            await metodo.Escrever(el.Filtro, data.NomeCotista, "Pesquisar Nome do Cotista no filtro");
            await metodo.ValidarTextoDoElemento(el.PosicaoFundoNaTabela, data.NomeFundo + " - " + "CNPJ: " + data.CnpjFundo, "Validar que o fundo está correto na tabela");
            await metodo.ValidarTextoDoElemento(el.PosicaoCotistaNaTabela, data.NomeCotista + " - " + data.CpfCotista, "Validar que o cotista está correto na tabela");
        }

        public async Task RealizarAporte()
        {

            var today = DateTime.Today.ToString();
            string valorAporte = "10000";

            await metodo.Clicar(el.BtnNovo, "Clicar em Novo, para inserir novo aporte");
            await Task.Delay(200);
            await metodo.Escrever(el.Calendario, today, "Clicar no calendario para inserir dia do aporte");
            await metodo.Clicar(el.ValorAporte, "Clicar em valor do aporte");
            await metodo.Escrever(el.ValorAporte, valorAporte, "Inserir valor do aporte");
            await metodo.Escrever(el.CnpjCotista, data.CpfCotista, "inserir CNPJ cotista");
            await metodo.Escrever(el.InputNomeCotista, data.NomeCotista, "inserir Nome do cotista");
            await metodo.ClicarNoSeletor(el.SelectCota, "COTAFIXA", "Selecionar Cota Fixa");
            await metodo.Escrever(el.ValorCota, data.ValorCota, "valor da cota");
            await metodo.ClicarNoSeletor(el.TipoAporte, "FINANCEIRO", "Selecionar aporte Financeiro");
            await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar fundo zitec tecnologia");
            if (!await page.Locator("//option[text()='CARTEIRA TESTE']").IsEnabledAsync())
            {
                Assert.Fail("Fundo não trouxe a carteira");
            }
            await metodo.Clicar(el.BtnEnviar, "Clicar em enviar");
            await metodo.ValidarTextoPresente(AporteData.MsgBoletaRecebidaSucesso, "Validar mensagem de sucesso retornada");
        }

        public async Task AprovarCustodia(bool aprove = true)
        {
            await ConsultarAporte();
            await metodo.Clicar(el.AprovacaoCustodia(data.NomeCotista), "Aprovar aporte na custodia");
            if (aprove is false)
            {
                await metodo.Clicar(el.BtnRejeitado, "Reprovar aporte na custodia");
            }
            await metodo.Escrever(el.Descricao, data.DescricaoAprovacao, "Inserir descrição da aprovação");
            await metodo.Clicar(el.BtnConfirmar, "Confirmar aprovação");
            await metodo.ValidarTextoPresente(aprove is false
                ? AporteData.MsgReprovadoCustodiaSucesso
                : AporteData.MsgAprovadoCustodiaSucesso, ("Validar mensagem de sucesso do status custodia"));
            await metodo.ValidarTextoDoElemento(el.AprovacaoCustodia(data.NomeCotista), aprove is false ? "Reprovado!" : "Aprovado!", "Validar que o status do aporte para Aprovacao Custodia está como Aprovado");

        }

        public async Task AprovarEscrituracao(bool aprove = true)
        {
            await ConsultarAporte();
            await metodo.Clicar(el.AprovacaoEscrituracao(data.NomeCotista), "Aprovar aporte na escrituracao");
            if (aprove is false)
            {
                await metodo.Clicar(el.BtnRejeitado, "Reprovar aporte na custodia");
            }
            await metodo.Escrever(el.Descricao, data.DescricaoAprovacao, "Inserir descrição da aprovação");
            await metodo.Clicar(el.BtnConfirmar, "Confirmar aprovação");
            await metodo.ValidarTextoPresente(aprove is false
                ? AporteData.MsgReprovadoEscrituracaoSucesso
                : AporteData.MsgAprovadoEscrituracaoSucesso, ("Validar mensagem de sucesso do status escrituracao"));

            await metodo.ValidarTextoDoElemento(el.AprovacaoEscrituracao(data.NomeCotista), aprove is false ? "Reprovado!" : "Aprovado!", "Validar que o status do aporte para Aprovacao Escrituracao está como Aprovado");
        }

        public async Task AprovarControladoria(bool aprove = true)
        {
            await ConsultarAporte();
            await metodo.Clicar(el.AprovacaoControladoria(data.NomeCotista), "Aprovar aporte na controladoria");
            if (aprove is false)
            {
                await metodo.Clicar(el.BtnRejeitado, "Reprovar aporte na custodia");
            }
            await metodo.Escrever(el.Descricao, data.DescricaoAprovacao, "Inserir descrição da aprovação");
            await metodo.Clicar(el.BtnConfirmar, "Confirmar aprovação");
            await metodo.ValidarTextoPresente(aprove is false
                ? AporteData.MsgReprovadoControladoriaSucesso
                : AporteData.MsgAprovadoControladoriaSucesso, ("Validar mensagem de sucesso do status controladoria"));
            await metodo.ValidarTextoDoElemento(el.AprovacaoControladoria(data.NomeCotista), aprove is false ? "Reprovado!" : "Aprovado!", "Validar que o status do aporte para Aprovacao Controladoria está como Aprovado");
        }

        public async Task ReprovarCustodia()
        {
            await AprovarCustodia(false);
            await metodo.ValidarTextoPresente(AporteData.MsgReprovadoCustodiaSucesso, "Validar mensagem de sucesso na reprovação da custódia");
            await metodo.ValidarTextoDoElemento(el.AprovacaoCustodia(data.NomeCotista), "Reprovado!", "Validar que o status do aporte para Aprovacao Custodia está como Aprovado");

        }
        public async Task ReprovarEscrituracao()
        {
            await AprovarEscrituracao(false);
            await metodo.ValidarTextoPresente(AporteData.MsgReprovadoEscrituracaoSucesso, "Validar mensagem de sucesso na reprovação da Escrituração");
            await metodo.ValidarTextoDoElemento(el.AprovacaoCustodia(data.NomeCotista), "Reprovado!", "Validar que o status do aporte para Aprovacao Custodia está como Aprovado");

        }
        public async Task ReprovarControladoria()
        {
            await AprovarControladoria(false);
            await metodo.ValidarTextoPresente(AporteData.MsgReprovadoControladoriaSucesso, "Validar mensagem de sucesso na reprovação da controladoria");
            await metodo.ValidarTextoDoElemento(el.AprovacaoCustodia(data.NomeCotista), "Reprovado!", "Validar que o status do aporte para Aprovacao Custodia está como Aprovado");

        }
    }
}
