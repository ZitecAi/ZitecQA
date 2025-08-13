using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestePortal.Pages.CadastroPage
{
    public class CadastroConsultoras
    {

        public static async Task<Model.Pagina> Consultoras (IPage Page)
        {
            var portalLink = TestePortalIDSF.Program.Config["Links:Portal"];
            var CadastroConsultoras = await Page.GotoAsync(portalLink + "/Consultoras.aspx");
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            

            if (CadastroConsultoras.Status == 200)
            {
                Console.Write("Consultoras - Cadastro: ");
                pagina.Nome = "Consultoras";
                pagina.StatusCode = CadastroConsultoras.Status;
                pagina.Listagem = "❓";
                pagina.BaixarExcel = "❓";
                pagina.Reprovar = "❓";
                pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                pagina.Perfil = TestePortalIDSF.Program.UsuarioAtual.Nivel.ToString();

                if (pagina.Acentos == "❌")
                {
                    errosTotais++;
                }

                var ApagarConsultoras2 = Repository.Consultoras.ConsultorasRepository.ApagarConsultoras("16695922000109", "Jessica Vitoria Tavares");

                await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar Consultora +" }).ClickAsync();
                await Page.Locator("#Nome").ClickAsync();
                await Page.Locator("#Nome").FillAsync("Jessica Vitoria Tavares");
                await Page.GetByPlaceholder("/0000-00").ClickAsync();
                await Page.GetByPlaceholder("/0000-00").FillAsync("16695922000109");
                await Page.Locator("#Endereco").ClickAsync();
                await Page.Locator("#Endereco").FillAsync("Av Alexandre Grandisoli ");
                await Page.GetByPlaceholder("(00) 0000-").ClickAsync();
                await Page.GetByPlaceholder("(00) 0000-").FillAsync("11960183248");
                await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();
                await Task.Delay(800);
                var ConsultorasExiste = Repository.Consultoras.ConsultorasRepository.VerificaExistenciaConsultoras("16695922000109", "Jessica Vitoria Tavares");

                if (ConsultorasExiste)
                {
                    Console.WriteLine("Consultora adicionada com sucesso na tabela.");
                    pagina.InserirDados = "✅";
                    var ApagarConsultoras = Repository.Consultoras.ConsultorasRepository.ApagarConsultoras("16695922000109", "Jessica Vitoria Tavares");

                    if (ApagarConsultoras)
                    {
                        Console.WriteLine("Consultora apagado com sucesso");
                        pagina.Excluir = "✅";
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível apagar Consultora");
                        pagina.Excluir = "❌";
                        errosTotais++;
                    }

                }
                else
                {
                    Console.WriteLine("Não foi possível inserir Consultora");
                    pagina.InserirDados = "❌";
                    errosTotais++;
                }

            }
            else
            {
                Console.Write("Erro ao carregar a página de Consultoras no tópico Cadastro");
                pagina.Nome = "Consultoras";
                pagina.StatusCode = CadastroConsultoras.Status;
                errosTotais++;         
                await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}
