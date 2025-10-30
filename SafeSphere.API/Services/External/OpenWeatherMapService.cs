using System.Text.Json;
using System.Text.Json.Serialization;

namespace SafeSphere.API.Services.External;

/// <summary>
/// Implementation of weather API service using OpenWeatherMap
/// </summary>
public class OpenWeatherMapService : IWeatherApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenWeatherMapService> _logger;
    private readonly string _apiKey;

    public OpenWeatherMapService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<OpenWeatherMapService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _apiKey = _configuration["OpenWeatherMap:ApiKey"] ?? "demo"; // Replace with actual key
    }

    public async Task<WeatherApiResponse?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        try
        {
            // Using OpenWeatherMap 2.5 API (free tier)
            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";

            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Weather API returned status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<OpenWeatherMapCurrentResponse>(json);

            if (weatherData == null) return null;

            return new WeatherApiResponse
            {
                LocationName = weatherData.Name ?? "Unknown",
                Latitude = latitude,
                Longitude = longitude,
                Condition = weatherData.Weather?.FirstOrDefault()?.Main ?? "Unknown",
                Description = weatherData.Weather?.FirstOrDefault()?.Description ?? "No description",
                Temperature = weatherData.Main?.Temp ?? 0,
                FeelsLike = weatherData.Main?.FeelsLike ?? 0,
                Humidity = weatherData.Main?.Humidity ?? 0,
                WindSpeed = weatherData.Wind?.Speed ?? 0,
                Visibility = weatherData.Visibility,
                Timestamp = DateTime.UtcNow,
                Alerts = new List<WeatherApiAlert>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching current weather for location ({Lat}, {Lon})", latitude, longitude);
            return null;
        }
    }

    public async Task<WeatherForecastResponse?> GetWeatherForecastAsync(double latitude, double longitude, int days = 5)
    {
        try
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric&cnt={days * 8}";

            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Weather forecast API returned status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var forecastData = JsonSerializer.Deserialize<OpenWeatherMapForecastResponse>(json);

            if (forecastData == null) return null;

            var forecast = new WeatherForecastResponse
            {
                LocationName = forecastData.City?.Name ?? "Unknown",
                Forecast = new List<WeatherForecastDay>()
            };

            // Group by date and calculate daily averages
            var dailyData = forecastData.List?
                .GroupBy(x => x.DtTxt?.Date)
                .Select(g => new WeatherForecastDay
                {
                    Date = g.Key ?? DateTime.UtcNow,
                    TempMin = g.Min(x => x.Main?.TempMin ?? 0),
                    TempMax = g.Max(x => x.Main?.TempMax ?? 0),
                    Condition = g.First().Weather?.FirstOrDefault()?.Main ?? "Unknown",
                    Description = g.First().Weather?.FirstOrDefault()?.Description ?? "",
                    WindSpeed = g.Average(x => x.Wind?.Speed ?? 0),
                    Humidity = (int)g.Average(x => x.Main?.Humidity ?? 0)
                }).ToList();

            forecast.Forecast = dailyData ?? new List<WeatherForecastDay>();
            return forecast;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather forecast for location ({Lat}, {Lon})", latitude, longitude);
            return null;
        }
    }

    public Task<List<WeatherApiAlert>?> GetWeatherAlertsAsync(double latitude, double longitude)
    {
        try
        {
            // Note: Weather alerts require One Call API 3.0 which needs subscription
            // For demo purposes, returning empty list or can integrate with NOAA/other free services
            
            _logger.LogInformation("Weather alerts endpoint called for ({Lat}, {Lon})", latitude, longitude);
            return Task.FromResult<List<WeatherApiAlert>?>(new List<WeatherApiAlert>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather alerts for location ({Lat}, {Lon})", latitude, longitude);
            return Task.FromResult<List<WeatherApiAlert>?>(null);
        }
    }

    #region OpenWeatherMap Response Models

    private class OpenWeatherMapCurrentResponse
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherInfo>? Weather { get; set; }

        [JsonPropertyName("main")]
        public MainInfo? Main { get; set; }

        [JsonPropertyName("wind")]
        public WindInfo? Wind { get; set; }

        [JsonPropertyName("visibility")]
        public int? Visibility { get; set; }
    }

    private class OpenWeatherMapForecastResponse
    {
        [JsonPropertyName("list")]
        public List<ForecastItem>? List { get; set; }

        [JsonPropertyName("city")]
        public CityInfo? City { get; set; }
    }

    private class ForecastItem
    {
        [JsonPropertyName("dt_txt")]
        public DateTime? DtTxt { get; set; }

        [JsonPropertyName("main")]
        public MainInfo? Main { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherInfo>? Weather { get; set; }

        [JsonPropertyName("wind")]
        public WindInfo? Wind { get; set; }
    }

    private class WeatherInfo
    {
        [JsonPropertyName("main")]
        public string? Main { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    private class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    private class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }

    private class CityInfo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    #endregion
}

