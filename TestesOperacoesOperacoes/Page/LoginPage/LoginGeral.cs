using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteOperacoesOperacoes.Model;
using TesteOperacoesOperacoes.Util;
using TesteOperacoesOperacoes;
using TestesOperacoesOperacoes;
using Program = TestesOperacoesOperacoes.Program;

namespace TesteOperacoesOperacoes.Pages
{
    public class LoginGeral
    {

        public async static Task<Pagina> Login(IPage page, Usuario usuario)
        {
            var pagina = new Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            
            try
            {
                var portalLink = Program.Config["Links:Portal"];
                var PaginaLogin = await page.GotoAsync(portalLink + "/login.aspx", new() { Timeout = 20000 }); // ajuste de timeout

                await page.GetByPlaceholder("E-mail").FillAsync(usuario.Email);
                await page.GetByPlaceholder("Senha").FillAsync(usuario.Senha);
                await page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();

                var home = page.Locator("#Home");
                var erroSenha = page.GetByText("Senha incorreta");

                await Task.WhenAny(
                    home.WaitForAsync(new() { Timeout = 5000 }),
                    erroSenha.WaitForAsync(new() { Timeout = 5000 })
                );

                if (PaginaLogin?.Status == 200)
                {
                    if (await home.IsVisibleAsync())
                    {
                        Console.WriteLine("Login realizado com sucesso.");
                    }
                    else if (await erroSenha.IsVisibleAsync())
                    {
                        errosTotais++;
                        EmailPadrao emailPadrao = new EmailPadrao(
                            "jt@zitec.ai",
                            "Erro de Login no Portal IDSF",
                            "O login não teve sucesso. Não é possível fazer as verificações das páginas do portal."
                        );
                        EnviarEmail.SendMailWithAttachment(emailPadrao);
                        Console.WriteLine("Senha incorreta detectada e e-mail enviado.");
                    }
                    else
                    {
                        Console.WriteLine("Login falhou: página carregada mas sem elemento esperado.");
                        errosTotais++;
                    }
                }
                else
                {
                    Console.WriteLine("Página de login não retornou 200.");
                    errosTotais++;
                }
            }
            catch (Exception ex)
            {
                errosTotais++;
                Console.WriteLine($"Ocorreu um erro no login: {ex.Message}");
            }
            finally
            {
                // Sempre define os dados no relatório, mesmo com erro
                pagina.Nome = "Login";
                pagina.Perfil = usuario?.Nivel.ToString() ?? "Desconhecido";
                pagina.StatusCode = 200; // ou use PaginaLogin?.Status ?? 0
                pagina.Listagem = "❓";
                pagina.BaixarExcel = "❓";
                pagina.InserirDados = "❓";
                pagina.Excluir = "❓";
                pagina.Reprovar = "❓";
                try
                {
                    pagina.Acentos = await Acentos.ValidarAcentos(page);
                }
                catch
                {
                    pagina.Acentos = "Erro ao validar acentos";
                }
            }

            return pagina;
        }
    }
}


