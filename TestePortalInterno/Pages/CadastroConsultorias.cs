using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortalInterno.Model;
using TestePortalInterno.Repositorys;
using static TestePortalInterno.Model.Usuario;
using System.IO.Packaging;

namespace TestePortalInterno.Pages
{
    public class CadastroConsultorias
    {
        public static async Task<(Model.Pagina pagina, Model.FluxosDeCadastros fluxosDeCadastros)> Consultorias(IPage Page, IBrowserContext context, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            int errosTotais2 = 0;
            int formularioOk = 0;
            var fluxoDeCadastros = new Model.FluxosDeCadastros();
            try
            {
                var CadastroConsultorias = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Consultoria.aspx");

                if (CadastroConsultorias.Status == 200)
                {
                    string seletorTabela = "#tabelaConsultoria";
                    string seletorBotao = "";

                    Console.Write("Consultorias - Cadastro: ");
                    pagina.Nome = "Consultorias";
                    pagina.StatusCode = CadastroConsultorias.Status;
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
                        fluxoDeCadastros.Fluxo = "Consultoria";
                        fluxoDeCadastros.DocumentoAssinado = "❓";
                        fluxoDeCadastros.statusAprovado = "❓";
                        fluxoDeCadastros.EmailRecebido = "❓";
                        var ApagarConsultorias2 = Repositorys.Consultorias.ApagarConsultorias("16695922000109", "robo@zitec.ai");
                        await Task.Delay(500);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                        await Page.Locator("#CnpjConsultoria").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#CnpjConsultoria").FillAsync("16695922000109");
                        await Task.Delay(300);
                        await Page.Locator("#btnAvancarCadastroConsultora").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#emailConsultora").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#emailConsultora").FillAsync("robo@zitec.ai");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                        // procurar o registro cadastrado e colar o link em uma nova para dar continuidade
                        await Page.ReloadAsync();
                        await Page.GetByLabel("Pesquisar").ClickAsync();
                        await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                        var primeiroTr = Page.Locator("#listaConsultoria tr").First;
                        var primeiroTd = primeiroTr.Locator("td").First;
                        await primeiroTd.ClickAsync();
                        await Task.Delay(400);

                        try
                        {

                            int? idConsultoria = Repositorys.Consultorias.ObterIdConsultoria("16695922000109", "robo@zitec.ai");
                            var buttonSelector = $"tr.child button#\\3{idConsultoria.ToString().Substring(0, 1)} {idConsultoria.ToString().Substring(1)}_url.btn.btn-default[title='Copiar Link']";
                            await Page.Locator(buttonSelector).ClickAsync();
                            await Task.Delay(400);

                            string token = Repositorys.Consultorias.ObterToken("16695922000109", "robo@zitec.ai");
                            string baseUrl = ConfigurationManager.AppSettings["LINK.FICHA.CONSULTORIA"];
                            string copiedUrl = $"{baseUrl}{token}";
                            var newPage = await context.NewPageAsync();
                            await newPage.GotoAsync(copiedUrl);


                            await newPage.Locator("#dtConstituicao").FillAsync("20/12/2023");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Atividade Principal (CNAE)", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Atividade Principal (CNAE)", new() { Exact = true }).FillAsync("info");
                            await Task.Delay(200);
                            await newPage.Locator("#controlAcionario").SelectOptionAsync(new[] { "nacional" });
                            await Task.Delay(200);
                            await newPage.Locator("#ppeNao").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#btnAvancar").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000-000", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000-000", new() { Exact = true }).FillAsync("07084-370");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Número", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Número", new() { Exact = true }).FillAsync("901");
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("(00)0000-").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("(00)0000-").FillAsync("(11)9601-8324");
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("(00)0000-").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Celular Comercial", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Celular Comercial", new() { Exact = true }).FillAsync("(11)96018-3248");
                            await Task.Delay(200);
                            await newPage.Locator("#btnAvancar").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#patrimLiq").FillAsync("100000");
                            await Task.Delay(200);
                            await newPage.Locator("#faturamento12Meses").FillAsync("1000");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Origem Capital").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Origem Capital").FillAsync("contratos");
                            await Task.Delay(200);
                            await Task.Delay(200);
                            await newPage.Locator("#nomeDiretor").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#nomeDiretor").FillAsync("Jessica Vitoria Tavares");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*CPF:").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*CPF:").FillAsync("496.248.668-30");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email:").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email:").FillAsync("teste@gmail.com");
                            await Task.Delay(200);
                            await newPage.Locator("#addDiretor").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#tipoPessoaAcionSoc").SelectOptionAsync(new[] { "fisica" });
                            await Task.Delay(200);
                            await newPage.Locator("#nomeBenefSoc").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#nomeBenefSoc").FillAsync("Barbara");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*CPF", Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*CPF", Exact = true }).FillAsync("496.248.668-30");
                            await Task.Delay(200);
                            await newPage.GetByLabel("E-mail:*").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("E-mail:*").FillAsync("robo@zitec.ai");
                            await Task.Delay(200);
                            await newPage.Locator("div:nth-child(11) > .inputRadio > div:nth-child(2)").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#vinculoPpeNao").CheckAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#addBenefAcionSoc").ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#btnAvancar").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome Completo", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome Completo", new() { Exact = true }).FillAsync("Guilherme Costa");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*CPF" }).FillAsync("496.248.668-30");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).FillAsync("14/08/2003");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).FillAsync("teste3@gmail.com");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).FillAsync("(11)95478-5474");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "ADICIONAR REPRESENTANTE" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome Completo", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Nome Completo", new() { Exact = true }).FillAsync("Caio Oliveira");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*CPF" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "*CPF" }).FillAsync("426.792.428-74");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).FillAsync("14/08/2023");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Email", new() { Exact = true }).FillAsync("robo@zitec.ai");
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("*Celular Telefone de Contato", new() { Exact = true }).FillAsync("(11)56498-4546");
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "ADICIONAR REPRESENTANTE" }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.Locator("#btnAvancar").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Conta Bancária Banco Agência").GetByLabel("Default select example").SelectOptionAsync(new[] { "69 " });
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("0000", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("0000", new() { Exact = true }).FillAsync("1564");
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByPlaceholder("00000", new() { Exact = true }).FillAsync("546456454");
                            await Task.Delay(200);
                            await newPage.GetByLabel("Digito", new() { Exact = true }).ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByLabel("Digito", new() { Exact = true }).FillAsync("5");
                            await Task.Delay(200);
                            await newPage.Locator("#addContaBancBtn").ClickAsync();
                            await Task.Delay(200);
                            await newPage.GetByRole(AriaRole.Button, new() { Name = "AVANÇAR" }).ClickAsync();
                            await newPage.Locator("#fileQddAmbima").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "Arquivo teste 2.pdf" });
                            await Task.Delay(200);
                            await newPage.Locator("#btnFinalizar").ClickAsync();
                            await Task.Delay(8000);
                            await newPage.CloseAsync();
                            await Page.ReloadAsync();
                            await Task.Delay(1000);
                            await Page.GetByLabel("Pesquisar").ClickAsync();
                            await Task.Delay(800);
                            await Page.GetByLabel("Pesquisar").FillAsync("robo@zitec.ai");
                            var primeiroTr2 = Page.Locator("#listaConsultoria tr").First;
                            var primeiroTd2 = primeiroTr.Locator("td").First;
                            await primeiroTd.ClickAsync();//ModalResumoFormConsultoria('214')

