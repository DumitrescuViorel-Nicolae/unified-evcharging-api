using Domain.Entities;
using Domain.Interfaces.EVStationRepository;
using Infrastructure.Data;
using Infrastructure.Data.RefreshTokenRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.RefreshTokenRepository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefrehTokenRepository
    {
        public RefreshTokenRepository(ApplicationDBContext context) : base(context) { }
        public async Task SaveRefreshToken(string userId, string refreshToken)
        {
            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                CreatedAt= DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _context.RefreshTokens.AddAsync(tokenEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task<bool> DeleteRefreshToken(string refreshToken)
        {
            var tokenEntity = await GetRefreshToken(refreshToken);
            if (tokenEntity != null)
            {
                _context.RefreshTokens.Remove(tokenEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }

    
}
