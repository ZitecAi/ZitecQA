using Microsoft.Playwright;
using PortalIDSFTestes.elementos.cedentes;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.cedentes
{
    public class CedentesPage
    {
        private IPage page;
        Metodos metodo;
        CedentesElements el = new CedentesElements();
        string caminhoArquivo = @"C:\TempQA\Arquivos\";

        public CedentesPage(IPage page) 
        {
            this.page = page;
            metodo = new Metodos(page);
        }   


        public async Task ValidarAcentosCedentesPage()
        {
            await metodo.ValidarAcentosAsync(page, "Validar acentos na página de Cedentes");
        }

        public async Task DownloadExcel()
        {
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnDownloadExcel, "Clicar no botão para baixar Excel");
            });
            await metodo.ValidarDownloadAsync(download, "Download Arquivo Excel", "Validar Download de Excel na Página de cedentes");

        }


        public async Task CadastrarCedente()
        {
            await metodo.Clicar(el.BtnNovoCedente, "Clicar no botão para cadastrar novo Cedente.");
            await metodo.EnviarArquivo(el.InputNovoCedente, caminhoArquivo,"Enviar arquivo no input para cadastrar novo cedente");
            await metodo.ValidarMsgRetornada(el.MsgAcaoSucesso,"Validar mensagem Ação realizada com sucesso presente na tela");
        }


    }
}
