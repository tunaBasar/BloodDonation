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
        _httpClient.Timeout = TimeSpan.FromSeconds(30); // Timeout ekle
    }

    public async Task<Response<UserResponseDto>> LoginAsync(LoginRequestDto loginRequest)
    {
        try
        {
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine($"Login Request: {json}");

            var response = await _httpClient.PostAsync($"{_baseUrl}/auth/userlogin", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Login Error: {response.StatusCode} - {errorContent}");
                return new Response<UserResponseDto>
                {
                    Success = false,
                    Message = $"API hatası: {response.StatusCode} - {errorContent}"
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"Login Response: {responseString}");
            
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
            System.Diagnostics.Debug.WriteLine($"Login HTTP Exception: {httpEx.Message}");
            return new Response<UserResponseDto>
            {
                Success = false,
                Message = $"Bağlantı hatası: {httpEx.Message}"
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Login Exception: {ex.Message}");
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

            System.Diagnostics.Debug.WriteLine($"Register Request: {json}");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Register Error: {responseContent}");
                return (false, $"Sunucu cevabı: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Register Exception: {ex.Message}");
            return (false, $"İstisna: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> CreateRequestAsync(object requestData)
    {
        try
        {
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine($"Request URL: {_baseUrl}/Request");
            System.Diagnostics.Debug.WriteLine($"Request Body: {json}");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Request", content);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"CreateRequest Error: {responseContent}");
                return (false, $"Sunucu cevabı: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CreateRequest Exception: {ex.Message}");
            return (false, $"İstisna: {ex.Message}");
        }
    }

    public async Task<Response<List<DonationRequest>>> GetDonationRequestsAsync(int Id)
    {
        try
        {
            var url = $"{_baseUrl}/Request/{Id}";
            System.Diagnostics.Debug.WriteLine($"GetDonationRequests URL: {url}");

            var response = await _httpClient.GetAsync(url);

            System.Diagnostics.Debug.WriteLine($"GetDonationRequests Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"GetDonationRequests Error: {response.StatusCode} - {errorContent}");
                return new Response<List<DonationRequest>>
                {
                    Success = false,
                    Message = $"API hatası: {response.StatusCode} - {errorContent}",
                    Data = new List<DonationRequest>()
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"GetDonationRequests Response: {responseString}");

            var result = JsonSerializer.Deserialize<Response<List<DonationRequest>>>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result != null)
            {
                System.Diagnostics.Debug.WriteLine($"Deserialized {result.Data?.Count ?? 0} donation requests");
            }

            return result ?? new Response<List<DonationRequest>>
            {
                Success = false,
                Message = "Geçersiz API yanıtı",
                Data = new List<DonationRequest>()
            };
        }
        catch (HttpRequestException httpEx)
        {
            System.Diagnostics.Debug.WriteLine($"GetDonationRequests HTTP Exception: {httpEx.Message}");
            return new Response<List<DonationRequest>>
            {
                Success = false,
                Message = $"Bağlantı hatası: {httpEx.Message}",
                Data = new List<DonationRequest>()
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetDonationRequests Exception: {ex.Message}");
            return new Response<List<DonationRequest>>
            {
                Success = false,
                Message = $"Beklenmeyen hata: {ex.Message}",
                Data = new List<DonationRequest>()
            };
        }
    }

    public async Task<Response<bool>> DoDonation(int Id, int UserId)
    {
        try
        {
            var requestupdatedto = new RequestUpdateDto
            {
                IsActive = false
            };

            var jsonContent = JsonSerializer.Serialize(requestupdatedto);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine($"DoDonation URL: {_baseUrl}/Request/{Id}");
            System.Diagnostics.Debug.WriteLine($"DoDonation Body: {jsonContent}");

            var response = await _httpClient.PutAsync($"{_baseUrl}/Request/{Id}", httpContent);
            
            if (response.IsSuccessStatusCode)
            {
                return new Response<bool>
                {
                    Success = true,
                    Message = "Bağış başarıyla kaydedildi",
                    Data = true
                };
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"DoDonation Error: {responseContent}");
                return new Response<bool>
                {
                    Success = false,
                    Message = $"Bağış işlemi başarısız: {responseContent}",
                    Data = false
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DoDonation Exception: {ex.Message}");
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