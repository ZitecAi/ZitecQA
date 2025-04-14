using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AtualizarCnab
{
    public static async Task<(string NomeArquivoTxt, string CaminhoArquivoTxt)> AtualizarDataEConverterParaBase64(string caminhoArquivo)
    {
        try
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo não encontrado.", caminhoArquivo);

            string[] linhas = await File.ReadAllLinesAsync(caminhoArquivo);

            if (linhas.Length == 0)
                throw new InvalidOperationException("O arquivo está vazio.");

            // Atualiza a data na primeira linha
            string dataAtual = DateTime.Now.ToString("ddMMyy");
            linhas[0] = linhas[0].Replace("#DATA#", dataAtual);

            // Gera números aleatórios para substituir os placeholders
            Random random = new Random();
            for (int i = 1; i < Math.Min(linhas.Length, 7); i++)
            {
                string randomNumber = string.Concat(Enumerable.Range(0, 25).Select(_ => random.Next(0, 10)));
                linhas[i] = linhas[i].Replace("#DOC_NUMERO_CONSULTORIA_#", randomNumber);

                string randomDocNumber = string.Concat(Enumerable.Range(0, 10).Select(_ => random.Next(0, 10)));
                linhas[i] = linhas[i].Replace("#NUM_DOCU#", randomDocNumber);
            }

            // Criar diretório temporário seguro
            string pastaTemporaria = Path.GetTempPath();
            string dataFormatada = DateTime.Now.ToString("yyyyMMdd");
            string uniqueIdentifier = Guid.NewGuid().ToString().Split('-')[0];

            // Criar um nome de arquivo simples sem caracteres especiais
            string nomeArquivoTxt = $"FundoQA_{dataFormatada}_{uniqueIdentifier}.txt";
            nomeArquivoTxt = RemoverCaracteresEspeciais(nomeArquivoTxt); // Remover caracteres especiais
            string caminhoArquivoTxt = Path.Combine(pastaTemporaria, nomeArquivoTxt);

            // Salvar o novo arquivo
            await File.WriteAllLinesAsync(caminhoArquivoTxt, linhas);

            return (nomeArquivoTxt, caminhoArquivoTxt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            throw;
        }
    }

    // Função para remover caracteres especiais
    private static string RemoverCaracteresEspeciais(string texto)
    {
        int lastDotIndex = texto.LastIndexOf('.');
        if (lastDotIndex > 0)
        {
            string nomeSemExtensao = texto.Substring(0, lastDotIndex);
            string extensao = texto.Substring(lastDotIndex); // Mantém a extensão
            return string.Concat(nomeSemExtensao.Where(c => char.IsLetterOrDigit(c) || c == '_')) + extensao;
        }
        return string.Concat(texto.Where(c => char.IsLetterOrDigit(c) || c == '_'));
    }

}
