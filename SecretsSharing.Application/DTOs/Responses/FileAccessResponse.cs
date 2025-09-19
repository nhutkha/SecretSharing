using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Application.DTOs.Responses
{
    /// <summary>
    /// File access response
    /// </summary>
    public class FileAccessResponse
    {
        public Stream Stream { get; set; } = null!;
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }
}
