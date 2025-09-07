using System;
using Cashier.Common;
using Cashier.Data;
using Cashier.Models;
using Cashier.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel.ProjectLockFile;

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
                LogHelper.Warn($"Cashier: 订单号:{orderId} 不存在");
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


            // 调用网关下单之前先查询订单是否存在
            var orderInService = await _huyaOrderService.GetOrderAsync(orderId);

            if (orderInService != null && orderInService.Status != "not_found")
            {
                LogHelper.Warn($"Cashier: 订单号:{orderId} 已存在于支付网关, 无需重复创建");
                return BadRequest("创建支付订单失败, 已存在相同单号的支付订单");
            }
            var response = await _huyaOrderService.CreateOrderAsync(Order);

            if (response == null)
            {
                LogHelper.Warn($"Cashier: 订单号:{orderId} 创建支付订单失败");
                return BadRequest("创建支付订单失败");
            }
            // 跳转到真实支付页面
            LogHelper.Info($"Cashier: 订单号:{orderId} 创建支付订单成功. 跳转到支付页面, payUrl:{response.PaymentUrl}");
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
            _db.PaymentRequests.Add(order);
            await _db.SaveChangesAsync();
            LogHelper.Info($"Cashier: 订单号:{order.MerchantOrderId} 创建成功.");

            // 调用网关下单
            var response = await _huyaOrderService.CreateOrderAsync(order);

            if (response == null || string.IsNullOrEmpty(response.PaymentUrl))
            {
                LogHelper.Warn($"Cashier: 订单号:{order.MerchantOrderId} 创建支付订单失败");
                return BadRequest("创建支付订单失败");
            }

            // 直接跳转到支付页面
            LogHelper.Info($"Cashier: 订单号:{order.MerchantOrderId} 创建支付订单成功 跳转到支付页面. payUrl:{response.PaymentUrl}");
            return Redirect(response.PaymentUrl);
        }
    }
}
