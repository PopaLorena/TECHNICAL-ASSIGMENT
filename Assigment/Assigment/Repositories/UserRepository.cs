using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {

            var userDao = await context.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
            return mapper.Map<UserModel>(userDao);
        }
        
        public async Task<UserModel> Register(UserDao userDao)
        {
            await context.Users.AddAsync(userDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return mapper.Map<UserModel>(userDao);
        }
    }
}
