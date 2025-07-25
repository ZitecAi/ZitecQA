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
            await metodo.Clicar(el.menuOperaoes, "Clicar em operações menu hamburguer");
            await metodo.Clicar(el.paginaBaixas, "Clicar arquivos baixas 2.0 para acesasr a pagina");
            await metodo.Clicar(el.importarBaixaBtn, "Clicar no Botão para importar Baixa");
            await metodo.ClicarNoSeletorFundo(el.selectFundoZitec, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            //await metodo.AtualizarEEnviarArquivo(caminhoArquivo, el.enviarBaixas, "Enviar arquivo Baixa com data atualizado");
            //criar metodo genérico que consome o arquivo depois de atualizar a data e envia no setinputfilesasync
        }
        

    }
}
