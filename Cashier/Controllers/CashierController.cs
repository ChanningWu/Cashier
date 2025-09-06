using System;
using Cashier.Common;
using Cashier.Data;
using Cashier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cashier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashierController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CashierController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("create")]
        public IActionResult CreateCashier([FromBody] HuyaPaymentRequest request)
        {
            if (request == null|| request.Amount <= 0)
            {
                LogHelper.Warn("CreateCashier: Invalid request data: Amount <= 0");
                return BadRequest("Invalid request data.");
            }
            // 生成唯一订单号
            request.MerchantOrderId = string.IsNullOrEmpty(request.MerchantOrderId) ? Guid.NewGuid().ToString("N") : request.MerchantOrderId;
            var cashierUrl = "";
            var orderInDb = _db.PaymentRequests.FirstOrDefault(x => x.MerchantOrderId == request.MerchantOrderId);
            if (orderInDb != null)
            {
                LogHelper.Warn($"CreateCashier: 订单号:{request.MerchantOrderId} 已存在, 下单时间：{orderInDb.CreatedAt:G}");
                cashierUrl = $"{Request.Scheme}://{Request.Host}/Cashier?orderId={request.MerchantOrderId}";
                return Ok(new { paymentUrl = cashierUrl, orderId = request.MerchantOrderId });
            }
            request.CreatedAt = DateTime.Now;
            _db.PaymentRequests.Add(request);
            _db.SaveChanges();

            LogHelper.Info($"CreateCashier: 订单号:{request.MerchantOrderId} 创建成功.");

            cashierUrl = $"{Request.Scheme}://{Request.Host}/Cashier?orderId={request.MerchantOrderId}";

            return Ok(new { paymentUrl = cashierUrl, orderId = request.MerchantOrderId });
        }
    }
}
