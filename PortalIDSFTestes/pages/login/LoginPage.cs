using Microsoft.Playwright;
using PortalIDSFTestes.elementos.login;
using PortalIDSFTestes.metodos;

namespace PortalIDSFTestes.pages.login
{
    public class LoginPage
    {
        private readonly IPage page;
        Utils metodo;
        LoginElements el = new LoginElements();
        public LoginPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }

        public async Task validarAcentosLoginPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Login");
        }

        public async Task LoginSucessoInterno()
        {
            await metodo.Escrever(el.campoEmail, "qazitec01@gmail.com", "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, "Testeqa01?!", "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.ValidarUrl("https://portal.idsf.com.br/Home.aspx", "Validar Url Logada na Página Home");
        }
        public async Task LogarInterno()
        {
            await metodo.Escrever(el.campoEmail, "qazitec01@gmail.com", "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, "Testeqa01?!", "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
        }

    }
}
