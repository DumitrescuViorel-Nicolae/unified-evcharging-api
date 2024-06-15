using Domain.DTOs;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralResponse<string>> CreateAccount(RegisterDTO user);
        Task<GeneralResponse<LoginResponse>> LoginAccount(LoginDTO login);
        Task<GeneralResponse<LoginResponse>> RefreshToken(string refreshToken);
        Task<GeneralResponse<string>> Logout(string refreshToken);
    }
}
