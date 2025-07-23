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
    public class ExamTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public ExamTypeService(ApiConfig apiConfig = null)
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

        // GET: /api/ExamTypes
        public async Task<List<ExamType>> GetExamTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/ExamType/GetListExamType");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<ExamType>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<ExamType>();
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

        // GET: /api/ExamTypes/{id}
        public async Task<ExamType> GetExamTypeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/ExamTypes/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<ExamType>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new ExamType();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
        }

        // POST: /api/ExamTypes
        public async Task<ExamType> CreateExamTypeAsync(ExamType ExamType)
        {
            try
            {
                var json = JsonSerializer.Serialize(ExamType);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PostAsync($"{_apiConfig.BaseUrl}/api/ExamTypes", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var createdExamType = JsonSerializer.Deserialize<ExamType>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return createdExamType;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tạo nhân viên: {ex.Message}", ex);
            }
        }

        // PUT: /api/ExamTypes/{id}
        public async Task<ExamType> UpdateExamTypeAsync(int id, ExamType ExamType)
        {
            try
            {
                var json = JsonSerializer.Serialize(ExamType);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PutAsync($"{_apiConfig.BaseUrl}/api/ExamTypes/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedExamType = JsonSerializer.Deserialize<ExamType>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return updatedExamType;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        // DELETE: /api/ExamTypes/{id}
        public async Task<bool> DeleteExamTypeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiConfig.BaseUrl}/api/ExamTypes/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi xóa nhân viên: {ex.Message}", ex);
            }
        }

        // GET: /api/ExamTypes/search?query={searchQuery}
        public async Task<List<ExamType>> SearchExamTypesAsync(string searchQuery)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(searchQuery);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/ExamTypes/search?query={encodedQuery}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var ExamTypes = JsonSerializer.Deserialize<List<ExamType>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return ExamTypes ?? new List<ExamType>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tìm kiếm: {ex.Message}", ex);
            }
        }

        // GET: /api/ExamTypes/ExamType/{ExamType}
        public async Task<List<ExamType>> GetExamTypesByExamTypeAsync(string ExamType)
        {
            try
            {
                var encodedExamType = Uri.EscapeDataString(ExamType);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/ExamTypes/ExamType/{encodedExamType}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var ExamTypes = JsonSerializer.Deserialize<List<ExamType>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return ExamTypes ?? new List<ExamType>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi lấy danh sách theo phòng ban: {ex.Message}", ex);
            }
        }

        // Dispose HttpClient
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
