using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.operacoes
{
    public class AtivosPage
    {
        private IPage page;
        Metodos metodo;
        AtivosElements el = new AtivosElements();
        public AtivosPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        
        public static string GetPath()
        {
            ConfigurationManager config = new ConfigurationManager();
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            string path = config["Paths:Arquivo"].ToString();
            return path;
        }

        public async Task ValidarAcentosAtivosPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Ativos");
        }

        public async Task DownloadExcel()
        {
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnExcel, "Clicar no botão para baixar Excel");
            });
            await metodo.ValidarDownloadAsync(download, "Download Validação Layout", "Validar Download de Excel");

        }

        public async Task CadastrarEConsultarAtivo()
        {
            Random random = new Random();

            int uniqueNumber = random.Next(0, 9999);

            try
            {
                await metodo.Clicar(el.BtnNovoAtivo, "Clicar no Botão Para Cadastrar Um Novo Ativo");
                await Task.Delay(500);
                await metodo.ClicarNoSeletor(el.SelectFundo, "53300608000106", "Selecionar Fundo");
                await metodo.Escrever(el.CampoNomeCedente, "Cedente Teste NUnit", "Escrever Nome Cedente");
                await metodo.Escrever(el.CampoCpfPjCedente, "533.006.080-00106", "Escrever CPF/CNPJ Cedente");
                await metodo.ClicarNoSeletor(el.SelectTipoContrato, "AtivosImobiliarios", "Selecionar Tipo de Contrato");
                await metodo.Escrever(el.CampoAgencia, "0001", "Digitar Agencia");
                await metodo.Escrever(el.CampoConta, "460915", "Digitar Conta");
                await metodo.Escrever(el.CampoRazSocialDestinatario, $"Teste NUnit {uniqueNumber}", "Digitar Razão Social");
                await metodo.Escrever(el.CampoCpfPj, "49624866830", "Digitar CPF/CNPJ Ativo");
                await metodo.Escrever(el.CampoValor, "10", "Digitar Valor");
                await metodo.Escrever(el.CampoResumoOperacao, "Teste NUnit", "Digitar Resumo da operação");
                await metodo.Clicar(el.BtnAnexos, "Clicar no Botão Anexos");
                await metodo.EnviarArquivo(el.InputAnexos, GetPath() + "21321321321.pdf", "Enviar Anexo no Input de anexos");
                await metodo.Clicar(el.BtnVoltar, "Clicar no botão voltar após enviar anexo");
                await metodo.Clicar(el.CheckBoxTermo, "Aceitar Termos");
                await metodo.Clicar(el.BtnSalvar, "Clicar em Salvar");
                await metodo.ValidarMsgRetornada(el.MsgSucessoRetornada, "Validar mensagem de sucesso visivel na tela ");
                await Task.Delay(1000);
                await page.ReloadAsync();
            }catch(Exception ex)
            {
                throw new Exception("Não foi possivel cadastrar Ativo" + ex.Message);
            }
            try
            {
                //consultar ativo cadastrado
                await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
                await metodo.Escrever(el.BarraPesquisa, $"Teste NUnit {uniqueNumber}", "Clicar na Barra de Pesquisa");
                await metodo.VerificarElementoPresenteNaTabela(page, el.TabelaAtivos, $"Teste NUnit {uniqueNumber}", "Validar se ativo com destinatário Teste NUnit está presente na tabela");

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel consultar Ativo" + ex.Message);
            }

            try
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmAnalise("1"), "Clicar no botão para abrir modal de situação do gestor");
                await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo gestor");
                await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
                await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo gestor");
            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possivel Aprovar gestora do Ativo" + ex.Message);
            }
            try
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmAnalise("3"), "Clicar no botão para abrir modal de situação do risco");
                await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo risco");
                await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
                await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo risco");
            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possivel Aprovar risco do Ativo" + ex.Message);
            }
            try
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmAnalise("4"), "Clicar no botão para abrir modal de situação do jurídico");
                await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo jurídico");
                await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
                await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo jurídico");
            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possivel Aprovar jurídico do Ativo" + ex.Message);
            }
            try
            {
                await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");
                await metodo.Clicar(el.BtnEmAnalise("5"), "Clicar no botão para abrir modal de situação do cadastro");
                await metodo.Clicar(el.BtnAprovado, "Clicar no botão para aprovar pelo cadastro");
                await metodo.Escrever(el.CampoObservacaoParecer, "Teste Aprovação", "Digitar Observação");
                await metodo.Clicar(el.BtnAprovadoGestora, "Clicar no Submit para aprovar pelo cadastro");
            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possivel Aprovar cadastro do Ativo" + ex.Message);
            }

        }



        public async Task DownloadArquivo()
        {
            await metodo.Clicar(el.BarraPesquisa, "Clicar na Barra de Pesquisa");
            await metodo.Escrever(el.BarraPesquisa, "Teste NUnit", "Clicar na Barra de Pesquisa");
            await Task.Delay(1000);
            await metodo.Clicar(el.PrimeiroTd, "Clicar ´no primeiro TD para expandir dados");            
            await metodo.ValidateDownloadAndLength(page, el.BtnBaixarArquivo, "Baixar Arquivo");
        }


        public async Task ContagemDeAtivosTotais()
        {
            await metodo.CapturarTextoDoElemento(el.TotalAtivos, "capturar e validar o total de ativos na página");
        }

    }
}
