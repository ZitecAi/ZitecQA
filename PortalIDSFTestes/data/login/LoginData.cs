using Microsoft.Extensions.Configuration;

namespace PortalIDSFTestes.data.login
{
    public class LoginData
    {
        public static string Config(bool isEmail)
        {
            var config = new ConfigurationManager();
            config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            var emailEnv = Environment.GetEnvironmentVariable("PORTAL_EMAIL");
            var passEnv = Environment.GetEnvironmentVariable("PORTAL_PASS");
            var emailConfig = config["Credentials:Email"];
            var passConfig = config["Credentials:Password"];

            var email = emailConfig ?? emailEnv;
            var senha = passConfig ?? passEnv;

            return isEmail ? $"{email}" : $"{senha}";
        }

        public string Email { get; set; }
        public string Password { get; set; }

        public LoginData()
        {
            Email = Config(true);
            Password = Config(false);
        }
        public string InvalidEmail { get; set; } = "al@zitecai";
        public string InvalidPassword { get; set; } = "invalid";
    }

}

