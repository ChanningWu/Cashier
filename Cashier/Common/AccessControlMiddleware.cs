namespace Cashier.Common
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class AccessControlMiddleware
    {
        private readonly RequestDelegate _next;

        public AccessControlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAccessControlService accessControl)
        {
#if DEBUG
            // 开发环境不拦截
            await _next(context);
            return;
#endif
            var path = context.Request.Path.Value?.ToLower();

            // sys 接口不拦截
            if (path != null && path.StartsWith("/sys"))
            {
                await _next(context);
                return;
            }

            if (!accessControl.IsOpen)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service is temporarily unavailable.");
                return;
            }

            await _next(context);
        }
    }

}
