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

            // ע�����
            builder.Services.AddScoped<HuyaOrderService>();
            builder.Services.AddSingleton<IAccessControlService, AccessControlService>();
            var app = builder.Build();

            // ʹ���м��
            app.UseMiddleware<AccessControlMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();   // ���û�б����Զ�����������ֱ��ʹ��
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
