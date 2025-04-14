using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Azure;

namespace AutomacaoZCustodia.Pages
{
    public class CadastroUsuarios
    {
        public static async Task<Models.Pagina> CadastroUsuario (IPage Page)
        {
            var pagina = new Models.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {

                var cadastroUsuario = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.ZCUSTODIA"].ToString() + "home/registers/users");

                if (cadastroUsuario.Status == 200)
                {
                    string seletorTabela = "table.w-100.mat-elevated-item.overflow-auto";

                    Console.Write("Cadastro de usuarios: ");
                    pagina.Nome = "Cadastro de usuarios ";
                    pagina.StatusCode = cadastroUsuario.Status;
                    pagina.Acentos = Utils.VerificarAcentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Utils.VerificarListagem.Listagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.BaixarExcel = "❓";

                    //await Page.PauseAsync();
                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Usuário" }).ClickAsync();
                    //await Page.Locator(".mat-mdc-form-field-infix").First.ClickAsync();
                    //await Page.GetByLabel("Nome", new() { Exact = true }).FillAsync("usuario teste");
                    //await Page.GetByLabel("CPF").ClickAsync();
                    //await Page.GetByLabel("CPF").FillAsync("496.248.668-30");
                    //await Page.GetByLabel("Nome de usuário").ClickAsync();
                    //await Page.GetByLabel("Nome de usuário").FillAsync("usuario teste");
                    //await Page.Locator("#mat-mdc-form-field-label-4 span").ClickAsync();
                    //await Page.GetByLabel("E-mail").FillAsync("robo@zitec.ai");
                    //await Page.Locator("#mat-mdc-form-field-label-6 span").ClickAsync();
                    //await Page.GetByRole(AriaRole.Option, new() { Name = "Usuário" }).Locator("span").ClickAsync();
                    //await Page.GetByText("Senha", new() { Exact = true }).ClickAsync();
                    //await Page.GetByLabel("Senha", new() { Exact = true }).FillAsync("teste123");
                    //await Page.GetByText("Repita a senha").ClickAsync();
                    //await Page.GetByLabel("Repita a senha").FillAsync("teste123");
                    //await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                    var usuarioExiste = AutomacaoZCustodia.Repository.UsuariosRepository.VerificarExistenciaUsuarios("49624866830");

                    if (usuarioExiste)
                    {

                        pagina.InserirDados = "✅";

                        var apagarUsuario = AutomacaoZCustodia.Repository.UsuariosRepository.ApagarUsuario("49624866830");

                        if (apagarUsuario)
                        {
                            pagina.Excluir = "✅";

                        }
                        else
                        {
                            pagina.Excluir = "❌";
                           

                        }



                    }
                    else 
                    {
                        pagina.Excluir = "❌";
                        pagina.InserirDados = "❌";



                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de cadastro de usuarios.");
                    pagina.Nome = "Cadastro de usuarios";
                    pagina.StatusCode = cadastroUsuario.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://custodia.idsf.com.br/home/dashboard");


                }
            }
            catch (Exception ex) 
            {

                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                return pagina;



            }

            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