                            var button = Page.Locator($"tr.child button[onclick=\"ModalResumoFormConsultoria('{idConsultoria}')\"]");

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


                            //Verificar dados Consultoria
                            await Task.Delay(2000);
                            await Page.WaitForSelectorAsync("#razaoSocialResumo");
                            var razaoSocial = await Page.EvaluateAsync<string>("() => document.getElementById('razaoSocialResumo').value");
                            await Page.WaitForSelectorAsync("#cnpjResumo");
                            var cnpj = await Page.EvaluateAsync<string>("() => document.getElementById('cnpjResumo').value");
                            await Page.WaitForSelectorAsync("#dtConstituicaoResumo");
                            var dtConstituicao = await Page.EvaluateAsync<string>("() => document.getElementById('dtConstituicaoResumo').value");
                            await Page.WaitForSelectorAsync("#ativPrincipalResumo");
                            var ativPrincipal = await Page.EvaluateAsync<string>("() => document.getElementById('ativPrincipalResumo').value");
                            await Page.WaitForSelectorAsync("#controlAcionarioResumo");
                            var controlAcionario = await Page.EvaluateAsync<string>("() => document.getElementById('controlAcionarioResumo').value");
                            await Page.WaitForSelectorAsync("#paisConstituicaoResumo");
                            var paisConstituicao = await Page.EvaluateAsync<string>("() => document.getElementById('paisConstituicaoResumo').value");
                            await Page.WaitForSelectorAsync("#paisDomFiscalResumo");
                            var paisDomFiscal = await Page.EvaluateAsync<string>("() => document.getElementById('paisDomFiscalResumo').value");
                            await Page.WaitForSelectorAsync("#emailResumo");
                            var email = await Page.EvaluateAsync<string>("() => document.getElementById('emailResumo').value");
                            await Page.WaitForSelectorAsync("#ppeResumoNao");
                            var isPpeNaoChecked = await Page.EvaluateAsync<bool>("() => document.getElementById('ppeResumoNao').checked");

