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
    public class ArquivosBaixaPage
    {
        private readonly IPage page;
        Metodos metodo;
        ArquivosBaixaElements el = new ArquivosBaixaElements();
        string caminhoArquivo = @"C:\TempQA\Arquivos\template.txt";
        string caminhoArquivoNegativo = @"C:\TempQA\Arquivos\";

        public ArquivosBaixaPage(IPage page) 
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task ValidarAcentosArquivosBaixaPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de ArquivosBaixa");
        }

        public async Task EnviarArquivoBaixa()
        {
            
            await metodo.Clicar(el.ImportarBaixaBtn, "Clicar no Botão para importar Baixa");
            await metodo.ClicarNoSeletor(el.SelectFundoZitec, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            var arquivoAtualizado = await metodo.AtualizarDataArquivo(caminhoArquivo, "Atualizar Data Arquivo");
            await metodo.EnviarArquivo(el.EnviarBaixas, caminhoArquivo, "Enviar Arquivo Baixa");
            await metodo.ValidarMsgRetornada(el.MsgArquivoRecebido, "Validação mensagem arquivo recebido mas aguardando validação");
        }

        public async Task EnviarArquivoBaixaNegativo(string nomeArquivoBaixaNegativo, string validacao)
        {
            await metodo.Clicar(el.ImportarBaixaBtn, "Clicar no Botão para importar Baixa");
            await metodo.ClicarNoSeletor(el.SelectFundoZitec, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            //var arquivoAtualizado = await metodo.AtualizarDataArquivo(caminhoArquivoNegativo + nomeArquivoBaixaNegativo, "Atualizar Data Arquivo");
            var nomeNovoArquivo = await metodo.EnviarArquivoNomeAtualizado(el.EnviarBaixas, caminhoArquivoNegativo + nomeArquivoBaixaNegativo, "Enviar Arquivo Baixa Negativo");
            await metodo.Clicar(el.BtnFecharModal, "Clicar no Botão para fechar modal");            
            await metodo.EsperarTextoPresente("Arquivo processado com sucesso!", "Esperar mensagem aparecer para prosseguir o fluxo");
            await metodo.Clicar(el.BarraDePesquisa, "CLicar na barra de pesquisa");
            await metodo.Escrever(el.BarraDePesquisa, nomeNovoArquivo, "Clicar na barra de pesquisa");

            await metodo.ValidarTextoDoElemento(el.QtdTitulos,"0","Validar que a quantidade de Titulos é zerada");
            //await metodo.ValidarTextoDoElemento(el.QtdOcorrencias,"0","Validar que a quantidade de Ocorrências é zerada");
            
        }



        public async Task ConsultarArquivoBaixa()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.BarraDePesquisa, "Clicar na barra de pesquisa");
            await metodo.Escrever(el.BarraDePesquisa, "cnab", "Escrever na barra de pesquisa");
            await metodo.VerificarElementoPresenteNaTabela(page,el.TabelaBaixas, "cnab", "Verificar se arquivo Baixa esta presente na tabela.");
        }

        public async Task BaixarRelatorioDeTitulos()
        {
            await Task.Delay(1000);
            //await metodo.Clicar(el.barraDePesquisa,"Clicar na barra de pesquisa");
            //await metodo.Escrever(el.barraDePesquisa,"QA","Escrever na barra de pesquisa");
            await metodo.Clicar(el.PrimeiroTd,"Clicar no primeiro TD");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.Locator(el.BtnDownloadRelatorioTitulos).ClickAsync();
            });

            await metodo.ValidarDownloadAsync(download, "RelatorioDeTitulos", "Baixar Relatório Titulos");
        }
        public async Task BaixarRelatorioDeMovimentos()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.BarraDePesquisa, "Clicar na barra de pesquisa");
            await metodo.Escrever(el.BarraDePesquisa, "QA", "Escrever na barra de pesquisa");
            await metodo.Clicar(el.PrimeiroTd, "Clicar no primeiro TD");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.Locator(el.BtnDownloadRelatorioMovimentos).ClickAsync();
            });

            await metodo.ValidarDownloadAsync(download, "RelatorioDeMovimentos", "Baixar Relatorio De Movimentos");

        }
        public async Task GerarArquivoCnab()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.BarraDePesquisa, "Clicar na barra de pesquisa");
            await metodo.Escrever(el.BarraDePesquisa, ".rem", "Escrever na barra de pesquisa");
            await metodo.Clicar(el.PrimeiroTd, "Clicar no primeiro TD");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.Locator(el.BtnDownloadArquivoCNAB).ClickAsync();
            });

            await metodo.ValidarDownloadAsync(download, "RelatorioDeMovimentos", "Baixar Relatorio De Movimentos");

        }


    }
}
