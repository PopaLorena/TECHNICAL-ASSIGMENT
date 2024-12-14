using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<string> Login(UserModel user);
        Task Register(UserModel user);
    }
}
