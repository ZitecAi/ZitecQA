using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Model;
using TestePortal.Repository.Consultorias;
using TestePortal.Repository.GestoraInterna;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class CadastroGestorasInternas
    {
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxosDeCadastros)> GestorasInternas(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            int errosTotais2 = 0;
            var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");
            int formularioOk = 0;
            try
            {
                var CadastroGestorasInternas = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/GestoraInterno.aspx");

                if (CadastroGestorasInternas.Status == 200)
                {
                    string seletorTabela = "#tabelaGestoras";

                    Console.Write("Gestoras Internas - Cadastro: ");
                    pagina.Nome = "Gestoras Internas";
                    pagina.StatusCode = CadastroGestorasInternas.Status;
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;

                    }
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;
                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    if (nivelLogado == NivelEnum.Master)
                    {
                        fluxoDeCadastros.Fluxo = ("Gestoras");
                        fluxoDeCadastros.statusAprovado = "❓";
                        fluxoDeCadastros.DocumentoAssinado = "❓";
                        fluxoDeCadastros.EmailRecebido = "❓";
                        var apagarGestoraInterna2 = Repository.GestoraInterna.GestoraInternaRepository.ApagarGestoraInterna("16695922000109", "robo@zitec.ai");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#CnpjGestoraInterno").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#CnpjGestoraInterno").FillAsync("16695922000109");
                        await Task.Delay(300);
                        await Page.Locator("#btnAvancarCadastroGestora").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#emailGestora").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#emailGestora").FillAsync("robo@zitec.ai");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                        await Task.Delay(500);

                        await Page.ReloadAsync();
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.aim");
                        var primeiroTr = Page.Locator("#listaGestoras tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Task.Delay(400);

                        try
                        {
                            int? idGestora = GestoraInternaRepository.ObterIdGestora("16695922000109", "robo@zitec.ai");
                            var buttonSelector = $"tr.child button#\\3{idGestora.ToString().Substring(0, 1)} {idGestora.ToString().Substring(1)}_url.btn.btn-default[title='Copiar Link']";
                            await Page.Locator(buttonSelector).ClickAsync();
                            await Task.Delay(400);

                            string token = GestoraInternaRepository.ObterTokenGestora("16695922000109", "robo@zitec.ai");
                            string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.GESTORA"];
                            string copiedUrl = $"{baseUrl}{token}";
                            var newPage = await context.NewPageAsync();
                            await newPage.GotoAsync(copiedUrl);


                            await newPage.GetByLabel("*Atividade Principal (CNAE)", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Atividade Principal (CNAE)", new() { Exact = true }).FillAsync("info");
                            await Task.Delay(200);
                            await newPage.Locator("#controlAcionario").SelectOptionAsync(new[] { "nacional" });
                            await Task.Delay(200);
                            await newPage.Locator("#usPersonDadosInvestNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#irrf").SelectOptionAsync(new[] { "isento" });
                            await Task.Delay(200);
                            await newPage.Locator("#iof").SelectOptionAsync(new[] { "isento" });
                            await Task.Delay(200);
                            await newPage.Locator("#fatcaNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#ppeNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#cepComercial").FillAsync("07084-370");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Número", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Número", new() { Exact = true }).FillAsync("901");
                            await Task.Delay(200);
                            await newPage.Locator("#telComercial").FillAsync("(11)6498-4785");
                            await Task.Delay(200);
                            await newPage.Locator("#celComercial").FillAsync("(16)84848-4545");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#patrimLiq").FillAsync("100000");
                            await Task.Delay(200);
                            await newPage.Locator("#faturamento12Meses").FillAsync("1100");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Origem Capital").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Origem Capital").FillAsync("origem");
                            await Task.Delay(200);
                            await newPage.Locator("#beneficAcionistaSocNao").CheckAsync();
                            await Task.Delay(200);
                            await Task.Delay(200);
                            await newPage.Locator("#possuiDiretoresNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#benfFinaisNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome Completo", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome Completo", new() { Exact = true }).FillAsync("Jessica Vitoria Tavares");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*CPF" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*CPF" }).FillAsync("496.248.668-30");
                            await Task.Delay(200);
                            await newPage.Locator("#dtNascimentoProcurador").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#dtNascimentoProcurador").FillAsync("17/05/2004");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome da Mãe", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome da Mãe", new() { Exact = true }).FillAsync("Bernadete Maria Cassimiro ");
                            await Task.Delay(200);
                            await newPage.Locator("#situacaoLegalProcurador").SelectOptionAsync(new[] { "maior" });
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).FillAsync("teste@gmail.com");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).FillAsync("(11)65464-5454");
                            await Task.Delay(200);
                            await newPage.Locator("#redMensalProLab").FillAsync("5000");
                            await Task.Delay(200);
                            await newPage.Locator("#validIndetSim").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#vincDistribuidorNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#referente").SelectOptionAsync(new[] { "titular" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "ADICIONAR REPRESENTANTE" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#nacionalidadeProcurador").SelectOptionAsync(new[] { "br" });
                            await Task.Delay(200);
                            await newPage.Locator("#ufNascimentoProc").SelectOptionAsync(new[] { "SP" });
                            await Task.Delay(200);
                            await newPage.Locator("#natProcurador").SelectOptionAsync(new[] { "Guarulhos" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "Razão Social" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "Razão Social" }).FillAsync("raz");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Ramal", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Ramal", new() { Exact = true }).FillAsync("1234");
                            await Task.Delay(200);
                            await newPage.Locator("#partcipSocNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#descBensImovNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#outroBensInvestNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Conta Bancária Contas Bancá").GetByLabel("Default select example").SelectOptionAsync(new[] { "260" });
                            await newPage.GetByPlaceholder("0000", new() { Exact = true }).ClickAsync();
                            await newPage.GetByPlaceholder("0000", new() { Exact = true }).FillAsync("5465");
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).ClickAsync();
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).FillAsync("687467487");
                            await newPage.GetByLabel("Digito", new() { Exact = true }).ClickAsync();
                            await newPage.GetByLabel("Digito", new() { Exact = true }).FillAsync("4");
                            await newPage.GetByRole(AriaRole.Radio, new() { Name = "Não" }).CheckAsync();
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "ADICIONAR" }).ClickAsync();
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await newPage.Locator("#fileQddAmbima").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await newPage.Locator("#btnFinalizar").ClickAsync();
                            await Task.Delay(2000);
                            await newPage.CloseAsync();
                            await Page.ReloadAsync();
                            await Task.Delay(6000);
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                            var primeiroTr2 = Page.Locator("#listaGestoras tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();

                            var button = Page.Locator($"tr.child button[onclick=\"ModalResumoFormGestora('{idGestora}')\"]");

                            if (await button.CountAsync() > 0)
                            {
                                await button.ClickAsync();
                                fluxoDeCadastros.Formulario = "✅";
                                Console.WriteLine("Botão de resumo de formulário encontrado.");

                            }
                            else
                            {
                                fluxoDeCadastros.Formulario = "❌";
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao encontrar botão para acessar o resumo do formulário.");
                            }

                            //verificar gestor 


                            await Task.Delay(2000);
                            await Page.WaitForSelectorAsync("#razaoSocialResumo");
                            var razaoSocialResumo = await Page.EvaluateAsync<string>("() => document.getElementById('razaoSocialResumo').value");
                            await Page.WaitForSelectorAsync("#cnpjResumo");
                            var cnpjResumo = await Page.EvaluateAsync<string>("() => document.getElementById('cnpjResumo').value");
                            await Page.WaitForSelectorAsync("#dtConstituicaoResumo");
                            var dtConstituicaoResumo = await Page.EvaluateAsync<string>("() => document.getElementById('dtConstituicaoResumo').value");
                            await Page.WaitForSelectorAsync("#ativPrincipalResumo");
                            var ativPrincipalResumo = await Page.EvaluateAsync<string>("() => document.getElementById('ativPrincipalResumo').value");
                            await Page.WaitForSelectorAsync("#controlAcionarioResumo");
                            var controlAcionarioResumo = await Page.EvaluateAsync<string>("() => document.getElementById('controlAcionarioResumo').value");
                            await Page.WaitForSelectorAsync("#paisConstituicaoResumo");
                            var paisConstituicaoResumo = await Page.EvaluateAsync<string>("() => document.getElementById('paisConstituicaoResumo').value");
                            await Page.WaitForSelectorAsync("#paisDomFiscalResumo");
                            var paisDomFiscalResumo = await Page.EvaluateAsync<string>("() => document.getElementById('paisDomFiscalResumo').value");
                            await Page.WaitForSelectorAsync("#emailResumo");
                            var emailResumo = await Page.EvaluateAsync<string>("() => document.getElementById('emailResumo').value");
                            await Page.WaitForSelectorAsync("#usPersonDadosInvestResumoNao");
                            var usPerson = await Page.EvaluateAsync<bool>("() => document.getElementById('usPersonDadosInvestResumoNao').checked");
                            await Page.WaitForSelectorAsync("#irrfResumo");
                            var irrfResumo = await Page.EvaluateAsync<string>("() => document.getElementById('irrfResumo').value");
                            await Page.WaitForSelectorAsync("#iofResumo");
                            var iofResumo = await Page.EvaluateAsync<string>("() => document.getElementById('iofResumo').value");
                            await Page.WaitForSelectorAsync("#fatcaResumoNao");
                            var fatcaResumo = await Page.EvaluateAsync<bool>("() => document.getElementById('fatcaResumoNao').checked");
                            await Page.WaitForSelectorAsync("#ppeResumoNao");
                            var ppeResumo = await Page.EvaluateAsync<bool>("() => document.getElementById('ppeResumoNao').checked");


                            if (razaoSocialResumo == "CENTER NORTE S/A CONSTRUCAO EMPREEND ADM E PARTICIPACAO" && cnpjResumo == "45.246.402/0005-32" && dtConstituicaoResumo == dataAtual &&
                                ativPrincipalResumo == "info" && controlAcionarioResumo == "Nacional" && paisConstituicaoResumo == "BRASIL" && paisDomFiscalResumo == "BRASIL"
                                && emailResumo == "robo@zitec.ai" && usPerson == true && irrfResumo == "Isento" && iofResumo == "Isento" && fatcaResumo == true
                                && ppeResumo == true
                                )
                            {
                                
                                Console.WriteLine("Dados gestor salvos corretamente");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados de gestor");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao salvar dados de gestor");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar dados de endereço
                            await Task.Delay(200);
                            await Page.Locator("#Endereco-tabResumo").ClickAsync();

                            await Page.WaitForSelectorAsync("#paisComercialResumo");
                            var paisComercial = await Page.EvaluateAsync<string>("() => document.getElementById('paisComercialResumo').value");
                            await Page.WaitForSelectorAsync("#cepComercialResumo");
                            var cepComercial = await Page.EvaluateAsync<string>("() => document.getElementById('cepComercialResumo').value");
                            await Page.WaitForSelectorAsync("#lograComercialResumo");
                            var lograComercial = await Page.EvaluateAsync<string>("() => document.getElementById('lograComercialResumo').value");
                            await Page.WaitForSelectorAsync("#numComercialResumo");
                            var numComercial = await Page.EvaluateAsync<string>("() => document.getElementById('numComercialResumo').value");
                            await Page.WaitForSelectorAsync("#bairroComercialResumo");
                            var bairroComercial = await Page.EvaluateAsync<string>("() => document.getElementById('bairroComercialResumo').value");
                            await Page.WaitForSelectorAsync("#ufComercialResumo");
                            var ufComercial = await Page.EvaluateAsync<string>("() => document.getElementById('ufComercialResumo').value");
                            await Page.WaitForSelectorAsync("#cidadeComercialResumo");
                            var cidadeComercial = await Page.EvaluateAsync<string>("() => document.getElementById('cidadeComercialResumo').value");
                            await Page.WaitForSelectorAsync("#telComercialResumo");
                            var telComercial = await Page.EvaluateAsync<string>("() => document.getElementById('telComercialResumo').value");
                            await Page.WaitForSelectorAsync("#celComercialResumo");
                            var celComercial = await Page.EvaluateAsync<string>("() => document.getElementById('celComercialResumo').value");

                            if (paisComercial == "BRASIL" && cepComercial == "07084-370" && lograComercial == "Avenida Alexandre Grandisoli" && numComercial == "901" &&
                                bairroComercial == "Parque Continental II" && ufComercial == "São Paulo" && cidadeComercial == "Guarulhos" && telComercial == "(11)6498-4785"
                                && celComercial == "(16)84848-4545"
                                )
                            {
                                Console.WriteLine("Dados de endereço salvos corretamente");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados de endereço");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados de endereço não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar situação patrimonial 
                            await Task.Delay(200);
                            await Page.Locator("#SituacaoPatrimonialEmp-tabResumo").ClickAsync();   
                            await Page.WaitForSelectorAsync("#patrimLiqResumo");
                            var patrimLiqResumo = await Page.EvaluateAsync<string>("() => document.getElementById('patrimLiqResumo').value");
                            await Page.WaitForSelectorAsync("#faturamento12MesesResumo");
                            var faturamento12MesesResumo = await Page.EvaluateAsync<string>("() => document.getElementById('faturamento12MesesResumo').value");
                            await Page.WaitForSelectorAsync("#origemCapResumo");
                            var origemCapResumo = await Page.EvaluateAsync<string>("() => document.getElementById('origemCapResumo').value");
                            await Page.WaitForSelectorAsync("#beneficAcionistaSocResumoNao");
                            var beneficAcionista = await Page.EvaluateAsync<bool>("() => document.getElementById('beneficAcionistaSocResumoNao').checked");
                            await Page.WaitForSelectorAsync("#possuiDiretoresResumoNao");
                            var possuiDiretores = await Page.EvaluateAsync<bool>("() => document.getElementById('possuiDiretoresResumoNao').checked");
                            await Page.WaitForSelectorAsync("#benfFinaisResumoNao");
                            var benfFinais = await Page.EvaluateAsync<bool>("() => document.getElementById('benfFinaisResumoNao').checked");

                            if (patrimLiqResumo == "100000" && faturamento12MesesResumo == "1100" && origemCapResumo == "origem" && beneficAcionista == true
                                && possuiDiretores == true && benfFinais == true)
                            {
                                Console.WriteLine("Dados de situação patrimonial salvos corretamente");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados de situação patrimonial");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados de situação patrimonial não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar dados de representantes
                            await Task.Delay(200);
                            await Page.Locator("#Representantes-tabResumo").ClickAsync();
                            await Page.WaitForSelectorAsync("#tabelaProcuradorAdd");

                            var nomeProcurador = await Page.EvaluateAsync<string>(@"
                            () => {
                            const tabela = document.getElementById('tabelaProcuradorAdd');
                            const primeiraLinha = tabela.querySelector('tbody tr');
                            const colunaNome = primeiraLinha.querySelector('td:nth-child(1)');
                            return colunaNome ? colunaNome.innerText : null;
                            }
                            ");

                            if (nomeProcurador == "Jessica Vitoria Tavares")
                            {
                                Console.WriteLine("Dados de representantes salvos corretamente");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados de representantes");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados de representantes não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            // verificar conta bancária 
                            await Task.Delay(200);
                            await Page.Locator("#ContaBancaria-tabResumo").ClickAsync();

                          
                            await Page.WaitForSelectorAsync("#tabelaContasBancoResumo tbody");
                            var banco = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaContasBancoResumo tbody tr:nth-child(1) td:nth-child(1)').innerText");
                            await Task.Delay(200);
                            var agencia = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaContasBancoResumo tbody tr:nth-child(1) td:nth-child(2)').innerText");
                            await Task.Delay(200);
                            var conta = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaContasBancoResumo tbody tr:nth-child(1) td:nth-child(3)').innerText");
                            await Task.Delay(200);
                            var desde = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaContasBancoResumo tbody tr:nth-child(1) td:nth-child(4)').innerText");

                            if (banco == "260 - NUBANK" && agencia == "5465" && conta == "687467487-4" && desde == dataAtual)
                            {
                                Console.WriteLine("Dados de conta bancária salvos corretamente");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados de conta bancária");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados de conta bancária não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //Verificar documentos tabelaDocQddAmbimaAddResumo

                            await Page.Locator("#Documentos-tabResumo").ClickAsync();

                            var arquivoEncontrado = await Page.EvaluateAsync<bool>(@"() => {
                            const tbody = document.getElementById('listaQddAmbimaResumo');
                            if (tbody) {
                            const rows = tbody.getElementsByTagName('tr');
                            for (let row of rows) {
                            const cellText = row.cells[0].textContent.trim();
                            if (cellText === 'Arquivo teste 2.pdf') {
                            return true; // O arquivo foi encontrado
                            }
                            }
                            }
                            return false; // O arquivo não foi encontrado
                            }");

                           if (arquivoEncontrado == true)
                            {
                                Console.WriteLine("Arquivo anexado corretamente");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao anexar arquivo");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao anexar arquivo.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"erro {ex.Message}");
                        }


                        if (formularioOk == 6)
                        {
                            fluxoDeCadastros.FormularioCompletoNoPortal = "✅";

                        }
                        else
                        {
                            fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                            errosTotais2++;

                        }



                        var gestoraInternaExiste = Repository.GestoraInterna.GestoraInternaRepository.VerificaExistenciaGestoraInterna("16695922000109", "robo@zitec.ai");

                        if (gestoraInternaExiste)
                        {
                            Console.WriteLine("Gestora Interna adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            var verificarStatus = Repository.GestoraInterna.GestoraInternaRepository.VerificarStatus("16695922000109", "robo@zitec.ai");

                            if (verificarStatus)
                            {
                                Console.WriteLine("Status trocado para em análise");
                                fluxoDeCadastros.StatusEmAnalise = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Status não foi trocado");
                                fluxoDeCadastros.StatusEmAnalise = "❌";
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Erro ao trocar status na tabela");
                            }


                            var apagarGestoraInterna = Repository.GestoraInterna.GestoraInternaRepository.ApagarGestoraInterna("16695922000109", "robo@zitec.ai");

                            if (apagarGestoraInterna)
                            {
                                Console.WriteLine("Gestora Interna apagada com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Gestora Interna");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Gestora Interna");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }
                    }
                    else if (nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Gestoras Internas no tópico Cadastro ");
                    pagina.Nome = "Gestoras Internas";
                    pagina.StatusCode = CadastroGestorasInternas.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
            {
                if (fluxoDeCadastros.ListaErros.Count == 0)
                {
                    fluxoDeCadastros.ListaErros.Add("0");
                }
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                fluxoDeCadastros.TotalErros = errosTotais2;
                return (pagina, fluxoDeCadastros);
            }
            if (fluxoDeCadastros.ListaErros.Count == 0)
            {
                fluxoDeCadastros.ListaErros.Add("0");
            }
            pagina.TotalErros = errosTotais;
            pagina.TotalErros = errosTotais2;
            return (pagina, fluxoDeCadastros);
        }
    }
}
