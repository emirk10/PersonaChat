// Controllers/ChatController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication1.Services; // <-- KENDİ PROJE ADINIZA GÖRE DÜZENLEYİN
using WebApplication1.Models;   // <-- KENDİ PROJE ADINIZA GÖRE DÜZENLEYİN

namespace WebApplication1.Controllers // <-- KENDİ PROJE ADINIZA GÖRE DÜZENLEYİN
{
    public class ChatController : Controller
    {
        private readonly GeminiChatService _geminiChatService;

        public ChatController(GeminiChatService geminiChatService)
        {
            _geminiChatService = geminiChatService;
        }

        public IActionResult Index()
        {
            return View();
        }

        
    }
}