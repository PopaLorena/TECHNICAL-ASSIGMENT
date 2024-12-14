using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("login")]
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

        [HttpPost("register")]
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
