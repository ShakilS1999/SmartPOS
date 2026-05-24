namespace SmartPOS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task ChangePasswordAsync(string username, string oldPassword, string newPassword);
    }
}