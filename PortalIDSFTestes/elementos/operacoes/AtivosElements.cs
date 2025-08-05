using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.elementos.operacoes
{
    public class AtivosElements
    {
        public string MenuOperacoes { get; } = "(//p[text()='Operações'])[2]";
        public string PaginaAtivos { get; } = "//a[@href='/Operacoes/Contratos.aspx']";
        public string BtnNovoAtivo { get; } = "//span[text()='+ Novo']";
        public string SelectFundo { get; } = "#Fundos";
        public string CampoNomeCedente { get; } = "#nomeCedente";
        public string CampoCpfPjCedente { get; } = "#cpfCnpjCedente";
        public string SelectTipoContrato{ get; } = "#tipoNota";
        public string CampoAgencia{ get; } = "#AgenciaAtivos";
        public string CampoConta{ get; } = "#ContaAtivos";
        public string CampoRazSocialDestinatario{ get; } = "#RazSocDestino";
        public string CampoCpfPj{ get; } = "#CpfCnpjAtivos";
        public string CampoValor{ get; } = "#valorAtivos";
        public string CampoResumoOperacao{ get; } = "#mensagemObservacao";
        public string BtnAnexos{ get; } = "#submitButton";
        public string InputAnexos{ get; } = "#filePagamentosNotas";
        public string BtnVoltar{ get; } = "#salvar";
        public string CheckBoxTermo{ get; } = "#termoRespCheck";

    }
}
