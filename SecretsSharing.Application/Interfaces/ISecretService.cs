using Microsoft.AspNetCore.Http;
using SecretsSharing.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Application.Interfaces
{
    /// <summary>
    /// Secret service interface
    /// </summary>
    public interface ISecretService
    {
        Task<string> UploadFileAsync(IFormFile file, Guid userId, bool autoDelete);
        Task<string> UploadTextAsync(string content, Guid userId, bool autoDelete);
        Task<FileAccessResponse> AccessFileAsync(string accessToken);
        Task<string> AccessTextAsync(string accessToken);
        string GenerateAccessToken();
    }
}
