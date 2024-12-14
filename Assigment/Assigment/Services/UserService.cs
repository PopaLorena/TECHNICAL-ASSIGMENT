using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Assigment.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public UserService(IConfiguration configuration, IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> Login(UserModel user)
        {
            var userDao = await userRepository.GetUserByEmail(user.Email).ConfigureAwait(false);

            if (userDao == null)
                throw new InvalidOperationException("Wrong credentials");

            if (!VerifyPasswordHask(user.Password, userDao.PasswordHash!, userDao.PasswordSalt!))
            {
                throw new InvalidOperationException("Wrong credentials");
            }

            return CreateToken(user);
        }

        public async Task<UserModel> Register(UserModel user)
        {
            CreatePassworHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            UserDao userDao = new()
            {
                Name = user.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = user.Email,
                PartyId = user.PartyId
            };
            return await userRepository.Register(userDao).ConfigureAwait(false);
        }

        private string CreateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static void CreatePassworHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHask(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
