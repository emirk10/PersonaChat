// Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; // IConfiguration i�in
using WebApplication1.Services; // GeminiChatService'in namespace'ini buraya ekleyin!

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. (Servisleri kapsay�c�ya ekle.)
// Bu k�s�m, Startup.cs'deki ConfigureServices metodunun yerini al�r.

builder.Services.AddControllersWithViews();

// GeminiChatService'i ba��ml�l�k enjeksiyonu i�in kaydet
// AddSingleton: Uygulaman�n �mr� boyunca bu servisten sadece bir �rnek olu�ur.
// IConfiguration, zaten builder.Configuration ile eri�ilebilir ve DI taraf�ndan otomatik sa�lan�r.
builder.Services.AddSingleton<GeminiChatService>();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


var app = builder.Build();

// Configure the HTTP request pipeline. (HTTP iste�i pipeline'�n� yap�land�r.)
// Bu k�s�m, Startup.cs'deki Configure metodunun yerini al�r.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Geli�tirme ortam� i�in detayl� hata sayfas�
}
else
{
    app.UseExceptionHandler("/Home/Error"); // �retim i�in hata i�leyici
    app.UseHsts(); // HTTPS Strict Transport Security
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'ye y�nlendir
app.UseStaticFiles(); // wwwroot klas�r�ndeki statik dosyalar� (CSS, JS, resimler) sun

app.UseRouting(); // Y�nlendirme sistemini etkinle�tir

app.UseAuthorization(); // Yetkilendirme (e�er kullan�l�yorsa)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");

app.MapReverseProxy(); // Varsay�lan Controller ve Action

app.Run(); // Uygulamay� �al��t�r