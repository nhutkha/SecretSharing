using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Application.DTOs.Requests;
using SecretsSharing.Application.Interfaces;
using SecretsSharing.Domain.Entities;
using System.Security.Claims;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SecretsController : ControllerBase
    {
        private readonly ISecretService _secretService;
        private readonly ILogger<SecretsController> _logger;

        public SecretsController(ISecretService secretService, ILogger<SecretsController> logger)
        {
            _secretService = secretService;
            _logger = logger;
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="autoDelete">Whether to delete after first download</param>
        /// <returns>URL to access the file</returns>
        [HttpPost("upload-file")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] bool autoDelete = false)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var result = await _secretService.UploadFileAsync(file, userId, autoDelete);
                return Ok(new { Url = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("File upload failed: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during file upload");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// Upload text content
        /// </summary>
        /// <param name="request">Text content and auto-delete preference</param>
        /// <returns>URL to access the text</returns>
        [HttpPost("upload-text")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadText([FromBody] UploadTextRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var result = await _secretService.UploadTextAsync(request.Content, userId, request.AutoDelete);
                return Ok(new { Url = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Text upload failed: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during text upload");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }
    }
}