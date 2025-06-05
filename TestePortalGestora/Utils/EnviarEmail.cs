using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestePortalGestora.Model;
using System.Security.Cryptography;
using TestePortalGestora.Pages;
using TestePortalGestora.Utils;
using System.Xml.Schema;
using static TestePortalGestora.Model.FluxosDeCadastros;


namespace TestePortalGestora.Utils
{
    public static class EnviarEmail
    {
        public static void SendMailWithAttachment(EmailPadrao emails)
        {
            string from = "no-reply@idsf.com.br";
            string fromName = "No-reply ZITEC";
            string smtp_username = "AKIAUPAU2HHA2TXXSNHI";
            string smtp_password = "BI14sHOsn0aYjv1frsN21UgKgGhfTe+CD8Sk7/vJEdOk";
            string host = "email-smtp.us-east-1.amazonaws.com";//"email-smtp.us-east-1.amazonaws.com";


            InternaAmazonEmailSender(emails.Email, emails.Subject, emails.Body, from, fromName, smtp_username, smtp_password, host);

        }

        private static void InternaAmazonEmailSender(string to, string subject, string body, string from, string fromName, string smtp_username, string smtp_password, string host)
        {
            string FROM = from;
            string FROMNAME = fromName;
            string TO = to;
            string SMTP_USERNAME = smtp_username;
            string SMTP_PASSWORD = smtp_password;
            string HOST = host;
            int PORT = 587;
            string SUBJECT = subject;
            string BODY = body;


            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(to);
            message.Subject = SUBJECT;
            message.Body = BODY;


            //if (!string.IsNullOrEmpty(fileAttached))
            //    message.Attachments.Add(new Attachment(fileAttached));

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
                    // client.TargetName = "smtp.office365.com";
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }

        public static string GerarHtml(List<Pagina> listaPagina, List<FluxosDeCadastros> listaFluxos, List<Operacoes> operacoes, Conciliacao conciliacao)
        {


            Console.WriteLine("A lista contém " + listaPagina.Count + " itens.");
            int totalPaginas = listaPagina.Count;
            var listaMaster = listaPagina.Where(p => p.Perfil == "Master").ToList();
            var listaConsultoria = listaPagina.Where(p => p.Perfil == "Consultoria").ToList();
            var listaGestora = listaPagina.Where(p => p.Perfil == "Gestora").ToList();
            var listaDenver = listaPagina.Where(p => p.Perfil == "Denver").ToList();

            var listaErros = listaPagina.Where(p => p.TotalErros > 0 || p.Listagem == "❌" || p.Acentos == "❌" || p.InserirDados == "❌" || p.Excluir == "❌").ToList();

            string Html = "<html>" +
           "<head>" +
           "<style>" +
           "table {" +
           "   border-collapse: collapse;" +
           "   width: 100%;" +
           "   margin-top: 10px;" +
           "   margin-bottom: 10px;" +
           "}" +
           "th, td {" +
           "   border: 1px solid #dddddd;" +
           "   text-align: left;" +
           "   padding: 8px;" +
           "   background-color: #f2f2f2;" +
           "}" +
           "hr.solid {" +
           "     border-top: 2px solid #bbb;" +
           "}" +
           "</style>" +
           "</head>" +
           "<body>" +
           "<h4>Olá, prezados. Segue o relatório com os status code das páginas e com as verificações solicitadas:</h4>" +
           "<h4>Total de Páginas Verificadas: " + totalPaginas + "</h4>";

        
            //segunda tabela com o relatório das páginas
            Html += "<h2>Aprovação de operação no nível gestora</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th></tr>";

            foreach (var pagina in listaGestora)
            {
                Html += "<tr>";
                Html += "<td> " + pagina.Nome + "</td>\n";
                Html += "<td> " + pagina.StatusCode + "</td>\n";
                Html += "<td> " + pagina.Acentos + "</td>\n";
                Html += "<td> " + pagina.Listagem + "</td>\n";
                Html += "<td> " + pagina.BaixarExcel + "</td>\n";
                Html += "<td> " + pagina.InserirDados + "</td>\n";
                Html += "<td> " + pagina.Excluir + "</td>\n";
                Html += "<td> " + pagina.TotalErros + "</td>\n";
                Html += "</tr>";
            }
            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";


            // tabela com validações especificas de conciliacao 

            Html += "<h2>Fluxo de Conciliação</h2>";
            Html += "<table>";
            Html += "<tr><th>Extrato Enviado</th><th>Aprovação modal</th><th>Aprovação em lote</th><th>Status conciliado</th><th>Total erros</th><th>Lista de erros</th></tr>";
            Html += "<tr>";
            Html += "<td> " + conciliacao.ExtratoEnviado + "</td>\n";
            Html += "<td> " + conciliacao.AprovacaoModal + "</td>\n";
            Html += "<td> " + conciliacao.AprovacaoEmLote + "</td>\n";
            Html += "<td> " + conciliacao.StatusConciliado + "</td>\n";
            Html += "<td> " + conciliacao.TotalErros + "</td>\n";
            Html += "<td> " + string.Join(", ", conciliacao.ListaErros) + "</td>\n";
            Html += "</tr>";

            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            //Legenda

            Html += "<h2>Legenda</h2>";
            Html += "<table>";
            Html += "<tr><td>✅</td><td>OK</td></tr>";
            Html += "<tr><td>❓</td><td>Não se aplica</td></tr>";
            Html += "<tr><td>❌</td><td>Erro</td></tr>";
            Html += "<tr><td>❗</td><td>Verificar/Atenção</td></tr>";
            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            Html += "</body></html>";
            return Html;

        }
    }
}
