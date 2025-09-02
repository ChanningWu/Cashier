namespace Cashier.Common
{
    using System.Security.Cryptography;
    public class AccessControlService : IAccessControlService
    {
        private readonly object _lock = new();
        private bool _isOpen;
        private DateTime? _expireAt;
        private string _secret;
        private bool _secretInitialized = false; // 标记是否已初始化

        public AccessControlService()
        {
            _secret = GenerateSecret();
        }

        public bool IsOpen
        {
            get
            {
                lock (_lock)
                {
                    if (_expireAt.HasValue && DateTime.UtcNow > _expireAt.Value)
                    {
                        _isOpen = false;
                    }
                    return _isOpen;
                }
            }
        }

        public DateTime? ExpireAt
        {
            get { lock (_lock) return _expireAt; }
        }

        public string CurrentSecret => _secret;

        public bool SecretInitialized => _secretInitialized;

        public void OpenForDays(int days, string secret)
        {
            if (!ValidateSecret(secret)) throw new UnauthorizedAccessException();
            lock (_lock)
            {
                _isOpen = true;
                _expireAt = DateTime.UtcNow.AddDays(days);
                _secretInitialized = true;
            }
        }

        public void Close(string secret)
        {
            if (!ValidateSecret(secret)) throw new UnauthorizedAccessException();
            lock (_lock)
            {
                _isOpen = false;
                _expireAt = null;
                _secretInitialized = true;
            }
        }

        public string RefreshSecret(string oldSecret)
        {
            if (!ValidateSecret(oldSecret)) throw new UnauthorizedAccessException();
            lock (_lock)
            {
                _secret = GenerateSecret();
                _secretInitialized = true;
                return _secret;
            }
        }

        public bool ValidateSecret(string? secret)
        {
            if (!_secretInitialized) return true; // 初始阶段允许匿名访问
            return !string.IsNullOrEmpty(secret) && secret == _secret;
        }

        private static string GenerateSecret()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(16)); // 32位HEX
        }
    }
}
