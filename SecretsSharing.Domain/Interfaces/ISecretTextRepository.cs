using SecretsSharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Domain.Interfaces
{
    public interface ISecretTextRepository : IRepository<SecretText>
    {
        Task<SecretText?> GetByAccessTokenAsync(string accessToken);
        Task<IEnumerable<SecretText>> GetByUserIdAsync(Guid userId);
    }
}
