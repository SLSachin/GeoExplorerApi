using GeoExplorerApi.Dtos;
using GeoExplorerApi.Models;

namespace GeoExplorerApi.Interfaces
{

    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
        Task<User?> GetUser(string username, string password);
    }
}