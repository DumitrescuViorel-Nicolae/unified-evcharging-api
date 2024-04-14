using Application.Interfaces;
using Domain.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Domain.DTOs.ServiceResponses;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<GeneralResponse> CreateAccount(UserDTO user)
        {
            if (user == null) return new GeneralResponse(false, "Model is empty");
            var newUser = new ApplicationUser()
            {
                Name = user.Username,
                Email = user.Email,
                PasswordHash = user.Password,
                UserName = user.Email
            };

            var existingUser = await userManager.FindByEmailAsync(newUser.Email);
            if (existingUser is not null) return new GeneralResponse(false, "User already registered");

            var createUser = await userManager.CreateAsync(newUser!, user.Password);
            if (!createUser.Succeeded) return new GeneralResponse(false, $"Error occured in user creation, please try again - {createUser.Errors.FirstOrDefault()?.Description}");

            var checkUser = await roleManager.FindByNameAsync("User");
            if (checkUser is null)
                await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

            await userManager.AddToRoleAsync(newUser, "User");

            return new GeneralResponse(true, "Account created");
        }

        public async Task<LoginResponse> LoginAccount(LoginDTO login)
        {
            if (login == null)
                return new LoginResponse(false, null!, "Login container empty", null);

            var getUser = await userManager.FindByEmailAsync(login.Email);
            if (getUser is null)
                return new LoginResponse(false, null!, "User not registered", null);

            bool checkUserPassword = await userManager.CheckPasswordAsync(getUser, login.Password);
            if (!checkUserPassword)
                return new LoginResponse(false, null!, "Invalid email/password", null);

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());

            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "User logged in", Role: getUserRole.FirstOrDefault());
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id?.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
