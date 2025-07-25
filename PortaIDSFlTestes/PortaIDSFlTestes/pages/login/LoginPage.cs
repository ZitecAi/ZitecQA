using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortaIDSFTestes.metodos;
using PortaIDSFTestes.elementos.login;

namespace PortaIDSFTestes.pages.login
{ 
    public class LoginPage
    {
        private readonly IPage page;
        Metodos metodo;
        LoginElements el = new LoginElements();
        public LoginPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
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
            await metodo.validarUrl("https://portal.idsf.com.br/Home.aspx", "Validar Url Logada na Página Home");
        }

        public async Task LoginNegativoInterno(string email, string senha)
        {
            await metodo.Escrever(el.campoEmail, email, "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, senha, "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.validarMsgRetornada(el.errorMessage, "Validar mensagem de erro Ao Tentar Realizar login Negativo");
        }


        public async Task LoginSucessoConsultoria()
        {
            await metodo.Escrever(el.campoEmail, "jessica.tavares@aluno.ifsp.edu.br", "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, "Jehtavares?123", "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.validarUrl("https://portal.idsf.com.br/Investidor/SaldosAplicacoes.aspx", "Validar Url Logada na Página Home");
        }
        public async Task LoginNegativoConsultoria(string email, string senha)
        {
            await metodo.Escrever(el.campoEmail, email, "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, senha, "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.validarMsgRetornada(el.errorMessage, "Validar mensagem de erro Ao Tentar Realizar login Negativo");
        }

        public async Task LoginSucessoGestora()
        {
            await metodo.Escrever(el.campoEmail, "caiooliweira@gmail.com", "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, "id2021", "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.validarUrl("https://portal.idsf.com.br/Home.aspx", "Validar Url Logada na Página Home");
        }

        public async Task LoginNegativoGestora(string email, string senha)
        {
            await metodo.Escrever(el.campoEmail, email, "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, senha, "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.validarMsgRetornada(el.errorMessage, "Validar mensagem de erro Ao Tentar Realizar login Negativo");
        }


        public async Task LoginSucessoDenver()
        {
            await metodo.Escrever(el.campoEmail, "jessica.vitoria.tavares044@gmail.com", "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, "Jehtavares?123", "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.validarUrl("https://portal.idsf.com.br/Home.aspx", "Validar Url Logada na Página Home");
        }

        public async Task LoginNegativoDenver(string email, string senha)
        {
            await metodo.Escrever(el.campoEmail, email, "Inserir Email para Login");
            await metodo.Escrever(el.campoSenha, senha, "Inserir Senha para Login");
            await metodo.Clicar(el.loginBtn, "Clicar no Botão Para Realizar Login");
            await metodo.validarMsgRetornada(el.errorMessage, "Validar mensagem de erro Ao Tentar Realizar login Negativo");
        }

       





    }
}
