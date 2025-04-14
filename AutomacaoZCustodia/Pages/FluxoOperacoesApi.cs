using AutomacaoZCustodia.Utils;
using AutomacaoZCustodia.Repository;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutomacaoZCustodia.Models;
using Newtonsoft.Json;
using System.Configuration;

public class FluxoOperacoesApi
{

    public static class OperacoesApi
    {
        public static readonly HttpClient _httpClient = new HttpClient();

        static OperacoesApi()
        {
            _httpClient.DefaultRequestHeaders.Add("Ambiente", "custodia");
        }

        public static async Task<Operacoes> EnviarArquivoAsync(Operacoes operacoes)
        {
            operacoes.TipoOperacao = "Operações via API";
            operacoes.ListaErros1 = new List<string>();

            var processamentoFundo = AutomacaoZCustodia.Repository.OperacoesApiRepository.VerificaProcessamentoFundo(9991);

            if (!processamentoFundo)
            {
                Console.WriteLine("Falha no processamento do fundo");
                operacoes.ListaErros1.Add("Falha no processamento do fundo");
                return operacoes;
            }

            try
            {
                string caminhoArquivo = "C:\\Temp\\Arquivos\\CNABz.txt";
                (string nomeArquivoTxt, string caminhoArquivoTxt) = await AtualizarCnab.AtualizarDataEConverterParaBase64(caminhoArquivo);


                if (!File.Exists(caminhoArquivoTxt))
                {
                    Console.WriteLine("Arquivo TXT não encontrado!");
                    return operacoes;
                }

                string url = "https://custodiabackend-prod.idsf.com.br/api/Arquivo/InsertArquivo";

                using (var form = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(caminhoArquivoTxt));
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                    form.Add(new StringContent("54638076000176"), "CnpjFundo");
                    form.Add(new StringContent(DateTime.Now.ToString("yyyy-MM-dd")), "Data");
                    form.Add(new StringContent("custodia"), "Ambiente");
                    form.Add(fileContent, "file", nomeArquivoTxt);

                    var response = await _httpClient.PostAsync(url, form);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"Resposta da API: {responseContent}");

                    if (response.IsSuccessStatusCode && responseContent.Contains("success"))
                    {
                        Console.WriteLine("Arquivo enviado com sucesso!");

                        //// **🔹 Extraindo o ID da operação da resposta JSON**
                        //var responseJson = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        //Guid idOperacaoRecebivel = responseJson?.data?.idArquivoGeral;
                        
                        var idOperacaoRecebivel = AutomacaoZCustodia.Repository.OperacoesApiRepository.ObterIdOperacao(nomeArquivoTxt);
                        var idCertificadora = AutomacaoZCustodia.Repository.OperacoesApiRepository.ObterIdCertificadora(9991);

                        if (idOperacaoRecebivel != null)
                        {
                            // **🔹 Chama as aprovações após o envio bem-sucedido**
                            operacoes.InsertOperacao = "✅";
                            await AprovarOperacaoConsultoriaAsync(operacoes, idOperacaoRecebivel, idCertificadora);
                            await AprovarOperacaoGestoraAsync(operacoes, idOperacaoRecebivel, "custodia");



                        }
                        else
                        {
               
                            operacoes.InsertOperacao = "❌";
                            operacoes.ListaErros1.Add("ID da operação não encontrado.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao enviar arquivo. Status: {response.StatusCode}, Resposta: {responseContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                operacoes.InsertOperacao = "❌";
                operacoes.ListaErros1.Add(ex.Message);
            }

            return operacoes;
        }
        public static async Task <bool> AprovarOperacaoConsultoriaAsync(Operacoes operacoes, int idOperacaoRecebivel, int idCertificadora)
        {
            var sucesso = true;

            var body = new
            {
                idOperacaoRecebivel,
                idContaCorrente = 1,
                idCertificadora,
                idsAvalistas = new[] { 33509316 },
                valorReembolso = 0
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://custodiabackend-prod.idsf.com.br/api/Operation/approval/consultancy", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && responseBody.Contains("success"))
                {
                    Console.WriteLine("Operação de consultoria aprovada com sucesso!");
                    operacoes.AprovacaoConsultoria = "✅";
                    sucesso = true;
                }
                else
                {
                    Console.WriteLine($"Falha ao aprovar a operação de consultoria. Resposta: {responseBody}");
                    operacoes.AprovacaoConsultoria = "❌";
                    sucesso = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na aprovação da operação de consultoria: {ex.Message}");
            }
            return sucesso;
        }

        public static async Task <bool> AprovarOperacaoGestoraAsync(Operacoes operacoes, int idOperacaoRecebivel, string ambiente)
        {
            string BaseUrlApiZitec = ConfigurationManager.AppSettings["BaseUrlApiZitec"];

            var jsonBody = JsonConvert.SerializeObject(idOperacaoRecebivel); // Apenas o número, sem chaves
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // Garante que seja JSON válido

            var sucesso = true;

            try
            {
                Console.WriteLine($"Enviando requisição para {BaseUrlApiZitec}Operation/approval/manager com o corpo: {jsonBody}");

                using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrlApiZitec}Operation/approval/manager");
                request.Headers.Add("Ambiente", ambiente);
                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Resposta da API: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Operação aprovada com sucesso!");
                    operacoes.AprovacaoGestora = "✅";
                    sucesso = true;
                    
                }
                
                else
                {
                    Console.WriteLine($"Falha na aprovação. Status: {response.StatusCode}");
                    operacoes.AprovacaoGestora = "❌";
                    sucesso = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na aprovação da operação gestora: {ex.Message}");
            }
            return sucesso;
        }

    }
}