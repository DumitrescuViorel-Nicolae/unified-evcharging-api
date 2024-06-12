namespace Infrastructure.Data.RefreshTokenRepository
{
    public interface IRefrehTokenRepository : IRepository<RefreshToken>
    {
        Task SaveRefreshToken(string userId, string refreshToken);
        Task<RefreshToken?> GetRefreshToken(string refreshToken);
        Task<bool> DeleteRefreshToken(string refreshToken);
    }
}
