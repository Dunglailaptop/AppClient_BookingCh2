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
    public class DepartMentAppointSchedulingService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public DepartMentAppointSchedulingService(ApiConfig apiConfig = null)
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

        // GET: /api/DepartMentAppointSchedulings
        public async Task<List<DepartMentAppointScheduling>> GetDepartMentAppointSchedulingsAsync(int Year, int Week, int ZoneId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/DepartmentalAppointmentScheduling/GetListDepartmentalAppointmentScheduling?Week={Week}&Year={Year}&ZoneId={ZoneId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<DepartMentAppointScheduling>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<DepartMentAppointScheduling>();
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

        // GET: /api/DepartMentAppointSchedulings/{id}
        public async Task<DepartMentAppointScheduling> GetDepartMentAppointSchedulingByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/DepartMentAppointSchedulings/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<DepartMentAppointScheduling>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new DepartMentAppointScheduling();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
        }

        // POST: /api/DepartMentAppointSchedulings
        public async Task<bool> CreateDepartMentAppointSchedulingAsync(List<DepartMentAppointScheduling> departMentAppointScheduling)
        {
            try
            {
                var json = JsonSerializer.Serialize(departMentAppointScheduling);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);
                var response = await _httpClient.PostAsync($"{_apiConfig.BaseUrl}/api/DepartmentalAppointmentScheduling/CreateListDepartmentalAppointmentScheduling", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();

                //var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<DepartMentAppointScheduling>>>(responseJson, new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true
                //});
                if (responseJson != null) {
                    return true;
                }
                return false;
                
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tạo lịch hẹn khoa: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Lỗi parse JSON response: {ex.Message}", ex);
            }
        }

        // PUT: /api/DepartMentAppointSchedulings/{id}
        public async Task<DepartMentAppointScheduling> UpdateDepartMentAppointSchedulingAsync(int id, DepartMentAppointScheduling DepartMentAppointScheduling)
        {
            try
            {
                var json = JsonSerializer.Serialize(DepartMentAppointScheduling);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PutAsync($"{_apiConfig.BaseUrl}/api/DepartMentAppointSchedulings/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedDepartMentAppointScheduling = JsonSerializer.Deserialize<DepartMentAppointScheduling>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return updatedDepartMentAppointScheduling;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        // DELETE: /api/DepartMentAppointSchedulings/{id}
        public async Task<bool> DeleteDepartMentAppointSchedulingAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiConfig.BaseUrl}/api/DepartMentAppointSchedulings/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi xóa nhân viên: {ex.Message}", ex);
            }
        }

        // GET: /api/DepartMentAppointSchedulings/search?query={searchQuery}
        public async Task<List<DepartMentAppointScheduling>> SearchDepartMentAppointSchedulingsAsync(string searchQuery)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(searchQuery);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/DepartMentAppointSchedulings/search?query={encodedQuery}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var DepartMentAppointSchedulings = JsonSerializer.Deserialize<List<DepartMentAppointScheduling>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return DepartMentAppointSchedulings ?? new List<DepartMentAppointScheduling>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tìm kiếm: {ex.Message}", ex);
            }
        }

        // GET: /api/DepartMentAppointSchedulings/department/{department}
        public async Task<List<DepartMentAppointScheduling>> GetDepartMentAppointSchedulingsByDepartmentAsync(string department)
        {
            try
            {
                var encodedDepartment = Uri.EscapeDataString(department);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/DepartMentAppointSchedulings/department/{encodedDepartment}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var DepartMentAppointSchedulings = JsonSerializer.Deserialize<List<DepartMentAppointScheduling>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return DepartMentAppointSchedulings ?? new List<DepartMentAppointScheduling>();
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
