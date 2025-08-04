using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalIDSFTestes.metodos;
using PortalIDSFTestes.elementos.notaComercial;

namespace PortalIDSFTestes.pages.notaComercial
{
    public class NotaComercialPage
    {
        private IPage page;
        Metodos metodo;
        NotaComercialElements el = new NotaComercialElements();
        public NotaComercialPage(IPage page) 
        {
            this.page = page;
            metodo = new Metodos(page);
        }
        
        public async Task ValidarAcentosNotaComercialPage()
        {
            await metodo.ValidarAcentosAsync(page,"Validar Acentos na Página de Nota Comercial");
        }
        public async Task DownloadExcel()
        {
            var download = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await metodo.Clicar(el.BtnExcel, "Clicar no botão para baixar Excel");
            });
            await metodo.ValidarDownloadAsync(download, "Download Arquivo Excel", "Validar Download de Excel na Página de Nota Comercial");
        }

        public async Task CadastrarNotaComercial()
        {
            await metodo.Clicar(el.BtnNovoNotaComercial, "Clicar no Botão para inserir nova nota comercial");
            await metodo.ClicarNoSeletor(el.SelectFundoCessionario, "54638076000176", "Selecionar Fundo Zitec Tecnologia LTDA");
            await metodo.ClicarNoSeletor(el.SelectProduto, "125", "Selecionar Produto Teste");
            await metodo.ClicarNoSeletor(el.SelectTipoLiquidacao, "PIX", "Selecionar tipo liquidação PIX");
            await metodo.Escrever(el.CampoTomador, "CEDENTE TESTE", "Selecionar CEDENTE TESTE no campo tomador");
            await metodo.Clicar(el.BtnLupaTomador,"Clicar na lupa de pesquisa para pesquisar Tomador");

            
        }




    }
}
