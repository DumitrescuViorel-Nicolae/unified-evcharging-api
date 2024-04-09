using Domain.DTOs;
using static Domain.DTOs.ServiceResponses;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralResponse> CreateAccount(UserDTO user);
        Task<LoginResponse> LoginAccount(LoginDTO login);
    }
}
