using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BloodDonationAppV2.Models;

namespace BloodDonationAppV2.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://yourapi.com"; 

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<LoginResponse> UserLoginAsync(string tcKimlikNo, string sifre)
        {
            try
            {
                var loginRequest = new { TcKimlikNo = tcKimlikNo, Sifre = sifre };
                var json = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/Auth/userlogin", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    return new LoginResponse { Success = false, Message = "Giriş başarısız" };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse { Success = false, Message = $"Hata: {ex.Message}" };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/Auth/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    return new RegisterResponse { Success = false, Message = "Kayıt başarısız" };
                }
            }
            catch (Exception ex)
            {
                return new RegisterResponse { Success = false, Message = $"Hata: {ex.Message}" };
            }
        }

        public async Task<List<DonationRequest>> GetDonationRequestsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/Request");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<DonationRequest>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return new List<DonationRequest>();
            }
            catch (Exception)
            {
                return new List<DonationRequest>();
            }
        }

        public async Task<DonationRequest> GetDonationRequestByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/Request/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<DonationRequest>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CreateDonationRequestAsync(CreateDonationRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/Request", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<DonationRequest>> GetUserDonationRequestsAsync(string tcKimlikNo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/Request/user/{tcKimlikNo}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<DonationRequest>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return new List<DonationRequest>();
            }
            catch (Exception)
            {
                return new List<DonationRequest>();
            }
        }
    }
}