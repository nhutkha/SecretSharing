using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Application.DTOs.Responses
{
    /// <summary>
    /// Text upload response
    /// </summary>
    public class TextUploadResponse
    {
        public string TextUrl { get; set; } = string.Empty;
        public Guid TextId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
