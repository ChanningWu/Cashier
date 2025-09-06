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

        // ʱ�����
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

            // ʱ�����
            if (StartDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= StartDate.Value);
            }
            if (EndDate.HasValue)
            {
                // ����ʱ���������
                var end = EndDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.CreatedAt <= end);
            }

            Orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
