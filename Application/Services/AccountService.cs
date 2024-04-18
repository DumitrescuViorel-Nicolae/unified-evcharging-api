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
            var foundUser = await _userManager.FindByNameAsync(userEmail);
            if (foundUser == null)
            {
                return new GeneralResponse<UserDTO>(false, "User not found, please try again!");
            }

            var user = _mapper.Map<UserDTO>(foundUser);
            var userRole = await _userManager.GetRolesAsync(foundUser);

            if(userRole != null)
            {
                user.Role = userRole.FirstOrDefault();
            }

            return new GeneralResponse<UserDTO>(true, "User found", user);
        }
    }
}
