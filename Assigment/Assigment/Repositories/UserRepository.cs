using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.ModelsDao;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserDao> GetUserByEmail(string email)
        {

            return await context.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
        }
        
        public async Task register(UserDao userDao)
        {
            await context.Users.AddAsync(userDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
