using IdentityServer.AuthServer.Models;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Repositories.Abstract
{
    public interface ICustomUserRepository
    {
        Task<bool> Validate(string email, string password);

        Task<CustomUser> FindById(int Id);
        Task<CustomUser> FindByEmail(string email);
    }
}
