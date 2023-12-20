using Microsoft.AspNetCore.Identity;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_BUSINESS.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel)
        {
            if (loginRequestModel != null)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(loginRequestModel.Username);
                    if (user == null)
                    {
                        return new LoginResponseModel
                        {
                            Message = "No user Found 🙄",
                            IsSuccessful = false,
                        };
                    }
                    if (user.IsDeleted)
                    {
                        return new LoginResponseModel
                        {
                            Message = "You are not an active user of the Application",
                            IsSuccessful = false,
                        };
                    }
                    var result = await _signInManager.PasswordSignInAsync(loginRequestModel.Username, loginRequestModel.Password, true, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        // Successful login
                        return new LoginResponseModel
                        {
                            Message = "Login successfully ✔",
                            IsSuccessful = true,
                            ApplicationUserDto = new ApplicationUserDto
                            {
                                UserId = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                RoleName = user.RoleName,
                                IsDeleted = user.IsDeleted,
                                Email = user.Email
                            }
                        };
                    }

                    else if (result.RequiresTwoFactor)
                    {
                        // Two-factor authentication is required
                        return new LoginResponseModel
                        {
                            Message = "Two-factor authentication is required",
                            IsSuccessful = false
                        };
                    }
                    else if (result.IsLockedOut)
                    {
                        // Account is locked due to too many failed attempts
                        // Display a message or redirect the user to a lockout page
                        // You may want to inform the user when they can try again
                        return new LoginResponseModel
                        {
                            Message = "Account is locked due to too many failed attempts, try to come back after 15 minutes",
                            IsSuccessful = false
                        };
                    }
                    else
                    {
                        // Other failure cases
                        // Display a general login error message
                        return new LoginResponseModel
                        {
                            Message = "error loging in, maybe email or password is invalid",
                            IsSuccessful = false
                        };
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return new LoginResponseModel { };
            }
        }
    }
}
