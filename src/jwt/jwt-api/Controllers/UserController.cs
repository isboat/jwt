using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace jwt_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public UserController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserModel model)
        {
            if (string.IsNullOrEmpty(model?.Username) || string.IsNullOrEmpty(model?.Password))
            {
                return BadRequest();
            }

            //1. userService checks
            var user = _userService.Get(model);

            //2. Generate token with user details
            var token = _jwtService.GenerateToken(user);    
            return new JsonResult(token);
        }

        [HttpPost]
        [Route("validatetoken")]
        [AllowAnonymous]
        public IActionResult ValidateToken()
        {
            try
            {
                var bearerExist = Request.Headers.TryGetValue(HeaderNames.Authorization, out var auth);
                if (!bearerExist ||string.IsNullOrEmpty(auth))
                {
                    return BadRequest();
                }

                var bearer = auth.ToString().Replace("Bearer ", "");

                _jwtService.ValidateToken(bearer);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("readtoken")]
        public IActionResult ReadToken()
        {
            try
            {
                var bearerExist = Request.Headers.TryGetValue(HeaderNames.Authorization, out var auth);
                if (!bearerExist || string.IsNullOrEmpty(auth))
                {
                    return BadRequest();
                }

                var bearer = auth.ToString().Replace("Bearer ", "");
                var jwt = _jwtService.ReadToken(bearer);
                return new JsonResult(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
