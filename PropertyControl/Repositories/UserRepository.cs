using Microsoft.EntityFrameworkCore;
using PropertyControl.Databases;
using PropertyControl.Databases.Entities;

namespace PropertyControl.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAndPassword(string username, string password);
        Task<List<User>> GetAllUsers();
    }

    public class UserRepository(DataContext context) : IUserRepository
    {
        private readonly DataContext _context = context;

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}