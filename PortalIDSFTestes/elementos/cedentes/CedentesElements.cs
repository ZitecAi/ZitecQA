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


    }
}
