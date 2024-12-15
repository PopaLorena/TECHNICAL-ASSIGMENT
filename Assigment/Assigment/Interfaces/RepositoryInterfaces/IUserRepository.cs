using Assigment.Models;
using Assigment.ModelsDao;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    /// <summary>
    /// Handling user data interactions with the repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The user model as the result.</returns>
        Task<UserModel> GetUserByEmail(string email);

        /// <summary>
        /// Retrieves a user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user model as the result. 
        /// Returns null if no user is found with the given ID.</returns>
        Task<UserModel?> GetUserById(string userId);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userDao">The user data to register.</param>
        /// <returns>The registered user model as the result.</returns>
        Task<UserModel> Register(UserDao userDao);
    }

}
