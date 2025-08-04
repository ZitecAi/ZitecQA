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

        public string SelectFundoCessionario { get; } = "#Fundos";
        public string SelectProduto { get; } = "#Produtos";
        public string SelectTipoLiquidacao { get; } = "#tipo";
        public string SelectTomador { get; } = "#searchInput";
        public string SelectConta { get; } = "#contaLiquidacao";
        public string CampoObservacaoInfo { get; } = "#mensagemObservacao";

    }
}