                            if (razaoSocial == "ID CORRETORA DE TITULOS E VALORES MOBILIARIOS SA" && cnpj == "16.695.922/0001-09" && dtConstituicao == "20/12/2023" && ativPrincipal == "info"
                                && controlAcionario == "Nacional" && paisConstituicao == "BRASIL" && paisDomFiscal == "BRASIL" && email == "robo@zitec.ai"
                                && isPpeNaoChecked == true
                                )
                            {
                                Console.WriteLine("Dados da consultoria salvos corretamente em dados do investidor!");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados da Consultoria");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados da Consultoria não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar dados do endereço 

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
                                bairroComercial == "Parque Continental II" && ufComercial == "São Paulo" && cidadeComercial == "Guarulhos" && telComercial == "(11)9601-8324"
                                && celComercial == "(11)96018-3248"
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

                            //verificar situação patrimonial Empresa 

                            await Page.Locator("#SituacaoPatrimonialEmp-tabResumo").ClickAsync();

                            await Page.WaitForSelectorAsync("#patrimLiqResumo");
                            var patrimLiq = await Page.EvaluateAsync<string>("() => document.getElementById('patrimLiqResumo').value");
                            await Page.WaitForSelectorAsync("#faturamento12MesesResumo");
                            var faturamento12Meses = await Page.EvaluateAsync<string>("() => document.getElementById('faturamento12MesesResumo').value");
                            await Page.WaitForSelectorAsync("#origemCapResumo");
                            await Task.Delay(200);
                            var origemCap = await Page.EvaluateAsync<string>("() => document.getElementById('origemCapResumo').value");
                            await Task.Delay(200);
                            var nomeDiretor = await Page.EvaluateAsync<string>("() => document.querySelector('#listaDiretoresResumo tr td:nth-child(1)').innerText");
                            await Task.Delay(200);
                            var cpfDiretor = await Page.EvaluateAsync<string>("() => document.querySelector('#listaDiretoresResumo tr td:nth-child(2)').innerText");
                            await Task.Delay(200);
                            var tipoPessoa = await Page.EvaluateAsync<string>("() => document.querySelector('#benefAcionSocioListResumo tr td:nth-child(1)').innerText");
                            await Task.Delay(200);
                            var nomeBeneficiario = await Page.EvaluateAsync<string>("() => document.querySelector('#benefAcionSocioListResumo tr td:nth-child(2)').innerText");
                            await Task.Delay(200);
                            var cpfCnpjBeneficiario = await Page.EvaluateAsync<string>("() => document.querySelector('#benefAcionSocioListResumo tr td:nth-child(3)').innerText");
                            await Task.Delay(200);

                            if (patrimLiq == "100000" && faturamento12Meses == "1000" && origemCap == "contratos" && nomeDiretor == "Jessica Vitoria Tavares" && cpfDiretor == "496.248.668-30"
                                && tipoPessoa == "Física" && nomeBeneficiario == "Barbara" && cpfCnpjBeneficiario == "496.248.668-30"
                                )
                            {
                                Console.WriteLine("Dados de situação patrimonial salvos corretamente");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Erro ao salvar dados de situação patrimonial");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados situação patrimonial não foram salvos corretamente.");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";
                            }

                            //verificar representantes 

                            await Page.Locator("#Representantes-tabResumo").ClickAsync();


