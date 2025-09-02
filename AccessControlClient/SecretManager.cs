using System;
using System.IO;
using System.Text.Json;

namespace AccessControlClient
{
    internal class SecretData
    {
        public string? Url { get; set; }
        public string? Secret { get; set; }
    }

    internal static class SecretManager
    {
        private static readonly string FilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static SecretData Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    var data = JsonSerializer.Deserialize<SecretData>(json);
                    if (data != null) return data;
                }
            }
            catch { }
            return new SecretData();
        }

        public static void Save(string? url, string? secret)
        {
            try
            {
                var data = new SecretData { Url = url, Secret = secret };
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }
    }
}
