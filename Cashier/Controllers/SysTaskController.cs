using Cashier.Common;
using Microsoft.AspNetCore.Mvc;

namespace Cashier.Controllers
{
    [ApiController]
    [Route("sys")]
    public class SysTaskController : ControllerBase
    {
        private readonly IAccessControlService _accessControl;

        public SysTaskController(IAccessControlService accessControl)
        {
            _accessControl = accessControl;
        }

        [HttpPost("task")]
        public IActionResult Open([FromBody] OpenRequest req)
        {
            try
            {
                _accessControl.OpenForDays(req.Days, req.Secret);
                return Ok(new { msg = $"Service opened for {req.Days} days", expire = _accessControl.ExpireAt });
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("task/stop")]
        public IActionResult Close([FromBody] CloseRequest req)
        {
            try
            {
                _accessControl.Close(req.Secret);
                return Ok(new { msg = "Service stopped" });
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("task/refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest req)
        {
            try
            {
                var newKey = _accessControl.RefreshSecret(req.Secret);
                return Ok(new { newSecret = newKey });
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpGet("metrics")]
        public IActionResult Status([FromHeader(Name = "X-Admin-Key")] string? key)
        {
            // 如果还没初始化，允许匿名访问并返回初始 secret
            if (!_accessControl.SecretInitialized)
            {
                return Ok(new
                {
                    open = _accessControl.IsOpen,
                    expire = _accessControl.ExpireAt,
                    secret = _accessControl.CurrentSecret,
                    note = "This is the initial secret. Future calls require this key."
                });
            }

            // 已初始化后，必须校验 secret
            if (!_accessControl.ValidateSecret(key))
                return Unauthorized();

            return Ok(new { open = _accessControl.IsOpen, expire = _accessControl.ExpireAt, timeLeft = (_accessControl.ExpireAt - DateTime.Now).ToChineseString() });
        }

    }

    public record OpenRequest(int Days, string Secret);
    public record CloseRequest(string Secret);
    public record RefreshRequest(string Secret);
}
