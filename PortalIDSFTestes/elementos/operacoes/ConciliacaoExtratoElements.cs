namespace PortalIDSFTestes.elementos.operacoes
{
    public class ConciliacaoExtratoElements
    {
        public string MenuOperacoes { get; } = "(//p[text()='Operações'])[2]";
        public string PaginaConciliacaoExtrato { get; } = "//p[text()='Conciliação & Extrato']";
    }
}
