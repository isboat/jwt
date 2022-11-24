using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jwt_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public TokenController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Generate()
        {
            var token = _jwtService.GenerateToken();    
            return new JsonResult(token);
        }

        [HttpPost]
        [Route("validatetoken")]
        public IActionResult ValidateToken([FromBody] DataModel dataModel)
        {
            try
            {
                _jwtService.ValidateToken(dataModel.Token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("readtoken")]
        public IActionResult ReadToken([FromBody] string token)
        {
            try
            {
                var jwt = _jwtService.ReadToken(token);
                return new JsonResult(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
