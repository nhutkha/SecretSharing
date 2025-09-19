using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Domain.Entities
{
    public class SecretFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string StoragePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = "application/octet-stream";
        public bool AutoDelete { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int DownloadCount { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string AccessToken { get; set; } = string.Empty;

        // Foreign key
        public Guid UserId { get; set; }

        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
