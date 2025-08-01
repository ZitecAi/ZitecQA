using Microsoft.Extensions.Configuration;
using System.IO;

namespace TesteCedente
{
    public static class AppSettings
    {
        public static IConfigurationRoot Config;

        static AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Config = builder.Build();
        }

        public static string GetValue(string chave) =>
            Config[chave];

        public static string GetConnectionString(string name) =>
            Config.GetConnectionString(name);
    }
}
