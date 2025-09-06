using System.Text.Json.Serialization;

namespace Cashier.Models
{
    public class GetOrderResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("user")]
        public string User { get; set; }
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }
    }

}
