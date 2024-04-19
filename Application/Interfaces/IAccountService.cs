using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<GeneralResponse<UserDTO>> GetUser(string userEmail);
        Task<GeneralResponse<IdentityResult>> UpdateUser(UserDTO user);
        Task<GeneralResponse<IdentityResult>> DeleteAccount(string accountID);
    }
}
