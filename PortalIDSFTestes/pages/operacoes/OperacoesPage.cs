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
    public class OperacoesPage
    {
        private IPage page;
        Metodos metodo;
        OperacoesElements el = new OperacoesElements();

        string caminhoArquivo = @"C:\TempQA\Arquivos\CNABz - Copia.txt";



        public OperacoesPage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }

        public async Task validarAcentosOperacoesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Pagina de Operações");
        }

        public async Task enviarOperacaoCNAB()
        {
            await metodo.Clicar(el.btnNovaOperacaoCNAB,"Clicar no botão para enviar uma Nova Operação CNAB");
            await metodo.ClicarNoSeletorFundo(el.selectFundo, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            await metodo.AtualizarDataEEnviarArquivo(page, caminhoArquivo);
            await metodo.ValidarMsgRetornada(el.msgSucessoRetornada, "Validar Mensagem de Sucesso retornada");

        }
    }
}
