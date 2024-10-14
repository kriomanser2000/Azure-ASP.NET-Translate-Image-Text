using System.Text;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Translater.Services
{
    public class TranslationService
    {
        private readonly string subscriptionKey = "<my-trans-api123>";
        private readonly string endpoint = "<my-trans-api123-endpoint1223>";
        public async Task<string> TranslateTextAsync(string inputText, string fromLanguage, string toLanguage)
        {
            string route = $"/translate?api-version=3.0&from={fromLanguage}&to={toLanguage}";
            string requestBody = JsonConvert.SerializeObject(new[] { new { Text = inputText } });
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("sub-key", subscriptionKey);
                client.DefaultRequestHeaders.Add("sub-region", "easteurope");
                HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint + route, content);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}