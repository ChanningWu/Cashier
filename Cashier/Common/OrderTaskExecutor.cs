namespace Cashier.Common
{
    using Cashier.Data;
    using Cashier.Services;
    using Microsoft.Extensions.Logging;

    public interface ITaskExecutor
    {
        Task Execute(string data);
    }

    public class OrderTaskExecutor(AppDbContext db, HuyaOrderService huyaOrderService) : ITaskExecutor
    {
        private readonly AppDbContext _db = db;
        private readonly HuyaOrderService _huyaOrderService = huyaOrderService;

        public async Task Execute(string data)
        {
            // - 更新数据库状态
            // - 调用外部 API
            // - 写日志
            LogHelper.Info($"[OrderTaskExecutor] Executing scheduled task: {data}");

            var order = _db.PaymentRequests.FirstOrDefault(o => o.MerchantOrderId == data);

            if (order != null)
            {
                // 查询订单状态
                var orderInService = await _huyaOrderService.GetOrderAsync(data);

                if (orderInService != null)
                {
                    if (orderInService.Status == "success")
                    {
                        order.Status = "PaySuccess";
                    }
                    else
                    {
                        order.Status = "PayTimeout";
                    }

                    _db.PaymentRequests.Update(order);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }

}
