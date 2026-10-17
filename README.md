# PersonaChat

ASP.NET Core (MVC & YARP Reverse Proxy) ve Python (Flask & LangChain) tabanlı, Google Gemini destekli kişiselleştirilebilir yapay zekâ sohbet uygulaması.

---

## 🚀 Başlangıç

### 1. Gereksinimler
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) *(Önerilen)*
- veya yerel çalıştırma için:
  - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  - [Python 3.10+](https://www.python.org/)

---

### 2. Ortam Değişkenleri
`python_app` klasöründe `.env` dosyasını oluşturun ve Gemini API anahtarınızı ekleyin:

```bash
# python_app/.env
GOOGLE_API_KEY=your_gemini_api_key_here
```

> **Not:** `python_app/.env.example` dosyasını referans alabilirsiniz.

---

### 3. Çalıştırma

#### Docker ile (Önerilen)
Proje kök dizininde:

```bash
docker-compose up --build
```

- **Web Arayüzü:** [http://localhost:8080](http://localhost:8080)
- **Python API:** [http://localhost:5000](http://localhost:5000)

#### Manuel / Yerel Çalıştırma

1. **Python Servisini Başlatın:**
   ```bash
   cd python_app
   python -m venv venv
   # Windows için:
   venv\Scripts\activate
   # Linux/macOS için:
   # source venv/bin/activate
   pip install -r requirements.txt
   python app.py
   ```

2. **.NET Web Uygulamasını Başlatın:**
   ```bash
   # Proje kök dizininde
   dotnet run --project WebApplication1.csproj
   ```

---

## 🛠️ Mimari

- **Frontend / Proxy:** ASP.NET Core 8.0 MVC + YARP Reverse Proxy (port `8080`)
- **Backend:** Python Flask + LangChain + Google Gemini 1.5 Flash (port `5000`)
- **İletişim:** ASP.NET Core YARP üzerinden gelen istekleri (`/send_message`, `/send_persona_message`) Python servisine yönlendirir.

---

## 📝 Lisans
Bu proje [MIT](LICENSE) lisansı ile lisanslanmıştır veya kişisel kullanım içindir.
