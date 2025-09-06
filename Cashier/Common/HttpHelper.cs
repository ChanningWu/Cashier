using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cashier.Models;

namespace Cashier.Common
{
    public static class HttpHelper
    {
        private static readonly HttpClient _httpClient;

        static HttpHelper()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30) // 默认超时10秒
            };
        }

        /// <summary>
        /// GET 请求
        /// </summary>
        public static async Task<string> GetAsync(string url, Dictionary<string, string>? headers = null)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"[GET Error] {ex.Message}";
            }
        }

        /// <summary>
        /// POST 请求 - JSON
        /// </summary>
        public static async Task<string> PostJsonAsync(string url, string json, Dictionary<string, string>? headers = null)
        {
            try
            {
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"[POST JSON Error] {ex.Message}";
            }
        }

        /// <summary>
        /// POST 请求 - 表单
        /// </summary>
        public static async Task<string> PostFormAsync(string url, Dictionary<string, string> formData, Dictionary<string, string>? headers = null)
        {
            try
            {
                using var content = new FormUrlEncodedContent(formData);
                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                AddHeaders(request, headers);

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"[POST Form Error] {ex.Message}";
            }
        }

        /// <summary>
        /// 添加请求头
        /// </summary>
        private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
        {
            if (headers == null) return;
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        public static async Task<string> PostJsonAsync<T>(
    string url, T body,
    Dictionary<string, string>? headers = null,
    JsonSerializerOptions? options = null)
        {
            try
            {
                options ??= new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(body, options);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
                AddHeaders(request, headers);
                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new HuyaOrderResponse { Success = false, ErrorMsg = ex.Message });
            }
            
        }
    }
}
