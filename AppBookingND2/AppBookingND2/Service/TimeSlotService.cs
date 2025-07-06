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
    public class TimeSlotService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public TimeSlotService(ApiConfig apiConfig = null)
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

        // GET: /api/GetTimeSlotsAsync
        public async Task<List<TimeSlot>> GetTimeSlotsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/TimeSlot/GetListTimeSlot");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<TimeSlot>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<TimeSlot>();
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

        // GET: /api/TimeSlots/{id}
        public async Task<List<TimeSlot>> GetTimeSlotByDepartmentAppointSchedulingIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/GetTimeSlotByDepartMentScheduling?id={id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                // Deserialize ApiResponse wrapper trước
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<TimeSlot>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả về data từ ApiResponse
                return apiResponse?.Data ?? new List<TimeSlot>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối API: {ex.Message}", ex);
            }
        }

        //// POST: /api/TimeSlots
        //public async Task<TimeSlot> CreateTimeSlotAsync(TimeSlot TimeSlot)
        //{
        //    try
        //    {
        //        var json = JsonSerializer.Serialize(TimeSlot);
        //        var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

        //        var response = await _httpClient.PostAsync($"{_apiConfig.BaseUrl}/api/TimeSlots", content);
        //        response.EnsureSuccessStatusCode();

        //        var responseJson = await response.Content.ReadAsStringAsync();
        //        var createdTimeSlot = JsonSerializer.Deserialize<TimeSlot>(responseJson, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return createdTimeSlot;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi tạo nhân viên: {ex.Message}", ex);
        //    }
        //}

        //// PUT: /api/TimeSlots/{id}
        //public async Task<TimeSlot> UpdateTimeSlotAsync(int id, TimeSlot TimeSlot)
        //{
        //    try
        //    {
        //        var json = JsonSerializer.Serialize(TimeSlot);
        //        var content = new StringContent(json, Encoding.UTF8, _apiConfig.ContentType);

        //        var response = await _httpClient.PutAsync($"{_apiConfig.BaseUrl}/api/TimeSlots/{id}", content);
        //        response.EnsureSuccessStatusCode();

        //        var responseJson = await response.Content.ReadAsStringAsync();
        //        var updatedTimeSlot = JsonSerializer.Deserialize<TimeSlot>(responseJson, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return updatedTimeSlot;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
        //    }
        //}

        //// DELETE: /api/TimeSlots/{id}
        //public async Task<bool> DeleteTimeSlotAsync(int id)
        //{
        //    try
        //    {
        //        var response = await _httpClient.DeleteAsync($"{_apiConfig.BaseUrl}/api/TimeSlots/{id}");
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi xóa nhân viên: {ex.Message}", ex);
        //    }
        //}

        //// GET: /api/TimeSlots/search?query={searchQuery}
        //public async Task<List<TimeSlot>> SearchTimeSlotsAsync(string searchQuery)
        //{
        //    try
        //    {
        //        var encodedQuery = Uri.EscapeDataString(searchQuery);
        //        var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/TimeSlots/search?query={encodedQuery}");
        //        response.EnsureSuccessStatusCode();

        //        var json = await response.Content.ReadAsStringAsync();
        //        var TimeSlots = JsonSerializer.Deserialize<List<TimeSlot>>(json, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return TimeSlots ?? new List<TimeSlot>();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        throw new Exception($"Lỗi tìm kiếm: {ex.Message}", ex);
        //    }
        //}

        //// GET: /api/TimeSlots/department/{department}
        //public async Task<List<TimeSlot>> GetTimeSlotsByDepartmentAsync(string department)
        //{
        //    try
        //    {
        //        var encodedDepartment = Uri.EscapeDataString(department);
        //        var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/api/TimeSlots/department/{encodedDepartment}");
        //        response.EnsureSuccessStatusCode();

        //        var json = await response.Content.ReadAsStringAsync();
        //        var TimeSlots = JsonSerializer.Deserialize<List<TimeSlot>>(json, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        return TimeSlots ?? new List<TimeSlot>();
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
