using System.Text.Json;
using Cashier.Common;
using Cashier.Models;

namespace Cashier.Services
{
    public class HuyaOrderService
    {
        readonly string _baseUrl = "http://www.dayunfanc.com:3000";
        public async Task<HuyaOrderResponse?> CreateOrderAsync(HuyaPaymentRequest request)
        {
            var resp = await HttpHelper.PostJsonAsync($"{_baseUrl}/initiate-payment", request);

            if (string.IsNullOrEmpty(resp))
            {
                return null;
            }
            var result = JsonSerializer.Deserialize<HuyaOrderResponse>(resp);
            return result;
        }

        public async Task<GetOrderResponse?> GetOrderAsync(string orderId)
        {
            var resp = await HttpHelper.GetAsync($"{_baseUrl}/check-payment-status?id={orderId}");

            if (string.IsNullOrEmpty(resp))
            {
                return null;
            }

            var result = JsonSerializer.Deserialize<GetOrderResponse>(resp);
            return result;
        }
    }
}
