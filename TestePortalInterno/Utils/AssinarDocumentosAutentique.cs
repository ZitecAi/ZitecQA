using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace TestePortalInterno.Utils
{
    public class AssinarDocumentosAutentique
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static CustomResponse AssinarDocumento(string token, string idDocumento)
        {
            string responseBody = "";
            string apiUrl = "";

            try
            {
                apiUrl = "https://prod.idsf.com.br/api/Autentique/SignRemessa";

                var requestData = new
                {
                    IdDocument = idDocumento,
                    UserToken = token
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("token", ConfigurationManager.AppSettings["TokenPortal"]);

                    var response = httpClient.PostAsync(apiUrl, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = response.Content.ReadAsStringAsync().Result;
                    }
                    return JsonConvert.DeserializeObject<CustomResponse>(responseBody);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                Service.LogRequestService.Add(apiUrl, "", "", Convert.ToString(responseBody), "GET", "200", DateTime.Now, "make");
            }
        }
    }
}
