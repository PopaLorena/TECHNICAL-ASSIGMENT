
using Assigment.ModelsDao;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<UserDao> GetUserByEmail(string email);
        Task register(UserDao userDao);
    }
}
