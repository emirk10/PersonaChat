// Models/ChatModels.cs

// Bu namespace'i projenizin gerçek adıyla eşleştirin.
// Hata çıktınızda proje adı "WebApplication1" idi, bu yüzden büyük ihtimalle budur.
namespace WebApplication1.Models
{
    // Sohbet mesajları için temel model
    public class ChatMessage
    {
        public string? Role { get; set; } // "user" veya "model"
        public string? Text { get; set; } // Mesajın içeriği
    }

    // Kullanıcıdan gelen sohbet isteği modeli
    public class ChatRequest
    {
        public string? UserMessage { get; set; }
        public List<ChatMessage> History { get; set; } = new List<ChatMessage>();
        public string? CharacterPrompt { get; set; } // YENİ EKLENDİ: Karakter promptu
    }

    // Sunucudan frontend'e dönen sohbet yanıtı modeli
    public class ChatResponse
    {
        public bool Success { get; set; }
        public string? Response { get; set; } // Gemini'den gelen yanıt
        public List<ChatMessage>? UpdatedHistory { get; set; } // Güncellenmiş sohbet geçmişi
    }

    // *** BURADAN BAŞLAYARAK HATAYI VEREN SINIFLAR ***

    // Gemini API'sinden gelen ANA yanıt yapısı
    public class GeminiApiResponse
    {
        public List<Candidate>? candidates { get; set; }
        // API'den farklı alanlar geliyorsa buraya ekleyebilirsiniz
        // public UsageMetadata usageMetadata { get; set; }
    }

    // API yanıtındaki aday (model tarafından üretilen) içerik
    public class Candidate
    {
        public Content? content { get; set; }
        public string? finishReason { get; set; }
        public int index { get; set; }
        // public List<SafetyRating> safetyRatings { get; set; }
    }

    // İçerik (konuşmanın metinsel parçaları)
    public class Content
    {
        public List<Part>? parts { get; set; }
        public string? role { get; set; } // "user" veya "model"
    }

    // İçeriğin tek bir metin parçası
    public class Part
    {
        public string? text { get; set; }
        // Eğer görsel veya diğer medya türleri varsa buraya eklenebilir.
    }

    // API'den hata yanıtı geldiğinde ayrıştırmak için (iyi bir uygulama)
    public class GeminiErrorResponse
    {
        public Error? error { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string? message { get; set; }
        public string? status { get; set; }
        // public List<ErrorDetail> details { get; set; }
    }

    // *** BURAYA KADAR OLAN KISIM ***
}