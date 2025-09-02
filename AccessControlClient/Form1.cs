using System.Net.Http;
using System.Net.Http.Json;

namespace AccessControlClient
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _http = new();
        private string _baseUrl = "";
        private string? _secret;


        public Form1()
        {
            InitializeComponent();

            var data = SecretManager.Load();
            if (!string.IsNullOrEmpty(data.Url))
            {
                _baseUrl = data.Url;
                txtBaseUrl.Text = _baseUrl;
            }
            if (!string.IsNullOrEmpty(data.Secret))
            {
                _secret = data.Secret;
                txtSecret.Text = _secret;
            }
        }

        private async void btnStatus_Click(object sender, EventArgs e)
        {
            try
            {
                _baseUrl = txtBaseUrl.Text.Trim().TrimEnd('/');
                var req = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/sys/metrics");
                if (!string.IsNullOrEmpty(_secret))
                    req.Headers.Add("X-Admin-Key", _secret);

                var resp = await _http.SendAsync(req);
                var txt = await resp.Content.ReadAsStringAsync();

                txtResult.Text = txt;

                if (resp.IsSuccessStatusCode)
                {
                    try
                    {
                        var json = System.Text.Json.JsonDocument.Parse(txt);
                        if (json.RootElement.TryGetProperty("secret", out var sec))
                        {
                            _secret = sec.GetString();
                            if (!string.IsNullOrEmpty(_secret))
                            {
                                txtSecret.Text = _secret;
                                _baseUrl = txtBaseUrl.Text.Trim().TrimEnd('/');
                                SecretManager.Save(_baseUrl, _secret); // ✅ 保存URL和Secret
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        private async void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                var days = int.Parse(txtDays.Text);
                var resp = await _http.PostAsJsonAsync($"{txtBaseUrl.Text}/sys/task",
                    new { Days = days, Secret = _secret });

                if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(_secret))
                {
                    if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(_secret))
                    {
                        _baseUrl = txtBaseUrl.Text.Trim().TrimEnd('/');
                        SecretManager.Save(_baseUrl, _secret);
                    }
                }

                txtResult.Text = await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        private async void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                var resp = await _http.PostAsJsonAsync($"{txtBaseUrl.Text}/sys/task/stop",
                    new { Secret = _secret });

                if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(_secret))
                {
                    if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(_secret))
                    {
                        _baseUrl = txtBaseUrl.Text.Trim().TrimEnd('/');
                        SecretManager.Save(_baseUrl, _secret);
                    }
                }

                txtResult.Text = await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var resp = await _http.PostAsJsonAsync($"{txtBaseUrl.Text}/sys/task/refresh",
                    new { Secret = _secret });

                try
                {
                    var txt = await resp.Content.ReadAsStringAsync();
                    txtResult.Text = txt;

                    if (resp.IsSuccessStatusCode)
                    {
                        var json = System.Text.Json.JsonDocument.Parse(txt);
                        if (json.RootElement.TryGetProperty("newSecret", out var sec))
                        {
                            _secret = sec.GetString();
                            if (!string.IsNullOrEmpty(_secret))
                            {
                                txtSecret.Text = _secret;
                                _baseUrl = txtBaseUrl.Text.Trim().TrimEnd('/');
                                SecretManager.Save(_baseUrl, _secret); // ✅ 更新文件
                            }
                        }
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }
    }
}
