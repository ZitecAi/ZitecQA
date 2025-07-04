using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace TestesOperacoesOperacoes
{
    using global::TestesOperacoesOperacoes.Model;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    namespace TestesOperacoesOperacoes.Model
    {
        public static class AppSettings
        {
            private static IConfigurationRoot _config;

            static AppSettings()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // ou AppDomain.CurrentDomain.BaseDirectory
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                _config = builder.Build();
            }

            public static Configurações.linkSettings Links =>
                _config.GetSection("Links").Get<Configurações.linkSettings>();

            public static Configurações.TokenSettings Tokens =>
                _config.GetSection("Tokens").Get<Configurações.TokenSettings>();

            public static Configurações.PathSettings Paths =>
                _config.GetSection("Paths").Get<Configurações.PathSettings>();

            public static string GetConnectionString(string name) =>
                _config.GetConnectionString(name);
        }
    }
}
