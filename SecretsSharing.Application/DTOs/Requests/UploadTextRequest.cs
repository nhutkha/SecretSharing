using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Application.DTOs.Requests
{
    /// <summary>
    /// Text upload request
    /// </summary>
    public class UploadTextRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(10000)]
        public string Content { get; set; } = string.Empty;

        public bool AutoDelete { get; set; } = false;
    }
}
