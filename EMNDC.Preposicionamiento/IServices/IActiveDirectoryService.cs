using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Responses;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface IActiveDirectoryService
    {
        Task<ActiveDirectoryUserResponse> AuthenticateAsync(string username, string password);
        Task<ActiveDirectoryUserResponse> GetUserDetailsAsync(string username);
        Task<bool> ValidateUserInGroupAsync(string username, string groupName);
    }
}
