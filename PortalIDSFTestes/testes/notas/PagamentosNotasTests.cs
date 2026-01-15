using Allure.NUnit;
using Allure.NUnit.Attributes;
using PortalIDSFTestes.data.notas;
using PortalIDSFTestes.elementos.notas;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.pages.login;
using PortalIDSFTestes.pages.notas;
using PortalIDSFTestes.runner;

namespace PortalIDSFTestes.testes.notas
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Category("Suíte:  Pagamentos notas")]
    [Category("Criticidade: Crítica")]
    [Category("Regressivos")]
    [AllureNUnit]
    [AllureSuite("PagamentosNotasTests UI")]
    [AllureOwner("Levi")]
    public class PagamentosNotasTests : TestBase
    {
        Utils metodo;
        PagamentosNotasElements el = new PagamentosNotasElements();


        [SetUp]
        [AllureBefore]
        public async Task Setup()
        {
            page = await AbrirBrowserAsync();
            var login = new LoginPage(page);
            metodo = new Utils(page);
            await login.LogarInterno();
            await metodo.Clicar(el.MenuNotas, "Clicar na sessão Notas no menú hamburguer");
            await metodo.Clicar(el.PaginaPagamentosNotas, "Clicar na página Pagamentos notas");
            await Task.Delay(500);
        }

        [TearDown]
        [AllureAfter]
        public async Task TearDown()
        {
            await FecharBrowserAsync();

        }

        [Test, Order(1)]
        [AllureTag("Regressivos")]
        [AllureName("Nao Deve Conter Acentos Quebrados Pagamentos Notas")]
        public async Task Nao_Deve_Conter_Acentos_Quebrados()
        {
            var pagamentosNotas = new PagamentosNotasPage(page);
            await pagamentosNotas.ValidarAcentosPagamentosNotasPage();
        }

        [Test, Order(2)]
        [AllureName("Deve Criar Nova Nota Com Sucesso")]
        public async Task Deve_Criar_Nova_Nota_Interna_Com_Sucesso()
        {
            var data = new PagamentosNotasData();
            var pagamentoNotas = new PagamentosNotasPage(page);
            await pagamentoNotas.ClicarBtnNovoNotasInternasPage();
            await pagamentoNotas.PreencherFormularioNovaNota();
            await pagamentoNotas.ClicarBtnEnviarNota();
            await pagamentoNotas.ValidarTextoPresente(data.MsgSucessoNotaEnviada);
        }
        [Test, Order(3)]
        [AllureName("Deve Consultar nota com sucesso")]
        public async Task Deve_Consultar_Nota_Criada_Com_Sucesso()
        {
            var data = new PagamentosNotasData();
            var pagamentoNotas = new PagamentosNotasPage(page);
            await pagamentoNotas.ConsultarNotaCriada();
            await pagamentoNotas.AbrirHistoricoDeEventos();
            await pagamentoNotas.ValidarEventoNoHistoricoDeEventos(data.EventoNotaRegistradaNoPortal);
        }
        [Test, Order(4)]
        [AllureName("Deve Baixar nota com sucesso")]
        public async Task Deve_Baixar_Nota_Criada_Com_Sucesso()
        {
            var data = new PagamentosNotasData();
            var pagamentoNotas = new PagamentosNotasPage(page);
            await pagamentoNotas.ConsultarNotaCriada();
            await pagamentoNotas.GerarNota();
        }
        [Test, Order(5)]
        [AllureName("Deve Aprovar nota com sucesso")]
        public async Task Deve_Aprovar_Nota_Com_Sucesso()
        {
            var data = new PagamentosNotasData();
            var pagamentoNotas = new PagamentosNotasPage(page);
            await pagamentoNotas.ConsultarNotaCriada();
            await pagamentoNotas.AbrirModalAprovacao();
            await pagamentoNotas.ClicarBtnAprovado();
            await pagamentoNotas.SubmeterStatusNota();
            await pagamentoNotas.ValidarTextoPresente(data.MsgSucessoNotaAtualizadaStatus);
            await pagamentoNotas.ValidarStatusNaTabela(data.StatusAguardandoPagamento);
            await pagamentoNotas.AbrirHistoricoDeEventos();
            await pagamentoNotas.ValidarEventoNoHistoricoDeEventos(data.EventoNotaAprovadaPelaCustodia);
        }
        [Test, Order(6)]
        [AllureName("Deve Reprovar Pagamento da Nota com sucesso")]
        public async Task Deve_Reprovar_Pagamento_Nota_Com_Sucesso()
        {
            var data = new PagamentosNotasData();
            var pagamentoNotas = new PagamentosNotasPage(page);
            await pagamentoNotas.ConsultarNotaCriada();
            await pagamentoNotas.AbrirModalPagamento();
            await pagamentoNotas.InserirObservacaoPagamento();
            await pagamentoNotas.ClicarBtnReprovarNota();
            await pagamentoNotas.ValidarTextoPresente(data.MsgSucessoNotaAtualizadaStatus);
            await pagamentoNotas.ValidarStatusNaTabela(data.StatusReprovadoPelaLiquidacao);
            await pagamentoNotas.AbrirHistoricoDeEventos();
            await pagamentoNotas.ValidarEventoNoHistoricoDeEventos(data.EventoNotaReprovadaPelaCustodia);
        }
    }
}
