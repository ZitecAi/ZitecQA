using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortalExecutavel.Repository;
using TestePortalExecutavel.Utils;
using static TestePortalExecutavel.Model.Usuario;


namespace TestePortalExecutavel.Pages.OperacoesPage
{
    public class OperacoesAtivos
    {
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxoDeCadastro)> Ativos(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            int errosTotais2 = 0;

            try
            {

                var portalLink = Program.Config["Links:Portal"];
                var OperacoesAtivos = await Page.GotoAsync(portalLink + "/Operacoes/Contratos.aspx");


                if (OperacoesAtivos.Status == 200)
                {
                    string seletorTabela = "#tabelaContratos";
                    Console.Write("Operações - Ativos : ");
                    pagina.Nome = "Operações - Ativos";
                    pagina.StatusCode = OperacoesAtivos.Status;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Listagem = Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }
                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        var apagarAtivo2 = Repository.Ativos.AtivosRepository.ApagarAtivos("24426716000113", "teste robo");
                        fluxoDeCadastros.Fluxo = "Operações - ativos";
                        fluxoDeCadastros.Formulario = "❓";
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❓";
                        fluxoDeCadastros.EmailRecebido = "❓";
                        await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "24426716000113" });
                        await Task.Delay(300);
                        await Page.FillAsync("#nomeCedente", "teste robo");
                        await Task.Delay(300);
                        await Page.FillAsync("#cpfCnpjCedente", "533.006.080-00106");
                        await Task.Delay(300);
                        await Page.Locator("#tipoNota").SelectOptionAsync(new[] { "Ações Judiciais" });
                        await Task.Delay(300);
                        await Page.Locator("#AgenciaAtivos").FillAsync("0001");
                        await Task.Delay(300);
                        await Page.Locator("#ContaAtivos").FillAsync("460915");
                        await Task.Delay(300);
                        await Page.Locator("#RazSocDestino").FillAsync("teste robo");
                        await Task.Delay(300);
                        await Page.Locator("#CpfCnpjAtivos").FillAsync("49624866830");
                        await Page.GetByLabel("Valor").ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByLabel("Valor").FillAsync("10");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste robo");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Anexos" }).ClickAsync();
                        await Task.Delay(300);
                        await EnviarArquivoZipAsync(Page);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Voltar" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#termoRespCheck").ClickAsync();
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();
                        await Task.Delay(1000);

                        //aprovação 

                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Task.Delay(800);
                        await Page.GetByLabel("Pesquisar").FillAsync("teste robo");
                        var primeiroTr = Page.Locator("#listaContratos tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Page.Locator("li").Filter(new() { HasText = "Gestor Análise" }).Locator("[id=\"\\32 4426716000113_53300608000106_GESTORA\"]").ClickAsync();
                        await Page.Locator("#ctl00").GetByText("Aprovado").ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste ");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        await Task.Delay(40000);

                        var idAtivo = Repository.Ativos.AtivosRepository.RetornaIdAtivo("24426716000113", "teste robo");

                        bool statusAtual = false;

                        for (int i = 0; i < 5; i++)
                        {
                            statusAtual = Repository.Ativos.AtivosRepository.StatusAgrAss("24426716000113", "teste robo");

                            if (statusAtual)
                            {
                                Console.WriteLine("Status trocado para aguardando assinatura");
                                fluxoDeCadastros.StatusEmAnalise = "✅";
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Tentativa {i + 1} de verificar o status falhou. Tentando novamente...");
                                await Task.Delay(1000);
                            }
                        }

                        if (statusAtual == true)
                        {
                            string idDocumentoAutentique = Repository.Ativos.AtivosRepository.ObterDocAutentique(idAtivo);
                            var response = Utils.AssinarDocumentosAutentique.AssinarDocumento("9ad54b27a864625573ad40327a1916db61b687c3fe8641ff7f3efdc3e985d3b3", idDocumentoAutentique);

                            if (response != null && response.Success)
                            {
                                fluxoDeCadastros.DocumentoAssinado = "✅";
                                Console.WriteLine("Documento assinado");
                            }
                            else
                            {
                                errosTotais2++;
                                fluxoDeCadastros.DocumentoAssinado = "❌";
                                fluxoDeCadastros.ListaErros.Add("Erro ao assinar documento");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Status não foi trocado para aguardando assinatura");
                            fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando assinatura");

                        }

                        await Task.Delay(20000);

                        bool statusAgdLiqui = false;

                        for (int i = 0; i < 5; i++)
                        {
                            statusAgdLiqui = Repository.Ativos.AtivosRepository.StatusAprovado("24426716000113", "teste robo");

                            if (statusAgdLiqui)
                            {
                                Console.WriteLine("Status trocado para aguardando liquidação");
                                fluxoDeCadastros.statusAprovado = "✅";
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Tentativa {i + 1} de verificar o status falhou. Tentando novamente...");
                                await Task.Delay(1000);
                            }

                            if (i == 4)
                            {
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando liquidação");
                            }
                        }

                        var ativoExiste = Repository.Ativos.AtivosRepository.VerificaExistenciaAtivos("24426716000113", "teste robo");

                        if (ativoExiste)
                        {
                            Console.WriteLine("Ativo adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarAtivo = Repository.Ativos.AtivosRepository.ApagarAtivos("24426716000113", "teste robo");
                            if (apagarAtivo)
                            {
                                Console.WriteLine("Ativo apagado com sucesso");
                                pagina.Excluir = "✅";


                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar ativo ");

                                pagina.Excluir = "❌";
                                errosTotais++;

                            }
                        }

                        else
                        {
                            Console.WriteLine("Não foi possível inserir ativo");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                            await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                        }

                    }
                    else
                    {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";


                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Ativos de baixa no tópico Operações ");
                    pagina.Nome = "Operações - Ativos";
                    pagina.StatusCode = OperacoesAtivos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                if (fluxoDeCadastros.ListaErros.Count == 0)
                {
                    fluxoDeCadastros.ListaErros.Add("0");
                }
                return (pagina, fluxoDeCadastros);
            }
            if (fluxoDeCadastros.ListaErros.Count == 0)
            {
                fluxoDeCadastros.ListaErros.Add("0");
            }
            pagina.TotalErros = errosTotais;
            return (pagina, fluxoDeCadastros);



        }

        public static async Task EnviarArquivoZipAsync(IPage Page)
        {
            // Caminho do arquivo único
            var caminho = Path.Combine(Program.Config["Paths:Arquivo"], "Arquivo teste.zip");

            // Localiza o input oculto
            var input = Page.Locator("input.file-input[data-id-anexo='6']");

            // Envia o arquivo para o input
            await input.SetInputFilesAsync(caminho);

            // Localiza o ícone de upload (drop zone visual)
            var dropTarget = Page.Locator("i.fas.fa-cloud-upload-alt[data-id-anexo='6']");
            var box = await dropTarget.BoundingBoxAsync();

            if (box != null)
            {
                // Move o mouse até o centro do ícone (ponto de drop)
                await Page.Mouse.MoveAsync(box.X + box.Width / 2, box.Y + box.Height / 2);

                // Simula os eventos dragenter e drop com o arquivo do input
                await Page.EvaluateAsync(@"() => {
            const input = document.querySelector('input.file-input[data-id-anexo=""6""]');
            const dropZone = document.querySelector('i.fas.fa-cloud-upload-alt[data-id-anexo=""6""]');

            if (input && dropZone) {
                const dataTransfer = new DataTransfer();

                for (let i = 0; i < input.files.length; i++) {
                    dataTransfer.items.add(input.files[i]);
                }

                const dragEnter = new DragEvent('dragenter', {
                    dataTransfer: dataTransfer,
                    bubbles: true,
                    cancelable: true
                });

                const drop = new DragEvent('drop', {
                    dataTransfer: dataTransfer,
                    bubbles: true,
                    cancelable: true
                });

                dropZone.dispatchEvent(dragEnter);
                dropZone.dispatchEvent(drop);
            }
        }");
            }
            else
            {
                Console.WriteLine("Não foi possível localizar a área de drop.");
            }
        }
    }
}
