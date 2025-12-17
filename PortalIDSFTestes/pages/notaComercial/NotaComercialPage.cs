using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.notaComercial;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.data.notaComercial;

namespace PortalIDSFTestes.pages.notaComercial
{
    public class NotaComercialPage
    {
        private IPage page;
        Utils metodo;
        NotaComercialElements el = new NotaComercialElements();
        NotaComercialData data = new NotaComercialData();
        public NotaComercialPage(IPage page)
        {
            this.page = page;
            metodo = new Utils(page);
        }
        public static string GetPath()
        {
            ConfigurationManager config = new ConfigurationManager();
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            string path = config["Paths:Arquivo"].ToString();
            return path;
        }

        public async Task ValidarAcentosNotaComercialPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página de Nota Comercial");
        }
        public async Task DownloadExcel()
        {
            await metodo.ValidateDownloadAndLength(page, el.BtnExcel, "Validar Download do Excel na página de Nota Comercial");
        }

        public async Task CadastrarNotaComercial()
        {
            var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");


            await metodo.Clicar(el.BtnNovoNotaComercial, "Clicar no Botão para inserir nova nota comercial");
            await metodo.ClicarNoSeletor(el.SelectFundoCessionario, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            await metodo.ClicarNoSeletor(el.SelectProduto, "125", "Selecionar Produto Teste");
            await metodo.ClicarNoSeletor(el.SelectTipoLiquidacao, "pix", "Selecionar tipo liquidação PIX");
            await metodo.Escrever(el.CampoTomador, data.TomadorNome, $"Buscar por {data.TomadorNome} no campo tomador");
            await metodo.Clicar(el.BtnLupaTomador, "Clicar na lupa para buscar tomador");
            await metodo.Clicar(el.CedenteTest, "Clicar no cedente de teste");
            await metodo.ClicarNoSeletor(el.SelectConta, "58066857", "Selecionar conta bancaria ID");
            await metodo.Escrever(el.CampoObservacaoInfo, data.ObservacaoCadastro, "Digitar a observação no modal");
            //Sessão Envolvidos
            await metodo.Clicar(el.SessaoEnvolvidos, "Clicar na sessão Envolvidos");
            await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
            await Task.Delay(500);
            await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "1", "Selecionar Relacionado A");
            await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
            await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "empregador", "Selecionar Tipo de relação");
            await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
            await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "assinaturaSelfie", "Selecionar Forma de Validação");
            await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
            await Task.Delay(500);
            await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
            await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "idsf", "Selecionar Relacionado A");
            await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
            await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "cedente", "Selecionar Tipo de relação");
            await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
            await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "biometriaFacial", "Selecionar Forma de Validação");
            await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
            //sessão Operação
            await metodo.Clicar(el.SessaoOperacaoes, "Clicar na sessão Operações");
            await metodo.Escrever(el.CampoValorSolicitado, data.ValorSolicitado, "Preencher Valor Solicitado na sessão operações");
            await metodo.Escrever(el.CampoTaxaJuros, data.TaxaJurosPadrao, "Preencher Taxa de Juros na sessão operações");
            await metodo.Escrever(el.CampoDuração, data.DuracaoPadrao, "Preencher Duração sessão operações");
            await metodo.Escrever(el.CampoCarenciaAmortizacao, data.CarenciaAmortizacaoPadrao, "Preencher Carência de amortização sessão operações");
            await metodo.ClicarNoSeletor(el.SelectTipoCalculo, "bruto", "Selecionar Tipo de calculo na sessão operações");
            await metodo.Escrever(el.CampoDiaVencimento, data.DiaVencimento, "Preencher Dia de Vencimento na sessão operações");
            await metodo.Clicar(el.CampoDataInicio, "Clicar no campo Data de Início");
            await metodo.Escrever(el.CampoDataInicio, dataAtual, "Inserir Data Atual no seletor Data de inicio");
            await metodo.ClicarNoSeletor(el.SelectIndexPosFix, "CDI", "Selecionar Indexador Pós-Fixado na sessão Operações");
            //sessão Documentos
            await metodo.Clicar(el.SessaoDocumentos, "Clicar na sessão Documentos");
            await metodo.Clicar(el.BtnAddDocumento, "Clicar no botão Adicionar Documento");
            await metodo.EnviarArquivo(el.InputFileNotaComercial, GetPath() + "Arquivo teste 2.pdf", "Enviar Arquivo no input");
            await metodo.ClicarNoSeletor(el.SelectTipoDocumento, "cpf", "Selecionar tipo Documento na sessão Documentos");
            await metodo.Clicar(el.BtnAtualizarDocumento, "Clicar no botão Atualizar Documento");
            await metodo.Clicar(el.BtnSalvarMudancas, "Clicar no botão Salvar Mudanças");
            await metodo.ValidarTextoPresente(NotaComercialData.MsgSucessoSimulacao, "Validar mensagem de sucesso que pode visualizar simulação");
            await metodo.ValidarTextoPresente(NotaComercialData.MsgSucessoCadastro, "Validar mensagem de sucesso, nota comercial cadastrada");


        }

        public async Task CadastrarNotaComercialNegativa(string cenarioTeste)
        {
            if (cenarioTeste == "CamposEmBranco")
            {
            await metodo.Clicar(el.BtnNovoNotaComercial, "Clicar no Botão para inserir nova nota comercial");
                await metodo.Clicar(el.BtnSalvarMudancas, "Clicar no botão Salvar Mudanças");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, NotaComercialData.MsgPreenchaInfo, "Validar mensagem retornada sobre campos em branco");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, "Preencha todos os campos para simular", "Validar mensagem retornada sobre campos em branco");
            }
            else if (cenarioTeste == "CamposEmBrancoEnvolvidos")
            {
                await metodo.Clicar(el.BtnNovoNotaComercial, "Clicar no Botão para inserir nova nota comercial");
                await metodo.ClicarNoSeletor(el.SelectFundoCessionario, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
                await metodo.ClicarNoSeletor(el.SelectProduto, "125", "Selecionar Produto Teste");
                await metodo.ClicarNoSeletor(el.SelectTipoLiquidacao, "pix", "Selecionar tipo liquidação PIX");
                await metodo.Escrever(el.CampoTomador, data.TomadorNome, $"Buscar por {data.TomadorNome} no campo tomador");
                await metodo.Clicar(el.BtnLupaTomador, "Clicar na lupa para buscar tomador");
                await metodo.Clicar(el.CedenteTest, "Clicar no cedente de teste");
                await metodo.ClicarNoSeletor(el.SelectConta, "58066857", "Selecionar conta bancaria ID");
                await metodo.Escrever(el.CampoObservacaoInfo, data.ObservacaoCadastro, "Digitar a observação no modal");
                await metodo.Clicar(el.SessaoEnvolvidos, "Clicar na sessão Envolvidos");
                await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
                await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, "Preencha todos os campos obrigatórios", "Validar mensagem retornada sobre campos em branco");
            }
            else if (cenarioTeste == "CamposEmBrancoOperacao")
            {
                await metodo.Clicar(el.BtnNovoNotaComercial, "Clicar no Botão para inserir nova nota comercial");
                await metodo.ClicarNoSeletor(el.SelectFundoCessionario, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
                await metodo.ClicarNoSeletor(el.SelectProduto, "125", "Selecionar Produto Teste");
                await metodo.ClicarNoSeletor(el.SelectTipoLiquidacao, "pix", "Selecionar tipo liquidação PIX");
                await metodo.Escrever(el.CampoTomador, data.TomadorNome, $"Buscar por {data.TomadorNome} no campo tomador");
                await metodo.Clicar(el.BtnLupaTomador, "Clicar na lupa para buscar tomador");
                await metodo.Clicar(el.CedenteTest, "Clicar no cedente de teste");
                await metodo.ClicarNoSeletor(el.SelectConta, "58066857", "Selecionar conta bancaria ID");
                await metodo.Escrever(el.CampoObservacaoInfo, data.ObservacaoCadastro, "Digitar a observação no modal");
                //Sessão Envolvidos
                await metodo.Clicar(el.SessaoEnvolvidos, "Clicar na sessão Envolvidos");
                await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
                await Task.Delay(500);
                await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "1", "Selecionar Relacionado A");
                await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
                await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "empregador", "Selecionar Tipo de relação");
                await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
                await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "assinaturaSelfie", "Selecionar Forma de Validação");
                await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
                await Task.Delay(500);
                await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
                await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "idsf", "Selecionar Relacionado A");
                await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
                await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "cedente", "Selecionar Tipo de relação");
                await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
                await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "biometriaFacial", "Selecionar Forma de Validação");
                await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
                //sessão Operação
                await metodo.Clicar(el.SessaoOperacaoes, "Clicar na sessão Operações");
                await metodo.Clicar(el.BtnSalvarMudancas, "Clicar no botão Salvar Mudanças");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, "Preencha todos os campos para simular", "Validar mensagem retornada sobre campos em branco");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, NotaComercialData.MsgPreenchaInfo, "Validar mensagem retornada sobre campos em branco");
            }
            else if (cenarioTeste == "AmortizacaoMaiorQueDuracao")
            {
                var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

                await metodo.Clicar(el.BtnNovoNotaComercial, "Clicar no Botão para inserir nova nota comercial");
                await metodo.ClicarNoSeletor(el.SelectFundoCessionario, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
                await metodo.ClicarNoSeletor(el.SelectProduto, "125", "Selecionar Produto Teste");
                await metodo.ClicarNoSeletor(el.SelectTipoLiquidacao, "pix", "Selecionar tipo liquidação PIX");
                await metodo.Escrever(el.CampoTomador, data.TomadorNome, $"Buscar por {data.TomadorNome} no campo tomador");
                await metodo.Clicar(el.BtnLupaTomador, "Clicar na lupa para buscar tomador");
                await metodo.Clicar(el.CedenteTest, "Clicar no cedente de teste");
                await metodo.ClicarNoSeletor(el.SelectConta, "58066857", "Selecionar conta bancaria ID");
                await metodo.Escrever(el.CampoObservacaoInfo, data.ObservacaoCadastro, "Digitar a observação no modal");
                //Sessão Envolvidos
                await metodo.Clicar(el.SessaoEnvolvidos, "Clicar na sessão Envolvidos");
                await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
                await Task.Delay(500);
                await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "1", "Selecionar Relacionado A");
                await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
                await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "empregador", "Selecionar Tipo de relação");
                await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
                await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "assinaturaSelfie", "Selecionar Forma de Validação");
                await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
                await Task.Delay(500);
                await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
                await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "idsf", "Selecionar Relacionado A");
                await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
                await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "cedente", "Selecionar Tipo de relação");
                await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
                await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "biometriaFacial", "Selecionar Forma de Validação");
                await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
                //sessão Operação
                await metodo.Clicar(el.SessaoOperacaoes, "Clicar na sessão Operações");
                await metodo.Escrever(el.CampoValorSolicitado, data.ValorSolicitado, "Preencher Valor Solicitado na sessão operações");
                await metodo.Escrever(el.CampoTaxaJuros, data.TaxaJurosPadrao, "Preencher Taxa de Juros na sessão operações");
                await metodo.Escrever(el.CampoDuração, data.DuracaoPadrao, "Preencher Duração sessão operações");
                await metodo.Escrever(el.CampoCarenciaAmortizacao, data.CarenciaAmortizacaoNegativa, "Preencher Carência de amortização sessão operações");
                await metodo.ClicarNoSeletor(el.SelectTipoCalculo, "bruto", "Selecionar Tipo de calculo na sessão operações");
                await metodo.Escrever(el.CampoDiaVencimento, data.DiaVencimento, "Preencher Dia de Vencimento na sessão operações");
                await metodo.Clicar(el.CampoDataInicio, "Clicar no campo Data de Início");
                await metodo.Escrever(el.CampoDataInicio, dataAtual, "Inserir Data Atual no seletor Data de inicio");
                await metodo.ClicarNoSeletor(el.SelectIndexPosFix, "CDI", "Selecionar Indexador Pós-Fixado na sessão Operações");
                await metodo.Clicar(el.BtnSalvarMudancas, "Clicar no botão Salvar Mudanças");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, "A carência de amortização não pode ser igual ou maior que a duração", "Validar mensagem retornada sobre campos em branco");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, "Parcelas devem ser geradas corretamente", "Validar mensagem retornada sobre campos em branco");

            }
            else if (cenarioTeste == "CarenciaJurosMaiorQueJurosPrincipal")
            {
                var dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

                await metodo.Clicar(el.BtnNovoNotaComercial, "Clicar no Botão para inserir nova nota comercial");
                await metodo.ClicarNoSeletor(el.SelectFundoCessionario, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
                await metodo.ClicarNoSeletor(el.SelectProduto, "125", "Selecionar Produto Teste");
                await metodo.ClicarNoSeletor(el.SelectTipoLiquidacao, "pix", "Selecionar tipo liquidação PIX");
                await metodo.Escrever(el.CampoTomador, data.TomadorNome, $"Buscar por {data.TomadorNome} no campo tomador");
                await metodo.Clicar(el.BtnLupaTomador, "Clicar na lupa para buscar tomador");
                await metodo.Clicar(el.CedenteTest, "Clicar no cedente de teste");
                await metodo.ClicarNoSeletor(el.SelectConta, "58066857", "Selecionar conta bancaria ID");
                await metodo.Escrever(el.CampoObservacaoInfo, data.ObservacaoCadastro, "Digitar a observação no modal");
                //Sessão Envolvidos
                await metodo.Clicar(el.SessaoEnvolvidos, "Clicar na sessão Envolvidos");
                await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
                await Task.Delay(500);
                await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "1", "Selecionar Relacionado A");
                await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
                await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "empregador", "Selecionar Tipo de relação");
                await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
                await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "assinaturaSelfie", "Selecionar Forma de Validação");
                await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
                await Task.Delay(500);
                await metodo.Clicar(el.AdicionarEnvolvido, "Clicar no botão Adicionar Envolvido");
                await metodo.ClicarNoSeletor(el.SelectRelacionadoA, "idsf", "Selecionar Relacionado A");
                await page.Locator("#envolvido").SelectOptionAsync(new SelectOptionValue { Index = 1 });
                await metodo.ClicarNoSeletor(el.SelectTipoRelacao, "cedente", "Selecionar Tipo de relação");
                await metodo.ClicarNoSeletor(el.SelectFormaEnvio, "email", "Selecionar Forma de envio");
                await metodo.ClicarNoSeletor(el.SelectFormaValidacao, "biometriaFacial", "Selecionar Forma de Validação");
                await metodo.Clicar(el.BtnConfirmarAddEnvolvido, "Clicar no botão Confirmar Adição de Envolvido");
                //sessão Operação
                await metodo.Clicar(el.SessaoOperacaoes, "Clicar na sessão Operações");
                await metodo.Escrever(el.CampoValorSolicitado, data.ValorSolicitado, "Preencher Valor Solicitado na sessão operações");
                await metodo.Escrever(el.CampoTaxaJuros, data.TaxaJurosNegativa, "Preencher Taxa de Juros na sessão operações");
                await metodo.Escrever(el.CampoDuração, data.DuracaoNegativa, "Preencher Duração sessão operações");
                await metodo.Escrever(el.CampoCarenciaAmortizacao, data.CarenciaAmortizacaoCenario2, "Preencher Carência de amortização sessão operações");
                await metodo.Escrever(el.CarenciaJuros, data.CarenciaJuros, "Inserir Valor de Carencia Juros");
                await metodo.ClicarNoSeletor(el.SelectTipoCalculo, "bruto", "Selecionar Tipo de calculo na sessão operações");
                await metodo.Escrever(el.CampoDiaVencimento, data.DiaVencimento, "Preencher Dia de Vencimento na sessão operações");
                await metodo.Clicar(el.CampoDataInicio, "Clicar no campo Data de Início");
                await metodo.Escrever(el.CampoDataInicio, dataAtual, "Inserir Data Atual no seletor Data de inicio");
                await metodo.ClicarNoSeletor(el.SelectIndexPosFix, "CDI", "Selecionar Indexador Pós-Fixado na sessão Operações");
                await metodo.Clicar(el.BtnSalvarMudancas, "Clicar no botão Salvar Mudanças");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, "A carência de juros não pode ser maior que a carência do principal!", "Validar mensagem retornada sobre campos em branco");
                await metodo.ValidarMensagemPorTextoAsync(el.MsgErro, "Parcelas devem ser geradas corretamente", "Validar mensagem retornada sobre campos em branco");

            }
        }


        public async Task ConsultarNotaComercialNaTabela()
        {
            await metodo.Clicar(el.BarraPesquisaTabela, "Clicar na tabela para inserir nota a ser consultada");
            await metodo.Escrever(el.BarraPesquisaTabela, data.TextoPesquisaFundo, "Escrever Fundo nome do fundo na barra de pesquisa da tabela ");
            await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaNotasComerciais, "Zitec Tecnologia LTDA", "Validar nome do fundo presenta na tablea");
        }

        public async Task CancelarNotaComercialNaTabela()
        {
            await metodo.Clicar(el.BarraPesquisaTabela, "Clicar na tabela para inserir nota a ser consultada");
            await metodo.Escrever(el.BarraPesquisaTabela, data.TextoPesquisaFundo, "Escrever Fundo nome do fundo na barra de pesquisa da tabela ");
            await metodo.Clicar(el.PrimeiroTD, "Clicar no primeiro TD para Expandir menu na tabela");
            await metodo.Clicar(el.BtnEngrenagem, "Clicar no na engrenagem para abrir modal de cancelamento");
            await metodo.Clicar(el.RadioBtnCancelarOp, "Marcar Radio Button para Cancelar Operação");
            await metodo.Clicar(el.MinutaPdf, "Marcar Opção minuta PDF");
            await metodo.Clicar(el.ObsFecharOp, "Clicar no campo Observação");
            await metodo.Escrever(el.ObsFecharOp, data.ObservacaoCancelamento, "Inserir Observação");
            await metodo.Clicar(el.BtnSubmitFecharOp, "Clicar no ultimo botão Submit");
            await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar Msg Sucesso presente na tela");

        }

        public async Task DownloadMinuta()
        {
            await metodo.Clicar(el.BarraPesquisaTabela, "Clicar na tabela para inserir nota a ser consultada");
            await metodo.Escrever(el.BarraPesquisaTabela, data.TextoPesquisaFundo, "Escrever Fundo nome do fundo na barra de pesquisa da tabela ");
            await metodo.Clicar(el.PrimeiroTD, "Clicar no primeiro TD para Expandir menu na tabela");
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadMinuta, "Validar Download da Minuta");
            //var download1 = await page.RunAndWaitForDownloadAsync(async () =>
            //{
            //    await metodo.Clicar(el.BtnDownloadMinutaPDF, "Clicar no botão para baixar Excel");
            //});
            //await metodo.ValidarDownloadAsync(download,"Download Minuta PDF","Validar Download da Minuta PDF");

        }


    }
}
