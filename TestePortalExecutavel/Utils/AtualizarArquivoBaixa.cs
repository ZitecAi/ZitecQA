using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortalExecutavel.Utils
{
    public class AtualizarArquivoBaixa
    {
        public static string AtualizarDataArquivo(string caminhoTemplate)
        {
            if (!File.Exists(caminhoTemplate))
                throw new FileNotFoundException("Arquivo de template não encontrado.", caminhoTemplate);

            var linhas = File.ReadAllLines(caminhoTemplate);

            // Substitui o marcador #DATA# pela data atual no formato ddMMyy
            string dataAtual = DateTime.Now.ToString("ddMMyy");
            for (int i = 0; i < linhas.Length; i++)
            {
                if (linhas[i].Contains("#DATA#"))
                {
                    linhas[i] = linhas[i].Replace("#DATA#", dataAtual);
                }
            }

            // Gera um novo nome de arquivo com base na data atual e um identificador único
            string dataArquivo = DateTime.Now.ToString("yyyyMMdd");
            string idUnico = Guid.NewGuid().ToString().Split('-')[0];
            string novoNomeArquivo = $"FundoQA_{dataArquivo}_{idUnico}.txt";

            // Monta o novo caminho usando o mesmo diretório do template
            string novoCaminho = Path.Combine(Path.GetDirectoryName(caminhoTemplate), novoNomeArquivo);

            // Escreve o novo arquivo
            File.WriteAllLines(novoCaminho, linhas);

            Console.WriteLine($"Arquivo atualizado salvo como: {novoCaminho}");

            // Agora retorna apenas o nome do arquivo
            return novoNomeArquivo;
        }

    }

}
