using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    /// <summary>
    /// User-related services, including login and registration operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Logs in a user by validating their credentials and generating a token.
        /// </summary>
        /// <param name="user">The user model containing login credentials.</param>
        /// <returns>A string token on success.</returns>
        Task<string> Login(UserModel user);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user model containing registration details.</param>
        /// <returns>The registered user model.</returns>
        Task<UserModel> Register(UserModel user);
    }

}
