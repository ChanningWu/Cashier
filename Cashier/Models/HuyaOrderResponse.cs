using System.Text.Json.Serialization;

namespace Cashier.Models
{
    public class HuyaOrderResponse
    {
        [JsonPropertyName("paymentUrl")]
        public string PaymentUrl { get; set; }

        [JsonPropertyName("huyaOrderId")]
        public string HuyaOrderId { get; set; }
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("pendingId")]
        public string PendingId { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;

        [JsonPropertyName("errorMsg")]
        public string ErrorMsg { get; set; }
    }
}
