using Application.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<(int, string)> Register(UserDTO model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return (0, "User already exists");

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };
            var createUserResult = await userManager.CreateAsync(user, model.Password); //add the reason from the failure in response
            if (!createUserResult.Succeeded)
                return (0, "User creation failed! Please check user details and try again.");

            //can make an overload for admin registration
            //https://www.c-sharpcorner.com/article/jwt-authentication-and-authorization-in-net-6-0-with-identity-framework/

            //if (!await roleManager.RoleExistsAsync(role))
            //    await roleManager.CreateAsync(new IdentityRole(role));

            //if (await roleManager.RoleExistsAsync(UserRoles.User))
            //    await userManager.AddToRoleAsync(user, role);

            return (1, "User created successfully!");
        }

    }
}
