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
            if (page.Url.Contains("dev"))
            {
                await metodo.ValidarUrl("https://portal-dev.idsf.com.br/Home.aspx", "Validate URL at home Page in dev environment");
            }
            else if (page.Url.Contains("staging"))
            {
                await metodo.ValidarUrl("https://portal-staging.idsf.com.br/Home.aspx", "Validate URL at home Page in stg environment");
            }
            else
            {
                await metodo.ValidarUrl("https://portal.idsf.com.br/Home.aspx", "Validate URL at home Page in prod environment");
            }
        }
        public async Task LogarInterno()
        {
            await metodo.Escrever(el.campoEmail, "qazitec01@gmail.com", "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, "Testeqa01?!", "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
        }

    }
}
