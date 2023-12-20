using Microsoft.AspNetCore.Mvc;
using PTS_API.Authentication;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;

namespace PTS_API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJWTAuthentication _jwtAuth;

        public AccountController(IAccountService accountService, IJWTAuthentication jwtAuth)
        {
           _accountService = accountService;
            _jwtAuth = jwtAuth;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Provided neccessary credentials, please check your inputs"); }
                var result = await _accountService.Login(model);
                if(result.IsSuccessful == true)
                {
                    result.AccessToken = _jwtAuth.GenerateToken(result.ApplicationUserDto);
                    result.RefreshToken = _jwtAuth.GenerateRefreshToken();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       

      }

}
