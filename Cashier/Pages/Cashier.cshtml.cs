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
                LogHelper.Warn($"Cashier: ������:{orderId} ������");
                return NotFound("����������");
            }

            // ��ȡ��ʵ IP
            var realIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // ����д�������ȡ X-Forwarded-For
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                realIp = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault();
            }
            Order.ClientIp = realIp;


            // ���������µ�֮ǰ�Ȳ�ѯ�����Ƿ����
            var orderInService = await _huyaOrderService.GetOrderAsync(orderId);

            if (orderInService != null && orderInService.Status != "not_found")
            {
                LogHelper.Warn($"Cashier: ������:{orderId} �Ѵ�����֧������, �����ظ�����");
                return BadRequest("����֧������ʧ��, �Ѵ�����ͬ���ŵ�֧������");
            }
            var response = await _huyaOrderService.CreateOrderAsync(Order);

            if (response == null)
            {
                LogHelper.Warn($"Cashier: ������:{orderId} ����֧������ʧ��");
                return BadRequest("����֧������ʧ��");
            }
            // ��ת����ʵ֧��ҳ��
            LogHelper.Info($"Cashier: ������:{orderId} ����֧�������ɹ�. ��ת��֧��ҳ��, payUrl:{response.PaymentUrl}");
            return Redirect(response.PaymentUrl);
        }

        [BindProperty]
        public int Amount { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Amount <= 0) return BadRequest("�����Ч");

            // ��ȡ��ʵ IP
            var realIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // ����д�������ȡ X-Forwarded-For
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
            LogHelper.Info($"Cashier: ������:{order.MerchantOrderId} �����ɹ�.");

            // ���������µ�
            var response = await _huyaOrderService.CreateOrderAsync(order);

            if (response == null || string.IsNullOrEmpty(response.PaymentUrl))
            {
                LogHelper.Warn($"Cashier: ������:{order.MerchantOrderId} ����֧������ʧ��");
                return BadRequest("����֧������ʧ��");
            }

            // ֱ����ת��֧��ҳ��
            LogHelper.Info($"Cashier: ������:{order.MerchantOrderId} ����֧�������ɹ� ��ת��֧��ҳ��. payUrl:{response.PaymentUrl}");
            return Redirect(response.PaymentUrl);
        }
    }
}
