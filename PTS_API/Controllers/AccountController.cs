using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PTS_API.Authentication;
using PTS_API.GateWay.Email;
using PTS_API.Helper;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.Email;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJWTAuthentication _jwtAuth;
        private readonly IEmailSender _emailSender;
        private readonly IFluentEmail _fluentEmail;
        public AccountController(IAccountService accountService, IJWTAuthentication jwtAuth, IEmailSender emailSender, IFluentEmail fluentEmail)
        {
            _accountService = accountService;
            _jwtAuth = jwtAuth;
            _emailSender = emailSender;
            _fluentEmail = fluentEmail;
        }


        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(CreateUserRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _accountService.CreateUserAccount(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations....! User sign up successfully"});
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(CreateRoleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _accountService.CreateRole(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...!, Role created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Signin")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            try
            {
                
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _accountService.Login(model);
                if (result.IsSuccessful == true)
                {
                    result.AccessToken = _jwtAuth.GenerateToken(result.ApplicationUserDto);
                    result.RefreshToken = _jwtAuth.GenerateRefreshToken();

                    //revoving the token from cache as invalid token
                  //  await _tokenInvalidationService.ValidateTokenAsync(result.AccessToken);

                    // Storing the token on server side for future requests
                    var accessToken = result.AccessToken;

                    Response.Cookies.Append($"pts_access_token", accessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Set to true if using HTTPS
                      //  SameSite = SameSiteMode.None, // Adjust as needed
                       // Expires = DateTimeOffset.UtcNow.AddHours(1), // Set expiration time
                       // Domain = "yourdomain.com", // Specify the domain if needed
                      //  Path = "/yourpath", // Specify the path if needed
                    });
                    var response = await _accountService.UpdateRefreshToken(result.ApplicationUserDto.UserId, result.RefreshToken);
                    if (response == true)
                        return Ok(result);
                    else BadRequest(new { Message = "there is a problem updating the refresh token" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateUserAccount(UserUpdateModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                //  model.ModifierName = User.FindFirst(ClaimTypes.Email).Value;
                //  model.ModifierId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = await _accountService.UpdateUserAccount(model);
                if (result == true)
                {
                    return Ok(new { Message = "user updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating user" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeactivateUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId)) { return BadRequest("User identity can't be null"); }
                var result = await _accountService.DeleteUserAccount(userId);
                if (result == true)
                {
                    return Ok(new { Message = "user Deleted successfully" });
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("ActivateUser/{userId}")]
        public async Task<IActionResult> ActivateUserAccount(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId)) { return BadRequest("User identity can't be null"); }
                var result = await _accountService.ActivateUserAccount(userId);
                if (result == true)
                {
                    return Ok(new { Message = "user activated successfully" });
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetUserByEmail/{mail}")]
        public async Task<IActionResult> GetUserByEmail(string mail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mail)) { return BadRequest("Email can't be null"); }
                var result = await _accountService.GetUserByEmail(mail);
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _accountService.GetUserById(id);
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                   var result = await _accountService.GetAllUsers();
                
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllDrivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var result = await _accountService.Drivers();

                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchUsers/{keyword}")]
        public async Task<IActionResult> SearchUsers(string? keyword= null)
        {
            try
            {
                var result = await _accountService.SearchUsers(keyword);

                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllDeactivatedUsers")]
        public async Task<IActionResult> GetAllDeactivatedUsers()
        {
            try
            {
                var result = await _accountService.GetAllDeactivatedUsers();
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var result = await _accountService.GetAllRoles();
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassword/{mail}")]
        public async Task<IActionResult> ForgotPassword(string mail)
        {
            if (!string.IsNullOrWhiteSpace(mail))
            {
                try
                {
                    var result = await _accountService.SendOTPForPasswordReset(mail);
                    if (result != null)
                    {
                        //sending the opt as mail to the user is the next step
                         var mailModel = new EmailRequestModel
                         {
                            SenderEmail = "tijaniadebayoabdullahi@gmail.com",
                             ReceiverEmail = result.Email,
                             ReceiverName = result.FirstName,
                             OTP = result.PasswordOTP,
                             Subject = "Password Resset",
                             Number = result.Phonenumber,
                             Message = FileReader.ReadFileForOTP(result.FirstName, result.PasswordOTP)
                          };
                        await _emailSender.SendEmail1(mailModel);
                        // await _emailSender.SendSMS(mailModel);
                         await _emailSender.SendEmail(mailModel);
                        
                        return Ok(new { Message = "Check your mail for OTP" });
                    }
                    else return BadRequest(new { Message = "error initiating the request" });
                }
                catch (Exception ex) { throw; }
            }
            else return BadRequest(new { Message = "user email can't be null" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetRequestModel model)
        {
            if (!ModelState.IsValid) { return BadRequest("Your datas are not valid, please check your inputs"); }
            if (model != null)
            {
                try
                {
                    var result = await _accountService.ResetPassword(model);
                    if (result.IsSuccess == true)
                        return Ok(result);
                    else return BadRequest(new { Message = "error initiating the request" });
                }
                catch (Exception ex) { throw; }
            }
            else return BadRequest(new { Message = "bad Request" });
        }

        /* [HttpGet]
         [Route("Logout")]
         public async Task<IActionResult> Logout()
         {
            // var result = await _tokenInvalidationService.IsTokenInvalidAsync(await _distributedCache.GetStringAsync("invalid"));
             //await _tokenInvalidationService.InvalidateTokenAsync(result);
             return Ok();
         }*/


        [HttpGet]
        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var result = await _accountService.Dashboard();
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
