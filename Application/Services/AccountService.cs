using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;


namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMapper _mapper;
        public AccountService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<GeneralResponse<UserDTO>> GetUser(string userEmail)
        {
            var foundUser = await _userManager.FindByEmailAsync(userEmail);
            if (foundUser == null)
            {
                return new GeneralResponse<UserDTO>(false, "User not found, please try again!");
            }

            var user = _mapper.Map<UserDTO>(foundUser);
            user.Password = null;
            var userRole = await _userManager.GetRolesAsync(foundUser);

            if (userRole != null)
            {
                user.Role = userRole.FirstOrDefault();
            }

            return new GeneralResponse<UserDTO>(true, "User found", user);
        }

        public async Task<GeneralResponse<IdentityResult>> UpdateUser(UserDTO userDetails)
        {
            var user = await _userManager.FindByIdAsync(userDetails.Id);
            if (user != null)
            {

                user.PhoneNumber = userDetails.PhoneNumber;
                user.Email = userDetails.Email;
                user.UserName = userDetails.Username;
                user.PasswordHash = user.PasswordHash;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new GeneralResponse<IdentityResult>(true, "Update succeeded", result);
                }
                else
                {
                    return new GeneralResponse<IdentityResult>(false, "Update failed", result);
                }
            }
            else
            {
                return new GeneralResponse<IdentityResult>(false, "User details not found!");
            }
        }

        public async Task<GeneralResponse<IdentityResult>> DeleteAccount(string accountID)
        {
            var user = await _userManager.FindByIdAsync(accountID);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return new GeneralResponse<IdentityResult>(true, "Delete succeeded", result);
                }
                else
                {
                    return new GeneralResponse<IdentityResult>(false, "Delete failed", result);
                }
            }
            else
            {
                return new GeneralResponse<IdentityResult>(false, "User details not found!");
            }
        }
    }
}
