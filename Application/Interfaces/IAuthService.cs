using Domain.DTOs;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralResponse<string>> CreateAccount(UserDTO user);
        Task<GeneralResponse<string>> LoginAccount(LoginDTO login);
    }
}
