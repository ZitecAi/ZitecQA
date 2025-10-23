using NUnit.Framework;
using Allure.Net.Commons;
using System;
using System.IO;

[assembly: LevelOfParallelism(4)]
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace PortalIDSFTestes
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public void GlobalSetupMethod()
        {
            // Configurar diretório de resultados do Allure
            var allureDir = Environment.GetEnvironmentVariable("ALLURE_RESULTS_DIRECTORY");

            if (string.IsNullOrEmpty(allureDir))
            {
                // Se não definido via variável de ambiente, usar o padrão
                allureDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "allure-results");
            }

            Console.WriteLine($"Allure results directory: {allureDir}");

            // Criar diretório se não existir
            if (!Directory.Exists(allureDir))
            {
                Directory.CreateDirectory(allureDir);
                Console.WriteLine($"Created Allure directory: {allureDir}");
            }

            // Limpar resultados anteriores
            foreach (var file in Directory.GetFiles(allureDir))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Could not delete {file}: {ex.Message}");
                }
            }

            Console.WriteLine("Allure reporting initialized");
        }

        [OneTimeTearDown]
        public void GlobalTearDownMethod()
        {
            var allureDir = Environment.GetEnvironmentVariable("ALLURE_RESULTS_DIRECTORY")
                ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "allure-results");

            if (Directory.Exists(allureDir))
            {
                var fileCount = Directory.GetFiles(allureDir, "*.json").Length;
                Console.WriteLine($"Allure report generated: {fileCount} JSON files in {allureDir}");
            }
        }
    }
}
