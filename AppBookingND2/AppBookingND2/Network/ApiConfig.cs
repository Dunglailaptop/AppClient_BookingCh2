using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Configuration;
using System.IO;
// 1. CONFIG - Cấu hình API
namespace AppBookingND2.Network
{
    public class ApiConfig
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string BearerToken { get; set; }
        public int TimeoutSeconds { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, string> CustomHeaders { get; set; }

        public ApiConfig()
        {
            // Giá trị mặc định
            BaseUrl = "https://localhost:7138";
            TimeoutSeconds = 30;
            ContentType = "application/json";
            CustomHeaders = new Dictionary<string, string>();
        }

        // Load từ app.config hoặc appsettings.json
        public static ApiConfig LoadFromConfig()
        {
            var config = new ApiConfig();

            try
            {
                // Đọc từ app.config
                config.BaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? config.BaseUrl;
                config.ApiKey = ConfigurationManager.AppSettings["ApiKey"];
                config.BearerToken = ConfigurationManager.AppSettings["BearerToken"];

                if (int.TryParse(ConfigurationManager.AppSettings["ApiTimeoutSeconds"], out int timeout))
                {
                    config.TimeoutSeconds = timeout;
                }

                config.ContentType = ConfigurationManager.AppSettings["ApiContentType"] ?? config.ContentType;

                // Load custom headers từ config
                var customHeadersConfig = ConfigurationManager.AppSettings["CustomHeaders"];
                if (!string.IsNullOrEmpty(customHeadersConfig))
                {
                    var headers = customHeadersConfig.Split(';');
                    foreach (var header in headers)
                    {
                        var parts = header.Split('=');
                        if (parts.Length == 2)
                        {
                            config.CustomHeaders[parts[0].Trim()] = parts[1].Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error hoặc fallback to default
                MessageBox.Show($"Lỗi đọc cấu hình API: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return config;
        }

        // Load từ JSON file
        public static ApiConfig LoadFromJsonFile(string filePath = "appsettings.json")
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var configData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                    var config = new ApiConfig();

                    if (configData.ContainsKey("ApiSettings"))
                    {
                        var apiSettings = JsonSerializer.Deserialize<ApiConfig>(configData["ApiSettings"].ToString());
                        return apiSettings;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đọc file cấu hình: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return new ApiConfig(); // Fallback to default
        }

        // Validate cấu hình
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(BaseUrl) &&
                   Uri.TryCreate(BaseUrl, UriKind.Absolute, out _) &&
                   TimeoutSeconds > 0;
        }
    }
}
