using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AutomacaoZCustodia.Models;
using Microsoft.Playwright;
using AutomacaoZCustodia.Utils;
using Azure;

namespace AutomacaoZCustodia.Pages
{
    public class LoginZCustodia
    {
        public async static Task<Models.Pagina> Login (IPage Page)
        {

            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try

            {
                var PaginaLogin = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "/authentication/login");
                await Page.Locator(".mat-mdc-form-field-infix").First.ClickAsync();
                await Page.GetByLabel("E-mail").FillAsync("qa@zitec.ai");
                await Page.GetByLabel("Senha").ClickAsync();
                await Page.GetByLabel("Senha").FillAsync("testeqa01");
                await Page.GetByRole(AriaRole.Button, new() { Name = "Acessar" }).ClickAsync();
                await Page.WaitForTimeoutAsync(3000);

                if (PaginaLogin.Status == 200)
                {
                    var login = await Page.Locator("div.chart-container > h3:has-text('Operações Liquidadas')").ElementHandleAsync();
                    if (login != null && await login.IsVisibleAsync())
                    {
                        Console.Write("Login: ");
                        pagina.Nome = "Login";
                        pagina.StatusCode = PaginaLogin.Status;
                        pagina.Acentos = Utils.VerificarAcentos.ValidarAcentos(Page).Result;
                        pagina.Listagem = "❓";
                        pagina.BaixarExcel = "❓";
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                        await Page.GetByRole(AriaRole.Heading, new() { Name = "custodia" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    }
                    else
                    {
                        var errorLogin = await Page.GetByText("Usuário ou senha inválidos").ElementHandleAsync();
                        if (errorLogin != null && await errorLogin.IsVisibleAsync())
                        {
                            errosTotais++;
                            EmailPadrao emailPadrao = new EmailPadrao(
                                "jt@zitec.ai",
                                "Erro de Login no Portal IDSF",
                                "O login não teve sucesso. Não é possível fazer as verificações das páginas do portal.",
                                null
                            );

                            EnviarEmail.SendMailWithAttachment(emailPadrao);
                            Console.WriteLine("Senha incorreta detectada e e-mail enviado.");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                errosTotais++;
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}
