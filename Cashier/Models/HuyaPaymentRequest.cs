using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cashier.Models
{
    [Table("HuyaPaymentRequest")]
    public class HuyaPaymentRequest
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = "alipay";

        [JsonPropertyName("view")]
        public string View { get; set; } = "d";

        [JsonPropertyName("user")]
        public string? User { get; set; }

        [JsonPropertyName("merchant_order_id"), Key]
        public string? MerchantOrderId { get; set; }

        [JsonPropertyName("notify_url")]
        public string? NotifyUrl { get; set; }

        [JsonPropertyName("clientIp")]
        public string? ClientIp { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; internal set; }
        [JsonIgnore]
        public string Status { get; internal set; }
        [JsonIgnore]
        public string Message { get; internal set; }
    }
}
