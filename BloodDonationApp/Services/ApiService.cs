using BloodDonationApp.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BloodDonationApp.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5256/api";

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<Response<UserResponseDto>> LoginAsync(LoginRequestDto loginRequest)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/userlogin", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var userResponse = JsonSerializer.Deserialize<Response<UserResponseDto>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return userResponse!;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Login failed: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Login request failed: {ex.Message}", ex);
        }
    }



    public async Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(object registerRequest)
    {
        try
        {
            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return (false, $"Sunucu cevabı: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            return (false, $"İstisna: {ex.Message}");
        }
    }


}
