using Cashier.Common;
using Cashier.Data;
using Cashier.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;


namespace Cashier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 注册服务
            builder.Services.AddScoped<HuyaOrderService>();
            builder.Services.AddSingleton<IAccessControlService, AccessControlService>();
            var app = builder.Build();

            // 使用中间件
            app.UseMiddleware<AccessControlMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();   // 如果没有表，就自动建表；已有则直接使用
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}
