using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.notaComercial
{
    public class NotaComercialElements
    {
        public string MenuNotaComercial { get; } = "(//a[@class='nav-link']//p[text()='Nota Comercial'])[1]";
        public string PaginaNotaComercial { get; } = "//a[@href='/Operacoes/NotaComercial.aspx']";

        public string BtnNovoNotaComercial { get; } = "//span[text()='+ Novo']";
        public string BtnExcel { get; } = "//span[text()='Excel']";
        public string SelectFundoCessionario { get; } = "#Fundos";
        public string SelectProduto { get; } = "#Produtos";
        public string SelectTipoLiquidacao { get; } = "#tipo";
        public string LiquidacaoPix { get; } = "//option[@value='pix']";
        public string CampoTomador { get; } = "#searchInput";
        public string CedenteTest { get; } = "//li[text()='CEDENTE TESTE - 90400888000142']";
        public string BtnLupaTomador { get; } = "(//i[@class='fas fa-search'])[4]";
        public string SelectConta { get; } = "#contaLiquidacao";
        public string CampoObservacaoInfo { get; } = "#mensagemObservacao";
        //envolvidos
        public string SessaoEnvolvidos { get; } = "#Envolvidos-tab";
        public string AdicionarEnvolvido { get; } = "(//button[@class='btn btn-secondary btn-acoes2 btn-sm'])[3]";
        public string SelectRelacionadoA { get; } = "#relacionado";
        public string SelectEnvolvido { get; } = "#envolvido";
        public string SelectTipoRelacao { get; } = "#tipoRelacao";
        public string SelectFormaEnvio { get; } = "#formaEnvio";
        public string SelectFormaValidacao { get; } = "#formaValidacao";
        public string BtnConfirmarAddEnvolvido { get; } = "(//button[text()='Adicionar envolvido'])[1]";
        //operações
        public string SessaoOperacaoes { get; } = "#Operacao-tab";
        public string CampoValorSolicitado { get; } = "#valorSocilicitado";
        public string CampoTaxaJuros { get; } = "#taxaJuros";
        public string CampoDuração { get; } = "#duracao";
        public string CampoCarenciaAmortizacao { get; } = "#carenciaAmortizacao";

        public string CarenciaJuros { get; } = "#carenciaJuros";
        public string SelectTipoCalculo { get; } = "#tipoCalculo";
        public string CampoDiaVencimento { get; } = "#diaVencimento";
        public string CampoDataInicio{ get; } = "#dataInicio";
        public string CampoCorban{ get; } = "#corban";
        public string SelectIndexPosFix{ get; } = "#indexPosFix";
        //documentos
        public string SessaoDocumentos { get; } = "#Documentos-tab";
        public string BtnAddDocumento { get; } = "//button[@data-target='#modal-AdicionarDocumento']";
        public string InputFileNotaComercial { get; } = "#fileNotaComercial";
        public string CampoNomeExibicao { get; } = "#nomeDocumento";
        public string SelectTipoDocumento { get; } = "#tipoDocumento";
        public string BtnAtualizarDocumento { get; } = "(//button[text()='Atualizar documento'])[1]";
        public string BtnSalvarMudancas { get; } = "#btnSalvarMudancas";

        public string MsgSucessoRetornada { get; } = "//div[@class='toast toast-success']";
        public string BarraPesquisaTabela { get; } = "//label[text()='Pesquisar']";
        public string TabelaNotasComerciais { get; } = "#tabelaNotaComercial";
        public string PrimeiroTD { get; } = "(//table//tr[@class='odd']//td)[1]";
        public string BtnEngrenagem { get; } = "(//tr[td[contains(normalize-space(.), 'Zitec Tecnologia LTDA - 54.638.076/0001-76')]] //button[@title='Fechar operação!'])[1]";
        public string RadioBtnCancelarOp { get; } = "(//span[text()='Cancelar Operação'])[1]";
        public string MinutaPdf { get; } = "#downloadMinutaParHelperPDF";
        public string ObsFecharOp { get; } = "#obsFecharOP";
        public string BtnSubmitFecharOp { get; } = "#submitBtnFecharOp";
        public string BtnDownloadMinuta { get; } = "(//td//button[@title='Download Minuta'])[1]";
        public string BtnDownloadMinutaPDF { get; } = "//span//button[@title='Download Minuta PDF']";

        //Error Message

        public string MsgErro { get; } = "//div[@class='toast toast-warning']";




    }
}
