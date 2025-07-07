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
    public class ZoneService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public ZoneService(ApiConfig apiConfig = null)
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

        // GET: /api/GetZonesAsync
        public async Task<List<Zone>> GetZonesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/ZoneControllercs/GetListZone");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Zone>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<Zone>();
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

        // GET: /api/Zones/{id}
        public async Task<List<Zone>> GetZoneByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Zone/GetZoneByDepartMentScheduling?id={id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Zone>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<Zone>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
        }

        //// POST: /api/Zones
        //public async Task<Zone> CreateZoneAsync(Zone Zone)
        //{
        //    try
        //    {
        //        var json = JsonSerializer.Serialize(Zone);
        //        var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

        //        var response = await _httpClient.PostAsync($"{_apiConfig.BaseUrl}/api/Zones", content);
        //        response.EnsureSuccessStatusCode();

        //        var responseJson = await response.Content.ReadAsStringAsync();
        //        var createdZone = JsonSerializer.Deserialize<Zone>(responseJson, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return createdZone;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi tạo nhân viên: {ex.Message}", ex);
        //    }
        //}

        //// PUT: /api/Zones/{id}
        //public async Task<Zone> UpdateZoneAsync(int id, Zone Zone)
        //{
        //    try
        //    {
        //        var json = JsonSerializer.Serialize(Zone);
        //        var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

        //        var response = await _httpClient.PutAsync($"{_apiConfig.BaseUrl}/api/Zones/{id}", content);
        //        response.EnsureSuccessStatusCode();

        //        var responseJson = await response.Content.ReadAsStringAsync();
        //        var updatedZone = JsonSerializer.Deserialize<Zone>(responseJson, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return updatedZone;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
        //    }
        //}

        //// DELETE: /api/Zones/{id}
        //public async Task<bool> DeleteZoneAsync(int id)
        //{
        //    try
        //    {
        //        var response = await _httpClient.DeleteAsync($"{_apiConfig.BaseUrl}/api/Zones/{id}");
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi xóa nhân viên: {ex.Message}", ex);
        //    }
        //}

        //// GET: /api/Zones/search?query={searchQuery}
        //public async Task<List<Zone>> SearchZonesAsync(string searchQuery)
        //{
        //    try
        //    {
        //        var encodedQuery = Uri.EscapeDataString(searchQuery);
        //        var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Zones/search?query={encodedQuery}");
        //        response.EnsureSuccessStatusCode();

        //        var json = await response.Content.ReadAsStringAsync();
        //        var Zones = JsonSerializer.Deserialize<List<Zone>>(json, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return Zones ?? new List<Zone>();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi tìm kiếm: {ex.Message}", ex);
        //    }
        //}

        //// GET: /api/Zones/department/{department}
        //public async Task<List<Zone>> GetZonesByDepartmentAsync(string department)
        //{
        //    try
        //    {
        //        var encodedDepartment = Uri.EscapeDataString(department);
        //        var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/Zones/department/{encodedDepartment}");
        //        response.EnsureSuccessStatusCode();

        //        var json = await response.Content.ReadAsStringAsync();
        //        var Zones = JsonSerializer.Deserialize<List<Zone>>(json, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return Zones ?? new List<Zone>();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi lấy danh sách theo phòng ban: {ex.Message}", ex);
        //    }
        //}

        // Dispose HttpClient
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
