using DentalSys.Api.Common.Interfaces;
using DentalSys.Api.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DentalSys.Api.Features.Identity
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        Task<User?> GetUsernameAsync(string username);
        Task<bool> UserExistAsync(string username);

    }
    public class UserRepository(DentalDbContext context) : IUserRepository
    {
        //public async Task<User?> AuthenticateAsync(string username, string password)
        //{
        //    var user = await GetAsync(u => u.Username == username && u.Password == password);
        //    return user;
        //}

        public async Task<Guid> CreateAsync(User entity)
        {
            await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();   

            return entity.UserId;
        }

        public Task<int> DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>?> GetAllAsync(Expression<Func<User, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users.SingleOrDefaultAsync(predicate);
        }

        public async Task<User?> GetUsernameAsync(string username)
        {
            return await GetAsync(u => u.Username == username);
        }

        public Task<int> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExistAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }
    }
}
