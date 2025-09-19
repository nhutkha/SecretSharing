using Microsoft.EntityFrameworkCore;
using SecretsSharing.Domain.Entities;
using SecretsSharing.Domain.Interfaces;
using SecretsSharing.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Persistence.Repositories
{
    /// <summary>
    /// Secret text repository implementation
    /// </summary>
    public class SecretTextRepository : ISecretTextRepository
    {
        private readonly ApplicationDbContext _context;

        public SecretTextRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SecretText?> GetByIdAsync(Guid id)
        {
            return await _context.SecretTexts.FindAsync(id);
        }

        public async Task<IEnumerable<SecretText>> GetAllAsync()
        {
            return await _context.SecretTexts.ToListAsync();
        }

        public async Task AddAsync(SecretText entity)
        {
            await _context.SecretTexts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SecretText entity)
        {
            _context.SecretTexts.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SecretText entity)
        {
            _context.SecretTexts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<SecretText?> GetByAccessTokenAsync(string accessToken)
        {
            return await _context.SecretTexts
                .FirstOrDefaultAsync(t => t.AccessToken == accessToken && t.DeletedAt == null);
        }

        public async Task<IEnumerable<SecretText>> GetByUserIdAsync(Guid userId)
        {
            return await _context.SecretTexts
                .Where(t => t.UserId == userId && t.DeletedAt == null)
                .ToListAsync();
        }
    }
}
