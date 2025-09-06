using System;
using Cashier.Data;
using Cashier.Models;
using Cashier.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cashier.Pages
{
    public class CashierModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly HuyaOrderService _huyaOrderService;

        public CashierModel(AppDbContext db, HuyaOrderService bService)
        {
            _db = db;
            _huyaOrderService = bService;
        }

        public HuyaPaymentRequest? Order { get; set; }
        public bool ShowCreate { get; set; }

        public async Task<IActionResult> OnGet(string orderId, string action)
        {
            ShowCreate = string.Equals(action, "create", StringComparison.OrdinalIgnoreCase);

            if (ShowCreate)
            {
                return Page();
            }

            Order = await _db.PaymentRequests.FirstOrDefaultAsync(o => o.MerchantOrderId == orderId);

            if (Order == null)
            {
                return NotFound("订单不存在");
            }

            // 获取真实 IP
            var realIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // 如果有代理，优先取 X-Forwarded-For
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                realIp = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault();
            }
            Order.ClientIp = realIp;


            // 调用 网关下单
            var response = await _huyaOrderService.CreateOrderAsync(Order);

            if (response == null || string.IsNullOrEmpty(response.PaymentUrl) || response.Success == false)
            {
                Order.Status = "Failed";
                Order.Message = response?.ErrorMsg ?? "创建订单失败";
                _db.PaymentRequests.Update(Order);
                await _db.SaveChangesAsync();
                return NotFound("创建订单失败");
            }

            Order.Status = "Successed";
            _db.PaymentRequests.Update(Order);
            await _db.SaveChangesAsync();
            // 跳转到真实支付页面
            return Redirect(response.PaymentUrl);
        }

        [BindProperty]
        public int Amount { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Amount <= 0) return BadRequest("金额无效");

            // 获取真实 IP
            var realIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // 如果有代理，优先取 X-Forwarded-For
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                realIp = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault();
            }

            var order = new HuyaPaymentRequest
            {
                Amount = Amount,
                MerchantOrderId = Guid.NewGuid().ToString("N"),
                CreatedAt = DateTime.Now,
                ClientIp = realIp,
            };

            // 调用网关下单
            var response = await _huyaOrderService.CreateOrderAsync(order);

            if (response == null || string.IsNullOrEmpty(response.PaymentUrl) || response.Success == false)
            {
                order.Status = "Failed";
                order.Message = response?.ErrorMsg ?? "创建订单失败";
                _db.PaymentRequests.Add(order);
                await _db.SaveChangesAsync();
                return BadRequest("创建订单失败");
            }

            order.Status = "Successed";
            _db.PaymentRequests.Add(order);
            await _db.SaveChangesAsync();
            // 直接跳转到支付页面
            return Redirect(response.PaymentUrl);
        }
    }
}
