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
    public class DoctorService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public DoctorService(ApiConfig apiConfig = null)
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

        // GET: /api/Doctors
        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/GetListDoctor");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Doctor>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<Doctor>();
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

        // GET: /api/Doctors/{id}
        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Doctors/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Doctor>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new Doctor();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
        }

        // POST: /api/Doctors
        public async Task<Doctor> CreateDoctorAsync(Doctor Doctor)
        {
            try
            {
                var json = JsonSerializer.Serialize(Doctor);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PostAsync($"{_apiConfig.BaseUrl}/api/Doctors", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var createdDoctor = JsonSerializer.Deserialize<Doctor>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return createdDoctor;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tạo nhân viên: {ex.Message}", ex);
            }
        }

        // PUT: /api/Doctors/{id}
        public async Task<Doctor> UpdateDoctorAsync(int id, Doctor Doctor)
        {
            try
            {
                var json = JsonSerializer.Serialize(Doctor);
                var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

                var response = await _httpClient.PutAsync($"{_apiConfig.BaseUrl}/api/Doctors/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedDoctor = JsonSerializer.Deserialize<Doctor>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return updatedDoctor;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        // DELETE: /api/Doctors/{id}
        public async Task<bool> DeleteDoctorAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiConfig.BaseUrl}/api/Doctors/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi xóa nhân viên: {ex.Message}", ex);
            }
        }

        // GET: /api/Doctors/search?query={searchQuery}
        public async Task<List<Doctor>> SearchDoctorsAsync(string searchQuery)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(searchQuery);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Doctors/search?query={encodedQuery}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var Doctors = JsonSerializer.Deserialize<List<Doctor>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Doctors ?? new List<Doctor>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi tìm kiếm: {ex.Message}", ex);
            }
        }

        // GET: /api/Doctors/Doctor/{Doctor}
        public async Task<List<Doctor>> GetDoctorsByDoctorAsync(string Doctor)
        {
            try
            {
                var encodedDoctor = Uri.EscapeDataString(Doctor);
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Doctors/Doctor/{encodedDoctor}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var Doctors = JsonSerializer.Deserialize<List<Doctor>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Doctors ?? new List<Doctor>();
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
