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
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/auth/userlogin", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new Response<UserResponseDto>
                {
                    Success = false,
                    Message = $"API hatası: {response.StatusCode} - {errorContent}"
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<UserResponseDto>>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new Response<UserResponseDto>
            {
                Success = false,
                Message = "Geçersiz API yanıtı"
            };
        }
        catch (HttpRequestException httpEx)
        {
            return new Response<UserResponseDto>
            {
                Success = false,
                Message = $"Bağlantı hatası: {httpEx.Message}"
            };
        }
        catch (Exception ex)
        {
            return new Response<UserResponseDto>
            {
                Success = false,
                Message = $"Beklenmeyen hata: {ex.Message}"
            };
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(object registerRequest)
    {
        try
        {
            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Auth/register", content);

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
    public async Task<(bool IsSuccess, string ErrorMessage)> CreateRequestAsync(object requestData)
    {
        try
        {
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"Request URL: {_baseUrl}/Request");
            Console.WriteLine($"Request Body: {json}");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Request", content);

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
    public async Task<Response<List<DonationRequest>>> GetDonationRequestsAsync(int Id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Request/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new Response<List<DonationRequest>>
                {
                    Success = false,
                    Message = $"API hatası: {response.StatusCode} - {errorContent}",
                    Data = new List<DonationRequest>()
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<DonationRequest>>>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new Response<List<DonationRequest>>
            {
                Success = false,
                Message = "Geçersiz API yanıtı",
                Data = new List<DonationRequest>()
            };
        }
        catch (HttpRequestException httpEx)
        {
            return new Response<List<DonationRequest>>
            {
                Success = false,
                Message = $"Bağlantı hatası: {httpEx.Message}",
                Data = new List<DonationRequest>()
            };
        }
        catch (Exception ex)
        {
            return new Response<List<DonationRequest>>
            {
                Success = false,
                Message = $"Beklenmeyen hata: {ex.Message}",
                Data = new List<DonationRequest>()
            };
        }
    }

    public async Task<Response<bool>> DoDonation(int Id,int UserId)
    {
        try
        {
            var requestupdatedto = new RequestUpdateDto
            {
                IsActive = false
            };

            var jsonContent = JsonSerializer.Serialize(requestupdatedto);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/Request/{Id}", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return new Response<bool>
                {
                    Success = true,
                    Message = $"başarılı:",
                    Data = true
                };


            }
            else
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = $"başarısız",
                    Data = false
                };
            }
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Success = false,
                Message = $"Bir hata oluştu: {ex.Message}",
                Data = false
            };
        }

    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}