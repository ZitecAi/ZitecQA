using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using PortalIDSFTestes.elementos.operacoes;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.data.operacoes;

namespace PortalIDSFTestes.pages.operacoes
{
    public class ArquivosBaixaPage
    {
        private readonly IPage page;
        Utils metodo;
        ArquivosBaixaElements el = new ArquivosBaixaElements();
        ArquivosBaixaData data = new ArquivosBaixaData();

        public ArquivosBaixaPage(IPage page)
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


        public async Task ValidarAcentosArquivosBaixaPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de ArquivosBaixa");
        }

        public async Task EnviarArquivoBaixa()
        {

            await metodo.Clicar(el.ImportarBaixaBtn, "Clicar no Botão para importar Baixa");
            await metodo.ClicarNoSeletor(el.SelectFundoZitec, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            var arquivoAtualizado = await metodo.AtualizarDataArquivo(GetPath() + "template.txt", "Atualizar Data Arquivo");
            await metodo.EnviarArquivo(el.EnviarBaixas, GetPath() + "template.txt", "Enviar Arquivo Baixa");
            await metodo.ValidarMsgRetornada(el.MsgArquivoRecebido, "Validação mensagem arquivo recebido mas aguardando validação");
        }

        public async Task EnviarArquivoBaixaNegativo(string nomeArquivoBaixaNegativo, string validacao)
        {
            await metodo.Clicar(el.ImportarBaixaBtn, "Clicar no Botão para importar Baixa");
            await metodo.ClicarNoSeletor(el.SelectFundoZitec, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            //var arquivoAtualizado = await metodo.AtualizarDataArquivo(caminhoArquivoNegativo + nomeArquivoBaixaNegativo, "Atualizar Data Arquivo");
            var nomeNovoArquivo = await metodo.EnviarArquivoNomeAtualizado(el.EnviarBaixas, GetPath() + nomeArquivoBaixaNegativo, "Enviar Arquivo Baixa Negativo");
            await metodo.Clicar(el.BtnFecharModal, "Clicar no Botão para fechar modal");
            await metodo.EsperarTextoPresente("Arquivo processado com sucesso!", "Esperar mensagem aparecer para prosseguir o fluxo");
            await metodo.Clicar(el.BarraDePesquisa, "CLicar na barra de pesquisa");
            await metodo.Escrever(el.BarraDePesquisa, nomeNovoArquivo, "Clicar na barra de pesquisa");

            await metodo.ValidarTextoDoElemento(el.QtdTitulos, "0", "Validar que a quantidade de Titulos é zerada");
            //await metodo.ValidarTextoDoElemento(el.QtdOcorrencias,"0","Validar que a quantidade de Ocorrências é zerada");

        }


        public async Task BaixarRelatorioDeTitulos()
        {
            await Task.Delay(1000);
            //await metodo.Clicar(el.barraDePesquisa,"Clicar na barra de pesquisa");
            //await metodo.Escrever(el.barraDePesquisa,"QA","Escrever na barra de pesquisa");
            await metodo.Clicar(el.PrimeiroTd, "Clicar no primeiro TD");
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadRelatorioTitulos, "Validar Download do Relatório de Titulos na página de Arquivos de Baixa");
        }
        public async Task BaixarRelatorioDeMovimentos()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.BarraDePesquisa, "Clicar na barra de pesquisa");
            await metodo.Escrever(el.BarraDePesquisa, data.TextoPesquisaQA, "Escrever na barra de pesquisa");
            await metodo.Clicar(el.PrimeiroTd, "Clicar no primeiro TD");
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadRelatorioMovimentos, "Validar Download do Relatório de Movimentos na página de Arquivos de Baixa");
        }
        public async Task GerarArquivoCnab()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.BarraDePesquisa, "Clicar na barra de pesquisa");
            await metodo.Escrever(el.BarraDePesquisa, data.ExtensaoArquivoREM, "Escrever na barra de pesquisa");
            await metodo.Clicar(el.PrimeiroTd, "Clicar no primeiro TD");
            await metodo.ValidateDownloadAndLength(page, el.BtnDownloadArquivoCNAB, "Validar Download do Arquivo CNAB na página de Arquivos de Baixa");

        }


    }
}
