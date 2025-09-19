using Microsoft.AspNetCore.Http;
using SecretsSharing.Application.DTOs.Responses;
using SecretsSharing.Application.Interfaces;
using SecretsSharing.Domain.Entities;
using SecretsSharing.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Application.Services
{
    /// <summary>
    /// Secret service implementation
    /// </summary>
    public class SecretService : ISecretService
    {
        private readonly ISecretFileRepository _fileRepository;
        private readonly ISecretTextRepository _textRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecretService(
            ISecretFileRepository fileRepository,
            ISecretTextRepository textRepository,
            IFileStorageService fileStorageService,
            IHttpContextAccessor httpContextAccessor)
        {
            _fileRepository = fileRepository;
            _textRepository = textRepository;
            _fileStorageService = fileStorageService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadFileAsync(IFormFile file, Guid userId, bool autoDelete)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required");

            if (file.Length > 100 * 1024 * 1024)
                throw new ArgumentException("File size exceeds maximum limit of 100MB");

            var storagePath = await _fileStorageService.UploadFileAsync(
                file.OpenReadStream(),
                file.FileName,
                file.ContentType);

            var accessToken = GenerateAccessToken();

            var secretFile = new SecretFile
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                StoragePath = storagePath,
                FileSize = file.Length,
                ContentType = file.ContentType,
                AutoDelete = autoDelete,
                UploadedAt = DateTime.UtcNow,
                AccessToken = accessToken,
                UserId = userId
            };

            await _fileRepository.AddAsync(secretFile);

            var baseUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";
            return $"{baseUrl}/api/public/file/{accessToken}";
        }

        public async Task<string> UploadTextAsync(string content, Guid userId, bool autoDelete)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content is required");

            var accessToken = GenerateAccessToken();

            var secretText = new SecretText
            {
                Id = Guid.NewGuid(),
                Content = content,
                AutoDelete = autoDelete,
                CreatedAt = DateTime.UtcNow,
                AccessToken = accessToken,
                UserId = userId
            };

            await _textRepository.AddAsync(secretText);

            var baseUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";
            return $"{baseUrl}/api/public/text/{accessToken}";
        }

        public async Task<FileAccessResponse> AccessFileAsync(string accessToken)
        {
            var secretFile = await _fileRepository.GetByAccessTokenAsync(accessToken);
            if (secretFile == null || secretFile.DeletedAt != null)
                throw new FileNotFoundException("File not found or has been deleted");

            secretFile.DownloadCount++;

            if (secretFile.AutoDelete && secretFile.DownloadCount >= 1)
            {
                secretFile.DeletedAt = DateTime.UtcNow;
                await _fileStorageService.DeleteFileAsync(secretFile.StoragePath);
            }

            await _fileRepository.UpdateAsync(secretFile);

            var fileStream = await _fileStorageService.DownloadFileAsync(secretFile.StoragePath);

            return new FileAccessResponse
            {
                Stream = fileStream,
                ContentType = secretFile.ContentType,
                FileName = secretFile.FileName,
                FileSize = secretFile.FileSize
            };
        }

        public async Task<string> AccessTextAsync(string accessToken)
        {
            var secretText = await _textRepository.GetByAccessTokenAsync(accessToken);
            if (secretText == null || secretText.DeletedAt != null)
                throw new KeyNotFoundException("Text not found or has been deleted");

            secretText.AccessCount++;

            if (secretText.AutoDelete && secretText.AccessCount >= 1)
                secretText.DeletedAt = DateTime.UtcNow;

            await _textRepository.UpdateAsync(secretText);

            return secretText.Content;
        }

        public string GenerateAccessToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
    }
}
