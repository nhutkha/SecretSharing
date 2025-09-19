using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Application.DTOs.Responses
{
    /// <summary>
    /// File upload response
    /// </summary>
    public class FileUploadResponse
    {
        public string FileUrl { get; set; } = string.Empty;
        public Guid FileId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
