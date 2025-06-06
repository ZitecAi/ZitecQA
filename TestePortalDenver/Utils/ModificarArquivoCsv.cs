using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortalDenver.Utils
{
    public class ModificarArquivoCsv
    {
        private static Random random = new Random();

        public static string ModificarCsv(string caminhoEntrada, string pastaSaida)
        {
            try
            {
                if (!File.Exists(caminhoEntrada))
                {
                    Console.WriteLine("Arquivo CSV não encontrado.");
                    return string.Empty;
                }

                var linhas = File.ReadAllLines(caminhoEntrada);
                if (linhas.Length < 2)
                {
                    Console.WriteLine("Arquivo CSV não contém dados suficientes.");
                    return string.Empty;
                }

                // Gera valores aleatórios para os campos
                string novoNumero = GerarNumeroAleatorio();
                string novoDocumento = GerarNumeroAleatorio();

                // Substitui os placeholders pelos valores gerados
                linhas[1] = linhas[1].Replace("#nudocumento#", novoNumero);
                linhas[1] = linhas[1].Replace("#seunumero#", novoDocumento);

                var colunas = linhas[1].Split(';');

                Console.WriteLine($"Linha original: {linhas[1]}");
                Console.WriteLine($"Colunas encontradas: {colunas.Length}");

                if (colunas.Length < 14)
                {
                    Console.WriteLine("A linha não tem colunas suficientes.");
                    return string.Empty;
                }

                // Gera um nome único para o arquivo
                string nomeUnico = $"arquivo_modificado_{random.Next(1000, 9999)}.csv";
                string caminhoCompleto = Path.Combine(pastaSaida, nomeUnico);

                // Salva o arquivo modificado
                File.WriteAllLines(caminhoCompleto, linhas);

                return nomeUnico; // Retorna apenas o nome do arquivo gerado
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }

        private static string GerarNumeroAleatorio()
        {
            try
            {
                int parte1 = random.Next(5000000, 5999999);
                int parte2 = random.Next(10, 99);
                int parte3 = random.Next(2020, 2025);
                int parte4 = random.Next(1, 30);
                int parte5 = random.Next(1000, 9999);

                return $"{parte1}-{parte2}.{parte3}.{parte4}.{parte5}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
