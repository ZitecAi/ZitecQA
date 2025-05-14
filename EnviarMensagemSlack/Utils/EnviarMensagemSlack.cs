using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace EnviarMensagemSlack.Utils
{
    public class EnviarMensagemSlack
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string slackToken = "xoxb-949825848999-4657262597863-QB2QXKdLUDzSXPLSIKzoQ5Cc"; // token de API do Slack
        private static readonly string channelId = "C03E3Q9SDME"; // id do canal no Slack
        private static readonly string teamsWebhookUrl = "https://zitecai.webhook.office.com/webhookb2/6671078e-8d97-48b4-84e5-0f8d702ba728@f969c1d3-c3d5-47bb-b197-d4fd82f29243/IncomingWebhook/daf91500958d479d8e61b9af94ec186c/d8206d2d-91ab-4ec6-a667-767437954b83/V28AtHaMWTjr4djuL32tGiecZ1uGPitEq9zN-Fy28Fyv01"; // URL do webhook do Teams

        public static async Task MandarMsgFundoQaAsync()
        {
            var linkSlack = "https://hooks.slack.com/services/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";

            var payload = new
            {
                text = "!custodia"
            };

            try
            {
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(linkSlack, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mensagem enviada com sucesso para o Slack.");
                    await Task.Delay(1500);
                    await VerificarRespostaAsync();
                }
                else
                {
                    Console.WriteLine($"Falha ao enviar mensagem. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }

        public static async Task VerificarRespostaAsync()
        {
            Console.WriteLine("Iniciando verificação da resposta...");

            int tentativas = 0;
            bool respostaEncontrada = false;

            while (tentativas < 5 && !respostaEncontrada)
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", slackToken);
                    var response = await client.GetAsync($"https://slack.com/api/conversations.history?channel={channelId}&limit=1");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var jsonDocument = JsonDocument.Parse(jsonString);
                        var messages = jsonDocument.RootElement.GetProperty("messages");

                        foreach (var message in messages.EnumerateArray())
                        {
                            try
                            {
                                if (message.GetProperty("text").GetString().Contains("!custodia"))
                                {
                                    var replyUsers = message.GetProperty("reply_users").EnumerateArray();
                                    bool userFound = replyUsers.Any(user => user.GetString() == "U04KB7QHKRD");

                                    if (userFound)
                                    {
                                        Console.WriteLine("Robo com id U04KB7QHKRD respondeu a mensagem.");
                                        respostaEncontrada = true;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Robo com id U04KB7QHKRD não respondeu a mensagem.");

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Mensagem não encontrada para verificação.");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Erro ao processar resposta: " + e.Message);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Falha ao buscar mensagens. Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao verificar a resposta: {ex.Message}");
                }

                tentativas++;
                await Task.Delay(1000);
            }

            if (!respostaEncontrada)
            {
                await EnviarMensagemTeams();
                Console.WriteLine("Mensagem enviada para o Teams");
            }
        }

        private static async Task EnviarMensagemTeams()
        {
            try
            {
                var message = new
                {
                    text = "O bot Id Comunicação não respondeu no Slack, favor religá-lo."

                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(message),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(teamsWebhookUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mensagem enviada com sucesso para o Teams!");
                }
                else
                {
                    Console.WriteLine("Falha ao enviar a mensagem para o Teams: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem para o Teams: {ex.Message}");
            }
        }
    }
}