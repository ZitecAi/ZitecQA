using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using SlackBotMessages;
using SlackBotMessages.Models;
using Attachment = System.Net.Mail.Attachment;
using Newtonsoft.Json.Linq;

namespace Idsf.Automatizacao.DownloadCarteira
{
    public class Util
    {
        public static void MsgAutomacaoRobo(string carteira, string data)
        {
            var client = new SbmClient("https://hooks.slack.com/services/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");
            var msg = new Message();
            msg.Text = string.Format("Carteira enviada {0}, na data {1}", carteira, data);
            SlackBotMessages.Models.Attachment attachment_S = new SlackBotMessages.Models.Attachment();
            attachment_S.AddField(Emoji.HeavyCheckMark, "", false);
            attachment_S.SetColor(SlackBotMessages.Enums.Color.Green);
            attachment_S.AuthorName = ":logo-id: Portal IDSF :logo-id:";
            attachment_S.AuthorLink = "https://portal.idsf.com.br/";
            msg.AddAttachment(attachment_S);
            client.Send(msg);

        }
        public static void MandarMsgErroGrupoDev(string mensagemErro, string Arquivo, string Projeto, string StackTrace)
        {
            var client = new SbmClient("https://hooks.slack.com/services/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");
            var msg = new Message();
            msg.Text = string.Format("O Erro no projeto {0} referente ao arquivo {1}.", Projeto, Arquivo);
            SlackBotMessages.Models.Attachment attachment_S = new SlackBotMessages.Models.Attachment();
            attachment_S.AddField(Emoji.X + "  Observação:", "", false);
            attachment_S.AddField("MensagemError: ", mensagemErro, false);
            attachment_S.AddField("StackTrace: ", StackTrace, false);
            attachment_S.SetColor(SlackBotMessages.Enums.Color.Red);
            attachment_S.AuthorName = ":logo-id: Portal IDSF :logo-id:";
            attachment_S.AuthorLink = "https://portal.idsf.com.br/";
            msg.AddAttachment(attachment_S);
            client.Send(msg);
        }
        public static void SendMailWithAttachment(string to, string subject, string body, string fileAttached)
        {
            string from = "sac@idsf.com.br";
            string fromName = "SAC IDSF";
            string smtp_username = "sac@idsf.com.br";
            string smtp_password = "t0CtvHGU7Utxaw%%%88(1S9tyOA!#T";
            string host = "smtp.office365.com";//"email-smtp.us-east-1.amazonaws.com";


            InternaAmazonEmailSender(to.Trim(' '), subject, body, from, fromName, smtp_username, smtp_password, host, fileAttached);

        }

        public static void SendMailWithAttachmentAndCC(string to, string cc, string subject, string body, string fileAttached)
        {

            // Replace sender@example.com with your "From" address. 
            // This address must be verified with Amazon SES.
            string FROM = "sac@idsf.com.br"; // "sac@sabiacapital.com.br";
            string FROMNAME = "SAC IDSF"; //"Sabia Capital";

            // Replace recipient@example.com with a "To" address. If your account 
            // is still in the sandbox, this address must be verified.
            string TO = to.Trim(' ');

            // Replace smtp_username with your Amazon SES SMTP user name.
            string SMTP_USERNAME = "sac@idsf.com.br"; //"AKIA4LTBIUFP4USXH42I"; //AKIAJ3EBESAKCAJKRUAQ

            // Replace smtp_password with your Amazon SES SMTP user name.
            string SMTP_PASSWORD = "t0CtvHGU7Utxaw%%%88(1S9tyOA!#T"; // "BMasw1XWwA5EJzZdNGCpv4xvcKXDc3FrT9ZNY1XLmIFH"; //BBGQ2L84qZxBOUW

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            //String CONFIGSET = "ConfigSet";

            // If you're using Amazon SES in a region other than Oeste dos EUA (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            string HOST = "smtp.office365.com"; //"email-smtp.us-east-1.amazonaws.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int PORT = 587;

            //// The subject line of the email
            //string SUBJECT = subject;

            //// The body of the email
            //string BODY = body;

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add("controladoria@idsf.com.br");

            message.Subject = subject;
            message.Body = body;
            message.Bcc.Add(TO);
            if (!string.IsNullOrEmpty(cc))
            {
                message.CC.Add(cc);
            }
            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            if (!string.IsNullOrEmpty(fileAttached))
                message.Attachments.Add(new Attachment(fileAttached));

            using (var client = new SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {
                    client.TargetName = "smtp.office365.com";
                    client.Send(message);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static DateTime GetUltimoDiaAnterior(DateTime date)
        {
            DateTime diaAnterior = date.AddDays(-1);

            while (diaAnterior.DayOfWeek == DayOfWeek.Sunday || diaAnterior.DayOfWeek == DayOfWeek.Saturday)
            {
                diaAnterior = diaAnterior.AddDays(-1);
            }
            return diaAnterior;
        }

        private static void InternaAmazonEmailSender(string to, string subject, string body, string from, string fromName, string smtp_username, string smtp_password, string host, string fileAttached)
        {
            /* Utilizado pelo SAC */


            // Replace sender@example.com with your "From" address. 
            // This address must be verified with Amazon SES.
            string FROM = from; // "sac@sabiacapital.com.br";
            string FROMNAME = fromName;//"Sabia Capital";

            // Replace recipient@example.com with a "To" address. If your account 
            // is still in the sandbox, this address must be verified.
            string TO = to;

            // Replace smtp_username with your Amazon SES SMTP user name.
            string SMTP_USERNAME = smtp_username; //"AKIA4LTBIUFP4USXH42I"; //AKIAJ3EBESAKCAJKRUAQ

            // Replace smtp_password with your Amazon SES SMTP user name.
            string SMTP_PASSWORD = smtp_password; // "BMasw1XWwA5EJzZdNGCpv4xvcKXDc3FrT9ZNY1XLmIFH"; //BBGQ2L84qZxBOUW

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            //String CONFIGSET = "ConfigSet";

            // If you're using Amazon SES in a region other than Oeste dos EUA (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            string HOST = host; //"email-smtp.us-east-1.amazonaws.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int PORT = 587;

            // The subject line of the email
            string SUBJECT = subject;

            // The body of the email
            string BODY = body;

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add("controladoria@idsf.com.br");

            message.Subject = SUBJECT;
            message.Body = BODY;
            message.Bcc.Add(TO);
            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            if (!string.IsNullOrEmpty(fileAttached))
                message.Attachments.Add(new Attachment(fileAttached));

            using (var client = new SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {
                    client.TargetName = "smtp.office365.com";
                    client.Send(message);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //public static List<Cotas> GetListaCotasByExcel(string pathExcel)
        //{
        //    List<Cotas> listacotas = new List<Cotas>();

        //    try
        //    {
        //        using (var excelWorkbook = new XLWorkbook(pathExcel))
        //        {
        //            var rows = excelWorkbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1); // Skip header row
        //            foreach (var row in rows)
        //            {
        //                Cotas cotas = new Cotas();
        //                var rowNumber = row.RowNumber();
        //                cotas.Carteira = row.Cell(1).Value.ToString();
        //                cotas.NomeFundo = row.Cell(2).Value.ToString();
        //                cotas.emails = row.Cell(3).Value.ToString();
        //                cotas.DataConfig = Int32.Parse(row.Cell(4).Value.ToString());
        //                cotas.Ativo = row.Cell(5).Value.ToString();
        //                listacotas.Add(cotas);
        //            }
        //        }
        //        return listacotas;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
