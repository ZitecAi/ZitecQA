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

            Random random = new Random();

            int randomNumber = random.Next(0, 9999);

            var today = DateTime.Today.ToString();
            string cpfCotista = "496.248.668-30";
            string nomeCotista = $"Cotista Zitec {randomNumber}";
            string nomeFundo = "Zitec Tecnologia LTDA";
            string cnpjFundo = "54.638.076/0001-76";
            string valorAporte = "10000";

            await metodo.Clicar(el.BtnNovo, "Clicar em Novo, para inserir novo aporte");
            await Task.Delay(200);
            await metodo.Escrever(el.Calendario, today, "Clicar no calendario para inserir dia do aporte");
            await metodo.Clicar(el.ValorAporte, "Clicar em valor do aporte");
            await metodo.Escrever(el.ValorAporte, valorAporte, "Inserir valor do aporte");
            await metodo.Escrever(el.CnpjCotista, cpfCotista, "inserir CNPJ cotista");
            await metodo.Escrever(el.NomeCotista, nomeCotista, "inserir Nome do cotista");
            await metodo.ClicarNoSeletor(el.SelectCota, "COTAFIXA", "Selecionar Cota Fixa");
            await metodo.Escrever(el.ValorCota, "1000", "valor da cota");
            await metodo.ClicarNoSeletor(el.TipoAporte, "FINANCEIRO", "Selecionar aporte Financeiro");
            await Task.Delay(1000);
            bool isVisible = false;

            while (isVisible == false)
            {
                await metodo.ClicarNoSeletor(el.SelectFundo, "54638076000176", "Selecionar fundo zitec tecnologia");
                if (await page.GetByText("CARTEIRA TESTE").IsEnabledAsync())
                {
                    isVisible = true;
                    break;
                }
            }
            //await metodo.Clicar(el.SelectFundo, "Expandir seletor do fundo");           
                      
            //await page.Keyboard.PressAsync("Escape");
            await Task.Delay(1000);
            await metodo.Clicar(el.BtnEnviar, "Clicar em enviar");
            await metodo.ValidarTextoPresente("Aporte realizado com sucesso", "Validar mensagem de sucesso retornada");
            await metodo.Escrever(el.Filtro, nomeCotista, "Pesquisar Nome do Cotista no filtro");
            await Task.Delay(3000);
            await metodo.Clicar(el.AprovacaoCustodia(nomeCotista), "Aprovar aporte na custódia");
            await metodo.Clicar(el.BtnAprovado, "Selecionar status aprovado");  
            await metodo.Escrever(el.Descricao, "Aprovado", "Inserir descrição da aprovação");
            await metodo.Clicar(el.BtnConfirmar, "Confirmar aprovação");
            await metodo.ValidarTextoPresente("Status atualizado com sucesso", "Validar mensagem de sucesso na aprovação da custódia");
            await Task.Delay(700);
            await metodo.Clicar(el.AprovacaoEscrituracao(nomeCotista), "Aprovar aporte na escrituracao");
            await metodo.Clicar(el.BtnAprovado, "Selecionar status aprovado");
            await metodo.Escrever(el.Descricao, "Aprovado", "Inserir descrição da aprovação");
            await metodo.Clicar(el.BtnConfirmar, "Confirmar aprovação");
            await metodo.ValidarTextoPresente("Status atualizado com sucesso", "Validar mensagem de sucesso na aprovação da escrituracao");
            await Task.Delay(700);
            await metodo.Clicar(el.AprovacaoControladoria(nomeCotista), "Aprovar aporte na controladoria");
            await metodo.Clicar(el.BtnAprovado, "Selecionar status aprovado");
            await metodo.Escrever(el.Descricao, "Aprovado", "Inserir descrição da aprovação");
            await metodo.Clicar(el.BtnConfirmar, "Confirmar aprovação");
            await metodo.ValidarTextoPresente("Status atualizado com sucesso", "Validar mensagem de sucesso na aprovação da controladoria");
            await metodo.ValidarTextoDoElemento(el.StatusBoletado, "Boletado!", "Validar que o status do aporte está como BOLETADO");

            await metodo.ValidarTextoDoElemento(el.PosicaoFundoNaTabela, nomeFundo + " - " + "CNPJ: "+ cnpjFundo, "Validar que o fundo está correto na tabela");
            await metodo.ValidarTextoDoElemento(el.PosicaoCotistaNaTabela, nomeCotista + " - " + cpfCotista, "Validar que o cotista está correto na tabela");


        }
    }
}
