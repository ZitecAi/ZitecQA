using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using TestePortal.Repository.Investidores;
using static TestePortal.Model.Usuario;
using TestePortal.Model;
using Segment.Model;
using TestePortal.Utils;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;

namespace TestePortal.Pages
{

    #region Cadastro investidores PF
    public class CadastroInvestidores
    {
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxosDeCadastros)> InvestidoresPf(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            int formularioOk = 0;
            fluxoDeCadastros.Fluxo = "Investidor - Pessoa Física";

            try
            {
                var CadastroInvestidores = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/CotistaInterno.aspx");
                if (CadastroInvestidores.Status == 200)
                {
                    string seletorTabela = "#tabelaCotista";

                    Console.Write("Investidores PF - Cadastro: ");
                    Console.WriteLine(CadastroInvestidores.Status);
                    pagina.StatusCode = CadastroInvestidores.Status;
                    pagina.Nome = "Investidores PF";
                    pagina.BaixarExcel = "❓";
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

                    if (nivelLogado == NivelEnum.Master)
                    {


                        var apagarInvestidor2 = Repository.Investidores.InvestidoresRepository.ApagarInvestidores("49624866830", "robo@zitec.ai");
                        // adicionar um novo cotista
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Investidor" }).ClickAsync();
                        await Page.Locator("#cpfCnpjCotistaInterno").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#cpfCnpjCotistaInterno").FillAsync("49624866830");
                        await Task.Delay(300);
                        await Page.Locator("#btnAvancarCadastroCotista").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#emailCotistaInterno").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#emailCotistaInterno").FillAsync("robo@zitec.ai");
                        await Task.Delay(300);
                        await Page.Locator("#obsCotistaInterno").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#obsCotistaInterno").FillAsync("teste ");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                        // procurar o registro cadastrado e colar o link em uma nova para dar continuidade
                        await Page.ReloadAsync();
                        await Task.Delay(3000);
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                        var primeiroTr = Page.Locator("#listaCotistas tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Task.Delay(400);

                        try
                        {
                            int idCotista = InvestidoresRepository.ObterIdCotista("49624866830", "robo@zitec.ai");
                            var buttonSelector = $"tr.child button#\\3{idCotista.ToString().Substring(0, 1)} {idCotista.ToString().Substring(1)}_url.btn.btn-default[title='Copiar Link']";
                            await Page.Locator(buttonSelector).ClickAsync();
                            await Task.Delay(400);

                            string token = InvestidoresRepository.ObterToken("49624866830", "robo@zitec.ai");
                            string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.COTISTA"];
                            string copiedUrl = $"{baseUrl}{token}";
                            var newPage = await context.NewPageAsync();
                            await newPage.GotoAsync(copiedUrl);

                            //Preenche a fica cadastral e depois clica no botão de resumo para verficiar as informações.
                            await Task.Delay(500);
                            await newPage.GetByPlaceholder("Nome Completo").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("Nome Completo").FillAsync("Jessica Vitoria Tavares");
                            await Task.Delay(200);
                            await newPage.Locator("#sexo").SelectOptionAsync(new[] { "fem" });
                            await Task.Delay(200);
                            await newPage.Locator("#dtNascimento").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#dtNascimento").FillAsync("17/05/2004");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*Nome do Pai" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*Nome do Pai" }).FillAsync("Roberto Tavares");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome da Mãe", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome da Mãe", new() { Exact = true }).FillAsync("Bernadete Maria Cassimiro ");
                            await Task.Delay(200);
                            await newPage.Locator("#estadoCivil").SelectOptionAsync(new[] { "uniaoEstavel" });
                            await Task.Delay(200);
                            await newPage.Locator("#situacaoLegal").SelectOptionAsync(new[] { "maior" });
                            await Task.Delay(200);
                            await newPage.Locator("#grauInstrucao").SelectOptionAsync(new[] { "emCompleto" });
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Telefone Celular", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Telefone Celular", new() { Exact = true }).FillAsync("(11)96018-3248");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Mensagem Eletrônica", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Mensagem Eletrônica", new() { Exact = true }).FillAsync("teste");
                            await Task.Delay(200);
                            await newPage.Locator("#tipoDocumento").SelectOptionAsync(new[] { "carteiraIdent" });
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Número do Documento", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Número do Documento", new() { Exact = true }).FillAsync("599997655");
                            await Task.Delay(200);
                            await newPage.Locator("#dtExpedicaoDoc").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#dtExpedicaoDoc").FillAsync("13/06/2022");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Órgão Expedidor", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Órgão Expedidor", new() { Exact = true }).FillAsync("SSP-SP");
                            await Task.Delay(200);
                            await newPage.Locator("#ufExpedidor").SelectOptionAsync(new[] { "SP" });
                            await Task.Delay(200);
                            await newPage.Locator("#ufNascimento").SelectOptionAsync(new[] { "SP" });
                            await Task.Delay(200);
                            await newPage.Locator("#naturalidade").SelectOptionAsync(new[] { "Guarulhos" });
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Ocupação Profissional", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Ocupação Profissional", new() { Exact = true }).FillAsync("teste");
                            await Task.Delay(200);
                            await newPage.Locator("#politicExpostaNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*CEP", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*CEP", new() { Exact = true }).FillAsync("07084-370");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Número", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Número", new() { Exact = true }).FillAsync("901");
                            await Task.Delay(200);
                            await newPage.Locator("#extratoMovNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#recebeExtratoMovNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#patrimEstimado").FillAsync("1000");
                            await Task.Delay(200);
                            await newPage.Locator("#rendMensal").FillAsync("100");
                            await Task.Delay(200);
                            await newPage.Locator("#outrosRend").FillAsync("500");
                            await Task.Delay(200);
                            await newPage.Locator("#descBensImovNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#transmOrdem").SelectOptionAsync(new[] { "naoAplica" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Conta Bancária Contas Bancá").GetByLabel("Default select example").SelectOptionAsync(new[] { "260" });
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("0000", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("0000", new() { Exact = true }).FillAsync("0001");
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).FillAsync("547111");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Digito*", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Digito*", new() { Exact = true }).FillAsync("5");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "ADICIONAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Não aceito responder o Questionário de Suitability.", new() { Exact = true }).CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#motivoRecuseSuitability").SelectOptionAsync(new[] { "investNaoResid" });
                            await Task.Delay(200);
                            await newPage.Locator("#perfilSuitability").SelectOptionAsync(new[] { "moderado" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Li e Concordo com as Declarações acima", new() { Exact = true }).CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#fileDocIdentificacao").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#fileDocComprovRes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "FINALIZAR" }).ClickAsync();
                            await Task.Delay(400);
                            await Task.Delay(3000);
                            await newPage.CloseAsync();
                            await Page.ReloadAsync();
                            await Task.Delay(1000);
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                            var primeiroTr2 = Page.Locator("#listaCotistas tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            var button = Page.Locator($"tr.child button[onclick=\"ModalResumoFormCotista('{idCotista}')\"]");

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


                            // verificar informações em Dados investidor 
                            await Task.Delay(800);
                            await Page.WaitForSelectorAsync("#nomeCompletoPfResumo");
                            string nomeCompleto = await Page.Locator("#nomeCompletoPfResumo").InputValueAsync();
                            await Page.WaitForSelectorAsync("#cpfResumo");
                            string cpf = await Page.Locator("#cpfResumo").InputValueAsync();
                            await Page.WaitForSelectorAsync("#sexoResumo");
                            string sexo = await Page.Locator("#sexoResumo").InputValueAsync();
                            await Page.WaitForSelectorAsync("#dtNascimentoResumo");
                            var dataNascimento = await Page.EvaluateAsync<string>("() => document.getElementById('dtNascimentoResumo').value");
                            await Page.WaitForSelectorAsync("#nomePaiResumo");
                            var nomePai = await Page.EvaluateAsync<string>("() => document.getElementById('nomePaiResumo').value");
                            await Page.WaitForSelectorAsync("#nomeMaeResumo");
                            var nomeMae = await Page.EvaluateAsync<string>("() => document.getElementById('nomeMaeResumo').value");
                            await Page.WaitForSelectorAsync("#estadoCivilResumo");
                            var estadoCivil = await Page.EvaluateAsync<string>("() => document.getElementById('estadoCivilResumo').value");
                            await Page.WaitForSelectorAsync("#situacaoLegalResumo");
                            var situacaoLegal = await Page.EvaluateAsync<string>("() => document.getElementById('situacaoLegalResumo').value");
                            await Page.WaitForSelectorAsync("#grauInstrucaoResumo");
                            var grau = await Page.EvaluateAsync<string>("() => document.getElementById('grauInstrucaoResumo').value");
                            await Page.WaitForSelectorAsync("#emailResumo");
                            var email = await Page.EvaluateAsync<string>("() => document.getElementById('emailResumo').value");
                            await Page.WaitForSelectorAsync("#telContatoResumo");
                            var telefone = await Page.EvaluateAsync<string>("() => document.getElementById('telContatoResumo').value");
                            await Page.WaitForSelectorAsync("#msgEletronicaResumo");
                            var msgEletronica = await Page.EvaluateAsync<string>("() => document.getElementById('msgEletronicaResumo').value");
                            await Task.Delay(400);
                            if (nomeCompleto == "Jessica Vitoria Tavares" && cpf == "496.248.668-30" && sexo == "Feminino" && dataNascimento == "17/05/2004" && nomePai == "Roberto Tavares" &&
                            nomeMae == "Bernadete Maria Cassimiro " && estadoCivil == "União Estável" && situacaoLegal == "Maior" && grau == "Ensino Médio Completo" && email == "robo@zitec.ai" &&
                            telefone == "(11)96018-3248" && msgEletronica == "teste")
                            {
                                await Task.Delay(00);
                                await Page.WaitForSelectorAsync("#tipoDocumentoResumo");
                                var tipoDoc = await Page.EvaluateAsync<string>("() => document.getElementById('tipoDocumentoResumo').value");
                                await Page.WaitForSelectorAsync("#numDocumentoResumo");
                                var numDoc = await Page.EvaluateAsync<string>("() => document.getElementById('numDocumentoResumo').value");
                                await Page.WaitForSelectorAsync("#dtExpedicaoDocResumo");
                                var dataExpe = await Page.EvaluateAsync<string>("() => document.getElementById('dtExpedicaoDocResumo').value");
                                await Page.WaitForSelectorAsync("#orgExpedidorResumo");
                                var orgExp = await Page.EvaluateAsync<string>("() => document.getElementById('orgExpedidorResumo').value");
                                await Page.WaitForSelectorAsync("#paisExpedidorResumo");
                                var paisExp = await Page.EvaluateAsync<string>("() => document.getElementById('paisExpedidorResumo').value");
                                await Page.WaitForSelectorAsync("#ufExpedidorResumo");
                                var ufExp = await Page.EvaluateAsync<string>("() => document.getElementById('ufExpedidorResumo').value");
                                await Page.WaitForSelectorAsync("#nacionalidadeResumo");
                                var nacionalidade = await Page.EvaluateAsync<string>("() => document.getElementById('nacionalidadeResumo').value");
                                await Page.WaitForSelectorAsync("#ufNascimentoResumo");
                                var ufNasc = await Page.EvaluateAsync<string>("() => document.getElementById('ufNascimentoResumo').value");
                                await Page.WaitForSelectorAsync("#naturalidadeResumo");
                                var nat = await Page.EvaluateAsync<string>("() => document.getElementById('naturalidadeResumo').value");
                                await Page.WaitForSelectorAsync("#ocupacaoProfResumo");
                                var ocupacao = await Page.EvaluateAsync<string>("() => document.getElementById('ocupacaoProfResumo').value");
                                await Page.WaitForSelectorAsync("#outraNacionalidadeResumoNao");
                                var outraNasc = await Page.EvaluateAsync<bool>("() => document.getElementById('outraNacionalidadeResumoNao').checked");
                                await Page.WaitForSelectorAsync("#authResPermOutroPaisResumoNao");
                                var authPend = await Page.EvaluateAsync<bool>("() => document.getElementById('authResPermOutroPaisResumoNao').checked");
                                await Page.WaitForSelectorAsync("#politicExpostaResumoNao");
                                var pessoaExp = await Page.EvaluateAsync<bool>("() => document.getElementById('politicExpostaResumoNao').checked");

                                if (tipoDoc == "Carteira de Identificação" && numDoc == "599997655" && dataExpe == "13/06/2022" && orgExp == "SSP-SP" &&
                                    paisExp == "BRASIL" && ufExp == "São Paulo" && nacionalidade == "BRASILEIRA" && ufNasc == "São Paulo" && nat == "Guarulhos" && ocupacao == "teste" &&
                                    outraNasc == true && authPend == true && pessoaExp == true)
                                {
                                    Console.WriteLine("Os campos foram salvos corretamente em dados do investidor!");
                                    formularioOk++;

                                }
                                else
                                {
                                    Console.WriteLine("Erro ao salvar campos corretamente em investidor!");
                                    errosTotais2++;
                                    fluxoDeCadastros.ListaErros.Add("Campos de investidores não foram salvos corretamente PF.");
                                    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                }
                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados nos campos de investidor!");
                            }


                            //verificar informações de endereço 
                            await Task.Delay(800);
                            await Page.Locator("#Endereco-tabResumo").ClickAsync();
                            var paisResidencia = await Page.EvaluateAsync<string>("() => document.getElementById('paisResidenciaResumo').value");
                            var cepResidencia = await Page.EvaluateAsync<string>("() => document.getElementById('cepResidenciaResumo').value");
                            var logResidencia = await Page.EvaluateAsync<string>("() => document.getElementById('logResidenciaResumo').value");
                            var numResidencia = await Page.EvaluateAsync<string>("() => document.getElementById('numResidenciaResumo').value");
                            var bairroResidencia = await Page.EvaluateAsync<string>("() => document.getElementById('bairroResidenciaResumo').value");
                            var ufResidencia = await Page.EvaluateAsync<string>("() => document.getElementById('ufResidenciaResumo').value");
                            var cidadeResidencia = await Page.EvaluateAsync<string>("() => document.getElementById('cidadeResidenciaResumo').value");
                            var extratoMov = await Page.EvaluateAsync<bool>("() => document.getElementById('extratoMovResumoNao').checked");
                            var recebeExtratoMov = await Page.EvaluateAsync<bool>("() => document.getElementById('recebeExtratoMovResumoNao').checked");


                            if (paisResidencia == "BRASIL" && cepResidencia == "07084-370" && logResidencia == "Avenida Alexandre Grandisoli" &&
                                bairroResidencia == "Parque Continental II" && numResidencia == "901" && ufResidencia == "São Paulo" && cidadeResidencia == "Guarulhos"
                                && extratoMov == true && recebeExtratoMov == true
                                )
                            {
                                Console.WriteLine("Os campos foram salvos corretamente em endereço! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados nos campos de endereço! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de endereço não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;


                            }

                            //verificar informações de situação patrimonial
                            await Task.Delay(800);
                            await Page.Locator("#SitPatrim-tabResumo").ClickAsync();
                            var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

                            var dataSitPat = await Page.EvaluateAsync<string>("() => document.getElementById('dtSituacaoPatrimResumo').value");
                            await Task.Delay(200);
                            var patriEst = await Page.EvaluateAsync<string>("() => document.getElementById('patrimEstimadoResumo').value");
                            await Task.Delay(200);
                            var rendMen = await Page.EvaluateAsync<string>("() => document.getElementById('rendMensalResumo').value");
                            await Task.Delay(200);
                            var outrosRend = await Page.EvaluateAsync<string>("() => document.getElementById('outrosRendResumo').value");
                            await Task.Delay(200);
                            var descBens = await Page.EvaluateAsync<bool>("() => document.getElementById('descBensImovNao').checked");
                            await Task.Delay(200);
                            var outrosBens = await Page.EvaluateAsync<bool>("() => document.getElementById('outroBensInvestResumoNao').checked");
                            //dataSitPat == dataAtual
                            // dataSitPat == "04/10/2024"

                            if (dataSitPat == dataAtual && patriEst == "1000" && rendMen == "100" && outrosRend == "500"
                                && descBens == true && outrosBens == true)

                            {
                                Console.WriteLine("Os campos foram salvos corretamente em situação patrimonial! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados em situação patrimonial! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de situação patrimonial não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            //verificar representantes 
                            await Task.Delay(800);
                            await Page.Locator("#Representantes-tabResumo").ClickAsync();
                            var usPerson = await Page.EvaluateAsync<bool>("() => document.getElementById('UsPersonProcuradorResumoNao').checked");

                            if (usPerson == true)

                            {
                                Console.WriteLine("Os campos foram salvos em representantes! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados em representantes! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de representantes não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;

                            }

                            //verificar dados da conta bancária 
                            await Task.Delay(800);
                            await Page.Locator("#ContaBancaria-tabResumo").ClickAsync();

                            var banco = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContasBancoResumo tr td:nth-child(1)').innerText");
                            await Task.Delay(200);
                            var agencia = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContasBancoResumo tr td:nth-child(2)').innerText");
                            await Task.Delay(200);
                            var conta = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContasBancoResumo tr td:nth-child(3)').innerText");
                            await Task.Delay(200);


                            if (banco == "260 - NUBANK" && agencia == "0001" && conta == "547111-5")
                            {
                                Console.WriteLine("Os campos foram salvos em conta bancária! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados em conta bancária! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de conta bancária não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;

                            }

                            //verificar suitability 
                            await Task.Delay(800);
                            await Page.Locator("#Suitability-tabResumo").ClickAsync();

                            var naoResp = await Page.EvaluateAsync<bool>("() => document.getElementById('recuseSuitability').checked");
                            await Task.Delay(200);
                            var motivo = await Page.EvaluateAsync<bool>("() => document.getElementById('motivoRecuseSuitabilityResumo').checked");
                            await Task.Delay(200);
                            var perfil = await Page.EvaluateAsync<bool>("() => document.getElementById('perfilSuitabilityResumo').checked");

                            if (naoResp == true && motivo == false && perfil == false)
                            {
                                Console.WriteLine("Os campos foram salvos em suitability! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados em suitability! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de suitability não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            //verificar declarações 
                            await Task.Delay(800);
                            await Page.Locator("#Declarações-tabResumo").ClickAsync();
                            var aceitaDeclaracao = await Page.EvaluateAsync<bool>("() => document.getElementById('aceiteDeclaracaoResumo').checked");

                            if (aceitaDeclaracao == true)

                            {
                                Console.WriteLine("os termos foram aceitos! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Os termos não foram aceitos! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de declarações não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;

                            }

                            // verificar documentos 
                            await Task.Delay(800);
                            await Page.Locator("#Documentos-tabResumo").ClickAsync();

                            var arquivoEnviado = await Page.EvaluateAsync<bool>(@"() => {
                        const tbody = document.getElementById('listaDocIdentResumo');
                        if (tbody) {
                        const rows = tbody.getElementsByTagName('tr');
                        for (let row of rows) {
                        const cellText = row.cells[0].textContent.trim();
                        if (cellText === 'Arquivoteste2.pdf') {
                        return true;
                        }
                        }
                        }
                        return false;
                        }");

                            var arquivoEnviado2 = await Page.EvaluateAsync<bool>(@"() => {
                        const tbody = document.getElementById('listaComprovResResumo');
                        if (tbody) {
                        const rows = tbody.getElementsByTagName('tr');
                        for (let row of rows) {
                        const cellText = row.cells[0].textContent.trim();
                        if (cellText === 'Arquivoteste2.pdf') {
                        return true;
                        }
                        }
                        }
                        return false;
                        }");

                            if (arquivoEnviado == true && arquivoEnviado2 == true)

                            {
                                Console.WriteLine("Arquivos anexados corretamente! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao anexar arquivos! ");
                                fluxoDeCadastros.ListaErros.Add("Arquivos não foram anexados corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;

                            }
                            var verificarStatus = Repository.Investidores.InvestidoresRepository.VerificarStatus("49624866830", "robo@zitec.ai");

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

                            if (formularioOk == 8)
                            {
                                fluxoDeCadastros.FormularioCompletoNoPortal = "✅";

                            }
                            else
                            {
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                                errosTotais2++;

                            }

                            await Page.GetByRole(AriaRole.Button, new() { Name = "Fechar" }).ClickAsync();


                            //aprovação

                            string locatorCadastro = "li:has(span.dtr-title:has-text('Cadastro')) >> span.dtr-data >> button[onclick*='OpenModalStatusCadastro']";
                            await Page.WaitForSelectorAsync(locatorCadastro);
                            await Page.Locator(locatorCadastro).ClickAsync();
                            await Page.Locator("#statusCadastro").GetByText("Aprovado").ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).FillAsync("Teste de aprovação");
                            await Page.Locator("#statusCadastroButton").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                            var primeiroTr3 = Page.Locator("#listaCotistas tr").First;
                            var primeiroTd3 = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();
                            var buttonResumo = Page.Locator($"tr.child button[onclick=\"ModalResumoFormCotista('{idCotista}')\"]");
                            string locatorCompliance = "li:has(span.dtr-title:has-text('Compliance')) >> span.dtr-data >> button[onclick*='OpenModalStatusCompliance']";
                            await Page.WaitForSelectorAsync(locatorCompliance);
                            await Page.Locator(locatorCompliance).ClickAsync();
                            await Page.Locator("#statusCompliance label").Filter(new() { HasText = "Aprovado" }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).ClickAsync();
                            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).FillAsync("teste aprovação");
                            await Page.Locator("#statusComplianceButton").ClickAsync();
                            await Task.Delay(1000);
                            await Task.Delay(39000);

                            bool statusAtual = false;

                            for (int i = 0; i < 5; i++)
                            {
                                statusAtual = InvestidoresRepository.VerificaStatusAgdAss("49624866830", "robo@zitec.ai");

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
                                string idDocumentoAutentique = Repository.Investidores.InvestidoresRepository.ObterIdDocumentoAutentique(idCotista);
                                var response = AssinarDocumentosAutentique.AssinarDocumento("9ad54b27a864625573ad40327a1916db61b687c3fe8641ff7f3efdc3e985d3b3", idDocumentoAutentique);

                                if (response != null && response.Success)
                                {

                                    Console.WriteLine("Documento assinado");

                                    var atualizarStatus = Repository.Investidores.InvestidoresRepository.UpdateStatusAprovado("49624866830", "robo@zitec.ai");

                                    if (atualizarStatus == true)
                                    {
                                        fluxoDeCadastros.DocumentoAssinado = "✅";

                                    }
                                    else
                                    {
                                        fluxoDeCadastros.DocumentoAssinado = "❌";
                                    }


                                }
                                else
                                {
                                    errosTotais2++;
                                    fluxoDeCadastros.DocumentoAssinado = "❌";
                                    fluxoDeCadastros.ListaErros.Add("Erro ao assinar documento");
                                }

                                await Task.Delay(15000);


                                bool statusAprovado = false;

                                for (int i = 0; i < 5; i++)
                                {

                                    statusAprovado = InvestidoresRepository.VerificaStatusAprovado("49624866830", "robo@zitec.ai");

                                    if (statusAprovado == true)
                                    {
                                        Console.WriteLine("Status trocado para aprovado");
                                        fluxoDeCadastros.statusAprovado = "✅";
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Tentativa {i + 1} de trocar status falhou. Tentando novamente...");
                                        await Task.Delay(5000);

                                        if (i == 5)
                                        {
                                            fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aprovado");
                                            errosTotais2++;

                                        }
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("Status não foi trocado para aguardando assinatura");
                                fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando assinatura");
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"erro {ex.Message}");

                        }

                        var emailChecker = new TestePortal.Utils.EmailChecker();
                        bool emailChegou = await emailChecker.CheckForNotificationEmailAsync("Cadastro de Investidor - IDSF!");

                        if (emailChegou)
                        {
                            Console.WriteLine("E-mail com dados de cadastro de investidor chegou!");
                            fluxoDeCadastros.EmailRecebido = "✅";
                        }
                        else
                        {
                            Console.WriteLine("E-mail para cadastro de investidor não chegou.");
                            fluxoDeCadastros.ListaErros.Add("E-mail para cadastro de investidor não chegou.");
                            errosTotais2++;
                            fluxoDeCadastros.EmailRecebido = "❌";
                        }



                        var investidorExiste = Repository.Investidores.InvestidoresRepository.VerificaExistenciaInvestidores("49624866830", "robo@zitec.ai");


                        if (investidorExiste)
                        {
                            Console.WriteLine("investidor adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                           



                            var apagarInvestidor = Repository.Investidores.InvestidoresRepository.ApagarInvestidores("49624866830", "robo@zitec.ai");

                            if (apagarInvestidor)
                            {
                                Console.WriteLine("Investidor apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Investidor");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir investidor");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    else if (nivelLogado != NivelEnum.Master)
                    {

                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }

                    else
                    {
                        Console.Write("Erro ao carregar a página de Investidores no tópico Cadastro ");
                        pagina.Nome = "Investidores";
                        pagina.StatusCode = CadastroInvestidores.Status;
                        errosTotais++;

                    }

                }
            }
            catch (TimeoutException ex)
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
            fluxoDeCadastros.TotalErros = errosTotais2;
            if (fluxoDeCadastros.ListaErros.Count == 0)
            {
                fluxoDeCadastros.ListaErros.Add("0");
            }
            return (pagina, fluxoDeCadastros);

        }
        #endregion

        #region Cadastro Investidores PJ
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxosDeCadastros)> InvestidoresPj(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {

            var pagina = new Model.Pagina();
            int errosTotais = 0;
            int errosTotais2 = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            int formularioOk = 0;
            fluxoDeCadastros.Fluxo = "Investidor - Pessoa Jurídica";
            var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            try
            {
                var CadastroInvestidores = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/CotistaInterno.aspx");
                if (CadastroInvestidores.Status == 200)
                {
                    string seletorTabela = "#tabelaCotista";

                    Console.Write("Investidores PJ - Cadastro: ");
                    Console.WriteLine(CadastroInvestidores.Status);
                    pagina.StatusCode = CadastroInvestidores.Status;
                    pagina.Nome = "Investidores";
                    pagina.BaixarExcel = "❓";
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

                    if (nivelLogado == NivelEnum.Master)
                    {
                        
                       var apagarInvestidor2 = Repository.Investidores.InvestidoresRepository.ApagarInvestidores("16695922000109", "robo@zitec.ai");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Investidor" }).ClickAsync();
                        await Page.Locator("#cpfCnpjCotistaInterno").ClickAsync();
                        await Page.Locator("#cpfCnpjCotistaInterno").FillAsync("166.959.220-00109");
                        await Page.Locator("#btnAvancarCadastroCotista").ClickAsync();
                        await Page.Locator("#emailCotistaInterno").ClickAsync();
                        await Page.Locator("#emailCotistaInterno").FillAsync("robo@zitec.ai");
                        await Page.Locator("#obsCotistaInterno").ClickAsync();
                        await Page.Locator("#obsCotistaInterno").FillAsync("teste");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                        //Procura o registro para copiar o link
                        await Page.ReloadAsync();
                        await Task.Delay(2000);
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                        var primeiroTr = Page.Locator("#listaCotistas tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Task.Delay(400);

                        int idCotista = InvestidoresRepository.ObterIdCotista("16695922000109", "robo@zitec.ai");

                        try
                        {
                            var buttonSelector = $"tr.child button#\\3{idCotista.ToString().Substring(0, 1)} {idCotista.ToString().Substring(1)}_url.btn.btn-default[title='Copiar Link']";
                            await Page.Locator(buttonSelector).ClickAsync();
                            await Task.Delay(400);

                            string token = InvestidoresRepository.ObterToken("16695922000109", "robo@zitec.ai");
                            string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.COTISTAPJ"];
                            string copiedUrl = $"{baseUrl}{token}";
                            var newPage = await context.NewPageAsync();
                            await newPage.GotoAsync(copiedUrl);

                            await newPage.GetByLabel("*Atividade Principal (CNAE)", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Atividade Principal (CNAE)", new() { Exact = true }).FillAsync("analista qa");
                            await Task.Delay(200);
                            await newPage.Locator("#usPersonDadosInvestNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#irrf").SelectOptionAsync(new[] { "isento" });
                            await Task.Delay(200);
                            await newPage.Locator("#iof").SelectOptionAsync(new[] { "isento" });
                            await Task.Delay(200);
                            await newPage.Locator("#fatcaNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#controlAcionario").SelectOptionAsync(new[] { "nacional" });
                            await Task.Delay(200);
                            await newPage.Locator("#ppeNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000-000", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000-000", new() { Exact = true }).FillAsync("07084-370");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Número", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Número", new() { Exact = true }).FillAsync("901");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Telefone Comercial", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Telefone Comercial", new() { Exact = true }).FillAsync("(11)96018-3248");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Celular Comercial", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Celular Comercial", new() { Exact = true }).FillAsync("(11)96018-3244");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#patLiquidTotal").FillAsync("10000");
                            await Task.Delay(200);
                            await newPage.Locator("#patrimLiquido").FillAsync("100");
                            await Task.Delay(200);
                            await newPage.Locator("#investFinanceiros").FillAsync("744");
                            await Task.Delay(200);
                            await newPage.Locator("#descBensImovNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#outroBensInvestNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#beneficAcionistaSocNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#possuiDiretoresNao").CheckAsync();
                            await Task.Delay(200);
                            await Task.Delay(200);
                            await newPage.Locator("#tipoPessoaBenefFinais").SelectOptionAsync(new[] { "fisica" });
                            await Task.Delay(200);
                            await newPage.Locator("#nomeBenefFinais").FillAsync("Jessica Vitoria Tavares");
                            await Task.Delay(200);
                            await newPage.Locator("#cpfBenefFinais").FillAsync("496.248.668-30");
                            await Task.Delay(200);
                            await newPage.Locator("#sexoBenefFinais").SelectOptionAsync(new[] { "fem" });
                            await Task.Delay(200);
                            await newPage.Locator("#dtNascBenefFinais").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#dtNascBenefFinais").FillAsync("17/05/2004");
                            await Task.Delay(200);
                            await newPage.Locator("#ufNascBenefFinais").SelectOptionAsync(new[] { "SP" });
                            await Task.Delay(200);
                            await newPage.Locator("#tipoDocBenefFinais").SelectOptionAsync(new[] { "carteiraIdent" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "Número do Documento", Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "Número do Documento", Exact = true }).FillAsync("59997655");
                            await Task.Delay(200);
                            await newPage.Locator("#ufIdentBenefFinais").SelectOptionAsync(new[] { "SP" });
                            await Task.Delay(200);
                            await newPage.Locator("#usPersonBenefFinaisNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#estadoCivilBenefFinais").SelectOptionAsync(new[] { "solteiro" });
                            await Task.Delay(200);
                            await newPage.Locator("#ppeBenefFinaisNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#vincPpeBenefFinaisNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "ADICIONAR" }).ClickAsync();
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
                            await newPage.Locator("#situacaoLegalProcurador").SelectOptionAsync(new[] { "maior" });
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome da Mãe", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome da Mãe", new() { Exact = true }).FillAsync("Bernadete Maria Cassimiro ");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).FillAsync("robo@zitec.ai");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).FillAsync("(11)94577-8896");
                            await Task.Delay(200);
                            await newPage.Locator("#grauRelacProcurador").SelectOptionAsync(new[] { "titular" });
                            await Task.Delay(200);
                            await newPage.Locator("#validIndetSim").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#vincDistribuidorNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#referente").SelectOptionAsync(new[] { "cotitular" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "INCLUIR REPRESENTANTE" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Conta Bancária Contas Bancá").GetByLabel("Default select example").SelectOptionAsync(new[] { "69 " });
                            await Task.Delay(200);
                            await newPage.GetByText("Contas Bancárias ADICIONAR *").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("0000", new() { Exact = true }).FillAsync("1112");
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).FillAsync("1245789");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Digito", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Digito", new() { Exact = true }).FillAsync("5");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Radio, new() { Name = "Não" }).CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "ADICIONAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Não aceito responder o Questionário de Suitability.", new() { Exact = true }).CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#motivoRecuseSuitability").SelectOptionAsync(new[] { "agAuton" });
                            await Task.Delay(200);
                            await newPage.GetByText("*Perfil: Escolha o Perfil").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#perfilSuitability").SelectOptionAsync(new[] { "moderado" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Li e Concordo com as Declarações acima", new() { Exact = true }).CheckAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await Task.Delay(200);
                            await Task.Delay(200);
                            await newPage.Locator("#fileDocIdentificacao").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#fileDocComprovRes").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#fileDocIdentifCpf").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#fileContratoSocUltimaAlt").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "FINALIZAR" }).ClickAsync();
                            await Task.Delay(6000);
                            await newPage.CloseAsync();
                            await Page.ReloadAsync();
                            await Task.Delay(3000);
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                            var primeiroTr2 = Page.Locator("#listaCotistas tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await Task.Delay(400);
                            await primeiroTd.ClickAsync();
                            var button = Page.Locator($"tr.child button[onclick=\"ModalResumoFormCotistaPJ('{idCotista}')\"]");

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

                            //verificar informações de investidor 

                            await Task.Delay(700);
                            await Page.WaitForSelectorAsync("#razaoSocialResumoPJ");
                            var razaoSocial = await Page.EvaluateAsync<string>("() => document.getElementById('razaoSocialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#cnpjResumoPJ");
                            var cnpj = await Page.EvaluateAsync<string>("() => document.getElementById('cnpjResumoPJ').value");
                            await Page.WaitForSelectorAsync("#dtConstituicaoResumoPJ");
                            var dtConstituicao = await Page.EvaluateAsync<string>("() => document.getElementById('dtConstituicaoResumoPJ').value");
                            await Page.WaitForSelectorAsync("#ativPrincipalResumoPJ");
                            var ativPrincipal = await Page.EvaluateAsync<string>("() => document.getElementById('ativPrincipalResumoPJ').value");
                            await Page.WaitForSelectorAsync("#irrfResumoPJ");
                            var irrf = await Page.EvaluateAsync<string>("() => document.getElementById('irrfResumoPJ').value");
                            await Page.WaitForSelectorAsync("#iofResumoPJ");
                            var iof = await Page.EvaluateAsync<string>("() => document.getElementById('iofResumoPJ').value");
                            await Page.WaitForSelectorAsync("#natJuridicaResumoPJ");
                            var natJuridica = await Page.EvaluateAsync<string>("() => document.getElementById('natJuridicaResumoPJ').value");
                            await Page.WaitForSelectorAsync("#formaConstituicaoResumoPJ");
                            var formaConstituicao = await Page.EvaluateAsync<string>("() => document.getElementById('formaConstituicaoResumoPJ').value");//
                            await Page.WaitForSelectorAsync("#controlAcionarioResumoPJ");
                            var controlAcionario = await Page.EvaluateAsync<string>("() => document.getElementById('controlAcionarioResumoPJ').value");
                            await Page.WaitForSelectorAsync("#paisDomFiscalResumoPJ");
                            var paisDomFiscal = await Page.EvaluateAsync<string>("() => document.getElementById('paisDomFiscalResumoPJ').value");
                            await Page.WaitForSelectorAsync("#paisConstituicaoResumoPJ");
                            var paisConst = await Page.EvaluateAsync<string>("() => document.getElementById('paisConstituicaoResumoPJ').value");
                            await Page.WaitForSelectorAsync("#emailResumoPJ");
                            var email = await Page.EvaluateAsync<string>("() => document.getElementById('emailResumoPJ').value");
                            await Page.WaitForSelectorAsync("#usPersonDadosInvestNao");
                            var usPerson = await Page.EvaluateAsync<bool>("() => document.getElementById('usPersonDadosInvestNao').checked");
                            await Page.WaitForSelectorAsync("#fatcaNao");
                            var fatca = await Page.EvaluateAsync<bool>("() => document.getElementById('fatcaNao').checked");
                            await Page.WaitForSelectorAsync("#ppeNao");
                            var ppe = await Page.EvaluateAsync<bool>("() => document.getElementById('ppeNao').checked");

                            await Task.Delay(350);
                            if (razaoSocial == "ID CORRETORA DE TITULOS E VALORES MOBILIARIOS SA" && cnpj == "16.695.922/0001-09" && dtConstituicao == dataAtual &&
                                ativPrincipal == "analista qa" && irrf == "Isento" && iof == "Isento" && natJuridica == "Valor não encontrado" &&
                                formaConstituicao == "Valor não encontrado" && controlAcionario == "Nacional" && paisDomFiscal == "BRASIL" && paisConst == "BRASIL"
                                && email == "robo@zitec.ai" && usPerson == true && fatca == true && ppe == true

                            )
                            {
                                Console.WriteLine("Os campos foram salvos corretamente em dados do investidor!");
                                formularioOk++;
                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar campos corretamente em investidor!");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Campos de investidores PJ não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            // verificar endereço comercial 
                            await Page.Locator("#EnderecoComercialPJ-tabResumo").ClickAsync();

                            await Page.WaitForSelectorAsync("#paisComercialResumoPJ");
                            var paisComercial = await Page.EvaluateAsync<string>("() => document.getElementById('paisComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#cepComercialResumoPJ");
                            var cep = await Page.EvaluateAsync<string>("() => document.getElementById('cepComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#logComercialResumoPJ");
                            var loguadouro = await Page.EvaluateAsync<string>("() => document.getElementById('logComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#numComercialResumoPJ");
                            var numComercial = await Page.EvaluateAsync<string>("() => document.getElementById('numComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#bairroComercialResumoPJ");
                            var bairroComercial = await Page.EvaluateAsync<string>("() => document.getElementById('bairroComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#ufComercialResumoPJ");
                            var ufComercial = await Page.EvaluateAsync<string>("() => document.getElementById('ufComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#cidadeComercialResumoPJ");
                            var cidadeComercial = await Page.EvaluateAsync<string>("() => document.getElementById('cidadeComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#telComercialResumoPJ");
                            var telComercial = await Page.EvaluateAsync<string>("() => document.getElementById('telComercialResumoPJ').value");
                            await Page.WaitForSelectorAsync("#celComercialResumoPJ");
                            var celComercial = await Page.EvaluateAsync<string>("() => document.getElementById('celComercialResumoPJ').value");

                            if (paisComercial == "BRASIL" && cep == "07084-370" && loguadouro == "Avenida Alexandre Grandisoli" && numComercial == "901" &&
                               bairroComercial == "Parque Continental II" && ufComercial == "São Paulo" && cidadeComercial == "Guarulhos" && telComercial == "(11)96018-3248" &&
                               celComercial == "(11)96018-3244"
                            )
                            {
                                Console.WriteLine("Os campos foram salvos corretamente em endereço comercial! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados nos campos de endereço! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de endereço não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            // verificar situação patrimonial 
                            await Page.Locator("#SituacaoPatrimonialPJ-tabResumo").ClickAsync();

                            await Page.WaitForSelectorAsync("#beneficAcionistaSocNaoResumoPJ");
                            var beneficAcionistaSocNao = await Page.EvaluateAsync<bool>("() => document.getElementById('beneficAcionistaSocNaoResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#possuiDiretoresNaoResumoPJ");
                            var possuiDiretoresNao = await Page.EvaluateAsync<bool>("() => document.getElementById('possuiDiretoresNaoResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#benfFinaisSimResumoPJ");
                            var benfFinaisSim = await Page.EvaluateAsync<bool>("() => document.getElementById('benfFinaisNaoResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#empColigadasNaoResumoPJ");
                            var empColigadasNao = await Page.EvaluateAsync<bool>("() => document.getElementById('empColigadasNaoResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#empControlNaoResumoPJ");
                            var empControlNao = await Page.EvaluateAsync<bool>("() => document.getElementById('empControlNaoResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#descBensImovNaoResumoPJ");
                            var descBensImovNao = await Page.EvaluateAsync<bool>("() => document.getElementById('descBensImovNaoResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#outroBensInvestNaoResumoPJ");
                            var outroBensInvestNao = await Page.EvaluateAsync<bool>("() => document.getElementById('outroBensInvestNaoResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#dtSituacaoPatrimResumoPJ");
                            var dtSituacaoPatrim = await Page.EvaluateAsync<string>("() => document.getElementById('dtSituacaoPatrimResumoPJ').value");
                            await Page.WaitForSelectorAsync("#patLiquidTotalResumoPJ");
                            var patLiquidTotal = await Page.EvaluateAsync<string>("() => document.getElementById('patLiquidTotalResumoPJ').value");
                            await Page.WaitForSelectorAsync("#patrimLiquidoResumoPJ");
                            var patrimLiquido = await Page.EvaluateAsync<string>("() => document.getElementById('patrimLiquidoResumoPJ').value");
                            await Page.WaitForSelectorAsync("#investFinanceirosResumoPJ");
                            var investFinanceiros = await Page.EvaluateAsync<string>("() => document.getElementById('investFinanceirosResumoPJ').value");
                            var tipoBeneficiario = await Page.EvaluateAsync<string>("() => document.querySelector('#listaBenefFinaisResumoPJ tr:nth-child(1) td:nth-child(1)').innerText");
                            var nomeBeneficiario = await Page.EvaluateAsync<string>("() => document.querySelector('#listaBenefFinaisResumoPJ tr:nth-child(1) td:nth-child(2)').innerText");
                            var cpfBeneficiario = await Page.EvaluateAsync<string>("() => document.querySelector('#listaBenefFinaisResumoPJ tr:nth-child(1) td:nth-child(3)').innerText");

                            if (beneficAcionistaSocNao == true && possuiDiretoresNao == true && benfFinaisSim == false && empColigadasNao == true && empControlNao == true
                                && descBensImovNao == true && outroBensInvestNao == true && dtSituacaoPatrim == dataAtual && patLiquidTotal == "10000" &&
                                patrimLiquido == "100" && investFinanceiros == "744" && tipoBeneficiario == "Física" && nomeBeneficiario == "Jessica Vitoria Tavares" && cpfBeneficiario == "496.248.668-30"
                                )
                            {


                                Console.WriteLine("Os campos foram salvos corretamente em situação patrimonial! ");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados nos campos de situação patrimonial! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de situação patrimonial não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            // verificar representantes 
                            await Page.Locator("#RepresentantesPJ-tabResumo").ClickAsync();
                            await Page.WaitForSelectorAsync("#tabelaProcuradorAdd");
                            var nomeTabe = await Page.EvaluateAsync<string>("() => document.querySelector('#listaProcuradorAdicionado tr td:nth-child(1)').innerText");
                            var cpfTabe = await Page.EvaluateAsync<string>("() => document.querySelector('#listaProcuradorAdicionado tr td:nth-child(2)').innerText");
                            var dataNascimentoTabela = await Page.EvaluateAsync<string>("() => document.querySelector('#listaProcuradorAdicionado tr td:nth-child(3)').innerText");
                            var emailTabela = await Page.EvaluateAsync<string>("() => document.querySelector('#listaProcuradorAdicionado tr td:nth-child(4)').innerText");

                            if (nomeTabe == "Jessica Vitoria Tavares" && cpfTabe == "496.248.668-30" && dataNascimentoTabela == "01/01/0001" &&
                                 emailTabela == "robo@zitec.ai")
                            {

                                Console.WriteLine("Os campos foram salvos corretamente em representantes! ");
                                formularioOk++;
                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados nos campos de representantes! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de representantes não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            // verificar procurador  

                            await Page.Locator("#ProcuradorRepresentantePJ-tabResumo").ClickAsync();

                            //await Page.WaitForSelectorAsync("#listaProcuradorAdicionadoResumoPJ");
                            //var nomeTabela = await Page.EvaluateAsync<string>("() => document.querySelector('#listaProcuradorAdicionadoResumoPJ td:nth-child(1)').innerText");
                            //var cpfTabela = await Page.EvaluateAsync<string>("() => document.querySelector('#listaProcuradorAdicionadoResumoPJ td:nth-child(2)').innerText");
                            //var validade = await Page.EvaluateAsync<bool>("() => document.getElementById('validIndetSimResumoPJ').checked");
                            //var vinculo = await Page.EvaluateAsync<bool>("() => document.getElementById('vincDistribuidorNaoResumoPJ').checked");
                            //var referente = await Page.EvaluateAsync<string>("() => document.getElementById('referenteResumoPJ').value");

                            //if (nomeTabela == "Jessica Vitoria Tavares" && cpfTabela == "496.248.668-30" && validade == false && vinculo == true
                            //    && referente == "Titular")
                            //{

                            //    Console.WriteLine("Os campos foram salvos corretamente em procurador! ");
                            //    formularioOk++;
                            //}
                            //else
                            //{
                            //    Console.WriteLine("Erro ao salvar os dados nos campos de procurador! ");
                            //    fluxoDeCadastros.ListaErros.Add("Campos de procurador não foram salvos corretamente.");
                            //    fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            //    errosTotais2++;
                            //}

                            //verificar conta bancária 


                            await Page.Locator("#ContaBancariaPJ-tabResumo").ClickAsync();

                            await Page.WaitForSelectorAsync("#listaContasBancoResumoPJ");
                            var bancoTabela = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContasBancoResumoPJ td:nth-child(1)').innerText");
                            var agenciaTabela = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContasBancoResumoPJ td:nth-child(2)').innerText");
                            var contaTabela = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContasBancoResumoPJ td:nth-child(3)').innerText");

                            if (bancoTabela == "69 - Crefisa" && agenciaTabela == "1112" && contaTabela == "1245789-5")
                            {
                                Console.WriteLine("Dados foram salvos corretamente em conta bancária");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados nos campos de conta bancária! ");
                                fluxoDeCadastros.ListaErros.Add("Campos de conta bancária não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            //verificar suitability 

                            await Page.Locator("#SuitabilityPJ-tabResumo").ClickAsync();

                            await Page.WaitForSelectorAsync("#recuseSuitabilityResumoPJ");
                            var recuseSuitability = await Page.EvaluateAsync<bool>("() => document.getElementById('recuseSuitabilityResumoPJ').checked");
                            await Page.WaitForSelectorAsync("#motivoRecuseSuitabilityResumoPJ");
                            var motivoRecuseSuitability = await Page.EvaluateAsync<string>("() => document.getElementById('motivoRecuseSuitabilityResumoPJ').value");
                            await Page.WaitForSelectorAsync("#perfilSuitabilityResumoPJ");
                            var perfilSuitability = await Page.EvaluateAsync<string>("() => document.getElementById('perfilSuitabilityResumoPJ').value");

                            if (recuseSuitability == true && motivoRecuseSuitability == "AGENTE AUTONOMO (RECURSOS PRÓPRIOS)" && perfilSuitability == "Moderado")
                            {
                                Console.WriteLine("Os campos foram salvos corretamente em suitability");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar os dados nos campos suitability! ");
                                fluxoDeCadastros.ListaErros.Add("Campos suitability não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            // declaração preenchida 

                            await Page.Locator("#DeclaracoesPJ-tabResumo").ClickAsync();

                            var declaracoes = await Page.EvaluateAsync<bool>("() => document.getElementById('aceiteDeclaracaoResumoPJ').checked");

                            if (declaracoes == true)
                            {
                                Console.WriteLine("As declarações foram aceitas");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao verificar declarações");
                                fluxoDeCadastros.ListaErros.Add("Declarações não aceitas");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            // verificar documentos 
                            await Page.Locator("#DocumentosPJ-tabResumo").ClickAsync();
                            // Verificar se o Arquivo teste 2.pdf está na tabela Documento de Identificação
                            var arquivoDocumentoIdentif = await Page.EvaluateAsync<bool>(@"() => {
                            const tbody = document.getElementById('listaDocumentoIdentifResumoPJ');
                            if (tbody) {
                            const rows = tbody.getElementsByTagName('tr');
                            for (let row of rows) {
                            const cellText = row.cells[0].textContent.trim();
                            if (cellText === 'Arquivoteste2.pdf') {
                            return true;
                            }
                            }
                            }
                            return false;
                            }");

                            // Verificar se o Arquivo teste 2.pdf está na tabela Comprovante de Residência
                            var arquivoComprovRes = await Page.EvaluateAsync<bool>(@"() => {
                            const tbody = document.getElementById('listaComprovResidenciaResumoPJ');
                            if (tbody) {
                            const rows = tbody.getElementsByTagName('tr');
                            for (let row of rows) {
                            const cellText = row.cells[0].textContent.trim();
                            if (cellText === 'Arquivoteste2.pdf') {
                            return true;
                            }
                            }
                            }
                            return false;
                            }");

                            // Verificar se o Arquivo teste 2.pdf está na tabela Documento de Identificação CPF
                            var arquivoDocIdentifCpf = await Page.EvaluateAsync<bool>(@"() => {
                            const tbody = document.getElementById('listaDocIdentifCpfResumoPJ');
                            if (tbody) {
                            const rows = tbody.getElementsByTagName('tr');
                            for (let row of rows) {
                            const cellText = row.cells[0].textContent.trim();
                            if (cellText === 'Arquivoteste2.pdf') {
                            return true;
                            }
                            }
                            }
                            return false;
                            }");

                            // Verificar se o Arquivo teste 2.pdf está na tabela Contrato Social Última Alteração
                            var arquivoContratoSocUltimaAlt = await Page.EvaluateAsync<bool>(@"() => {
                            const tbody = document.getElementById('listaContratoSocUltimaAltResumoPJ');
                            if (tbody) {
                            const rows = tbody.getElementsByTagName('tr');
                            for (let row of rows) {
                            const cellText = row.cells[0].textContent.trim();
                            if (cellText === 'Arquivoteste2.pdf') {
                            return true;
                            }
                            }
                            }
                            return false;
                            }");

                            if (arquivoDocumentoIdentif == true && arquivoComprovRes == true && arquivoDocIdentifCpf == true && arquivoContratoSocUltimaAlt == true)
                            {
                                Console.WriteLine("Os documentos foram anexados");
                                formularioOk++;
                            }
                            else
                            {
                                Console.WriteLine("Erro ao verificar documentos");
                                fluxoDeCadastros.ListaErros.Add("Documentos não anexados");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                errosTotais2++;
                            }

                            if (formularioOk == 8)
                            {
                                fluxoDeCadastros.FormularioCompletoNoPortal = "✅";
                            }
                            else
                            {
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                                fluxoDeCadastros.ListaErros.Add("Nem todos os campos da ficha foram levadas para o portal");
                                errosTotais2++;
                            }

                            var verificarStatus = Repository.Investidores.InvestidoresRepository.VerificarStatus("16695922000109", "robo@zitec.ai");

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

                            var emailChecker = new TestePortal.Utils.EmailChecker();
                            bool emailChegou = await emailChecker.CheckForNotificationEmailAsync("Cadastro de Investidor - IDSF!");

                            if (emailChegou)
                            {
                                Console.WriteLine("E-mail com dados de cadastro de investidor chegou!");
                                fluxoDeCadastros.EmailRecebido = "✅";
                            }
                            else
                            {
                                Console.WriteLine("E-mail com dados de cadastro de investidor não chegou.");
                                fluxoDeCadastros.ListaErros.Add("E-mail com dados de cadastro de investidor não chegou.");
                                errosTotais2++;
                                fluxoDeCadastros.EmailRecebido = "❌";
                            }
                        }
                        catch (Exception ex)

                        {
                            Console.WriteLine($"Exceção lançada: {ex}");

                        }


                        // aprovação e assinatura
                        await Page.ReloadAsync();
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                        var primeiroTr4 = Page.Locator("#listaCotistas tr").First;
                        var primeiroTd4 = primeiroTr.Locator("td").First;
                        await primeiroTd4.ClickAsync();
                        string locatorCadastro = "li:has(span.dtr-title:has-text('Cadastro')) >> span.dtr-data >> button[onclick*='OpenModalStatusCadastro']";
                        await Page.WaitForSelectorAsync(locatorCadastro);
                        await Page.Locator(locatorCadastro).ClickAsync();
                        await Page.Locator("#statusCadastro").GetByText("Aprovado").ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).FillAsync("Teste de aprovação");
                        await Page.Locator("#statusCadastroButton").ClickAsync();
                        await Task.Delay(800);
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                        var primeiroTr3 = Page.Locator("#listaCotistas tr").First;
                        var primeiroTd3 = primeiroTr.Locator("td").First;
                        await primeiroTd3.ClickAsync();
                        var buttonResumo = Page.Locator($"tr.child button[onclick=\"ModalResumoFormCotista('{idCotista}')\"]");
                        string locatorCompliance = "li:has(span.dtr-title:has-text('Compliance')) >> span.dtr-data >> button[onclick*='OpenModalStatusCompliance']";
                        await Page.WaitForSelectorAsync(locatorCompliance);
                        await Page.Locator(locatorCompliance).ClickAsync();
                        await Page.Locator("#statusCompliance label").Filter(new() { HasText = "Aprovado" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira sua mensagem..." }).FillAsync("teste aprovação");
                        await Page.Locator("#statusComplianceButton").ClickAsync();
                        await Task.Delay(1000);
                        await Task.Delay(50000);


                        bool statusAtual = false;

                        for (int i = 0; i < 5; i++)
                        {
                            statusAtual = InvestidoresRepository.VerificaStatusAgdAss("16695922000109", "robo@zitec.ai");

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

                            string idDocumentoAutentique = Repository.Investidores.InvestidoresRepository.ObterIdDocumentoAutentique(idCotista);
                            var response = AssinarDocumentosAutentique.AssinarDocumento("9ad54b27a864625573ad40327a1916db61b687c3fe8641ff7f3efdc3e985d3b3", idDocumentoAutentique);

                            if (response != null && response.Success)
                            {
                                Console.WriteLine("Documento assinado");

                                var updateStatus = Repository.Investidores.InvestidoresRepository.UpdateStatusAprovado("16695922000109", "robo@zitec.ai");

                                if (updateStatus == true)
                                {
                                    fluxoDeCadastros.DocumentoAssinado = "✅";

                                }
                                else {
                                    fluxoDeCadastros.DocumentoAssinado = "❌";
                                }

                            }
                            else
                            {
                                errosTotais2++;
                                fluxoDeCadastros.DocumentoAssinado = "❌";
                                fluxoDeCadastros.ListaErros.Add("Erro ao assinar documento");
                            }



                            await Task.Delay(15000);

                            bool statusAprovado = false;

                            for (int i = 0; i < 5; i++)
                            {

                                statusAprovado = InvestidoresRepository.VerificaStatusAprovado("16695922000109", "robo@zitec.ai");

                                if (statusAprovado == true)
                                {
                                    Console.WriteLine("Status trocado para aprovado");
                                    fluxoDeCadastros.statusAprovado = "✅";
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine($"Tentativa {i + 1} de trocar status falhou. Tentando novamente...");
                                    await Task.Delay(5000);

                                    if (i == 5)
                                    {
                                        fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aprovado");
                                        errosTotais2++;

                                    }
                                }
                            }

                        }
                        else
                        {
                            errosTotais2++;
                            fluxoDeCadastros.ListaErros.Add("Status não foi trocado para aguardando assinatura");
                        }





                        

                        //verificar no banco de dados

                        var investidorExiste = Repository.Investidores.InvestidoresRepository.VerificaExistenciaInvestidores("16695922000109", "robo@zitec.ai");

                        if (investidorExiste)
                        {
                            Console.WriteLine("investidor adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";



                            var apagarInvestidor = Repository.Investidores.InvestidoresRepository.ApagarInvestidores("16695922000109", "robo@zitec.ai");

                            if (apagarInvestidor)
                            {
                                Console.WriteLine("Investidor apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Investidor");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir investidor");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    else if (nivelLogado != NivelEnum.Master)
                    {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                        fluxoDeCadastros.statusAprovado = "❓";
                        fluxoDeCadastros.StatusEmAnalise = "❓";
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Investidores no tópico Cadastro ");
                    pagina.Nome = "Investidores";
                    pagina.StatusCode = CadastroInvestidores.Status;
                    errosTotais++;

                }

            }
            catch (TimeoutException ex)
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
            fluxoDeCadastros.TotalErros = errosTotais2;
            return (pagina, fluxoDeCadastros);

        }
        #endregion



        #region Cadastro Investidores - Fundo de investimento 


        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxosDeCadastros)> InvestidoresFundoDeInvestimento(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {

            var pagina = new Model.Pagina();
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int formularioOk = 0;
            fluxoDeCadastros.Fluxo = "Investidor - Fundo de investimento";
            fluxoDeCadastros.DocumentoAssinado = "❓";
            fluxoDeCadastros.statusAprovado = "❓";
            fluxoDeCadastros.EmailRecebido = "❓";
            var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            try
            {
                var CadastroInvestidores = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/CotistaInterno.aspx");
                if (CadastroInvestidores.Status == 200)
                {

                    string seletorTabela = "#tabelaCotista";

                    Console.Write("Investidores PJ - Cadastro: ");
                    Console.WriteLine(CadastroInvestidores.Status);
                    pagina.StatusCode = CadastroInvestidores.Status;
                    pagina.Nome = "Investidores";
                    pagina.BaixarExcel = "❓";
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

                    if (nivelLogado == NivelEnum.Master)
                    {

                        var apagarInvestidor2 = Repository.Investidores.InvestidoresFundInvest.ApagarInvestidores("24426716000113", "robo@zitec.ai");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo Investidor" }).ClickAsync();
                        await Page.Locator("#cpfCnpjCotistaInterno").ClickAsync();
                        await Page.Locator("#cpfCnpjCotistaInterno").FillAsync("244.267.160-00113");
                        await Page.Locator("#btnAvancarCadastroCotista").ClickAsync();
                        await Page.Locator("#emailFundoInvest").ClickAsync();
                        await Page.Locator("#emailFundoInvest").FillAsync("robo@zitec.ai");
                        await Page.Locator("#obsFundoInvest").ClickAsync();
                        await Page.Locator("#obsFundoInvest").FillAsync("teste");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                        //Procura o registro para copiar o link
                        await Page.ReloadAsync();
                        await Task.Delay(2000);
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                        var primeiroTr = Page.Locator("#listaCotistas tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Task.Delay(400);

                        int idCotista = InvestidoresFundInvest.ObterIdCotista("24426716000113", "robo@zitec.ai");
                        await Task.Delay(400);
                        string token = InvestidoresFundInvest.ObterToken("24426716000113", "robo@zitec.ai");
                        string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.COTISTAFUNDINVST"];
                        string copiedUrl = $"{baseUrl}{token}";
                        var newPage = await context.NewPageAsync();
                        await newPage.GotoAsync(copiedUrl);
                        //await newPage.PauseAsync();

                        await newPage.Locator("#email_PJ").ClickAsync();
                        await newPage.Locator("#email_PJ").FillAsync("robo@zitec.ai");
                        await Task.Delay(200);
                        await newPage.Locator("#GIIN_FI").ClickAsync();
                        await newPage.Locator("#GIIN_FI").FillAsync("teste");
                        await Task.Delay(200);
                        await newPage.Locator("#admFundo_FI").ClickAsync();
                        await newPage.Locator("#admFundo_FI").FillAsync("teste");
                        await Task.Delay(200);
                        await newPage.Locator("#admCNPJ_FI").ClickAsync();
                        await newPage.Locator("#admCNPJ_FI").FillAsync("53.300.608/0001-06");
                        await Task.Delay(200);
                        await newPage.Locator("#admContato_FI").ClickAsync();
                        await newPage.Locator("#admContato_FI").FillAsync("teste");
                        await Task.Delay(200);
                        await newPage.Locator("#admTelefone_OCA").ClickAsync();
                        await newPage.Locator("#admTelefone_OCA").FillAsync("(11) 97485-2148");
                        await Task.Delay(200);
                        await newPage.Locator("#admEmail_OCA").ClickAsync();
                        await newPage.Locator("#admEmail_OCA").FillAsync("administrador@gmail.com");
                        await Task.Delay(200);
                        await newPage.Locator("#body_OcupacaoContatoAdministrador div").Filter(new() { HasText = "Gestor do Fundo:" }).Nth(1).ClickAsync();
                        await newPage.Locator("#gestorFundo_OCA").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#gestorFundo_OCA").FillAsync("gestor");
                        await newPage.Locator("#gestorCPFCNPJ_OCA").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#gestorCPFCNPJ_OCA").FillAsync("496.248.668-30");
                        await newPage.Locator("#gestorContato_OCA").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#gestorContato_OCA").FillAsync("contato");
                        await newPage.GetByRole(AriaRole.Textbox, new() { Name = "___.___.___-__" }).ClickAsync();
                        await newPage.GetByRole(AriaRole.Textbox, new() { Name = "___.___.___-__" }).FillAsync("496.248.668-30");
                        await Task.Delay(200);
                        await newPage.Locator("#telefoneCustodiante_OCC").ClickAsync();
                        await newPage.Locator("#telefoneCustodiante_OCC").FillAsync("(11) 97547-8874");
                        await Task.Delay(200);
                        await newPage.Locator("#emailCustodiante_OCC").ClickAsync();
                        await newPage.Locator("#emailCustodiante_OCC").FillAsync("robo@zitec.ai");
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                        await newPage.GetByPlaceholder("_____-___", new() { Exact = true }).ClickAsync();
                        await newPage.GetByPlaceholder("_____-___", new() { Exact = true }).FillAsync("07084-370");
                        await Task.Delay(200);
                        await newPage.GetByText("Endereço Comercial: País: CEP").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#enderecoComercial_END").ClickAsync();
                        await newPage.Locator("#enderecoComercial_END").FillAsync("endereço");
                        await Task.Delay(200);
                        await newPage.Locator("#pais_END").ClickAsync();
                        await newPage.Locator("#pais_END").FillAsync("Brasil");
                        await Task.Delay(200);
                        await newPage.Locator("#numero_END").ClickAsync();
                        await newPage.Locator("#numero_END").FillAsync("777");
                        await Task.Delay(200);
                        await newPage.Locator("#bairro_END").ClickAsync();
                        await newPage.Locator("#bairro_END").FillAsync("bairro");
                        await Task.Delay(200);
                        await newPage.Locator("#cidade_END").ClickAsync();
                        await newPage.Locator("#cidade_END").FillAsync("cidade");
                        await Task.Delay(200);
                        await newPage.Locator("#estado_END").ClickAsync();
                        await newPage.Locator("#estado_END").FillAsync("SP");
                        await Task.Delay(200);
                        await newPage.Locator("#telefoneComercial_END").ClickAsync();
                        await newPage.Locator("#telefoneComercial_END").FillAsync("(11) 98745-2123");
                        await Task.Delay(200);
                        await newPage.Locator("#celularComercial_END").ClickAsync();
                        await newPage.Locator("#celularComercial_END").FillAsync("(11) 96547-8985");
                        await Task.Delay(200);
                        await newPage.Locator("#outroTelefone_END").ClickAsync();
                        await newPage.Locator("#outroTelefone_END").FillAsync("(77) 95478-5874");
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                        await newPage.Locator("#situacaoPatrimonial_SP").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#situacaoPatrimonial_SP").FillAsync("teste");
                        await newPage.Locator("#patrimonioRendimento_SP").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#patrimonioRendimento_SP").FillAsync("teste");
                        await newPage.Locator("#patrimonioLiquido_SP").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#patrimonioLiquido_SP").FillAsync("1000,000");
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "+ Adicionar Representante" }).ClickAsync();
                        await newPage.Locator("#nomeRepresentante_RL").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#nomeRepresentante_RL").FillAsync("Jessica Vitoria Tavares");
                        await newPage.GetByRole(AriaRole.Textbox, new() { Name = "___.___.___-__" }).ClickAsync();
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Textbox, new() { Name = "___.___.___-__" }).FillAsync("496.248.668-30");
                        await newPage.Locator("#dtNascRepresentante_RL").FillAsync("2004-05-17");
                        await Task.Delay(200);
                        await newPage.Locator("#emailRepresentante_RL").ClickAsync();
                        await newPage.Locator("#emailRepresentante_RL").FillAsync("robo@zitec.ai");
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(__) _____-____" }).ClickAsync();
                        await newPage.GetByRole(AriaRole.Textbox, new() { Name = "(__) _____-____" }).FillAsync("(11) 87451-4511");
                        await Task.Delay(200);
                        await newPage.Locator("#modal-AddRepresentante p").Filter(new() { HasText = "Não:" }).GetByRole(AriaRole.Radio).CheckAsync();
                        await newPage.Locator("#input_DocRepresentante_RL").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "Adicionar Representante", Exact = true }).ClickAsync();
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "+ Adicionar Conta Bancária" }).ClickAsync();
                        await newPage.Locator("#banco_CB").ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#banco_CB").FillAsync("439");
                        await newPage.Locator("#agencia_CB").ClickAsync();
                        await newPage.Locator("#agencia_CB").FillAsync("6541");
                        await Task.Delay(200);
                        await newPage.Locator("#conta_CB").ClickAsync();
                        await newPage.Locator("#conta_CB").FillAsync("54811");
                        await Task.Delay(200);
                        await newPage.Locator("#digitoconta_CB").ClickAsync();
                        await newPage.Locator("#digitoconta_CB").FillAsync("5");
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "Adicionar Conta", Exact = true }).ClickAsync();
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                        await Task.Delay(200);
                        await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                        await Task.Delay(200);
                        await newPage.Locator("#regulamentoFundo_DI").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                        await Task.Delay(200);
                        await newPage.Locator("#comprovanteEndereco_DI").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                        await newPage.Locator("#btnFinalizar").ClickAsync();
                        await Task.Delay(2000);
                        await newPage.CloseAsync();
                        await Page.ReloadAsync();
                        await Task.Delay(3000);
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Task.Delay(800);
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                        var primeiroTr2 = Page.Locator("#listaCotistas tr").First;
                        var primeiroTd2 = primeiroTr.Locator("td").First;
                        await Task.Delay(400);
                        await primeiroTd.ClickAsync();
                        var button = Page.Locator($"tr.child button[onclick=\"ModalResumoFormCotistaFundo('{idCotista}')\"]");

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

                        //verificar Resumo cotista - Fundo de investimento 

                        await Page.WaitForSelectorAsync("#razaoSocial_PJ");
                        var razaoSocialPJ = await Page.EvaluateAsync<string>("() => document.getElementById('razaoSocial_PJ').value");
                        await Page.WaitForSelectorAsync("#CNPJ_PJ");
                        var cnpjPJ = await Page.EvaluateAsync<string>("() => document.getElementById('CNPJ_PJ').value");
                        await Page.WaitForSelectorAsync("#dtConstituicao_PJ");
                        var dtConstituicaoPJ = await Page.EvaluateAsync<string>("() => document.getElementById('dtConstituicao_PJ').value");
                        await Page.WaitForSelectorAsync("#email_PJ");
                        var emailPJ = await Page.EvaluateAsync<string>("() => document.getElementById('email_PJ').value");
                        await Page.WaitForSelectorAsync("#GIIN_FI");
                        var giinFI = await Page.EvaluateAsync<string>("() => document.getElementById('GIIN_FI').value");
                        await Page.WaitForSelectorAsync("#admFundo_FI");
                        var admFundoFI = await Page.EvaluateAsync<string>("() => document.getElementById('admFundo_FI').value");
                        await Page.WaitForSelectorAsync("#admCNPJ_FI");
                        var admCnpjFI = await Page.EvaluateAsync<string>("() => document.getElementById('admCNPJ_FI').value");
                        await Page.WaitForSelectorAsync("#admContato_FI");
                        var admContatoFI = await Page.EvaluateAsync<string>("() => document.getElementById('admContato_FI').value");
                        await Page.WaitForSelectorAsync("#admTelefone_OCA");
                        var admTelefoneOCA = await Page.EvaluateAsync<string>("() => document.getElementById('admTelefone_OCA').value");
                        await Page.WaitForSelectorAsync("#admEmail_OCA");
                        var admEmailOCA = await Page.EvaluateAsync<string>("() => document.getElementById('admEmail_OCA').value");
                        await Page.WaitForSelectorAsync("#gestorFundo_OCA");
                        var gestorFundoOCA = await Page.EvaluateAsync<string>("() => document.getElementById('gestorFundo_OCA').value");
                        await Page.WaitForSelectorAsync("#gestorCPFCNPJ_OCA");
                        var gestorCnpjCpfOCA = await Page.EvaluateAsync<string>("() => document.getElementById('gestorCPFCNPJ_OCA').value");
                        await Page.WaitForSelectorAsync("#CPFcontatoGestor_OCA");
                        var cpfContatoGestorOCA = await Page.EvaluateAsync<string>("() => document.getElementById('CPFcontatoGestor_OCA').value");
                        await Page.WaitForSelectorAsync("#telefoneCustodiante_OCC");
                        var telefoneCustodianteOCC = await Page.EvaluateAsync<string>("() => document.getElementById('telefoneCustodiante_OCC').value");
                        await Page.WaitForSelectorAsync("#emailCustodiante_OCC");
                        var emailCustodianteOCC = await Page.EvaluateAsync<string>("() => document.getElementById('emailCustodiante_OCC').value");
                        await Page.WaitForSelectorAsync("#gestorContato_OCA");
                        var gestorContatoOCA = await Page.EvaluateAsync<string>("() => document.getElementById('gestorContato_OCA').value");


                        if (razaoSocialPJ == "ESMERALDA - FUNDO DE INVESTIMENTO EM DIREITOS CREDITORIOS" && dtConstituicaoPJ == "2016-02-24" &&
                            emailPJ == "robo@zitec.ai" && giinFI == "teste" && admContatoFI == "teste" &&
                            admTelefoneOCA == "(11) 97485-2148" && admEmailOCA == "administrador@gmail.com" && gestorFundoOCA == "gestor" &&
                            telefoneCustodianteOCC == "(11) 97547-8874" && emailCustodianteOCC == "robo@zitec.ai" &&
                            gestorContatoOCA == "contato"
                             )
                        {

                            if (cnpjPJ == "24.426.716/0001-13" || cnpjPJ == "24426716000113" && admCnpjFI == "53.300.608/0001-06" || admCnpjFI == "53300608000106" &&
                                  cpfContatoGestorOCA == "496.248.668-30" || cpfContatoGestorOCA == "49624866830")
                            {
                                Console.WriteLine("Os campos foram salvos corretamente em resumo cotista!");
                                formularioOk++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Erro ao salvar campos corretamente em resumo cotista!");
                            errosTotais2++;
                            fluxoDeCadastros.ListaErros.Add("Campos de resumo cotista não foram salvos corretamente.");
                            fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        }

                        // verificar endereço comercial

                        await Page.Locator("#EnderecoFI-tabResumo").ClickAsync();

                        await Page.WaitForSelectorAsync("#enderecoComercial_END");
                        var enderecoCom = await Page.EvaluateAsync<string>("() => document.getElementById('enderecoComercial_END').value");
                        await Page.WaitForSelectorAsync("#pais_END");
                        var paisEND = await Page.EvaluateAsync<string>("() => document.getElementById('pais_END').value");
                        await Page.WaitForSelectorAsync("#CEP_END");
                        var cepEND = await Page.EvaluateAsync<string>("() => document.getElementById('CEP_END').value");
                        await Page.WaitForSelectorAsync("#numero_END");
                        var numeroEND = await Page.EvaluateAsync<string>("() => document.getElementById('numero_END').value");
                        await Page.WaitForSelectorAsync("#bairro_END");
                        var bairroEND = await Page.EvaluateAsync<string>("() => document.getElementById('bairro_END').value");
                        await Page.WaitForSelectorAsync("#cidade_END");
                        var cidadeEND = await Page.EvaluateAsync<string>("() => document.getElementById('cidade_END').value");
                        await Page.WaitForSelectorAsync("#estado_END");
                        var estadoEND = await Page.EvaluateAsync<string>("() => document.getElementById('estado_END').value");
                        await Page.WaitForSelectorAsync("#telefoneComercial_END");
                        var telefoneComEND = await Page.EvaluateAsync<string>("() => document.getElementById('telefoneComercial_END').value");
                        await Page.WaitForSelectorAsync("#celularComercial_END");
                        var celularComEND = await Page.EvaluateAsync<string>("() => document.getElementById('celularComercial_END').value");
                        await Page.WaitForSelectorAsync("#outroTelefone_END");
                        var outroTelefoneEND = await Page.EvaluateAsync<string>("() => document.getElementById('outroTelefone_END').value");
                        await Page.WaitForSelectorAsync("input[name='notificaExtrato']");
                        var notificaoChecked = await Page.EvaluateAsync<bool>("() => document.querySelector('input[name=\"notificaExtrato\"]').checked");

                        if (enderecoCom == "endereço" && paisEND == "Brasil" && cepEND == "07084-370" && numeroEND == "777" && bairroEND == "bairro" &&
                            cidadeEND == "cidade" && estadoEND == "SP" && telefoneComEND == "(11) 98745-2123" && celularComEND == "(11) 96547-8985" && outroTelefoneEND == "(77) 95478-5874" &&
                            notificaoChecked == true

                            )
                        {
                            Console.WriteLine("Os campos foram salvos corretamente em endereço!");
                            formularioOk++;
                        }
                        else
                        {
                            Console.WriteLine("Erro ao salvar campos corretamente em endereço!");
                            errosTotais2++;
                            fluxoDeCadastros.ListaErros.Add("Campos de endereço não foram salvos corretamente.");
                            fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        }

                        // verificar patrimonios 

                        await Page.Locator("#PatrimonialFI-tabResumo").ClickAsync();
                        string dataAtual2 = DateTime.Now.ToString("yyyy-MM-dd");
                        await Page.WaitForSelectorAsync("#situacaoPatrimonial_SP");
                        var situacaoPatrimonialSP = await Page.EvaluateAsync<string>("() => document.getElementById('situacaoPatrimonial_SP').value");
                        await Page.WaitForSelectorAsync("#patrimonioRendimento_SP");
                        var patrimonioRendimentoSP = await Page.EvaluateAsync<string>("() => document.getElementById('patrimonioRendimento_SP').value");
                        await Page.WaitForSelectorAsync("#dataSituacaoPatrimonial_SP");
                        var dataSituacaoPatrimonialSP = await Page.EvaluateAsync<string>("() => document.getElementById('dataSituacaoPatrimonial_SP').value");
                        await Page.WaitForSelectorAsync("#patrimonioLiquido_SP");
                        var patrimonioLiquidoSP = await Page.EvaluateAsync<string>("() => document.getElementById('patrimonioLiquido_SP').value");
                        var patrimonioAtz = patrimonioLiquidoSP;

                        if (situacaoPatrimonialSP == "teste" && patrimonioRendimentoSP == "teste" && dataSituacaoPatrimonialSP == dataAtual2 && patrimonioLiquidoSP == patrimonioAtz)
                        {
                            Console.WriteLine("Os campos foram salvos corretamente em patrimônios");
                            formularioOk++;
                        }
                        else
                        {
                            Console.WriteLine("Erro ao salvar campos corretamente em patrimônios!");
                            errosTotais2++;
                            fluxoDeCadastros.ListaErros.Add("Campos de patrimônios não foram salvos corretamente.");
                            fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        }

                        //verificar dados de representates
                        await Page.WaitForSelectorAsync("#tabelaRepresentanteResumoFI");

                        var nomeRepresentante = await Page.EvaluateAsync<string>(
                            "() => document.querySelector('#tabelaRepresentanteResumoFI tbody tr td:nth-of-type(1)').textContent.trim()");

                        var cpfRepresentante = await Page.EvaluateAsync<string>(
                            "() => document.querySelector('#tabelaRepresentanteResumoFI tbody tr td:nth-of-type(2)').textContent.trim()");

                        var dataNascimento = await Page.EvaluateAsync<string>(
                            "() => document.querySelector('#tabelaRepresentanteResumoFI tbody tr td:nth-of-type(3)').textContent.trim()");

                        var emailRepresentante = await Page.EvaluateAsync<string>(
                            "() => document.querySelector('#tabelaRepresentanteResumoFI tbody tr td:nth-of-type(4)').textContent.trim()");

                        if (nomeRepresentante == "Jessica Vitoria Tavares" && cpfRepresentante == "49624866830" &&
                            dataNascimento == "17/05/2004" && emailRepresentante == "robo@zitec.ai")
                        {
                            Console.WriteLine("Os campos foram salvos corretamente em representantes");
                            formularioOk++;
                        }
                        else
                        {
                            Console.WriteLine("Erro ao salvar campos corretamente em representantes!");
                            errosTotais2++;
                            fluxoDeCadastros.ListaErros.Add("Campos de representantes não foram salvos corretamente.");
                            fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        }

                        //verificar conta bancária 

                        await Page.Locator("#ContaBancariaFI-tabResumo").ClickAsync();

                        var banco = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContaBancaria_CB .list-group-item').querySelector('b:nth-of-type(1)').nextSibling.textContent.trim()");
                        var agencia = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContaBancaria_CB .list-group-item').querySelector('b:nth-of-type(2)').nextSibling.textContent.trim()");
                        var conta = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContaBancaria_CB .list-group-item').querySelector('b:nth-of-type(3)').nextSibling.textContent.trim()");
                        var tipoConta = await Page.EvaluateAsync<string>("() => document.querySelector('#listaContaBancaria_CB .list-group-item').querySelector('b:nth-of-type(4)').nextSibling.textContent.trim()");

                        if (banco == "439" && agencia == "6541" && conta == "54811-5" && tipoConta == "CORRENTE")
                        {
                            Console.WriteLine("Os campos foram salvos corretamente em conta bancária");
                            formularioOk++;
                        }
                        else
                        {
                            Console.WriteLine("Erro ao salvar campos corretamente em conta bancária!");
                            errosTotais2++;
                            fluxoDeCadastros.ListaErros.Add("Campos de conta bancária não foram salvos corretamente.");
                            fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                        }

                        //verificar documentos

                        await Page.Locator("#DocumentosFI-tabResumo").ClickAsync();
                        bool downloadComprovanteConcluido = await Repository.Investidores.InvestidoresFundInvest.BaixarArquivo(Page, "btnDownloadComprovante", "Comprovante.pdf");
                        bool downloadRegulamentoConcluido = await Repository.Investidores.InvestidoresFundInvest.BaixarArquivo(Page, "btnDownloadRegulamento", "Regulamento.pdf");

                        if (downloadComprovanteConcluido == true && downloadRegulamentoConcluido == true)
                        {
                            Console.WriteLine("arquivos baixados corretamente");
                            formularioOk++;
                        }
                        else
                        {
                            Console.WriteLine("Erro ao baixar arquivos!");
                            errosTotais2++;
                            fluxoDeCadastros.ListaErros.Add("Erro ao fazer download dos arquivos.");
                            fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
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

                        //verificar no banco de dados

                        var investidorExiste = Repository.Investidores.InvestidoresFundInvest.VerificaExistenciaInvestidores("24426716000113", "robo@zitec.ai");

                        if (investidorExiste)
                        {

                            Console.WriteLine("investidor adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            var verificarStatus = Repository.Investidores.InvestidoresFundInvest.VerificarStatus("24426716000113", "robo@zitec.ai");

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

                            var apagarInvestidor = Repository.Investidores.InvestidoresFundInvest.ApagarInvestidores("24426716000113", "robo@zitec.ai");

                            if (apagarInvestidor)
                            {
                                Console.WriteLine("Investidor apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Investidor");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir investidor");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    else if (nivelLogado != NivelEnum.Master)
                    {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Investidores no tópico Cadastro ");
                    pagina.Nome = "Investidores";
                    pagina.StatusCode = CadastroInvestidores.Status;
                    errosTotais++;
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
            fluxoDeCadastros.TotalErros = errosTotais2;
            return (pagina, fluxoDeCadastros);




        }





        #endregion
    }

}

