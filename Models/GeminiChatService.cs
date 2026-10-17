// Services/GeminiChatService.cs
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models; // <-- KENDİ PROJE ADINIZA GÖRE DÜZENLEYİN

namespace WebApplication1.Services // <-- KENDİ PROJE ADINIZA GÖRE DÜZENLEYİN
{
    public class GeminiChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pythonApiUrl;

        public GeminiChatService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            // Python API'sinin adresini appsettings.json'dan veya ortam değişkenlerinden al
            _pythonApiUrl = configuration["PythonApi:Url"] ?? "http://python_app:5000/chat"; // Docker içinde servis adı
        }

        public async Task<string> GetGeminiResponse(string userMessage, List<ChatMessage> chatHistory, string characterPrompt)
        {
            try
            {
                var requestBody = new
                {
                    message = userMessage,
                    history = chatHistory,
                    character_prompt = characterPrompt
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_pythonApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Python API Hata Detayı: {response.StatusCode} - {errorResponseContent}");
                    return $"Python API isteği hatası: {response.StatusCode} - {errorResponseContent}";
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

                if (apiResponse != null && apiResponse.TryGetValue("response", out string geminiText))
                {
                    return geminiText;
                }
                else
                {
                    return "Yanıt alınamadı veya boş geldi.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}");
                return $"Beklenmeyen bir hata oluştu: {ex.Message}";
            }
        }
    }
}