using Microsoft.Extensions.FileProviders;
using System.Text.Json;
using WeatherPlugin;

const string WEATHER_API_URL = "https://api.open-meteo.com/v1/forecast?";
var builder = WebApplication.CreateBuilder(args);

// Add services and its settings.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
            "https://chat.openai.com", 
            "http://localhost:5139")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), ".well-known")),
    RequestPath = "/.well-known"
});

// Add weather-forecast endpoint.
app.MapGet("/weather-forecast", async (double latitude, double longitude) =>
{
    using (var httpClient = new HttpClient())
    {
        var queryParams = $"latitude={latitude}&longitude={longitude}&current_weather=true";
        var url = WEATHER_API_URL + queryParams;

        var result = await httpClient.GetStringAsync(url);
        var jsonDocument = JsonDocument.Parse(result);
        var currentWeather = jsonDocument.RootElement.GetProperty("current_weather");
        return JsonSerializer.Deserialize<GetWeatherResponse>(currentWeather);

    }
})
.WithName("getWeather");

app.Run();
