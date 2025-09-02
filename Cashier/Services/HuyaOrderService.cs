using System.Text.Json;
using Cashier.Common;
using Cashier.Models;

namespace Cashier.Services
{
    public class HuyaOrderService
    {
        public async Task<HuyaOrderResponse?> CreateOrderAsync(HuyaPaymentRequest request)
        {
            var resp = await HttpHelper.PostJsonAsync("http://www.dayunfanc.com:3000/initiate-payment", request);

            var result = JsonSerializer.Deserialize<HuyaOrderResponse>(resp);
            return result;
        }
    }
}
