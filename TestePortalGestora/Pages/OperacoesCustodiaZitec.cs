using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
//using System.Windows.Controls;
using TestePortalGestora.Utils;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using static TestePortalGestora.Model.Usuario;
//using TestePortalInterno.Repositorys.OperacoesZitec;
using TestePortalGestora.Model;

namespace TestePortalGestora.Pages
{
    public class OperacoesCustodiaZitec
    {
        public static async Task<(Model.Pagina pagina, Operacoes operacoes)> OperacoesZitec(IPage Page, NivelEnum nivelLogado, Operacoes operacoes)
        {
            var pagina = new Model.Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int statusTrocados = 0;
            string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";
            operacoes.ListaErros2 = new List<string>();

            try
            {
                var OperacoesZitec = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Operacoes/OperacoesZitec.aspx");

                if (OperacoesZitec?.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações Zitec : ");
                    pagina.Nome = "Operações Zitec";
                    pagina.StatusCode = OperacoesZitec.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Acentos = await Utils.Acentos.ValidarAcentos(Page) ?? "❌";
                    if (pagina.Acentos == "❌") errosTotais++;

                    pagina.Listagem = await Utils.Listagem.VerificarListagem(Page, seletorTabela) ?? "❌";
                    if (pagina.Listagem == "❌") errosTotais++;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";

                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");

                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                errosTotais2++;
                operacoes.ListaErros2.Add("Erro de timeout");
                operacoes.totalErros2 = errosTotais2;
                pagina.TotalErros = errosTotais;
                return (pagina, operacoes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex}");
                operacoes.ListaErros2.Add($"Exceção lançada: {ex}");
                errosTotais2++;
                operacoes.totalErros2 = errosTotais2;
            }

            pagina.TotalErros = errosTotais;
            if (operacoes.ListaErros2.Count == 0)
            {
                operacoes.ListaErros2.Add("0");
            }

            return (pagina, operacoes);
        }
    }
}
