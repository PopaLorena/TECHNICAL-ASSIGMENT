using Assigment.Models;
using Assigment.ModelsDao;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<UserModel?> GetUserByEmail(string email);
        Task<UserModel> Register(UserDao userDao);
    }
}
