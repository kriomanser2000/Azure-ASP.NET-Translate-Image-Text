using Microsoft.AspNetCore.Mvc;
using Translater.Services;

namespace Translater.Controllers
{
    public class TranslateController : Controller
    {
        private readonly TranslationService _translationService;
        private readonly ImageTextExtractionService _imageTextExtractionService;
        public TranslateController(TranslationService translationService, ImageTextExtractionService imageTextExtractionService)
        {
            _translationService = translationService;
            _imageTextExtractionService = imageTextExtractionService;
        }
        [HttpPost]
        public async Task<IActionResult> TranslateText(string inputText, string fromLanguage, string toLanguage)
        {
            string translatedText = await _translationService.TranslateTextAsync(inputText, fromLanguage, toLanguage);
            return View("Result", translatedText);
        }
        [HttpPost]
        public async Task<IActionResult> TranslateFromImage(IFormFile imageFile, string fromLanguage, string toLanguage)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", imageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            string extractedText = await _imageTextExtractionService.ExtractTextFromImageAsync(filePath);
            string translatedText = await _translationService.TranslateTextAsync(extractedText, fromLanguage, toLanguage);
            return View("Result", translatedText);
        }
    }
}