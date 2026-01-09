namespace PortalIDSFTestes.elementos.login
{
    public class LoginElements
    {

        public string campoEmail { get; } = "#email";
        public string campoSenha { get; } = "#password";
        public string loginBtn { get; } = "//button[text()='Entrar']";
        public string errorMessage { get; } = "#erroraccess";
        public string PopUpAtualizacoes { get; } = "#update-popup";
        public string BotaoFecharModalAtualizacoes { get; } = "#close-popup-btn";


    }
}
