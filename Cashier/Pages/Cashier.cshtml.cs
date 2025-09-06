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


            // ���� �����µ�
            var response = await _huyaOrderService.CreateOrderAsync(Order);

            if (response == null || string.IsNullOrEmpty(response.PaymentUrl) || response.Success == false)
            {
                Order.Status = "Failed";
                Order.Message = response?.ErrorMsg ?? "��������ʧ��";
                _db.PaymentRequests.Update(Order);
                await _db.SaveChangesAsync();
                return NotFound("��������ʧ��");
            }

            Order.Status = "Successed";
            _db.PaymentRequests.Update(Order);
            await _db.SaveChangesAsync();
            // ��ת����ʵ֧��ҳ��
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

            // ���������µ�
            var response = await _huyaOrderService.CreateOrderAsync(order);

            if (response == null || string.IsNullOrEmpty(response.PaymentUrl) || response.Success == false)
            {
                order.Status = "Failed";
                order.Message = response?.ErrorMsg ?? "��������ʧ��";
                _db.PaymentRequests.Add(order);
                await _db.SaveChangesAsync();
                return BadRequest("��������ʧ��");
            }

            order.Status = "Successed";
            _db.PaymentRequests.Add(order);
            await _db.SaveChangesAsync();
            // ֱ����ת��֧��ҳ��
            return Redirect(response.PaymentUrl);
        }
    }
}
