using Microsoft.Playwright;
using PortalIDSFTestes.elementos.Boletagem;
using PortalIDSFTestes.elementos.cadastro;
using PortalIDSFTestes.metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIDSFTestes.pages.boletagem
{
    public class AportePage
    {
        private readonly IPage page;
        Metodos metodo;
        AporteElements el = new AporteElements();

        public AportePage(IPage page)
        {
            this.page = page;
            metodo = new Metodos(page);
        }


        public async Task ValidarAcentosAporte()
        {
            await metodo.ValidarAcentosAsync(page, "Validar Acentos na Página Aporte");
        }

        public async Task RealizarAporte()
        {
            var today = DateTime.Today.ToString();

            await metodo.Clicar(el.BtnNovo, "Clicar em Novo, para inserir novo aporte");
            await metodo.Escrever(el.Calendario, today, "Clicar no calendario para inserir dia do aporte");
            await metodo.Clicar(el.ValorAporte, "Clicar em valor do aporte");
            await metodo.Escrever(el.ValorAporte, "10000", "Inserir valor do aporte");
            await metodo.Escrever(el.CnpjCotista, "496.248.668-30", "inserir CNPJ cotista");
            await metodo.Escrever(el.NomeCotista, "Cotista Zitec", "inserir Nome do cotista");
            await metodo.ClicarNoSeletor(el.SelectCota, "COTAFIXA", "Selecionar Cota Fixa");
            await metodo.Escrever(el.ValorCota, "1000", "valor da cota");
            await metodo.ClicarNoSeletor(el.TipoAporte, "FINANCEIRO", "Selecionar aporte Financeiro");
            await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar fundo zitec tecnologia");
            //await metodo.ClicarNoSeletor(el.SelectCarteira, "CARTEIRA TESTE", "Selecionar CARTEIRA TESTE");
            await metodo.Clicar(el.BtnEnviar, "Clicar em enviar");
            await metodo.ValidarTextoPresente("Aporte realizado com sucesso", "Validar mensagem de sucesso retornada");

        }
    }
}
