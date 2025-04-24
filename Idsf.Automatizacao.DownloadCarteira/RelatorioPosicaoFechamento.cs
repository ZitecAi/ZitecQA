using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idsf.Automatizacao.DownloadCarteira
{
    public class RelatorioPosicaoFechamento
    {
        public int IdCarteira { get; set; }
        public string Apelido { get; set; }
        public DateTime DataPosicao { get; set; }
        public PlCota PlCota { get; set; }
        public List<Posico> Posicoes { get; set; }
    }

    public class PlCota
    {
        public double Cota { get; set; }
        public double Qtde { get; set; }
        public double PL { get; set; }
    }

    public class Posico
    {
        public int TipoPosicao { get; set; }
        public string DescricaoTipoPosicao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string Moeda { get; set; }
        public object IdOperacao { get; set; }
        public string IdAtivo { get; set; }
        public string Ativo { get; set; }
        public double QtdeBloqueada { get; set; }
        public double QtdeDisponivel { get; set; }
        public double QtdeTotal { get; set; }
        public double ValorBruto { get; set; }
        public double ValorIOF { get; set; }
        public double ValorIR { get; set; }
        public double ValorLiquido { get; set; }
    }
}
