using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Repositories
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _dbContext;

        public CustomUserRepository(CustomDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _dbContext.CustomUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _dbContext.FindAsync<CustomUser>(id);
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _dbContext.CustomUsers.AnyAsync(x => x.Email == email && x.Password == password);
        }
    }
}
