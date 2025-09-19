using SecretsSharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Domain.Interfaces
{
    public interface ISecretFileRepository : IRepository<SecretFile>
    {
        Task<SecretFile?> GetByAccessTokenAsync(string accessToken);
        Task<IEnumerable<SecretFile>> GetByUserIdAsync(Guid userId);
    }
}
