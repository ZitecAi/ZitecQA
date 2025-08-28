using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestePortal.Model;
using System.Security.Cryptography;
using TestePortal.Pages;
using TestePortal.Utils;
using Microsoft.Playwright;
using System.IO;
using System.Windows.Controls;


namespace TestePortal.Utils
{
    public static class Excel
    {
        public static async Task<string> BaixarExcel(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 20000
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");
                    
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {
       
                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }

           
            return baixarExcel;
        }
        public static async Task<string> BaixarExcelPorTexto(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator("//button//span[text()='Excel']").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 20000
                    });
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");
                    
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {
       
                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }

           
            return baixarExcel;
        }
        public static async Task<string> BaixarExcelPorId(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator("#btnExportaExcel").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 15000
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");

                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }


            return baixarExcel;
        }
        public static async Task<string> BaixarExcelPorIdControleCapital(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator("#exportarControleCapitalBtn").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 15000
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");

                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }


            return baixarExcel;
        }
        public static async Task<string> BaixarExcelPorIdAporte(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator("#exportarButton").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 15000
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");

                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }


            return baixarExcel;
        }
        public static async Task<string> BaixarExcelPorIdResgate(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator("#btn-Excel").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 15000
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");

                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }


            return baixarExcel;
        }

        public static async Task<string> BaixarExcelRendimentoPorId(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator("#exportarRendimentoBtn").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 15000
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");

                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }


            return baixarExcel;
        }

        public static async Task<bool> BaixarExtrato(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            bool baixarExtrato = false;
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator("#Gerar").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 15000
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExtrato = true;

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExtrato = false;
                    Console.WriteLine("Erro ao baixar arquivo excel");

                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExtrato = false;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExtrato = false;
            }


            return baixarExtrato;
        }

        public static async Task<string> ValidarDownloadAsync(IDownload download, string nomeEsperado)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadPath, nomeEsperado);
            string resultado = "❌";

            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);

                if (File.Exists(filePath))
                {
                    Console.WriteLine("✅ Arquivo foi baixado.");
                    resultado = "✅";

                    File.Delete(filePath); // opcional
                    Console.WriteLine("Arquivo excluído.");
                }
                else
                {
                    Console.WriteLine("❌ Arquivo não encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao salvar/verificar download: {ex.Message}");
            }

            return resultado;
        }





    }
}

