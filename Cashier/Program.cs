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

            // ========= 服务注册 =========
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<HuyaOrderService>();
            builder.Services.AddSingleton<IAccessControlService, AccessControlService>();

            // 注册执行器
            builder.Services.AddScoped<OrderTaskExecutor>();
            // 注册调度器（用业务执行器的 Execute 方法作为委托）
            builder.Services.AddSingleton<DelayedTaskScheduler>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                return new DelayedTaskScheduler(
                    delay: TimeSpan.FromSeconds(200),
                    maxQps: 5,
                    qpsWindow: TimeSpan.FromSeconds(10),
                    execute: async data =>
                    {
                        using var scope = scopeFactory.CreateScope();
                        var executor = scope.ServiceProvider.GetRequiredService<OrderTaskExecutor>();
                        await executor.Execute(data);
                    });
            });

            var app = builder.Build();

            // ========= 数据库初始化 =========
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();   // 确保数据库文件存在
                DbManager.EnsureDatabaseSynchronized(db); // 同步表和字段
            }

            // ========= 中间件 =========
            app.UseMiddleware<AccessControlMiddleware>();

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

            // ========= 路由 =========
            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}