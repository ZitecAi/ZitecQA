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

        public ArquivosBaixaPage(IPage page) 
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task validarAcentosArquivosBaixaPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de ArquivosBaixa");
        }

        public async Task enviarArquivoBaixa()
        {
            
            await metodo.Clicar(el.importarBaixaBtn, "Clicar no Botão para importar Baixa");
            await metodo.ClicarNoSeletorFundo(el.selectFundoZitec, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            //await metodo.AtualizarEEnviarArquivo(caminhoArquivo, el.enviarBaixas, "Enviar arquivo Baixa com data atualizado");
            //criar metodo genérico que consome o arquivo depois de atualizar a data e envia no setinputfilesasync
        }

        public async Task baixarRelatorioDeTitulos()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.primeiroTd,"Clicar no primeiro TD");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.Locator("(//button[@title='Relatório de Títulos'])[2]").ClickAsync();
            });

            await metodo.ValidarDownloadAsync(download, "RelatorioDeTitulos", "Baixar Relatório Titulos");
        }
        public async Task baixarRelatorioDeMovimentos()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.primeiroTd, "Clicar no primeiro TD");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.Locator("(//button[@title='Relatório de Movimentos'])[2]").ClickAsync();
            });

            await metodo.ValidarDownloadAsync(download, "RelatorioDeMovimentos", "Baixar Relatorio De Movimentos");

        }
        public async Task gerarArquivoCnab()
        {
            await Task.Delay(1000);
            await metodo.Clicar(el.primeiroTd, "Clicar no primeiro TD");
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.Locator("(//button[@title='Relatório de Movimentos'])[2]").ClickAsync();
            });

            await metodo.ValidarDownloadAsync(download, "RelatorioDeMovimentos", "Baixar Relatorio De Movimentos");

        }


    }
}
