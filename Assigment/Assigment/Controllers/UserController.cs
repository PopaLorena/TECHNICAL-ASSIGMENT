using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Assigment.Controllers
{
    /// <summary>
    /// Web API Controller for user authentication and registration.
    /// </summary>
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor of the Controller
        /// </summary>
        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Login the user and generate an authentication token.
        /// </summary>
        /// <param name="loginUserDto">Login details (email and password) of the user.</param>
        /// <returns>The authentication token if login is successful.</returns>
        /// <response code="200">Successfully logged in and returned token.</response>
        /// <response code="400">Invalid login credentials.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("login")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Login the user and generate an authentication token.")]
        [ProducesResponseType(typeof(string), 200)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
        {
            try
            {
                var user = mapper.Map<UserModel>(loginUserDto);
                var token = await userService.Login(user).ConfigureAwait(false);
                return Ok(token);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="CreateUserDto">The user details to be registered (email, password, etc.).</param>
        /// <returns>The newly registered user's information.</returns>
        /// <response code="200">Successfully registered the user.</response>
        /// <response code="400">Email already exists or invalid data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Register a new user.")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)]
        public async Task<ActionResult> Register(CreateUserDto CreateUserDto)
        {
            try
            {
                var user = mapper.Map<UserModel>(CreateUserDto);
                user = await userService.Register(user).ConfigureAwait(false);
                var userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (DbUpdateException)
            {
                return BadRequest("The email is used by another user");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
