using System;
using Cashier.Data;
using Cashier.Models;
using Microsoft.AspNetCore.Mvc;

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
                return BadRequest("Invalid request data.");
            }
            // 生成唯一订单号
            request.MerchantOrderId = string.IsNullOrEmpty(request.MerchantOrderId) ? Guid.NewGuid().ToString("N") : request.MerchantOrderId;
            request.CreatedAt = DateTime.Now;
            request.Status = "Pending";
            _db.PaymentRequests.Add(request);
            _db.SaveChanges();

            var cashierUrl = $"{Request.Scheme}://{Request.Host}/Cashier?orderId={request.MerchantOrderId}";

            return Ok(new { paymentUrl = cashierUrl, orderId = request.MerchantOrderId });
        }
    }
}
