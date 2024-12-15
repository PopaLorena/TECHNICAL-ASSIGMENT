using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    /// <inheritdoc/>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the database for user-related operations.</param>
        /// <param name="mapper">The object mapper used to map between user entities and DTOs.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="context"/> or <paramref name="mapper"/> is <c>null</c>.</exception>
        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<UserModel?> GetUserById(string userId)
        {
            var userIdGuid = new Guid(userId);
            var userDao = await context.Users.Include(u => u.Party).FirstOrDefaultAsync(x => x.Id == userIdGuid).ConfigureAwait(false);
            return mapper.Map<UserModel>(userDao);
        }

        /// <inheritdoc/>
        public async Task<UserModel> GetUserByEmail(string email)
        {
            var userDao = await context.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
            return mapper.Map<UserModel>(userDao);
        }

        /// <inheritdoc/>
        public async Task<UserModel> Register(UserDao userDao)
        {
            await context.Users.AddAsync(userDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return mapper.Map<UserModel>(userDao);
        }
    }
}
