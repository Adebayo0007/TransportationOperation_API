﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using PTS_CORE.Domain.Entities;
using PTS_CORE.Utils;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteUserAccount(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId.Trim()))
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(userId.Trim());

                    if (user == null)
                        return false;

                    // Delete user properties based on your model
                    user.IsDeleted = true;
                    user.DeletedDate = DateTime.Now;

                    // Update other properties as needed
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return true;
                    else
                        // Handle errors if the update fails
                        return false;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> ActivateUserAccount(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId.Trim()))
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(userId.Trim());

                    if (user == null)
                        return false;

                    // Delete user properties based on your model
                    user.IsDeleted = false;
                    user.LastModified = DateTime.Now;

                    // Update other properties as needed
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return true;
                    else
                        // Handle errors if the update fails
                        return false;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel)
        {
            if (loginRequestModel != null)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(loginRequestModel.Username.Trim());
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
                    var result = await _signInManager.PasswordSignInAsync(loginRequestModel.Username.Trim(), loginRequestModel.Password.Trim(), true, lockoutOnFailure: true);

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
                                Email = user.Email,
                                Phonenumber = user.PhoneNumber
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

        public async Task<bool> UpdateUserAccount(UserUpdateModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(updateModel.UserId.Trim());

                    if (user == null)
                        return false;

                    // Update user properties based on your model
                    user.FirstName = !string.IsNullOrWhiteSpace(updateModel.FirstName.Trim()) ? updateModel.FirstName.Trim(): user.FirstName;
                    user.LastName = !string.IsNullOrWhiteSpace(updateModel.LastName.Trim()) ? updateModel.LastName.Trim() : user.LastName;
                    user.Email = !string.IsNullOrWhiteSpace(updateModel.Email.Trim()) ? updateModel.Email.Trim() : user.Email;
                    user.UserName = !string.IsNullOrWhiteSpace(updateModel.Email.Trim()) ? updateModel.Email.Trim() : user.Email;
                    user.RoleName = !string.IsNullOrWhiteSpace(updateModel.RoleName.Trim()) ? updateModel.RoleName.Trim() : user.RoleName;
                    user.PhoneNumber = !string.IsNullOrWhiteSpace(updateModel.Phonenumber.Trim()) ? updateModel.Phonenumber.Trim() : user.PhoneNumber;
                    user.IsModified = true;
                    user.LastModified = DateTime.Now;
                    user.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    user.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return true;
                    else
                        // Handle errors if the update fails
                        return false;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;

        }

        public async Task<bool> CreateUserAccount(CreateUserRequestModel model)
        {
            if(model != null)
            {
                if(await _userManager.FindByEmailAsync(model.Email.Trim()) != null) return false;
                var role = await _roleManager.FindByNameAsync(model.RoleName.Trim());
                var user = new ApplicationUser
                {
                    UserName = model.Email.Trim(),
                    Email = model.Email.Trim(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    ApplicationRoleId = role.Id.Trim(),
                    RoleName = model.RoleName.Trim(),
                    DateCreated = DateTime.Now,
                    PhoneNumber = model.Phonenumber.Trim(),
                    PhoneNumberConfirmed = !string.IsNullOrWhiteSpace(model.Phonenumber.Trim()) ? true: false,
                    EmailConfirmed = !string.IsNullOrWhiteSpace(model.Email.Trim()) ? true: false,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    TerminalId = model.TerminalId.Trim() ?? string.Empty,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null
                };

                var result = await _userManager.CreateAsync(user, model.Password.Trim());

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName.Trim());
                    return true;
                }
                else
                {
                    // Handle errors if user creation fails
                    Console.WriteLine($"There is error creating user {result.Errors}");
                    return false;
                }
            }
            else
                return false;
        }
        public async Task<bool> CreateRole(CreateRoleRequestModel model)
        {
            if (model != null)
            {
               
                if (await _roleManager.RoleExistsAsync(model.Name.Trim())) return false;
                var role = new ApplicationRole { 
                    Name = model.Name.Trim(), 
                    Description = model.Description,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    // Handle errors if user creation fails
                    Console.WriteLine($"There is error creating role {result.Errors}");
                    return false;
                }
            }
            else
                return false;
        }

        public async Task<BaseResponse<IEnumerable<ApplicationUserDto>>> GetAllUsers()
        {
            try
            {
                var user = await _userManager.Users.ToListAsync();
                if (user != null)
                    return new BaseResponse<IEnumerable<ApplicationUserDto>>
                    {
                        IsSuccess = true,
                        Message = $"users retrieved successfully",
                        Data = user.Select(x => ReturnApplicationUserDto(x)).ToList()
                    };
                    else return new BaseResponse<IEnumerable<ApplicationUserDto>>
                    {
                        IsSuccess = false,
                        Message = $"users failed to retrieved successfully",
                    };
                }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<ApplicationUserDto>>> GetUserByEmail(string email)
        {
            try
            {

                if(email == null) { throw new NullReferenceException(); }
                var user = await _userManager.Users.Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower()).ToListAsync();
                return new BaseResponse<IEnumerable<ApplicationUserDto>>
                {
                    IsSuccess = true,
                    Message = $"user having email {email} retrieved successfully",
                    Data = user.Select(x => ReturnApplicationUserDto(x)).ToList()
                };
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<ApplicationUserDto>>> GetUserById(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var user = await _userManager.Users.Where(x => x.Id.Trim().ToLower() == id.Trim().ToLower()).ToListAsync();
                return new BaseResponse<IEnumerable<ApplicationUserDto>>
                {
                    IsSuccess = true,
                    Message = $"user having id {id} retrieved successfully",
                    Data = user.Select(x => ReturnApplicationUserDto(x)).ToList()
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<ApplicationRoleDto>>> GetAllRoles()
        {
            try
            {
                var role = await _roleManager.Roles.ToListAsync();
                if (role != null)
                    return new BaseResponse<IEnumerable<ApplicationRoleDto>>
                    {
                        IsSuccess = true,
                        Message = $"roles retrieved successfully",
                        Data = role.Select(x => ReturnApplicationRoleDto(x)).ToList()
                    };
                else return new BaseResponse<IEnumerable<ApplicationRoleDto>>
                {
                    IsSuccess = false,
                    Message = $"roles failed to retrieved successfully",
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SendOTPForPasswordReset(string email)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(email.Trim()))
                {
                    var user = await _userManager.FindByEmailAsync(email.Trim());
                    if (user != null)
                    {
                        user.PasswordOTP = CommonHelper.RandomDigits(5);
                        var result = await _userManager.UpdateAsync(user);
                        //sending the opt as mail to the user is the next step
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch(Exception ex) { throw; }
        }

        public async Task<BaseResponse<bool>> ResetPassword(PasswordResetRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Email.Trim()))
                return new BaseResponse<bool>
                {
                    Message = "email can't be empty",
                    IsSuccess = false
                };

            var user = await _userManager.FindByEmailAsync(model.Email.Trim());
            if(user.IsDeleted == true) return new BaseResponse<bool>
            {
                Message = "Account has been deactivated...",
                IsSuccess = false
            };


            if (user.PasswordOTP != model.Code)
            {
                return new BaseResponse<bool>
                {
                    Message = "wrong OTP input, please check...",
                    IsSuccess = false
                };
            }

            var changeToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, changeToken, model.NewPassword.Trim());

            if (!result.Succeeded)
            {
                return new BaseResponse<bool>
                {
                    Message = "Password update is not successful...",
                    IsSuccess = false
                };
            }

            user.PasswordOTP = null;
            await _userManager.UpdateAsync(user);
            return new BaseResponse<bool>
            {
                Message = "Password changed successfully...",
                IsSuccess = true,
                Data = true
            };

        }


        public async Task<bool> UpdateRefreshToken(string id, string refreshToken)
        {
            if (id != null && refreshToken != null)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.RefreshToken = refreshToken;
                var result = await _userManager.UpdateAsync(user);
                return true;
            }
            return false;
        }

        /// <summary>
        /// /////////////
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private ApplicationRoleDto ReturnApplicationRoleDto(ApplicationRole model)
        {
            return new ApplicationRoleDto
            {
                RoleId = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsDeleted = model.IsDeleted,
                DeletedDate = model.DeletedDate,
                DeletedBy = model.DeletedBy,
                CreatorName = model.CreatorName,
                CreatorId = model.CreatorId,
                DateCreated = model.DateCreated,
                IsModified = model.IsModified,
                ModifierName = model.ModifierName,
                ModifierId = model.ModifierId,
                LastModified = model.LastModified,
               
               
            };
        }

        private ApplicationUserDto ReturnApplicationUserDto(ApplicationUser model)
        {
            return new ApplicationUserDto
            {
                UserId = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RoleName = model.RoleName,
                Phonenumber = model.PhoneNumber,
                Email = model.Email,
                IsDeleted = model.IsDeleted,
                DeletedDate = model.DeletedDate,
                DeletedBy = model.DeletedBy,
                ApplicationRoleId = model.ApplicationRoleId,
                CreatorName = model.CreatorName,
                CreatorId = model.CreatorId,
                DateCreated = model.DateCreated,
                IsModified = model.IsModified,
                ModifierName = model.ModifierName,
                ModifierId = model.ModifierId,
                LastModified = model.LastModified,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                TerminalId = model.TerminalId
                
               
            };
        }

      
    }
}
