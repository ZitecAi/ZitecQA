using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.cedentes
{
    public class CedentesElements
    {
        public string MenuCedentes { get; } = "(//a[@class='nav-link']//p[text()='Cedentes'])[1]";
        public string PaginaCedentes { get; } = "//a[@href='/Cedentes.aspx']";
        public string BtnNovoCedente { get; } = "#btonNovoCedente";
        public string InputNovoCedente { get; } = "#fileNovoCedente";
        public string MsgAcaoSucesso { get; } = "Ação Executada com Sucesso";
        public string BtnDownloadExcel { get; } = "#btnExportaExcel";
        public string BarraPesquisaCedentes { get; } = "//label[text()='Pesquisar']";
        public string TabelaCedentes { get; } = "#tabelaCedentes";
        public string BtnLixeiraCedentes { get; } = "//button[@class='buttonExcluirCedente btn btn-danger']";
        public string CampoObservacaoExcluir { get; } = "#TextAreaExclusao";
        public string BtnConfirmarExcluir { get; } = "#submitButtonExcluirCedente";
        public string MsgSucessoRetornada { get; } = "//div[@class='toast toast-success']";



    }
}
