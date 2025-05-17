
using PropertyControl.Commons;
using PropertyControl.Databases.Entities;

namespace PropertyControl.Databases.InitDb
{
    public partial class DbInitializer
    {
        public async Task SeedDataDefault()
        {
            try
            {
                _logger.LogInformation("[DbInitializer] Seeding default data");

                if (_context.Users.Any())
                {
                    return;
                }

                Role role1 = new()
                {
                    Name = "Admin",
                };

                User user1 = new()
                {
                    Username = "admin",
                    Password = "Admin@123",
                    Role = role1,
                };

                await _context.Users.AddAsync(user1);

                await _context.SaveChangesAsync();
                _logger.LogInformation("[DbInitializer] Seeding default data completed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}