using Cashier.Data;
using Cashier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cashier.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _db;
        public List<HuyaPaymentRequest> Orders { get; set; } = new();

        // 时间过滤
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; } = DateTime.Today.AddDays(-7);

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; } = DateTime.Today;

        public OrdersModel(AppDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            if (!EndDate.HasValue)
            {
                EndDate = DateTime.Today;
            }

            var query = _db.PaymentRequests.AsQueryable();

            // 时间过滤
            if (StartDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= StartDate.Value);
            }
            if (EndDate.HasValue)
            {
                // 结束时间包含当天
                var end = EndDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.CreatedAt <= end);
            }

            Orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
#if DEBUG
            Orders.AddRange(new List<HuyaPaymentRequest>
            {
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST123456",
                    Amount = 100,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Successed",
                    Message = "支付成功",
                    CreatedAt = DateTime.Now.AddMinutes(-120),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
                new() {
                    MerchantOrderId = "TEST654321",
                    Amount = 200,
                    PaymentMethod = "alipay",
                    User = "testuser",
                    Status = "Pending",
                    Message = "等待支付",
                    CreatedAt = DateTime.Now.AddMinutes(-60),
                    ClientIp = "123.45.67.89",
                    NotifyUrl = "https://example.com/notify/xxxxxxxxxxxxxxxxxxxxxxxxxxx"
                    },
            });
#endif
        }
    }
}
