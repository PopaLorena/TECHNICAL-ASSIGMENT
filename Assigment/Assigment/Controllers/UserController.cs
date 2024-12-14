using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> Login(UserDto userDto)
        {
            try
            {
                var user = mapper.Map<UserModel>(userDto);
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
        public async Task<ActionResult> Register(UserDto userDto)
        {

            try
            {
                var user = mapper.Map<UserModel>(userDto);
                await userService.Register(user).ConfigureAwait(false);
                return Created();
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
    }
}
