using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestePortalGestora.Utils
{
    public class EmailChecker
    {
        private static readonly string _email = "jehvittav@gmail.com";
        private static readonly string _password = "bekz qhef wspy zrss"; // Senha de aplicativo gerada
        private readonly string _imapServer;
        private readonly int _imapPort;

        public EmailChecker(string imapServer = "imap.gmail.com", int imapPort = 993)
        {
            _imapServer = imapServer;
            _imapPort = imapPort;
        }

        public async Task<bool> CheckForNotificationEmailAsync(string subjectKeyword)
        {
            using var client = new ImapClient();

            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Ignora validação SSL

                await client.ConnectAsync(_imapServer, _imapPort, true);
                await client.AuthenticateAsync(_email, _password);

                var inbox = client.Inbox;
                await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

                var uids = await inbox.SearchAsync(SearchQuery.SubjectContains(subjectKeyword));
                var recentUids = uids.TakeLast(4);

                foreach (var uid in recentUids)
                {
                    var message = await inbox.GetMessageAsync(uid);
                    Console.WriteLine($"Email encontrado: {message.Subject} de {message.From}");

                    if (message.Subject.Contains(subjectKeyword, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                await client.DisconnectAsync(true);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao verificar e-mail: " + ex.Message);
                return false;
            }
        }


    }

}
