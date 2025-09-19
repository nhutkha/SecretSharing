using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Domain.Entities
{
    public class SecretText
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool AutoDelete { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int AccessCount { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string AccessToken { get; set; } = string.Empty;

        // Foreign key
        public Guid UserId { get; set; }

        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
