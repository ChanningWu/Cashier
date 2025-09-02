using Cashier.Models;
using Microsoft.EntityFrameworkCore;

namespace Cashier.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<HuyaPaymentRequest> PaymentRequests { get; set; }
    }
}
