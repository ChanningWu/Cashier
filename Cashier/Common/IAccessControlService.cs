namespace Cashier.Common
{
    public interface IAccessControlService
    {
        bool IsOpen { get; }
        DateTime? ExpireAt { get; }
        string CurrentSecret { get; }
        bool SecretInitialized { get; }

        void OpenForDays(int days, string secret);
        void Close(string secret);
        string RefreshSecret(string oldSecret);
        bool ValidateSecret(string? secret);
    }
}
