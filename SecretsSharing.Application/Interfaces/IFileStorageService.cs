using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Application.Interfaces
{
    /// <summary>
    /// File storage service interface
    /// </summary>
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream> DownloadFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
}
