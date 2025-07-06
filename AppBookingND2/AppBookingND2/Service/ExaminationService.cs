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
    public class ExaminationService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public ExaminationService(ApiConfig apiConfig = null)
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

        // GET: /api/Examinations
        public async Task<List<Examination>> GetExaminationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/GetListExamination");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Examination>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<Examination>();
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

        // GET: /api/Examinations/{id}
        public async Task<Examination> GetExaminationByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Examinations/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Examination>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new Examination();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
        }

        // POST: /api/Examinations
        public async Task<Examination> CreateExaminationAsync(Examination Examination)
        {
            try
            {
                var json = JsonSerializer.Serialize(Examination);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PostAsync($"{_apiConfig.BaseUrl}/api/Examinations", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var createdExamination = JsonSerializer.Deserialize<Examination>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return createdExamination;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tạo nhân viên: {ex.Message}", ex);
            }
        }

        // PUT: /api/Examinations/{id}
        public async Task<Examination> UpdateExaminationAsync(int id, Examination Examination)
        {
            try
            {
                var json = JsonSerializer.Serialize(Examination);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PutAsync($"{_apiConfig.BaseUrl}/api/Examinations/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedExamination = JsonSerializer.Deserialize<Examination>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return updatedExamination;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        // DELETE: /api/Examinations/{id}
        public async Task<bool> DeleteExaminationAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiConfig.BaseUrl}/api/Examinations/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi xóa nhân viên: {ex.Message}", ex);
            }
        }

        // GET: /api/Examinations/search?query={searchQuery}
        public async Task<List<Examination>> SearchExaminationsAsync(string searchQuery)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(searchQuery);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Examinations/search?query={encodedQuery}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var Examinations = JsonSerializer.Deserialize<List<Examination>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Examinations ?? new List<Examination>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tìm kiếm: {ex.Message}", ex);
            }
        }

        // GET: /api/Examinations/Examination/{Examination}
        public async Task<List<Examination>> GetExaminationsByExaminationAsync(string Examination)
        {
            try
            {
                var encodedExamination = Uri.EscapeDataString(Examination);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Examinations/Examination/{encodedExamination}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var Examinations = JsonSerializer.Deserialize<List<Examination>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Examinations ?? new List<Examination>();
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
