using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.RegisteredCompaniesRepository;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private readonly ICompaniesRepository _companies;
        private readonly IPaymentService _paymentService;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ICompaniesRepository companies, IPaymentService paymentService)
        {
            this.userManager = userManager;
            _configuration = configuration;
            _companies = companies;
            _paymentService = paymentService;
        }

        public async Task<GeneralResponse<string>> CreateAccount(UserDTO user)
        {
            if (user == null) return new GeneralResponse<string>(false, "User is empty");
            var newUser = new ApplicationUser()
            {
                Name = user.Username,
                Email = user.Email,
                PasswordHash = user.Password,
                UserName = user.Email
            };

            var existingUser = await userManager.FindByEmailAsync(newUser.Email);
            if (existingUser is not null) return new GeneralResponse<string>(false, "User already registered");

            var createUser = await userManager.CreateAsync(newUser!, user.Password);
            if (!createUser.Succeeded) return new GeneralResponse<string>(false, $"Error occured in user creation, please try again - {createUser.Errors.FirstOrDefault()?.Description}");

            if (user.Role is not null)
            {
                if (user.Role == UserRoles.User || user.Role == UserRoles.Company || user.Role == UserRoles.Admin)
                {
                    await userManager.AddToRoleAsync(newUser, user.Role);

                    if (user.Role == UserRoles.Company)
                    {
                        // handle company add
                        // handle create stripe account as well
                        var createdUser = await userManager.FindByEmailAsync(newUser.Email);
                        var company = new RegisteredCompany
                        {
                            UserId = createdUser.Id,
                            CompanyName = user.CompanyName,
                            Country = user.Country,
                            City = user.City,
                            StreetName = user.StreetName,
                            RegistrationNumber = user.RegistrationNumber,
                            TaxNumber = user.TaxNumber,
                            ZipCode = user.ZipCode
                        };
                        var registeredCompany = await _companies.AddAsync(company);

                        if (registeredCompany is null) { return new GeneralResponse<string>(false, "Company registration failed!"); }

                        var stripeConnectAccountProperties = new StripeEVAccountDetails
                        {
                            UserAccount = user,
                            AdressDetails = new AdressDetails
                            {
                                City = company.City,
                                PostalCode = company.ZipCode,
                            },
                            CompanyName = company.CompanyName
                        };

                        var stripeAccount = await _paymentService.CreateEVConnectAccount(stripeConnectAccountProperties);

                        if (stripeAccount.Id is null)
                        {
                            return new GeneralResponse<string>(false, "Stripe account creation failed!");
                        }

                        await LinkStripeAccountID(registeredCompany.Id, stripeAccount.Id);
                    }


                    //var checkForUserRole = await roleManager.FindByNameAsync(user.Role);
                    //this can be deleted after all flows are created
                    //if (checkForUserRole is null)
                    //{
                    //    await roleManager.CreateAsync(new IdentityRole { Name = user.Role});
                    //}
                }
            }
            else
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            return new GeneralResponse<string>(true, "Account created");
        }

        private async Task<GeneralResponse<string>> LinkStripeAccountID(int companyId, string stripeAccountID)
        {
            try
            {
                await _companies.LinkStripeAccountID(companyId, stripeAccountID);
                return new GeneralResponse<string>(true, "The company has been linked!");
            }
            catch (Exception e)
            {
                return new GeneralResponse<string>(false, $"Link not successfull - {e.Message}");
            }
        }

        public async Task<GeneralResponse<string>> LoginAccount(LoginDTO login)
        {
            if (login == null)
                return new GeneralResponse<string>(false, "Login container empty");

            var getUser = await userManager.FindByEmailAsync(login.Email);
            if (getUser is null)
                return new GeneralResponse<string>(false, "User not registered");

            bool checkUserPassword = await userManager.CheckPasswordAsync(getUser, login.Password);
            if (!checkUserPassword)
                return new GeneralResponse<string>(false, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());

            string token = GenerateToken(userSession);
            return new GeneralResponse<string>(true, "User logged in", token!);
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
