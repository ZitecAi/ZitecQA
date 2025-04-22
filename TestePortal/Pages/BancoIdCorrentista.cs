using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using Segment.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Model;
using TestePortal.Repository.Correntistas;
using TestePortal.Repository.Investidores;
using static TestePortal.Model.Usuario;
using TestePortal.Utils;
using DocumentFormat.OpenXml.Spreadsheet;


namespace TestePortal.Pages
{
    public class BancoIdCorrentista

    {
        #region Cadastro Correntista PJ - Movimentação
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxoDeCadastros)> CorrentistaMov(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int formularioOk = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();


            try
            {
                var PaginaBancoIdCorrentista = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Correntistas.aspx");

                if (PaginaBancoIdCorrentista.Status == 200)
                {
                    Console.Write("Banco Id - Correntista: ");
                    Console.WriteLine(PaginaBancoIdCorrentista.Status);
                    pagina.StatusCode = PaginaBancoIdCorrentista.Status;
                    pagina.Nome = "Banco Id - Correntista ";
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    string seletorTabela = "#tabelaCorrentista";

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                    if (nivelLogado == NivelEnum.Master)
                    {
                        try

                        {
                            var apagarCorrentista = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");
                            fluxoDeCadastros.Fluxo = "Correntista - Movimentação";
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Correntista" }).ClickAsync();
                            await Page.Locator("#cpfcnpj").ClickAsync();
                            await Task.Delay(300);
                            await Page.Locator("#cpfcnpj").FillAsync("45543915000181");
                            await Task.Delay(300);
                            await Page.Locator("#tipoContaCorrentista").SelectOptionAsync(new[] { "MOVIMENTACAO" });
                            await Task.Delay(300);
                            await Page.Locator("#btnValidarCorrentista").ClickAsync();
                            await Task.Delay(300);
                            await Page.Locator("#paginaModalPJ #emailEmpresa").ClickAsync();
                            await Task.Delay(300);
                            await Page.Locator("#paginaModalPJ #emailEmpresa").FillAsync("jehvittav@gmail.com");
                            await Task.Delay(300);
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                            await Task.Delay(400);

                            var idCorrentista = Repository.Correntistas.CorrentistaRepository.ObterIdCorrentista("jehvittav@gmail.com", "45543915000181");
                            var token = Repository.Correntistas.CorrentistaRepository.ObterToken("jehvittav@gmail.com", "45543915000181");
                            string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.CORRENTISTA"];
                            string copiedUrl = $"{baseUrl}{token}";
                            var newPage = await context.NewPageAsync();
                            await newPage.GotoAsync(copiedUrl);

                            await Task.Delay(200);
                            await newPage.Locator("#NomeContato").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#NomeContato").FillAsync("Carrefur");
                            await Task.Delay(200);
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9547-87456");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                            await newPage.Locator("#NomeRepresentante").ClickAsync();
                            await newPage.Locator("#NomeRepresentante").FillAsync("Jessica Vitoria Tavares");
                            await Task.Delay(200);
                            await newPage.Locator("#CPFCNPJRepresentante").ClickAsync();
                            await newPage.Locator("#CPFCNPJRepresentante").FillAsync("49624866830");
                            await Task.Delay(200);
                            await newPage.Locator("#EmailRepresentante").ClickAsync();
                            await newPage.Locator("#EmailRepresentante").FillAsync("jehvittav@gmail.com");
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("0").ClickAsync();
                            await newPage.GetByPlaceholder("0").FillAsync("10");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "+ Adicionar Representante" }).ClickAsync();
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                            await newPage.Locator("#NomeUsuarioMaster").ClickAsync();
                            await newPage.Locator("#NomeUsuarioMaster").FillAsync("Jessica Vitoria Tavares");
                            await Task.Delay(200);
                            await newPage.Locator("div:nth-child(3) > .col-md-12 > div:nth-child(2) > .form-group").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#CPFUsuarioMaster").FillAsync("49624866830");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 7548-75944");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                            await newPage.Locator("#input-Contrato").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#input-Atas").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#input-Procuracoes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#input-BalancoPatrimonial").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#input-DocRepresentantes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                            await newPage.Locator("input[name=\"assinaRepresentantes\"]").CheckAsync();
                            await Task.Delay(300);
                            await newPage.Locator("#checkTermos").CheckAsync();
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                            await Task.Delay(300);
                            await newPage.CloseAsync();
                            await Task.Delay(3700);
                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                            var primeiroTr = Page.Locator("#listaCorrentistas tr").First;
                            var primeiroTd = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            var button = Page.Locator($"tr.child button[onclick=\"ModalResumoCorrentista('{idCorrentista}')\"]");

                            if (await button.CountAsync() > 0)
                            {
                                await button.ClickAsync();
                                fluxoDeCadastros.Formulario = "✅";
                                Console.WriteLine("Botão de resumo de correntista encontrado.");

                            }
                            else
                            {
                                fluxoDeCadastros.Formulario = "❌";
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao encontrar botão para acessar resumo do correntista.");
                            }

                            //verificar dados de razão social

                            await Task.Delay(900);
                            var razaoSocial = await Page.EvaluateAsync<string>("() => document.getElementById('RazaoSocial').value");
                            var cnpj = await Page.EvaluateAsync<string>("() => document.getElementById('CNPJEmpresa').value");
                            var dataConst = await Page.EvaluateAsync<string>("() => document.getElementById('DataConstituicao').value");

                            if (razaoSocial == "CARREFOUR COMERCIO E INDUSTRIA LTDA" && cnpj == "45.543.915/0001-81" && dataConst == "21/06/1974")
                            {
                                Console.WriteLine("Resumo do correntista salvo corretamente!");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados de resumo correntista");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados de correntista não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar dados de endereço 

                            await Page.Locator("#btnEndereco").ClickAsync();

                            var endereco = await Page.EvaluateAsync<string>("() => document.getElementById('EnderecoMatriz').value");
                            await Task.Delay(200);
                            var cidade = await Page.EvaluateAsync<string>("() => document.getElementById('CidadeMatriz').value");
                            await Task.Delay(200);
                            var nomeContato = await Page.EvaluateAsync<string>("() => document.getElementById('NomeContato').value");
                            await Task.Delay(200);
                            var estado = await Page.EvaluateAsync<string>("() => document.getElementById('EstadoMatriz').value");
                            await Task.Delay(200);
                            var email = await Page.EvaluateAsync<string>("() => document.getElementById('EmailContato').value");
                            await Task.Delay(200);
                            var numero = await Page.EvaluateAsync<string>("() => document.getElementById('NumeroMatriz').value");
                            await Task.Delay(200);
                            var bairro = await Page.EvaluateAsync<string>("() => document.getElementById('BairroMatriz').value");
                            await Task.Delay(200);
                            var complemento = await Page.EvaluateAsync<string>("() => document.getElementById('ComplementoMatriz').value");
                            await Task.Delay(200);
                            var cep = await Page.EvaluateAsync<string>("() => document.getElementById('CEPMatriz').value");
                            var telefone = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneContato').value");

                            if (endereco == "Avenida Tucunaré" && cidade == "Barueri" && nomeContato == "Carrefur" && estado == "SP" && email == "jehvittav@gmail.com"
                                && numero == "125" && bairro == "Tamboré" && complemento == "BLOCO C SALA 1 C101" && cep == "06460-020" && telefone == "(11) 95478-7456"
                                )
                            {
                                Console.WriteLine("Campos de endereço salvos corretamente!");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar campos de endereço");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar dados do representante

                            await Page.Locator("#btnRepresentantes").ClickAsync();
                            await Page.WaitForSelectorAsync("#listaRep li:nth-child(1)");
                            var nomeRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[0].replace('Nome: ', '').trim()");
                            await Task.Delay(200);
                            var emailRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[1].replace('Email: ', '').trim()");
                            await Task.Delay(200);
                            var cpfCnpjRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[2].replace('CPF/CNPJ: ', '').trim()");
                            await Task.Delay(200);
                            var participacaoRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[4].replace('Participação: ', '').trim()");
                            await Task.Delay(200);

                            if (nomeRep1 == "Jessica Vitoria Tavares" && email == "jehvittav@gmail.com" && cpfCnpjRep1 == "496.248.668-30" && participacaoRep1 == "10%")
                            {
                                Console.WriteLine("Campos de representantes salvos corretamente!");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar campos de representantes");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //cadastro de usuário

                            await Page.Locator("#btnUsuario").ClickAsync();
                            await Page.WaitForSelectorAsync("#NomeUsuarioMaster");
                            await Task.Delay(200);
                            var UsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('NomeUsuarioMaster').value");
                            await Task.Delay(200);
                            await Page.WaitForSelectorAsync("#TelefoneUsuarioMaster");
                            await Task.Delay(200);
                            var telefoneUsuMaster = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneUsuarioMaster').value");
                            await Task.Delay(200);
                            await Page.WaitForSelectorAsync("#CPFUsuarioMaster");
                            await Task.Delay(200);
                            var cpfUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('CPFUsuarioMaster').value");
                            await Task.Delay(200);
                            await Page.WaitForSelectorAsync("#EmailUsuarioMaster");
                            var emailUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('EmailUsuarioMaster').value");

                            if (UsuarioMaster == "Jessica Vitoria Tavares" && telefoneUsuMaster == "(11) 75487-5944" && cpfUsuarioMaster == "496.248.668-30" &&
                                emailUsuarioMaster == "jehvittav@gmail.com"
                                )
                            {
                                Console.WriteLine("Campos de usuário master salvos corretamente!");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar campos de usuário master");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de usuário master");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar documentos 
                            await Page.Locator("#btnDocumentacao").ClickAsync();

                            var todosArquivosEnviados = await Page.EvaluateAsync<bool>(@"() => {
                            const documentos = [
                            'Contrato/Estatuto Social (ORIGINAL)',
                            'Atas de eleição/nomeação do(s) representante(s) (CÓPIA SIMPLES)',
                            'Procuração dos representantes (CÓPIA SIMPLES)',
                            'Balanço Patrimonial + DRE (CÓPIA SIMPLES)',
                            'Documento dos Representantes (CÓPIA SIMPLES)'
                            ];
    
                           for (let doc of documentos) {
                           const listItem = Array.from(document.querySelectorAll('.form-group li'))
                          .find(item => item.textContent.includes(doc));
                           if (!listItem || !listItem.textContent.includes('Arquivo teste 2.pdf')) {
                           return false; // Retorna false se algum documento não tiver o arquivo
                           }
                           }
                           return true; // Retorna true se todos os documentos tiverem o arquivo
                           }");

                            if (todosArquivosEnviados == true)
                            {
                                Console.WriteLine("Documentos anexados");
                                formularioOk++;


                            }
                            else
                            {
                                Console.WriteLine("Erro ao anexar documentos");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao anexar documentos");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";

                            }

                            if (formularioOk == 5)
                            {
                                fluxoDeCadastros.FormularioCompletoNoPortal = "✅";

                            }
                            else
                            {
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                                errosTotais2++;
                            }

                            var status = Repository.Correntistas.CorrentistaRepository.VerificarStatus("jehvittav@gmail.com", "45543915000181");

                            if (status == true)
                            {

                                fluxoDeCadastros.StatusEmAnalise = "✅";
                            }
                            else
                            {

                                fluxoDeCadastros.StatusEmAnalise = "❌";
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao trocar status na tabela");

                            }


                            //aprovação

                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                            var primeiroTr6 = Page.Locator("#listaCorrentistas tr").First;
                            var primeiroTd6 = primeiroTr6.Locator("td").First;
                            await primeiroTd6.ClickAsync();
                            await Page.Locator("li").Filter(new() { HasText = "Cadastro Análise" }).GetByRole(AriaRole.Button).ClickAsync();
                            await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                            await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                            await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                            var primeiroTr2 = Page.Locator("#listaCorrentistas tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd2.ClickAsync();
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Análise" }).ClickAsync();
                            await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                            await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                            await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                            await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

                            await Task.Delay(30000);

                            bool statusAtual = false;

                            for (int i = 0; i < 5; i++)
                            {
                                statusAtual = CorrentistaRepository.statusAgrAss("45543915000181", "jehvittav@gmail.com");

                                if (statusAtual)
                                {
                                    Console.WriteLine("Status trocado para aguardando assinatura");
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
                                string idDocumentoAutentique = Repository.Correntistas.CorrentistaRepository.ObterDocumentosAutentique(idCorrentista);
                                var response = AssinarDocumentosAutentique.AssinarDocumento("9ad54b27a864625573ad40327a1916db61b687c3fe8641ff7f3efdc3e985d3b3", idDocumentoAutentique);

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



                            await Task.Delay(10000);


                            bool statusAgdConta = false;

                            for (int i = 0; i < 5; i++)
                            {

                                statusAgdConta = CorrentistaRepository.VerificaStatusAgdConta("45543915000181", "jehvittav@gmail.com");

                                if (statusAgdConta == true)
                                {
                                    await Page.ReloadAsync();
                                    var apagarContaBan = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("778899", "5", "Movimentacao", idCorrentista);
                                    await Page.GetByLabel("Pesquisar").ClickAsync();
                                    await Task.Delay(800);
                                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                                    var primeiroTr3 = Page.Locator("#listaCorrentistas tr").First;
                                    var primeiroTd3 = primeiroTr3.Locator("td").First;
                                    await primeiroTd3.ClickAsync();
                                    await Page.Locator($"button[onclick=\"cadastrarContaBancaria('{idCorrentista}')\"]").Nth(1).ClickAsync();
                                    await Page.Locator("#agenciaCorrentista").FillAsync("439");
                                    await Page.Locator("#contaCorrentista").FillAsync("778899");
                                    await Page.Locator("#digContaCorrentista").FillAsync("5");
                                    await Page.SelectOptionAsync("#TipoConta", "Movimentação");
                                    await Page.Locator("#observacoesToEmail").FillAsync("teste");
                                    await Page.Locator("#btnSalvarContaCorrentista").ClickAsync();
                                    await Task.Delay(10000);
                                    //verificar se enviou e-mail
                                    var emailChecker = new TestePortal.Utils.EmailChecker();
                                    bool emailChegou = await emailChecker.CheckForNotificationEmailAsync("Cadastro de Correntista - ID Banco Digital");

                                    if (emailChegou)
                                    {
                                        Console.WriteLine("E-mail com dados de cadastro de correntista chegou!");
                                        fluxoDeCadastros.EmailRecebido = "✅";
                                    }
                                    else
                                    {
                                        Console.WriteLine("E-mail com dados de cadastro de correntista não chegou.");
                                        fluxoDeCadastros.ListaErros.Add("E-mail com dados de cadastro de correntista não chegou.");
                                        errosTotais2++;
                                        fluxoDeCadastros.EmailRecebido = "❌";
                                    }


                                    await Task.Delay(8000);
                                    var statusAprovado = Repository.Correntistas.CorrentistaRepository.VerificaStsAprovado("45543915000181", "jehvittav@gmail.com");

                                    if (statusAprovado)
                                    {

                                        Console.WriteLine("Status trocado para aprovado");
                                        fluxoDeCadastros.statusAprovado = "✅";



                                    }
                                    else
                                    {

                                        Console.WriteLine("Status não foi trocado para aprovado");
                                        fluxoDeCadastros.statusAprovado = "❌";
                                        fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aprovado");
                                        errosTotais2++;

                                    }
                                    break;

                                }
                                else
                                {
                                    Console.WriteLine($"Tentativa {i + 1} de trocar status falhou. Tentando novamente...");
                                    await Task.Delay(5000);

                                    if (i == 5)
                                    {
                                        fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando conta");
                                        errosTotais2++;

                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"erro {ex.Message}");
                        }

                        var correntistaExiste = Repository.Correntistas.CorrentistaRepository.VerificaExistenciaCorrentista("jehvittav@gmail.com", "45543915000181");

                        if (correntistaExiste)
                        {
                            Console.WriteLine("Correntista adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";


                            var idCorrentista = Repository.Correntistas.CorrentistaRepository.ObterIdCorrentista("jehvittav@gmail.com", "45543915000181");

                            var apagarCorrentista = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");
                            if (apagarCorrentista)
                            {
                                Console.WriteLine("Correntista apagado com sucesso");
                                pagina.Excluir = "✅";

                                var apagarContaBan = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("778899", "5", "Movimentacao", idCorrentista);

                                if (apagarContaBan)
                                {


                                    Console.WriteLine("Conta bancária apagada");
                                }
                                else
                                {
                                    Console.WriteLine("Erro ao apagar conta bancária");

                                }
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Correntista");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Correntista");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
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
                    Console.Write("Erro ao carregar a página de Correntista no tópico Banco ID ");
                    Console.WriteLine(PaginaBancoIdCorrentista.Status);
                    listErros.Add("Erro ao carregar a página de Correntista no tópico Banco ID ");
                    pagina.Nome = "Banco Id - Correntista";
                    errosTotais++;
                    pagina.StatusCode = PaginaBancoIdCorrentista.Status;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }

            }
            catch (Exception ex)
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
        #endregion

        #region Cadastro de Correntista com conta escrow 
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxoDeCadastros)> CorrentistaEscrow(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int formularioOk = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();

            try
            {
                if (nivelLogado == NivelEnum.Master)
                {
                    var apagarCorrentista2 = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");
                    var PaginaBancoIdCorrentista = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Correntistas.aspx");
                    fluxoDeCadastros.Fluxo = "Correntista - Escrow";
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Correntista" }).ClickAsync();
                    await Page.Locator("#cpfcnpj").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#cpfcnpj").FillAsync("45543915000181");
                    await Task.Delay(300);
                    await Page.Locator("#tipoContaCorrentista").SelectOptionAsync(new[] { "ESCROW" });
                    await Task.Delay(300);
                    await Page.Locator("#btnValidarCorrentista").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").FillAsync("jehvittav@gmail.com");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                    await Task.Delay(400);

                    var idCorrentista = Repository.Correntistas.CorrentistaRepository.ObterIdCorrentista("jehvittav@gmail.com", "45543915000181");
                    var token = Repository.Correntistas.CorrentistaRepository.ObterToken("jehvittav@gmail.com", "45543915000181");
                    string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.CORRENTISTAESCROW"];
                    string copiedUrl = $"{baseUrl}{token}";
                    var newPage = await context.NewPageAsync();
                    await newPage.GotoAsync(copiedUrl);

                    await newPage.Locator("#NomeContato").FillAsync("carrefour");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9601-83248");
                    await Task.Delay(200);
                    await newPage.Locator("#NomeContato").ClickAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#NomeUsuarioMaster").ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#NomeUsuarioMaster").FillAsync("Jessica Vitoria Tavares");
                    await Task.Delay(200);
                    await newPage.Locator("#CPFUsuarioMaster").ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#CPFUsuarioMaster").FillAsync("49624866830");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9601-83248");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#checkTermos").CheckAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(300);
                    await newPage.CloseAsync();
                    await Task.Delay(3700);
                    await Page.ReloadAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd = primeiroTr.Locator("td").First;
                    await primeiroTd.ClickAsync();
                    var button = Page.Locator($"tr.child button[onclick=\"ModalResumoCorrentista('{idCorrentista}')\"]");

                    if (await button.CountAsync() > 0)
                    {
                        await button.ClickAsync();
                        fluxoDeCadastros.Formulario = "✅";
                        Console.WriteLine("Botão de resumo de correntista encontrado.");

                    }
                    else
                    {
                        fluxoDeCadastros.Formulario = "❌";
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao encontrar botão para acessar resumo do correntista.");
                    }

                    //verificar dados da razão social 

                    await Task.Delay(300);
                    var razaoSocial = await Page.EvaluateAsync<string>("() => document.getElementById('RazaoSocial').value");
                    var cnpj = await Page.EvaluateAsync<string>("() => document.getElementById('CNPJEmpresa').value");
                    var dataConst = await Page.EvaluateAsync<string>("() => document.getElementById('DataConstituicao').value");

                    if (razaoSocial == "CARREFOUR COMERCIO E INDUSTRIA LTDA" && cnpj == "45.543.915/0001-81" && dataConst == "21/06/1974")
                    {
                        Console.WriteLine("Resumo do correntista salvo corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar dados de resumo correntista");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Dados de correntista não foram salvos corretamente.");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //verificar endereço 

                    //verificar dados de endereço 
                    await Task.Delay(200);
                    await Page.Locator("#btnEndereco").ClickAsync();
                    var endereco = await Page.EvaluateAsync<string>("() => document.getElementById('EnderecoMatriz').value");
                    await Task.Delay(200);
                    var cidade = await Page.EvaluateAsync<string>("() => document.getElementById('CidadeMatriz').value");
                    await Task.Delay(200);
                    var nomeContato = await Page.EvaluateAsync<string>("() => document.getElementById('NomeContato').value");
                    await Task.Delay(200);
                    var estado = await Page.EvaluateAsync<string>("() => document.getElementById('EstadoMatriz').value");
                    await Task.Delay(200);
                    var email = await Page.EvaluateAsync<string>("() => document.getElementById('EmailContato').value");
                    await Task.Delay(200);
                    var numero = await Page.EvaluateAsync<string>("() => document.getElementById('NumeroMatriz').value");
                    await Task.Delay(200);
                    var bairro = await Page.EvaluateAsync<string>("() => document.getElementById('BairroMatriz').value");
                    await Task.Delay(200);
                    var cep = await Page.EvaluateAsync<string>("() => document.getElementById('CEPMatriz').value");
                    var telefone = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneContato').value");

                    if (endereco == "Avenida Tucunaré" && cidade == "Barueri" && nomeContato == "carrefour" && estado == "SP" && email == "jehvittav@gmail.com"
                        && numero == "125" && bairro == "Tamboré" && cep == "06460-020" && telefone == "(11) 96018-3248"
                        )
                    {
                        Console.WriteLine("Campos de endereço salvos corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar campos de endereço");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //verificar usuário master
                    await Task.Delay(200);
                    await Page.Locator("#btnUsuario").ClickAsync();
                    await Page.WaitForSelectorAsync("#NomeUsuarioMaster");
                    await Task.Delay(200);
                    var UsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('NomeUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#TelefoneUsuarioMaster");
                    await Task.Delay(200);
                    var telefoneUsuMaster = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#CPFUsuarioMaster");
                    await Task.Delay(200);
                    var cpfUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('CPFUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#EmailUsuarioMaster");
                    var emailUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('EmailUsuarioMaster').value");

                    if (UsuarioMaster == "Jessica Vitoria Tavares" && telefoneUsuMaster == "(11) 96018-3248" && cpfUsuarioMaster == "496.248.668-30" &&
                        emailUsuarioMaster == "jehvittav@gmail.com"
                        )
                    {
                        Console.WriteLine("Campos de usuário master salvos corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar campos de usuário master");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de usuário master");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    var status = Repository.Correntistas.CorrentistaRepository.VerificarStatus("jehvittav@gmail.com", "45543915000181");

                    if (status == true)
                    {

                        fluxoDeCadastros.StatusEmAnalise = "✅";
                    }
                    else
                    {

                        fluxoDeCadastros.StatusEmAnalise = "❌";
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao trocar status na tabela");

                    }

                    if (formularioOk == 3)
                    {
                        fluxoDeCadastros.FormularioCompletoNoPortal = "✅";

                    }
                    else
                    {
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                        errosTotais2++;

                    }

                    fluxoDeCadastros.DocumentoAssinado = "❓";
                    await Page.ReloadAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr6 = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd6 = primeiroTr6.Locator("td").First;
                    await primeiroTd6.ClickAsync();
                    await Page.Locator("li").Filter(new() { HasText = "Cadastro Análise" }).GetByRole(AriaRole.Button).ClickAsync();
                    await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr2 = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd2 = primeiroTr.Locator("td").First;
                    await primeiroTd2.ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Análise" }).ClickAsync();
                    await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

                    await Task.Delay(30000);


                    bool statusAgdConta = false;

                    for (int i = 0; i < 5; i++)
                    {

                        statusAgdConta = CorrentistaRepository.VerificaStatusAgdConta("45543915000181", "jehvittav@gmail.com");

                        if (statusAgdConta == true)
                        {
                            await Page.ReloadAsync();
                            var apagarContaBan = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("778899", "5", "Escrow", idCorrentista);
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                            var primeiroTr3 = Page.Locator("#listaCorrentistas tr").First;
                            var primeiroTd3 = primeiroTr3.Locator("td").First;
                            await primeiroTd3.ClickAsync();
                            await Page.Locator($"button[onclick=\"cadastrarContaBancaria('{idCorrentista}')\"]").Nth(1).ClickAsync();
                            await Page.Locator("#agenciaCorrentista").FillAsync("439");
                            await Page.Locator("#contaCorrentista").FillAsync("778899");
                            await Page.Locator("#digContaCorrentista").FillAsync("5");
                            await Page.SelectOptionAsync("#TipoConta", "Escrow");
                            await Page.Locator("#observacoesToEmail").FillAsync("teste");
                            await Page.Locator("#btnSalvarContaCorrentista").ClickAsync();
                            await Task.Delay(10000);

                            //verificar se enviou e-mail
                            var emailChecker = new TestePortal.Utils.EmailChecker();
                            bool emailChegou = await emailChecker.CheckForNotificationEmailAsync("Cadastro de Correntista - ID Banco Digital");

                            if (emailChegou)
                            {
                                Console.WriteLine("E-mail com dados de cadastro de correntista chegou!");
                                fluxoDeCadastros.EmailRecebido = "✅";
                            }
                            else
                            {
                                Console.WriteLine("E-mail com dados de cadastro de correntista não chegou.");
                                fluxoDeCadastros.ListaErros.Add("E-mail com dados de cadastro de correntista não chegou.");
                                errosTotais2++;
                                fluxoDeCadastros.EmailRecebido = "❌";
                            }


                            await Task.Delay(8000);
                            var statusAprovado = Repository.Correntistas.CorrentistaRepository.VerificaStsAprovado("45543915000181", "jehvittav@gmail.com");

                            if (statusAprovado)
                            {

                                Console.WriteLine("Status trocado para aprovado");
                                fluxoDeCadastros.statusAprovado = "✅";



                            }
                            else
                            {

                                Console.WriteLine("Status não foi trocado para aprovado");
                                fluxoDeCadastros.statusAprovado = "❌";
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aprovado");
                                errosTotais2++;

                            }
                            break;

                        }
                        else
                        {
                            Console.WriteLine($"Tentativa {i + 1} de trocar status falhou. Tentando novamente...");
                            await Task.Delay(5000);

                            if (i == 5)
                            {
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando conta");
                                errosTotais2++;

                            }
                        }
                    }


                    var correntistaExiste = Repository.Correntistas.CorrentistaRepository.VerificaExistenciaCorrentista("jehvittav@gmail.com", "45543915000181");

                    if (correntistaExiste)
                    {
                        Console.WriteLine("Correntista adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";


                        var apagarCorrentista = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");
                        if (apagarCorrentista)
                        {
                            Console.WriteLine("Correntista apagado com sucesso");
                            pagina.Excluir = "✅";

                            var apagarContaBan = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("778899", "5", "Escrow",idCorrentista);

                            if (apagarContaBan)
                            {


                                Console.WriteLine("Conta bancária apagada");
                            }
                            else
                            {
                                Console.WriteLine("Erro ao apagar conta bancária");

                            }
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Correntista");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Correntista");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }


                }
                else
                {
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
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
        #endregion

        #region Cadastro de correntista com conta cobrança
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxoDeCadastros)> CorrentistaCobranca(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {

            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int formularioOk = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            var apagarCorrentista2 = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");

            try
            {
                var PaginaBancoIdCorrentista = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Correntistas.aspx");
                fluxoDeCadastros.Fluxo = "Correntista - Cobrança";
                await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Correntista" }).ClickAsync();
                await Page.Locator("#cpfcnpj").ClickAsync();
                await Task.Delay(300);
                await Page.Locator("#cpfcnpj").FillAsync("45543915000181");
                await Task.Delay(300);
                await Page.Locator("#tipoContaCorrentista").SelectOptionAsync(new[] { "COBRANCA" });
                await Task.Delay(300);
                await Page.Locator("#btnValidarCorrentista").ClickAsync();
                await Task.Delay(300);
                await Page.Locator("#paginaModalPJ #emailEmpresa").ClickAsync();
                await Task.Delay(300);
                await Page.Locator("#paginaModalPJ #emailEmpresa").FillAsync("jehvittav@gmail.com");
                await Task.Delay(300);
                await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                await Task.Delay(400);

                var idCorrentista = Repository.Correntistas.CorrentistaRepository.ObterIdCorrentista("jehvittav@gmail.com", "45543915000181");
                var token = Repository.Correntistas.CorrentistaRepository.ObterToken("jehvittav@gmail.com", "45543915000181");
                string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.CORRENTISTA"];
                string copiedUrl = $"{baseUrl}{token}";
                var newPage = await context.NewPageAsync();
                await newPage.GotoAsync(copiedUrl);
                await Task.Delay(200);
                await newPage.Locator("#NomeContato").ClickAsync();
                await Task.Delay(200);
                await newPage.Locator("#NomeContato").FillAsync("Carrefur");
                await Task.Delay(200);
                await Task.Delay(200);
                await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9547-87456");
                await Task.Delay(200);
                await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                await newPage.Locator("#NomeRepresentante").ClickAsync();
                await newPage.Locator("#NomeRepresentante").FillAsync("Jessica Vitoria Tavares");
                await Task.Delay(200);
                await newPage.Locator("#CPFCNPJRepresentante").ClickAsync();
                await newPage.Locator("#CPFCNPJRepresentante").FillAsync("49624866830");
                await Task.Delay(200);
                await newPage.Locator("#EmailRepresentante").FillAsync("jehvittav@gmail.com");
                await Task.Delay(200);
                await newPage.GetByPlaceholder("0").ClickAsync();
                await Task.Delay(200);
                await newPage.GetByPlaceholder("0").FillAsync("50");
                await Task.Delay(200);
                await newPage.GetByRole(AriaRole.Button, new() { Name = "+ Adicionar Representante" }).ClickAsync();
                await Task.Delay(200);
                await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                await Task.Delay(200);
                await newPage.Locator("#NomeUsuarioMaster").ClickAsync();
                await Task.Delay(200);
                await newPage.Locator("#NomeUsuarioMaster").FillAsync("Jessica Vitoria Tavares");
                await Task.Delay(200);
                await newPage.Locator("#CPFUsuarioMaster").ClickAsync();
                await Task.Delay(200);
                await newPage.Locator("#CPFUsuarioMaster").FillAsync("49624866830");
                await Task.Delay(200);
                await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                await Task.Delay(200);
                await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9547-86244");
                await newPage.Locator("#EmailUsuarioMaster").ClickAsync();
                await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                await newPage.Locator("#input-Contrato").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                await Task.Delay(200);
                await newPage.Locator("#input-Atas").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                await Task.Delay(200);
                await newPage.Locator("#input-Procuracoes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                await Task.Delay(200);
                await newPage.Locator("#input-BalancoPatrimonial").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                await Task.Delay(200);
                await newPage.Locator("#input-DocRepresentantes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                await newPage.Locator("input[name=\"assinaRepresentantes\"]").CheckAsync();
                await newPage.Locator("#checkTermos").CheckAsync();
                await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                await Task.Delay(300);
                await newPage.CloseAsync();
                await Task.Delay(6700);
                await Page.ReloadAsync();
                await Page.GetByLabel("Pesquisar").ClickAsync();
                await Task.Delay(800);
                await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                var primeiroTr = Page.Locator("#listaCorrentistas tr").First;
                var primeiroTd = primeiroTr.Locator("td").First;
                await primeiroTd.ClickAsync();
                var button = Page.Locator($"tr.child button[onclick=\"ModalResumoCorrentista('{idCorrentista}')\"]");

                if (await button.CountAsync() > 0)
                {
                    await button.ClickAsync();
                    fluxoDeCadastros.Formulario = "✅";
                    Console.WriteLine("Botão de resumo de correntista encontrado.");

                }
                else
                {
                    fluxoDeCadastros.Formulario = "❌";
                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    errosTotais2++;
                    fluxoDeCadastros.ListaErros.Add("Erro ao encontrar botão para acessar resumo do correntista.");
                }

                //verificar dados da razão social 

                await Task.Delay(300);
                var razaoSocial = await Page.EvaluateAsync<string>("() => document.getElementById('RazaoSocial').value");
                var cnpj = await Page.EvaluateAsync<string>("() => document.getElementById('CNPJEmpresa').value");
                var dataConst = await Page.EvaluateAsync<string>("() => document.getElementById('DataConstituicao').value");

                if (razaoSocial == "CARREFOUR COMERCIO E INDUSTRIA LTDA" && cnpj == "45.543.915/0001-81" && dataConst == "21/06/1974")
                {
                    Console.WriteLine("Resumo do correntista salvo corretamente!");
                    formularioOk++;

                }
                else
                {
                    Console.WriteLine("Erro ao salvar dados de resumo correntista");
                    errosTotais2++;
                    fluxoDeCadastros.ListaErros.Add("Dados de correntista não foram salvos corretamente.");
                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                }

                //verificar dados de endereço 
                await Task.Delay(400);
                await Page.Locator("#btnEndereco").ClickAsync();

                var endereco = await Page.EvaluateAsync<string>("() => document.getElementById('EnderecoMatriz').value");
                await Task.Delay(200);
                var cidade = await Page.EvaluateAsync<string>("() => document.getElementById('CidadeMatriz').value");
                await Task.Delay(200);
                var nomeContato = await Page.EvaluateAsync<string>("() => document.getElementById('NomeContato').value");
                await Task.Delay(200);
                var estado = await Page.EvaluateAsync<string>("() => document.getElementById('EstadoMatriz').value");
                await Task.Delay(200);
                var email = await Page.EvaluateAsync<string>("() => document.getElementById('EmailContato').value");
                await Task.Delay(200);
                var numero = await Page.EvaluateAsync<string>("() => document.getElementById('NumeroMatriz').value");
                await Task.Delay(200);
                var bairro = await Page.EvaluateAsync<string>("() => document.getElementById('BairroMatriz').value");
                await Task.Delay(200);
                var complemento = await Page.EvaluateAsync<string>("() => document.getElementById('ComplementoMatriz').value");
                await Task.Delay(200);
                var cep = await Page.EvaluateAsync<string>("() => document.getElementById('CEPMatriz').value");
                var telefone = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneContato').value");

                if (endereco == "Avenida Tucunaré" && cidade == "Barueri" && nomeContato == "Carrefur" && estado == "SP" && email == "jehvittav@gmail.com"
                    && numero == "125" && bairro == "Tamboré" && complemento == "BLOCO C SALA 1 C101" && cep == "06460-020" && telefone == "(11) 95478-7456"
                    )
                {
                    Console.WriteLine("Campos de endereço salvos corretamente!");
                    formularioOk++;

                }
                else
                {
                    Console.WriteLine("Erro ao salvar campos de endereço");
                    errosTotais2++;
                    fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                }

                //verificar dados do representante
                await Task.Delay(300);
                await Page.Locator("#btnRepresentantes").ClickAsync();
                await Page.WaitForSelectorAsync("#listaRep li:nth-child(1)");
                var nomeRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[0].replace('Nome: ', '').trim()");
                await Task.Delay(200);
                var emailRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[1].replace('Email: ', '').trim()");
                await Task.Delay(200);
                var cpfCnpjRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[2].replace('CPF/CNPJ: ', '').trim()");
                await Task.Delay(200);
                var participacaoRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[4].replace('Participação: ', '').trim()");
                await Task.Delay(200);

                if (nomeRep1 == "Jessica Vitoria Tavares" && emailRep1 == "jehvittav@gmail.com" && cpfCnpjRep1 == "496.248.668-30" && participacaoRep1 == "50%")
                {
                    Console.WriteLine("Campos de representantes salvos corretamente!");
                    formularioOk++;

                }
                else
                {
                    Console.WriteLine("Erro ao salvar campos de representantes");
                    errosTotais2++;
                    fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                }

                //cadastro de usuário
                await Task.Delay(300);
                await Page.Locator("#btnUsuario").ClickAsync();
                await Page.WaitForSelectorAsync("#NomeUsuarioMaster");
                await Task.Delay(200);
                var UsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('NomeUsuarioMaster').value");
                await Task.Delay(200);
                await Page.WaitForSelectorAsync("#TelefoneUsuarioMaster");
                await Task.Delay(200);
                var telefoneUsuMaster = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneUsuarioMaster').value");
                await Task.Delay(200);
                await Page.WaitForSelectorAsync("#CPFUsuarioMaster");
                await Task.Delay(200);
                var cpfUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('CPFUsuarioMaster').value");
                await Task.Delay(200);
                await Page.WaitForSelectorAsync("#EmailUsuarioMaster");
                var emailUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('EmailUsuarioMaster').value");

                if (UsuarioMaster == "Jessica Vitoria Tavares" && telefoneUsuMaster == "(11) 95478-6244" && cpfUsuarioMaster == "496.248.668-30" &&
                    emailUsuarioMaster == "jehvittav@gmail.com"
                    )
                {
                    Console.WriteLine("Campos de usuário master salvos corretamente!");
                    formularioOk++;

                }
                else
                {
                    Console.WriteLine("Erro ao salvar campos de usuário master");
                    errosTotais2++;
                    fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de usuário master");
                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                }

                //verificar documentos 
                await Page.Locator("#btnDocumentacao").ClickAsync();

                var todosArquivosEnviados = await Page.EvaluateAsync<bool>(@"() => {
                            const documentos = [
                            'Contrato/Estatuto Social (ORIGINAL)',
                            'Atas de eleição/nomeação do(s) representante(s) (CÓPIA SIMPLES)',
                            'Procuração dos representantes (CÓPIA SIMPLES)',
                            'Balanço Patrimonial + DRE (CÓPIA SIMPLES)',
                            'Documento dos Representantes (CÓPIA SIMPLES)'
                            ];
    
                           for (let doc of documentos) {
                           const listItem = Array.from(document.querySelectorAll('.form-group li'))
                          .find(item => item.textContent.includes(doc));
                           if (!listItem || !listItem.textContent.includes('Arquivo teste 2.pdf')) {
                           return false; // Retorna false se algum documento não tiver o arquivo
                           }
                           }
                           return true; // Retorna true se todos os documentos tiverem o arquivo
                           }");

                if (todosArquivosEnviados == true)
                {
                    Console.WriteLine("Documentos anexados");
                    formularioOk++;



                }
                else
                {
                    Console.WriteLine("Erro ao anexar documentos");
                    errosTotais2++;
                    fluxoDeCadastros.ListaErros.Add("Erro ao anexar documentos");
                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";


                }



                if (formularioOk == 5)
                {
                    fluxoDeCadastros.FormularioCompletoNoPortal = "✅";
                    Console.WriteLine("Formulario Completo");
                }
                else
                {
                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                    errosTotais2++;

                }

                var status = Repository.Correntistas.CorrentistaRepository.VerificarStatus("jehvittav@gmail.com", "45543915000181");

                if (status == true)
                {

                    fluxoDeCadastros.StatusEmAnalise = "✅";
                }
                else
                {

                    fluxoDeCadastros.StatusEmAnalise = "❌";
                    errosTotais2++;
                    fluxoDeCadastros.ListaErros.Add("Erro ao trocar status na tabela");

                }


                //aprovação

                await Page.ReloadAsync();
                await Page.GetByLabel("Pesquisar").ClickAsync();
                await Task.Delay(800);
                await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                var primeiroTr6 = Page.Locator("#listaCorrentistas tr").First;
                var primeiroTd6 = primeiroTr6.Locator("td").First;
                await primeiroTd6.ClickAsync();
                await Page.Locator("li").Filter(new() { HasText = "Cadastro Análise" }).GetByRole(AriaRole.Button).ClickAsync();
                await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                await Page.GetByLabel("Pesquisar").ClickAsync();
                await Task.Delay(800);
                await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                var primeiroTr2 = Page.Locator("#listaCorrentistas tr").First;
                var primeiroTd2 = primeiroTr.Locator("td").First;
                await primeiroTd2.ClickAsync();
                await Page.GetByRole(AriaRole.Button, new() { Name = "Análise" }).ClickAsync();
                await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

                await Task.Delay(30000);


                bool statusAtual = false;

                for (int i = 0; i < 5; i++)
                {
                    statusAtual = CorrentistaRepository.statusAgrAss("45543915000181", "jehvittav@gmail.com");

                    if (statusAtual)
                    {
                        Console.WriteLine("Status trocado para aguardando assinatura");
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

                    string idDocumentoAutentique = Repository.Correntistas.CorrentistaRepository.ObterDocumentosAutentique(idCorrentista);
                    var response = AssinarDocumentosAutentique.AssinarDocumento("9ad54b27a864625573ad40327a1916db61b687c3fe8641ff7f3efdc3e985d3b3", idDocumentoAutentique);

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


                    await Task.Delay(10000);


                    bool statusAgdConta = false;

                    for (int i = 0; i < 5; i++)
                    {

                        statusAgdConta = CorrentistaRepository.VerificaStatusAgdConta("45543915000181", "jehvittav@gmail.com");

                        if (statusAgdConta == true)
                        
                        {
                            var apagarContaBancaria = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("121212", "5", "Cobranca", idCorrentista);
                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                            var primeiroTr3 = Page.Locator("#listaCorrentistas tr").First;
                            var primeiroTd3 = primeiroTr3.Locator("td").First;
                            await primeiroTd3.ClickAsync();
                            await Page.Locator($"button[onclick=\"cadastrarContaBancaria('{idCorrentista}')\"]").Nth(1).ClickAsync();
                            await Page.Locator("#agenciaCorrentista").FillAsync("439");
                            await Page.Locator("#contaCorrentista").FillAsync("121212");
                            await Page.Locator("#digContaCorrentista").FillAsync("5");
                            await Page.SelectOptionAsync("#TipoConta", "Cobrança");
                            await Page.Locator("#observacoesToEmail").FillAsync("teste");
                            await Page.Locator("#btnSalvarContaCorrentista").ClickAsync();
                            await Task.Delay(10000);
                            //verificar se enviou e-mail
                            var emailChecker = new TestePortal.Utils.EmailChecker();
                            bool emailChegou = await emailChecker.CheckForNotificationEmailAsync("Cadastro de Correntista - ID Banco Digital");

                            if (emailChegou)
                            {
                                Console.WriteLine("E-mail com dados de cadastro de correntista chegou!");
                                fluxoDeCadastros.EmailRecebido = "✅";
                                break;
                            }
                            else
                            {
                                Console.WriteLine("E-mail com dados de cadastro de correntista não chegou.");
                                fluxoDeCadastros.ListaErros.Add("E-mail com dados de cadastro de correntista não chegou.");
                                errosTotais2++;
                                fluxoDeCadastros.EmailRecebido = "❌";
                                await Task.Delay(8000);
                                break;
                            }


                            

                        }
                        else
                        {
                            Console.WriteLine($"Tentativa {i + 1} de trocar status falhou. Tentando novamente...");
                            await Task.Delay(5000);

                            if (i == 5)
                            {
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando conta");
                                errosTotais2++;

                            }
                        }
                    }

                    for (int tentativa = 1; tentativa <= 5; tentativa++)
                    {

                        var statusAprovado = Repository.Correntistas.CorrentistaRepository.VerificaStsAprovado("45543915000181", "jehvittav@gmail.com");

                        if (statusAprovado)
                        {
                            Console.WriteLine("Status trocado para aprovado");
                            fluxoDeCadastros.statusAprovado = "✅";
                            break;
                        }
                        else
                        {
                            tentativa++;
                        }

                        await Task.Delay(5000);


                        if (tentativa == 5 && statusAprovado == false)
                        {
                            Console.WriteLine("Status não foi trocado para aprovado");
                            fluxoDeCadastros.statusAprovado = "❌";
                            fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aprovado");
                            errosTotais2++;
                        }
                    }


                }
                else
                {
                    Console.WriteLine("Status não foi trocado para aguardando assinatura");
                    fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando assinatura");
                }


                var correntistaExiste = Repository.Correntistas.CorrentistaRepository.VerificaExistenciaCorrentista("jehvittav@gmail.com", "45543915000181");

                if (correntistaExiste)
                {
                    Console.WriteLine("Correntista adicionado com sucesso na tabela.");
                    pagina.InserirDados = "✅";

                    var apagarCorrentista = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");
                    if (apagarCorrentista)
                    {
                        Console.WriteLine("Correntista apagado com sucesso");
                        pagina.Excluir = "✅";

                        var apagarContaBancaria = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("121212", "5", "Cobranca",idCorrentista);

                        if (apagarContaBancaria)
                        {
                            Console.WriteLine("Conta bancária apagada");

                        } else
                        {
                            fluxoDeCadastros.ListaErros.Add("Erro ao apagar conta bancária");
                            errosTotais2++;

                        }


                    }
                    else
                    {
                        Console.WriteLine("Não foi possível apagar Correntista");
                        pagina.Excluir = "❌";
                        errosTotais++;
                    }

                }
                else
                {
                    Console.WriteLine("Não foi possível inserir Correntista");
                    pagina.InserirDados = "❌";
                    pagina.Excluir = "❌";
                    errosTotais += 2;
                }

            }
            catch (Exception ex)
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

        #endregion

        #region Cadastro de correntista com conta selic
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxoDeCadastros)> CorrentistaSelic(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int formularioOk = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            var apagarCorrentista2 = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");

            if (nivelLogado == NivelEnum.Master)
            {
                var PaginaBancoIdCorrentista = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Correntistas.aspx");
                try
                {
                    fluxoDeCadastros.Fluxo = "Correntista - Selic";
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Correntista" }).ClickAsync();
                    await Page.Locator("#cpfcnpj").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#cpfcnpj").FillAsync("45543915000181");
                    await Task.Delay(300);
                    await Page.Locator("#tipoContaCorrentista").SelectOptionAsync(new[] { "SELIC" });
                    await Task.Delay(300);
                    await Page.Locator("#btnValidarCorrentista").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").FillAsync("jehvittav@gmail.com");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                    var idCorrentista = Repository.Correntistas.CorrentistaRepository.ObterIdCorrentista("jehvittav@gmail.com", "45543915000181");
                    var token = Repository.Correntistas.CorrentistaRepository.ObterToken("jehvittav@gmail.com", "45543915000181");
                    string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.CORRENTISTAESCROW"];
                    string copiedUrl = $"{baseUrl}{token}";
                    var newPage = await context.NewPageAsync();
                    await newPage.GotoAsync(copiedUrl);

                    await newPage.Locator("#NomeContato").FillAsync("carrefour");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9601-83248");
                    await Task.Delay(200);
                    await newPage.Locator("#NomeContato").ClickAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#NomeUsuarioMaster").ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#NomeUsuarioMaster").FillAsync("Jessica Vitoria Tavares");
                    await Task.Delay(200);
                    await newPage.Locator("#CPFUsuarioMaster").ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#CPFUsuarioMaster").FillAsync("49624866830");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9601-83248");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#checkTermos").CheckAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(300);
                    await newPage.CloseAsync();
                    await Task.Delay(4700);
                    await Page.ReloadAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd = primeiroTr.Locator("td").First;
                    await primeiroTd.ClickAsync();
                    var button = Page.Locator($"tr.child button[onclick=\"ModalResumoCorrentista('{idCorrentista}')\"]");

                    if (await button.CountAsync() > 0)
                    {
                        await button.ClickAsync();
                        fluxoDeCadastros.Formulario = "✅";
                        Console.WriteLine("Botão de resumo de correntista encontrado.");

                    }
                    else
                    {
                        fluxoDeCadastros.Formulario = "❌";
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao encontrar botão para acessar resumo do correntista.");
                    }

                    //verificar dados da razão social 

                    await Task.Delay(300);
                    var razaoSocial = await Page.EvaluateAsync<string>("() => document.getElementById('RazaoSocial').value");
                    var cnpj = await Page.EvaluateAsync<string>("() => document.getElementById('CNPJEmpresa').value");
                    var dataConst = await Page.EvaluateAsync<string>("() => document.getElementById('DataConstituicao').value");

                    if (razaoSocial == "CARREFOUR COMERCIO E INDUSTRIA LTDA" && cnpj == "45.543.915/0001-81" && dataConst == "21/06/1974")
                    {
                        Console.WriteLine("Resumo do correntista salvo corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar dados de resumo correntista");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Dados de correntista não foram salvos corretamente.");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //verificar endereço 

                    //verificar dados de endereço 
                    await Task.Delay(200);
                    await Page.Locator("#btnEndereco").ClickAsync();
                    var endereco = await Page.EvaluateAsync<string>("() => document.getElementById('EnderecoMatriz').value");
                    await Task.Delay(200);
                    var cidade = await Page.EvaluateAsync<string>("() => document.getElementById('CidadeMatriz').value");
                    await Task.Delay(200);
                    var nomeContato = await Page.EvaluateAsync<string>("() => document.getElementById('NomeContato').value");
                    await Task.Delay(200);
                    var estado = await Page.EvaluateAsync<string>("() => document.getElementById('EstadoMatriz').value");
                    await Task.Delay(200);
                    var email = await Page.EvaluateAsync<string>("() => document.getElementById('EmailContato').value");
                    await Task.Delay(200);
                    var numero = await Page.EvaluateAsync<string>("() => document.getElementById('NumeroMatriz').value");
                    await Task.Delay(200);
                    var bairro = await Page.EvaluateAsync<string>("() => document.getElementById('BairroMatriz').value");
                    await Task.Delay(200);
                    var cep = await Page.EvaluateAsync<string>("() => document.getElementById('CEPMatriz').value");
                    var telefone = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneContato').value");

                    if (endereco == "Avenida Tucunaré" && cidade == "Barueri" && nomeContato == "carrefour" && estado == "SP" && email == "jehvittav@gmail.com"
                        && numero == "125" && bairro == "Tamboré" && cep == "06460-020" && telefone == "(11) 96018-3248"
                        )
                    {
                        Console.WriteLine("Campos de endereço salvos corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar campos de endereço");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //verificar usuário master
                    await Task.Delay(200);
                    await Page.Locator("#btnUsuario").ClickAsync();
                    await Page.WaitForSelectorAsync("#NomeUsuarioMaster");
                    await Task.Delay(200);
                    var UsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('NomeUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#TelefoneUsuarioMaster");
                    await Task.Delay(200);
                    var telefoneUsuMaster = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#CPFUsuarioMaster");
                    await Task.Delay(200);
                    var cpfUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('CPFUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#EmailUsuarioMaster");
                    var emailUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('EmailUsuarioMaster').value");

                    if (UsuarioMaster == "Jessica Vitoria Tavares" && telefoneUsuMaster == "(11) 96018-3248" && cpfUsuarioMaster == "496.248.668-30" &&
                        emailUsuarioMaster == "jehvittav@gmail.com"
                        )
                    {
                        Console.WriteLine("Campos de usuário master salvos corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar campos de usuário master");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de usuário master");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    if (formularioOk == 3)
                    {
                        fluxoDeCadastros.FormularioCompletoNoPortal = "✅";

                    }
                    else
                    {
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                        errosTotais2++;

                    }

                    var status = Repository.Correntistas.CorrentistaRepository.VerificarStatus("jehvittav@gmail.com", "45543915000181");

                    if (status == true)
                    {

                        fluxoDeCadastros.StatusEmAnalise = "✅";
                    }
                    else
                    {

                        fluxoDeCadastros.StatusEmAnalise = "❌";
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao trocar status na tabela");

                    }
                    //aprovação
                    
                    await Page.ReloadAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr6 = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd6 = primeiroTr6.Locator("td").First;
                    await primeiroTd6.ClickAsync();
                    await Page.Locator("li").Filter(new() { HasText = "Cadastro Análise" }).GetByRole(AriaRole.Button).ClickAsync();
                    await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr2 = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd2 = primeiroTr.Locator("td").First;
                    await primeiroTd2.ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Análise" }).ClickAsync();
                    await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

                    await Task.Delay(30000);

                    bool statusAgdConta = false;

                    for (int i = 0; i < 5; i++)
                    {

                        statusAgdConta = CorrentistaRepository.VerificaStatusAgdConta("45543915000181", "jehvittav@gmail.com");

                        if (statusAgdConta == true)
                        {
                            var apagarContaBan = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("778899", "5", "Selic", idCorrentista);
                            await Page.ReloadAsync();
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                            var primeiroTr3 = Page.Locator("#listaCorrentistas tr").First;
                            var primeiroTd3 = primeiroTr3.Locator("td").First;
                            await primeiroTd3.ClickAsync();
                            await Page.Locator($"button[onclick=\"cadastrarContaBancaria('{idCorrentista}')\"]").Nth(1).ClickAsync();
                            await Page.Locator("#agenciaCorrentista").FillAsync("439");
                            await Page.Locator("#contaCorrentista").FillAsync("778899");
                            await Page.Locator("#digContaCorrentista").FillAsync("5");
                            await Page.SelectOptionAsync("#TipoConta", "Selic");
                            await Page.Locator("#observacoesToEmail").FillAsync("teste");
                            await Page.Locator("#btnSalvarContaCorrentista").ClickAsync();
                            await Task.Delay(10000);

                            //verificar se enviou e-mail
                            var emailChecker = new TestePortal.Utils.EmailChecker();
                            bool emailChegou = await emailChecker.CheckForNotificationEmailAsync("Cadastro de Correntista - ID Banco Digital");

                            if (emailChegou)
                            {
                                Console.WriteLine("E-mail com dados de cadastro de correntista chegou!");
                                fluxoDeCadastros.EmailRecebido = "✅";
                            }
                            else
                            {
                                Console.WriteLine("E-mail com dados de cadastro de correntista não chegou.");
                                fluxoDeCadastros.ListaErros.Add("E-mail com dados de cadastro de correntista não chegou.");
                                errosTotais2++;
                                fluxoDeCadastros.EmailRecebido = "❌";
                            }


                            await Task.Delay(8000);
                            var statusAprovado = Repository.Correntistas.CorrentistaRepository.VerificaStsAprovado("45543915000181", "jehvittav@gmail.com");

                            if (statusAprovado)
                            {

                                Console.WriteLine("Status trocado para aprovado");
                                fluxoDeCadastros.statusAprovado = "✅";



                            }
                            else
                            {

                                Console.WriteLine("Status não foi trocado para aprovado");
                                fluxoDeCadastros.statusAprovado = "❌";
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aprovado");
                                errosTotais2++;

                            }
                            break;

                        }
                        else
                        {
                            Console.WriteLine($"Tentativa {i + 1} de trocar status falhou. Tentando novamente...");
                            await Task.Delay(5000);

                            if (i == 5)
                            {
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando conta");
                                errosTotais2++;

                            }
                        }
                    }


                    var correntistaExiste = Repository.Correntistas.CorrentistaRepository.VerificaExistenciaCorrentista("jehvittav@gmail.com", "45543915000181");

                    if (correntistaExiste)
                    {
                        Console.WriteLine("Correntista adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        fluxoDeCadastros.DocumentoAssinado = "❓";

                        var apagarCorrentista = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");
                        if (apagarCorrentista)
                        {
                            Console.WriteLine("Correntista apagado com sucesso");
                            pagina.Excluir = "✅";

                            var apagarContaBan = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("778899", "5", "Selic", idCorrentista);

                            if (apagarContaBan)
                            {


                                Console.WriteLine("Conta bancária apagada");
                            }
                            else
                            {
                                Console.WriteLine("Erro ao apagar conta bancária");

                            }
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Correntista");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Correntista");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }



                }
                catch (Exception ex)
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
            }

            pagina.TotalErros = errosTotais;
            return (pagina, fluxoDeCadastros);
        }
        #endregion

        #region Cadastro de correntista com conta Cetip 
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxoDeCadastros)> CorrentistaCetip(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int formularioOk = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            var apagarCorrentista2 = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");

            if (nivelLogado == NivelEnum.Master)
            {

                var PaginaBancoIdCorrentista = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Correntistas.aspx");

                try

                {

                    fluxoDeCadastros.Fluxo = "Correntista - Cetip";
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Correntista" }).ClickAsync();
                    await Page.Locator("#cpfcnpj").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#cpfcnpj").FillAsync("45543915000181");
                    await Task.Delay(300);
                    await Page.Locator("#tipoContaCorrentista").SelectOptionAsync(new[] { "CETIP" });
                    await Task.Delay(300);
                    await Page.Locator("#btnValidarCorrentista").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").FillAsync("jehvittav@gmail.com");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                    await Task.Delay(400);

                    var idCorrentista = Repository.Correntistas.CorrentistaRepository.ObterIdCorrentista("jehvittav@gmail.com", "45543915000181");
                    var token = Repository.Correntistas.CorrentistaRepository.ObterToken("jehvittav@gmail.com", "45543915000181");
                    string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.CORRENTISTA"];
                    string copiedUrl = $"{baseUrl}{token}";
                    var newPage = await context.NewPageAsync();
                    await newPage.GotoAsync(copiedUrl);

                    await Task.Delay(200);
                    await newPage.Locator("#NomeContato").ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#NomeContato").FillAsync("Carrefur");
                    await Task.Delay(200);
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 9547-87456");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await newPage.Locator("#NomeRepresentante").ClickAsync();
                    await newPage.Locator("#NomeRepresentante").FillAsync("Jessica Vitoria Tavares");
                    await Task.Delay(200);
                    await newPage.Locator("#CPFCNPJRepresentante").ClickAsync();
                    await newPage.Locator("#CPFCNPJRepresentante").FillAsync("49624866830");
                    await Task.Delay(200);
                    await newPage.Locator("#EmailRepresentante").ClickAsync();
                    await newPage.Locator("#EmailRepresentante").FillAsync("jehvittav@gmail.com");
                    await Task.Delay(200);
                    await newPage.GetByPlaceholder("0").ClickAsync();
                    await newPage.GetByPlaceholder("0").FillAsync("10");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "+ Adicionar Representante" }).ClickAsync();
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await newPage.Locator("#NomeUsuarioMaster").ClickAsync();
                    await newPage.Locator("#NomeUsuarioMaster").FillAsync("Jessica Vitoria Tavares");
                    await Task.Delay(200);
                    await newPage.Locator("div:nth-child(3) > .col-md-12 > div:nth-child(2) > .form-group").ClickAsync();
                    await Task.Delay(200);
                    await newPage.Locator("#CPFUsuarioMaster").FillAsync("49624866830");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).ClickAsync();
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(DD) XXXXX-XXXX" }).FillAsync("(11) 7548-75944");
                    await Task.Delay(200);
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await newPage.Locator("#input-Contrato").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                    await Task.Delay(200);
                    await newPage.Locator("#input-Atas").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                    await Task.Delay(200);
                    await newPage.Locator("#input-Procuracoes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                    await Task.Delay(200);
                    await newPage.Locator("#input-BalancoPatrimonial").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                    await Task.Delay(200);
                    await newPage.Locator("#input-DocRepresentantes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await newPage.Locator("input[name=\"assinaRepresentantes\"]").CheckAsync();
                    await Task.Delay(300);
                    await newPage.Locator("#checkTermos").CheckAsync();
                    await newPage.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(300);
                    await newPage.CloseAsync();
                    await Task.Delay(3700);
                    await Page.ReloadAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd = primeiroTr.Locator("td").First;
                    await primeiroTd.ClickAsync();
                    var button = Page.Locator($"tr.child button[onclick=\"ModalResumoCorrentista('{idCorrentista}')\"]");

                    if (await button.CountAsync() > 0)
                    {
                        await button.ClickAsync();
                        fluxoDeCadastros.Formulario = "✅";
                        Console.WriteLine("Botão de resumo de correntista encontrado.");

                    }
                    else
                    {
                        fluxoDeCadastros.Formulario = "❌";
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao encontrar botão para acessar resumo do correntista.");
                    }

                    //verificar dados de razão social

                    await Task.Delay(300);
                    var razaoSocial = await Page.EvaluateAsync<string>("() => document.getElementById('RazaoSocial').value");
                    var cnpj = await Page.EvaluateAsync<string>("() => document.getElementById('CNPJEmpresa').value");
                    var dataConst = await Page.EvaluateAsync<string>("() => document.getElementById('DataConstituicao').value");

                    if (razaoSocial == "CARREFOUR COMERCIO E INDUSTRIA LTDA" && cnpj == "45.543.915/0001-81" && dataConst == "21/06/1974")
                    {
                        Console.WriteLine("Resumo do correntista salvo corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar dados de resumo correntista");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Dados de correntista não foram salvos corretamente.");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //verificar dados de endereço 

                    await Page.Locator("#btnEndereco").ClickAsync();

                    var endereco = await Page.EvaluateAsync<string>("() => document.getElementById('EnderecoMatriz').value");
                    await Task.Delay(200);
                    var cidade = await Page.EvaluateAsync<string>("() => document.getElementById('CidadeMatriz').value");
                    await Task.Delay(200);
                    var nomeContato = await Page.EvaluateAsync<string>("() => document.getElementById('NomeContato').value");
                    await Task.Delay(200);
                    var estado = await Page.EvaluateAsync<string>("() => document.getElementById('EstadoMatriz').value");
                    await Task.Delay(200);
                    var email = await Page.EvaluateAsync<string>("() => document.getElementById('EmailContato').value");
                    await Task.Delay(200);
                    var numero = await Page.EvaluateAsync<string>("() => document.getElementById('NumeroMatriz').value");
                    await Task.Delay(200);
                    var bairro = await Page.EvaluateAsync<string>("() => document.getElementById('BairroMatriz').value");
                    await Task.Delay(200);
                    var complemento = await Page.EvaluateAsync<string>("() => document.getElementById('ComplementoMatriz').value");
                    await Task.Delay(200);
                    var cep = await Page.EvaluateAsync<string>("() => document.getElementById('CEPMatriz').value");
                    var telefone = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneContato').value");

                    if (endereco == "Avenida Tucunaré" && cidade == "Barueri" && nomeContato == "Carrefur" && estado == "SP" && email == "jehvittav@gmail.com"
                        && numero == "125" && bairro == "Tamboré" && complemento == "BLOCO C SALA 1 C101" && cep == "06460-020" && telefone == "(11) 95478-7456"
                        )
                    {
                        Console.WriteLine("Campos de endereço salvos corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar campos de endereço");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //verificar dados do representante

                    await Page.Locator("#btnRepresentantes").ClickAsync();
                    await Page.WaitForSelectorAsync("#listaRep li:nth-child(1)");
                    var nomeRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[0].replace('Nome: ', '').trim()");
                    await Task.Delay(200);
                    var emailRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[1].replace('Email: ', '').trim()");
                    await Task.Delay(200);
                    var cpfCnpjRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[2].replace('CPF/CNPJ: ', '').trim()");
                    await Task.Delay(200);
                    var participacaoRep1 = await Page.EvaluateAsync<string>("() => document.querySelector('#listaRep li:nth-child(1) span').innerText.split(' - ')[4].replace('Participação: ', '').trim()");
                    await Task.Delay(200);

                    if (nomeRep1 == "Jessica Vitoria Tavares" && email == "jehvittav@gmail.com" && cpfCnpjRep1 == "496.248.668-30" && participacaoRep1 == "10%")
                    {
                        Console.WriteLine("Campos de representantes salvos corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar campos de representantes");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de endereço");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //cadastro de usuário

                    await Page.Locator("#btnUsuario").ClickAsync();
                    await Page.WaitForSelectorAsync("#NomeUsuarioMaster");
                    await Task.Delay(200);
                    var UsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('NomeUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#TelefoneUsuarioMaster");
                    await Task.Delay(200);
                    var telefoneUsuMaster = await Page.EvaluateAsync<string>("() => document.getElementById('TelefoneUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#CPFUsuarioMaster");
                    await Task.Delay(200);
                    var cpfUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('CPFUsuarioMaster').value");
                    await Task.Delay(200);
                    await Page.WaitForSelectorAsync("#EmailUsuarioMaster");
                    var emailUsuarioMaster = await Page.EvaluateAsync<string>("() => document.getElementById('EmailUsuarioMaster').value");

                    if (UsuarioMaster == "Jessica Vitoria Tavares" && telefoneUsuMaster == "(11) 75487-5944" && cpfUsuarioMaster == "496.248.668-30" &&
                        emailUsuarioMaster == "jehvittav@gmail.com"
                        )
                    {
                        Console.WriteLine("Campos de usuário master salvos corretamente!");
                        formularioOk++;

                    }
                    else
                    {
                        Console.WriteLine("Erro ao salvar campos de usuário master");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao salvar campos de usuário master");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                    }

                    //verificar documentos 
                    await Page.Locator("#btnDocumentacao").ClickAsync();

                    var todosArquivosEnviados = await Page.EvaluateAsync<bool>(@"() => {
                            const documentos = [
                            'Contrato/Estatuto Social (ORIGINAL)',
                            'Atas de eleição/nomeação do(s) representante(s) (CÓPIA SIMPLES)',
                            'Procuração dos representantes (CÓPIA SIMPLES)',
                            'Balanço Patrimonial + DRE (CÓPIA SIMPLES)',
                            'Documento dos Representantes (CÓPIA SIMPLES)'
                            ];
    
                           for (let doc of documentos) {
                           const listItem = Array.from(document.querySelectorAll('.form-group li'))
                          .find(item => item.textContent.includes(doc));
                           if (!listItem || !listItem.textContent.includes('Arquivo teste 2.pdf')) {
                           return false; // Retorna false se algum documento não tiver o arquivo
                           }
                           }
                           return true; // Retorna true se todos os documentos tiverem o arquivo
                           }");

                    if (todosArquivosEnviados == true)
                    {
                        Console.WriteLine("Documentos anexados");
                        formularioOk++;


                    }
                    else
                    {
                        Console.WriteLine("Erro ao anexar documentos");
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao anexar documentos");
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";

                    }

                    if (formularioOk == 5)
                    {
                        fluxoDeCadastros.FormularioCompletoNoPortal = "✅";

                    }
                    else
                    {
                        fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                        errosTotais2++;

                    }

                    var status = Repository.Correntistas.CorrentistaRepository.VerificarStatus("jehvittav@gmail.com", "45543915000181");

                    if (status == true)
                    {

                        fluxoDeCadastros.StatusEmAnalise = "✅";
                    }
                    else
                    {

                        fluxoDeCadastros.StatusEmAnalise = "❌";
                        errosTotais2++;
                        fluxoDeCadastros.ListaErros.Add("Erro ao trocar status na tabela");

                    }

                    //aprovação

                    await Page.ReloadAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr6 = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd6 = primeiroTr6.Locator("td").First;
                    await primeiroTd6.ClickAsync();
                    await Page.Locator("li").Filter(new() { HasText = "Cadastro Análise" }).GetByRole(AriaRole.Button).ClickAsync();
                    await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
                    await Page.GetByLabel("Pesquisar").ClickAsync();
                    await Task.Delay(800);
                    await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                    var primeiroTr2 = Page.Locator("#listaCorrentistas tr").First;
                    var primeiroTd2 = primeiroTr.Locator("td").First;
                    await primeiroTd2.ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Análise" }).ClickAsync();
                    await Page.Locator("label").Filter(new() { HasText = "Aprovar" }).ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").ClickAsync();
                    await Page.GetByPlaceholder("Insira sua mensagem...").FillAsync("teste");
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

                    await Task.Delay(30000);

                    bool statusAtual = false;

                    for (int i = 0; i < 5; i++)
                    {
                        statusAtual = CorrentistaRepository.statusAgrAss("45543915000181", "jehvittav@gmail.com");

                        if (statusAtual)
                        {
                            Console.WriteLine("Status trocado para aguardando assinatura");
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


                        string idDocumentoAutentique = Repository.Correntistas.CorrentistaRepository.ObterDocumentosAutentique(idCorrentista);
                        var response = AssinarDocumentosAutentique.AssinarDocumento("9ad54b27a864625573ad40327a1916db61b687c3fe8641ff7f3efdc3e985d3b3", idDocumentoAutentique);

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


                        await Task.Delay(10000);


                        bool statusAgdConta = false;

                        for (int i = 0; i < 5; i++)
                        {

                            statusAgdConta = CorrentistaRepository.VerificaStatusAgdConta("45543915000181", "jehvittav@gmail.com");

                            if (statusAgdConta == true)
                            {
                                var apagarContaBancaria = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("988998", "5", "cetip", idCorrentista);
                                await Page.ReloadAsync();
                                await Page.GetByLabel("Pesquisar").ClickAsync();
                                await Task.Delay(800);
                                await Page.GetByLabel("Pesquisar").FillAsync("jehvittav@gmail.com");
                                var primeiroTr3 = Page.Locator("#listaCorrentistas tr").First;
                                var primeiroTd3 = primeiroTr3.Locator("td").First;
                                await primeiroTd3.ClickAsync();
                                await Page.Locator($"button[onclick=\"cadastrarContaBancaria('{idCorrentista}')\"]").Nth(1).ClickAsync();
                                await Page.Locator("#agenciaCorrentista").FillAsync("439");
                                await Page.Locator("#contaCorrentista").FillAsync("988998");
                                await Page.Locator("#digContaCorrentista").FillAsync("5");
                                await Page.SelectOptionAsync("#TipoConta", "Cetip");
                                await Page.Locator("#observacoesToEmail").FillAsync("teste");
                                await Page.Locator("#btnSalvarContaCorrentista").ClickAsync();
                                await Task.Delay(10000);
                                //verificar se enviou e-mail
                                var emailChecker = new TestePortal.Utils.EmailChecker();
                                bool emailChegou = await emailChecker.CheckForNotificationEmailAsync("Cadastro de Correntista - ID Banco Digital");

                                if (emailChegou)
                                {
                                    Console.WriteLine("E-mail com dados de cadastro de correntista chegou!");
                                    fluxoDeCadastros.EmailRecebido = "✅";
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("E-mail com dados de cadastro de correntista não chegou.");
                                    fluxoDeCadastros.ListaErros.Add("E-mail com dados de cadastro de correntista não chegou.");
                                    errosTotais2++;
                                    fluxoDeCadastros.EmailRecebido = "❌";
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Tentativa {i + 1} de trocar status falhou. Tentando novamente...");
                                await Task.Delay(5000);

                                if (i == 5)
                                {
                                    fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando conta");
                                    errosTotais2++;

                                }
                            }
                        }

                        for (int tentativa = 1; tentativa <= 5; tentativa++)
                        {

                            var statusAprovado = Repository.Correntistas.CorrentistaRepository.VerificaStsAprovado("45543915000181", "jehvittav@gmail.com");

                            if (statusAprovado)
                            {
                                Console.WriteLine("Status trocado para aprovado");
                                fluxoDeCadastros.statusAprovado = "✅";
                                break;
                            }
                            else
                            {
                                tentativa++;
                            }

                            await Task.Delay(5000);


                            if (tentativa == 5 && statusAprovado == false)
                            {
                                Console.WriteLine("Status não foi trocado para aprovado");
                                fluxoDeCadastros.statusAprovado = "❌";
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aprovado");
                                errosTotais2++;
                            }
                        }


                    }
                    else
                    {
                        Console.WriteLine("Status não foi trocado para aguardando assinatura");
                        fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando assinatura");
                    }

                    var correntistaExiste = Repository.Correntistas.CorrentistaRepository.VerificaExistenciaCorrentista("jehvittav@gmail.com", "45543915000181");

                    if (correntistaExiste)
                    {
                        Console.WriteLine("Correntista adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";


                        var apagarCorrentista = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("jehvittav@gmail.com", "45543915000181");
                        if (apagarCorrentista)
                        {
                            Console.WriteLine("Correntista apagado com sucesso");
                            pagina.Excluir = "✅";


                            var apagarContaBancaria = Repository.Correntistas.CorrentistaRepository.ApagarContaBancaria("988998", "5", "cetip", idCorrentista);

                            if (apagarContaBancaria)
                            {

                                Console.WriteLine("Conta bancária pagada");

                            }
                            else
                            {
                                fluxoDeCadastros.ListaErros.Add("Erro ao apagar conta bancária associada ao correntista");
                                errosTotais2++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Correntista");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Correntista");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }

                }
                catch (Exception ex)
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

            }
            pagina.TotalErros = errosTotais;
            return (pagina, fluxoDeCadastros);

        }
        #endregion
    }
}

