using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Application.Interfaces;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : ControllerBase
    {
        private readonly ISecretService _secretService;
        private readonly ILogger<PublicController> _logger;

        public PublicController(ISecretService secretService, ILogger<PublicController> logger)
        {
            _secretService = secretService;
            _logger = logger;
        }

        /// <summary>
        /// Access a shared file
        /// </summary>
        /// <param name="token">Access token for the file</param>
        /// <returns>The file content</returns>
        [HttpGet("file/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AccessFile(string token)
        {
            try
            {
                _logger.LogInformation("File access attempt with token: {Token}", token);
                var result = await _secretService.AccessFileAsync(token);
                return File(result.Stream, result.ContentType, result.FileName);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning("File not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during file access");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// Access shared text
        /// </summary>
        /// <param name="token">Access token for the text</param>
        /// <returns>The text content</returns>
        [HttpGet("text/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AccessText(string token)
        {
            try
            {
                _logger.LogInformation("Text access attempt with token: {Token}", token);
                var result = await _secretService.AccessTextAsync(token);
                return Ok(new { Content = result });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Text not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during text access");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }
    }
}