using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using quiz_game.Utilites;
using quiz_game.Models.Commands;
using quiz_game.Repositories.Interfaces;
using quiz_game.Services.interfalces;
using quiz_game.Models.Events;

namespace quiz_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IGameService _gameService;

        public UserController(IUserRepository userRepository, IGameService gameService)
        {
            _userRepository = userRepository;
            _gameService = gameService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserCommands command)
        {
            try
            {
                var model = await _gameService.StartGame(command, Events.REGISTER_USER);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpPost]
        [Route("login")]
        public IActionResult LoginUser([FromBody] UserCommands command)
        {

            try
            {
                var model = _gameService.StartGame(command,Events.LOGIN_USER);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutCommand command)
        {

            try
            {
                await _gameService.OnDisconecntClient(command);

                 return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    
    }
}
