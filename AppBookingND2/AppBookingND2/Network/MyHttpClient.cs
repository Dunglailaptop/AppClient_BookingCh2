using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;

public class MyHttpClient
{
    private readonly HttpClient _httpClient;
    private bool _disposed = false;

    public MyHttpClient()
    {
        _httpClient = new HttpClient();
        string token = "ZygFCvbmNwFMm0aZD8FWtbGLhDb0rpZ_LsrOFCPlgF0e1Jqs9dMP0RjnnNEI6gBFlsLmZWYFyEmo83aChm3wg4LpeEGyUVN_-7OEY16qiaiPybE5giX_ZuWR2IjPYx5hd4LHKKQZx5_ewXzSAOrTKtjuajRY3-CXrgE_u6pP9WLliPiLHcu4XBtIQMWmua-sESUXH32Lm3eriLwg7gi_MA";
        // Gắn sẵn Bearer token vào Authorization header
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // Thêm header khác nếu cần
        _httpClient.DefaultRequestHeaders.Add("X-App-Version", "1.0.0");
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await _httpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
    {
        return await _httpClient.PostAsync(url, content);
    }

    public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
    {
        return await _httpClient.PostAsync(url, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        return await _httpClient.DeleteAsync(url);
    }

    // Triển khai IDisposable
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _httpClient.Dispose(); // Giải phóng HttpClient
            }

            _disposed = true;
        }
    }
    // Bạn có thể mở rộng thêm các method như PutAsync, DeleteAsync, v.v.
}
