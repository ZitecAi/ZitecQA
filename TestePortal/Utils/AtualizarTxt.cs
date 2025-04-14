using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.Windows.Controls;
using System.IO.Compression;


namespace TestePortal.Utils
{
    public class AtualizarTxt
    {
        public static async Task<string> AtualizarDataEEnviarArquivo(IPage page, string caminhoArquivo)
        {
            var linhas = File.ReadAllLines(caminhoArquivo);

            // Atualizando data
            string dataAtual = DateTime.Now.ToString("ddMMyy");
            string DataArquivoTemplate = linhas[0].Substring(94, 6);
            string AnteriorData = linhas[0].Substring(0, 94);
            string PosData = linhas[0].Substring(101);
            linhas[0] = linhas[0].Replace("#DATA#", dataAtual);

            // Atualizando num consultoria
            Random random = new Random();
            for (int i = 1; i <= 7; i++)
            {
                string randomNumber = "";
                for (int j = 0; j < 25; j++)
                {
                    randomNumber += random.Next(0, 10).ToString();
                }

                linhas[i] = linhas[i].Replace("#DOC_NUMERO_CONSULTORIA_#", randomNumber);

                string randomNumberNumDoc = "";
                for (int j = 0; j < 10; j++)
                {
                    randomNumberNumDoc += random.Next(0, 10).ToString();
                }

                linhas[i] = linhas[i].Replace("#NUM_DOCU#", randomNumberNumDoc);
            }

            string dataFormatada = DateTime.Now.ToString("yyyyMMdd");

            // Usar GUID para garantir que o nome do arquivo seja único
            string uniqueIdentifier = Guid.NewGuid().ToString().Split('-')[0]; // Pega apenas a primeira parte do GUID
            string novoNomeArquivo = $"FundoQA_{dataFormatada}_{uniqueIdentifier}.txt";
            string novoCaminhoArquivo = Path.Combine(Path.GetDirectoryName(caminhoArquivo), novoNomeArquivo);


            File.WriteAllLines(novoCaminhoArquivo, linhas);

            await page.Locator("#fileEnviarOperacoes").SetInputFilesAsync(new[] { novoCaminhoArquivo });

            Console.WriteLine($"Arquivo {novoNomeArquivo} enviado com sucesso.");

            return novoNomeArquivo;
        }

        //para API
        public static async Task<(string NomeArquivoZip, string ArquivoBase64)> AtualizarDataEConverterParaBase64(string caminhoArquivo)
        {
            var linhas = File.ReadAllLines(caminhoArquivo);


            string dataAtual = DateTime.Now.ToString("ddMMyy");
            linhas[0] = linhas[0].Replace("#DATA#", dataAtual);

            Random random = new Random();
            for (int i = 1; i <= 7; i++)
            {
                string randomNumber = new string(Enumerable.Range(0, 25).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());
                linhas[i] = linhas[i].Replace("#DOC_NUMERO_CONSULTORIA_#", randomNumber);

                string randomDocNumber = new string(Enumerable.Range(0, 10).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());
                linhas[i] = linhas[i].Replace("#NUM_DOCU#", randomDocNumber);
            }

            string dataFormatada = DateTime.Now.ToString("yyyyMMdd");
            string uniqueIdentifier = Guid.NewGuid().ToString().Split('-')[0];
            string nomeArquivoTxt = $"FundoQA_{dataFormatada}_{uniqueIdentifier}.txt";
            string caminhoArquivoTxt = Path.Combine(Path.GetDirectoryName(caminhoArquivo), nomeArquivoTxt);
            File.WriteAllLines(caminhoArquivoTxt, linhas);

            // Criar o arquivo ZIP
            string nomeArquivoZip = $"FundoQA_{dataFormatada}_{uniqueIdentifier}.zip";
            string caminhoArquivoZip = Path.Combine(Path.GetDirectoryName(caminhoArquivo), nomeArquivoZip);

            using (var zip = ZipFile.Open(caminhoArquivoZip, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(caminhoArquivoTxt, nomeArquivoTxt);
            }

            byte[] fileBytes = File.ReadAllBytes(caminhoArquivoZip);
            string arquivoBase64 = Convert.ToBase64String(fileBytes);

            return (nomeArquivoZip, arquivoBase64);
        }
    }

}