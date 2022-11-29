using Microsoft.AspNetCore.Mvc;

namespace jwt_api.Controllers
{
    /*
     JSON Web Tokens are an open, industry standard RFC 7519 method for representing claims securely between two parties.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public TokenController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }


        [HttpGet]
        [Route("generatetoken")]
        public IActionResult GenerateToken()
        {
            var model = new UserData
            {
                Username = "user123@test.com",
                Id = "001",
                Name = "Foo bar"
            };

            //2. Generate token with user details
            var token = _jwtService.GenerateToken(model);    
            return new JsonResult(token);
        }

        [HttpPost]
        [Route("validatetoken")]
        public IActionResult ValidateToken([FromBody] DataModel model)
        {
            try
            {
                _jwtService.ValidateToken(model.Token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("readtoken")]
        public IActionResult ReadToken([FromBody] DataModel model)
        {
            try
            {
                var jwt = _jwtService.ReadToken(model.Token);
                return new JsonResult(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
