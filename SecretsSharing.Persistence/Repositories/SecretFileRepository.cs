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
    /// Secret file repository implementation
    /// </summary>
    public class SecretFileRepository : ISecretFileRepository
    {
        private readonly ApplicationDbContext _context;

        public SecretFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SecretFile?> GetByIdAsync(Guid id)
        {
            return await _context.SecretFiles.FindAsync(id);
        }

        public async Task<IEnumerable<SecretFile>> GetAllAsync()
        {
            return await _context.SecretFiles.ToListAsync();
        }

        public async Task AddAsync(SecretFile entity)
        {
            await _context.SecretFiles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SecretFile entity)
        {
            _context.SecretFiles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SecretFile entity)
        {
            _context.SecretFiles.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<SecretFile?> GetByAccessTokenAsync(string accessToken)
        {
            return await _context.SecretFiles
                .FirstOrDefaultAsync(f => f.AccessToken == accessToken && f.DeletedAt == null);
        }

        public async Task<IEnumerable<SecretFile>> GetByUserIdAsync(Guid userId)
        {
            return await _context.SecretFiles
                .Where(f => f.UserId == userId && f.DeletedAt == null)
                .ToListAsync();
        }
    }
}
