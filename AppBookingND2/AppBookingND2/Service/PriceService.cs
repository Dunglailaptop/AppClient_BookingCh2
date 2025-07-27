using AppBookingND2.Model;
using AppBookingND2.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppBookingND2.Service
{
    public class PriceService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public PriceService(ApiConfig apiConfig = null)
        {
            _apiConfig = apiConfig ?? ApiConfig.LoadFromConfig();
            _httpClient = new HttpClient();

            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            // Validate config trước khi sử dụng
            if (!_apiConfig.IsValid())
            {
                throw new InvalidOperationException("API configuration is invalid");
            }

            // Cấu hình timeout
            _httpClient.Timeout = TimeSpan.FromSeconds(_apiConfig.TimeoutSeconds);

            // Cấu hình headers mặc định
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", _apiConfig.ContentType);

            // Thêm API Key nếu có
            if (!string.IsNullOrEmpty(_apiConfig.ApiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiConfig.ApiKey);
            }

            // Thêm Bearer Token nếu có
            if (!string.IsNullOrEmpty(_apiConfig.BearerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiConfig.BearerToken);
            }

            // Thêm custom headers
            foreach (var header in _apiConfig.CustomHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        // GET: /api/GetServicePricesAsync
        public async Task<List<ServicePrice>> GetServicePricesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}{ApiUrlConstants.P_List}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<ServicePrice>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<ServicePrice>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Lỗi parse JSON: {ex.Message}", ex);
            }
        }

        // GET: /api/ServicePrices/{id}
        public async Task<ServicePrice> GetServicePriceByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}{ApiUrlConstants.P_Detail(id)}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<ServicePrice>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new ServicePrice();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
        }

        // POST: /api/ServicePrices
        public async Task<ServicePrice> CreateServicePriceAsync(ServicePrice ServicePrice)
        {
            try
            {
                var json = JsonSerializer.Serialize(ServicePrice);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PostAsync($"{_apiConfig.BaseUrl}{ApiUrlConstants.P_Create}", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var createdServicePrice = JsonSerializer.Deserialize<ServicePrice>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return createdServicePrice;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tạo nhân viên: {ex.Message}", ex);
            }
        }

        // PUT: /api/ServicePrices/{id}
        public async Task<ServicePrice> UpdateServicePriceAsync(int id, ServicePrice ServicePrice)
        {
            try
            {
                var json = JsonSerializer.Serialize(ServicePrice);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PutAsync($"{_apiConfig.BaseUrl}{ApiUrlConstants.P_Update(id)}", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedServicePrice = JsonSerializer.Deserialize<ServicePrice>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return updatedServicePrice;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        // DELETE: /api/ServicePrices/{id}
        public async Task<bool> DeleteServicePriceAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiConfig.BaseUrl}{ApiUrlConstants.P_Delete(id)}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi xóa nhân viên: {ex.Message}", ex);
            }
        }

     

        // Dispose HttpClient
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
