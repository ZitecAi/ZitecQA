using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestePortalExecutavel.Utils
{
    public static class Service
    {
        
        public static class LogRequestService
        {
         
            public static void Add(string apiUrl, string param1, string param2, string responseBody, string method, string status, DateTime date, string source)
            {
                try
                {
          
                    string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
                    string logEntry = $"[{date}] - API: {apiUrl}, Method: {method}, Status: {status}, Params: ({param1}, {param2}), Response: {responseBody}, Source: {source}\n";
                    File.AppendAllText(logFilePath, logEntry);
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine($"Erro ao gravar log: {ex.Message}");
                }
            }
        }
    }
}
