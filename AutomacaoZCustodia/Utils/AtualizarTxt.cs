using Azure;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomacaoZCustodia.Utils
{
    public class AtualizarTxt
    {

        public static async Task<string> AtualizarDataEEnviarArquivo(IPage page, string caminhoArquivo)
        { 
            var linhas = File.ReadAllLines(caminhoArquivo);

            // Atualizando a data
            string dataAtual = DateTime.Now.ToString("ddMMyy");
            linhas[0] = linhas[0].Substring(0, 94) + dataAtual + linhas[0].Substring(100);

            // Atualizando número de consultoria e número do documento
            Random random = new Random();
            for (int i = 1; i <= 7; i++)
            {
                string randomNumber = new string(Enumerable.Range(0, 20).Select(_ => random.Next(0, 10).ToString()[0]).ToArray()) + "TESTE";
                linhas[i] = linhas[i].Replace("#DOC_NUMERO_CONSULTORIA_#", randomNumber);

                string randomNumberNumDoc = new string(Enumerable.Range(0, 10).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());
                linhas[i] = linhas[i].Replace("#NUM_DOCU#", randomNumberNumDoc);
            }

            // Gerando novo nome de arquivo único
            string dataFormatada = DateTime.Now.ToString("yyyyMMdd");
            string uniqueIdentifier = Guid.NewGuid().ToString().Split('-')[0];
            string novoNomeArquivo = $"FundoQA_{dataFormatada}_{uniqueIdentifier}.txt";

          
            string novoCaminhoArquivo = Path.Combine(Path.GetDirectoryName(caminhoArquivo), novoNomeArquivo);

            File.WriteAllLines(novoCaminhoArquivo, linhas);

            await DragAndDropArquivo(page, novoCaminhoArquivo);

            return novoNomeArquivo;
        }

        public static async Task DragAndDropArquivo(IPage page, string caminhoArquivo)
        {
            var dropZoneSelector = "div.drop-zone[dropzone]"; 

            byte[] fileBuffer = await File.ReadAllBytesAsync(caminhoArquivo);
            string base64File = Convert.ToBase64String(fileBuffer);
            string fileName = Path.GetFileName(caminhoArquivo);

            await page.EvaluateAsync(@"({ selector, fileName, base64Data }) => {
        const dropZone = document.querySelector(selector);
        if (!dropZone) {
            console.error('Área de drop não encontrada');
            return;
        }

        const dataTransfer = new DataTransfer();

        // Criando um Blob a partir dos dados Base64
        try {
            const byteCharacters = Uint8Array.from(atob(base64Data), c => c.charCodeAt(0));
            const fileBlob = new Blob([byteCharacters], { type: 'text/plain' });

            // Criando um objeto File
            const file = new File([fileBlob], fileName, { type: 'text/plain' });
            dataTransfer.items.add(file);

            // Simulando os eventos de drag-and-drop
            ['dragenter', 'dragover', 'drop'].forEach(eventType => {
                const event = new DragEvent(eventType, { bubbles: true, cancelable: true });
                Object.defineProperty(event, 'dataTransfer', { value: dataTransfer });
                dropZone.dispatchEvent(event);
            });

            console.log('Arquivo arrastado e solto com sucesso!');
        } catch (error) {
            console.error('Erro ao converter Base64:', error);
        }
    }", new { selector = dropZoneSelector, fileName, base64Data = base64File });

            Console.WriteLine("Arquivo arrastado e solto com sucesso!");
        }
    }
}