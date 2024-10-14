using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Translater.Services
{
    public class ImageTextExtractionService
    {
        private readonly string subscriptionKey = "<my-compVisi-api-key123>";
        private readonly string endpoint = "<my-compVisi-api-key123-endpoint>";
        public async Task<string> ExtractTextFromImageAsync(string imagePath)
        {
            ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
            {
                Endpoint = endpoint
            };
            using (Stream imageStream = File.OpenRead(imagePath))
            {
                var textHeaders = await client.ReadInStreamAsync(imageStream);
                string operationLocation = textHeaders.OperationLocation;
                string operationId = operationLocation.Substring(operationLocation.Length - 36);
                ReadOperationResult result;
                do
                {
                    result = await client.GetReadResultAsync(Guid.Parse(operationId));
                    await Task.Delay(1000);
                } 
                while (result.Status == OperationStatusCodes.Running || result.Status == OperationStatusCodes.NotStarted);
                var textResult = result.AnalyzeResult.ReadResults;
                StringBuilder extractedText = new StringBuilder();
                foreach (var page in textResult)
                {
                    foreach (var line in page.Lines)
                    {
                        extractedText.AppendLine(line.Text);
                    }
                }
                return extractedText.ToString();
            }
        }
    }
}