using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TesteOperacoesOperacoes.Model;
using System.Security.Cryptography;
using TesteOperacoesOperacoes.Pages;
using TesteOperacoesOperacoes.Util;
using System.Xml.Schema;
using static TesteOperacoesOperacoes.Model.FluxosDeCadastros;
using static TesteOperacoesOperacoes.Model.TesteNegativoResultado;
using static TesteOperacoesOperacoes.Pages.OperacoesPage.CadastroOperacoesZitecCsv;

namespace TesteOperacoesOperacoes.Util
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

        public static string GerarHtml(List<Pagina> listaPagina, List<FluxosDeCadastros> listaFluxos, List<Operacoes> operacoes, List<TesteNegativoResultado> resultadosTestesNegativos)
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

            //primeira tabela com o relatório das páginas
            Html += "<h2>Relatório com o usuário: Interno</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th>";

            foreach (var pagina in listaMaster)
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

            Html += "<h2>Relatório de Testes Negativos Cadastro de Operações CSV </h2>\n";
            Html += "<table>\n";
            Html += "<tr><th>ID do Teste</th><th>Resultado</th></tr>\n";

            foreach (var teste in resultadosTestesNegativos)
            {
                Html += $"<tr><td>{teste.IdDoTeste}</td><td>{teste.Resultado}</td></tr>\n";
            }

            Html += "</table>\n";


            //terceira tabela
            Html += "<h2>Relatório com o usuário: Consultoria</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th></tr>";

            foreach (var pagina in listaConsultoria)
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

            //segunda tabela com o relatório das páginas
            Html += "<h2>Relatório com o usuário: Gestora</h2>";
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


            //quarta tabela

            //Html += "<h2>Relatório com o usuário: Denver</h2>";
            //Html += "<table>";
            //Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th></tr>";

            //foreach (var pagina in listaDenver)
            //{
            //    Html += "<tr>";
            //    Html += "<td> " + pagina.Nome + "</td>\n";
            //    Html += "<td> " + pagina.StatusCode + "</td>\n";
            //    Html += "<td> " + pagina.Acentos + "</td>\n";
            //    Html += "<td> " + pagina.Listagem + "</td>\n";
            //    Html += "<td> " + pagina.BaixarExcel + "</td>\n";
            //    Html += "<td> " + pagina.InserirDados + "</td>\n";
            //    Html += "<td> " + pagina.Excluir + "</td>\n";
            //    Html += "<td> " + pagina.TotalErros + "</td>\n";
            //    Html += "</tr>";
            //}
            //Html += "</table>";
            //Html += "<br>";
            //Html += "<hr class=\"solid\">";


            // tabela com validações especificas de operações 

            Html += "<h2>Fluxo de cadastro de Operações</h2>";
            Html += "<table border='1'>";
            Html += "<tr><th>Arquivo</th><th>Tipo Operação</th><th>Arquivo Enviado</th><th>Aprovações realizadas</th><th>Todos os status trocados</th><th>Exclusão Btn</th><th>Total de erros</th><th>Lista de erros</th></tr>";

            foreach (var item in operacoes)
            {

                if (item.ListaErros2 != null)
                {
                    // Linha para os dados com sufixo 2
                    Html += "<tr>";
                    Html += "<td>" + item.NovoNomeArquivo2 + "</td>\n";
                    Html += "<td>" + item.TipoOperacao2 + "</td>\n";
                    Html += "<td>" + item.ArquivoEnviado + "</td>\n";
                    Html += "<td>" + item.AprovacoesRealizadas2 + "</td>\n";
                    Html += "<td>" + item.StatusTrocados2 + "</td>\n";
                    Html += "<td>" + item.OpApagadaBtn + "</td>\n";
                    Html += "<td>" + item.totalErros2 + "</td>\n";
                    if (item.ListaErros2 != null)
                        Html += "<td>" + string.Join(", ", item.ListaErros2) + "</td>\n";
                    else Html += "<td></td>";
                    Html += "</tr>";
                }
                if (item.ListaErros3 != null)
                {
                    // Linha para os dados com sufixo 3
                    Html += "<tr>";
                    Html += "<td>" + item.NovoNomeArquivo3 + "</td>\n";
                    Html += "<td>" + item.TipoOperacao3 + "</td>\n";
                    Html += "<td>" + item.ArquivoEnviado + "</td>\n";
                    Html += "<td>" + item.AprovacoesRealizadas3 + "</td>\n";
                    Html += "<td>" + item.StatusTrocados3 + "</td>\n";
                    Html += "<td>" + item.OpApagadaBtn + "</td>\n";
                    Html += "<td>" + item.totalErros3 + "</td>\n";
                    if (item.ListaErros3 != null)
                        Html += "<td>" + string.Join(", ", item.ListaErros3) + "</td>\n";
                    else Html += "<td></td>";
                    Html += "</tr>";
                }
            }

            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            //tabela com fluxos de cadastros

            //Html += "<h2>Tabela com o resultado dos fluxos de cadastro.</h2>";
            //Html += "<table>";
            //Html += "<tr><th>Fluxo</th><th>Formulario</th><th>Status em análise</th><th>Formulario completo</th><th>Doc Assinado</th><th>Email recebido</th><th>Status Aprovado</th><th>Total erros</th><th>Lista de erros</th></tr>";

            //foreach (var fluxoDeCadastros in listaFluxos)
            //{
            //    Html += "<tr>";
            //    Html += "<td> " + fluxoDeCadastros.Fluxo + "</td>\n";
            //    Html += "<td> " + fluxoDeCadastros.Formulario + "</td>\n";
            //    Html += "<td> " + fluxoDeCadastros.StatusEmAnalise + "</td>\n";
            //    Html += "<td> " + fluxoDeCadastros.FormularioCompletoNoPortal + "</td>\n";
            //    Html += "<td> " + fluxoDeCadastros.DocumentoAssinado + "</td>\n";
            //    Html += "<td> " + fluxoDeCadastros.EmailRecebido + "</td>\n";
            //    Html += "<td> " + fluxoDeCadastros.statusAprovado + "</td>\n";
            //    Html += "<td> " + fluxoDeCadastros.TotalErros + "</td>\n";
            //    Html += "<td> " + string.Join(", ", fluxoDeCadastros.ListaErros) + "</td>\n";
            //    Html += "</tr>";
            //}
            //Html += "</table>";
            //Html += "<br>";
            //Html += "<hr class=\"solid\">";

            //tabela com os erros 

            Html += "<h2>Resumo de Páginas com Erros (Perfil: Master)</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome da Página</th><th>Erro</th></tr>";

            foreach (var pagina in listaErros.Where(p => p.Perfil == "Master"))
            {
                var erros = new List<string>();

                // Adiciona erros específicos
                if (pagina.Listagem == "❌") erros.Add("Listagem");
                if (pagina.Acentos == "❌") erros.Add("Acentos");
                if (pagina.BaixarExcel == "❌") erros.Add("BaixarExcel");
                if (pagina.InserirDados == "❌") erros.Add("InserirDados");
                if (pagina.Excluir == "❌") erros.Add("Excluir");
                //if (pagina.TotalErros > 0) erros.Add("Total de Erros: " + pagina.TotalErros);

                // Adiciona à tabela
                Html += "<tr>";
                Html += "<td> " + pagina.Nome + "</td>\n";
                Html += "<td>" + string.Join(", ", erros) + "</td>\n"; // Lista de erros
                Html += "</tr>";
            }

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