                            await Page.WaitForSelectorAsync("#tabelaProcuradorAdd tbody");
                            var nomeRepresentante1 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(1) td:nth-child(1)').innerText");
                            await Task.Delay(200);
                            var cpfRepresentante1 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(1) td:nth-child(2)').innerText");
                            await Task.Delay(200);
                            var dataNascimentoRepresentante1 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(1) td:nth-child(3)').innerText");
                            await Task.Delay(200);
                            var emailRepresentante1 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(1) td:nth-child(4)').innerText");
                            await Task.Delay(200);
                            var celularRepresentante1 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(1) td:nth-child(5)').innerText");
                            await Task.Delay(200);
                            var nomeRepresentante2 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(2) td:nth-child(1)').innerText");
                            await Task.Delay(200);
                            var cpfRepresentante2 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(2) td:nth-child(2)').innerText");
                            await Task.Delay(200);
                            var dataNascimentoRepresentante2 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(2) td:nth-child(3)').innerText");
                            await Task.Delay(200);
                            var emailRepresentante2 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(2) td:nth-child(4)').innerText");
                            await Task.Delay(200);
                            var celularRepresentante2 = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaProcuradorAdd tbody tr:nth-child(2) td:nth-child(5)').innerText");
                            await Task.Delay(200);

                            if (nomeRepresentante1 == "Guilherme Costa" && cpfRepresentante1 == "496.248.668-30" && dataNascimentoRepresentante1 == "14/08/2003" && emailRepresentante1 == "teste3@gmail.com" &&
                                nomeRepresentante2 == "Caio Oliveira" && cpfRepresentante2 == "426.792.428-74" && dataNascimentoRepresentante2 == "14/08/2023" && emailRepresentante2 == "robo@zitec.ai"
                                && celularRepresentante2 == "11564984546"
                                )
                            {
                                Console.WriteLine("Dados de representantes foram salvos");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Dados de representantes não foram salvos corretamente");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados de representantes não foram salvos corretamente");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";

                            }

                            // verificar dados da conta bancária ContaBancaria-tabResumo

                            await Page.Locator("#ContaBancaria-tabResumo").ClickAsync();

                            await Page.WaitForSelectorAsync("#tabelaContasBancoResumo tbody");
                            var banco = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaContasBancoResumo tbody tr:nth-child(1) td:nth-child(1)').innerText");
                            await Task.Delay(200);
                            var agencia = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaContasBancoResumo tbody tr:nth-child(1) td:nth-child(2)').innerText");
                            await Task.Delay(200);
                            var conta = await Page.EvaluateAsync<string>("() => document.querySelector('#tabelaContasBancoResumo tbody tr:nth-child(1) td:nth-child(3)').innerText");
                            await Task.Delay(200);

                            if (banco == "69 - Crefisa" && agencia == "1564" && conta == "546456454-5")
                            {
                                Console.WriteLine("Dados de conta bancária foram salvos");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Dados de conta bancária não foram salvos corretamente");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Dados de conta bancária não foram salvos corretamente");
                                fluxoDeCadastros.FormularioCompletoNoPortal = "❌";

                            }

                            //verificar documentos

                            await Page.Locator("#Documentos-tabResumo").ClickAsync();

                            var arquivoEnviado = await Page.EvaluateAsync<bool>(@"() => {
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

                            if (arquivoEnviado == true)
                            {
                                Console.WriteLine("Arquivo aparece no resumo do formulário");
                                formularioOk++;

                            }
                            else
                            {
                                Console.WriteLine("Arquivo não aparece no resumo do formulário");
                                errosTotais2++;
                                fluxoDeCadastros.ListaErros.Add("Arquivo não aparece no resumo do formulário");
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

                        var ConsultoriasExiste = Repositorys.Consultorias.VerificaExistenciaConsultorias("16695922000109", "robo@zitec.ai");

                        if (ConsultoriasExiste)
                        {
                            Console.WriteLine("Consultoria adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            var verificarStatus = Repositorys.Consultorias.VerificarStatus("16695922000109", "robo@zitec.ai");
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


                            var ApagarConsultorias = Repositorys.Consultorias.ApagarConsultorias("16695922000109", "robo@zitec.ai");

                            if (ApagarConsultorias)
                            {
                                Console.WriteLine("Consultoria apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Consultoria");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }

                        else
                        {
                            Console.WriteLine("Não foi possível inserir Consultoria");
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
                    Console.Write("Erro ao carregar a página de Consultorias no tópico Cadastro ");
                    pagina.Nome = "Consultorias";
                    pagina.StatusCode = CadastroConsultorias.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
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
                fluxoDeCadastros.TotalErros = errosTotais;
                pagina.TotalErros = errosTotais;
                return (pagina, fluxoDeCadastros);
            }
            if (fluxoDeCadastros.ListaErros.Count == 0)
            {
                fluxoDeCadastros.ListaErros.Add("0");
            }
            fluxoDeCadastros.TotalErros = errosTotais2;
            pagina.TotalErros = errosTotais;
            return (pagina, fluxoDeCadastros);
        }
    }
}
