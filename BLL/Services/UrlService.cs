using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public class UrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return "";

            return $"{request.Scheme}://{request.Host}";
        }

        public string GetFileUrl(string subDirectory, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "";
            
            return $"{GetBaseUrl()}/Files/{subDirectory}/{fileName}";
        }

        /// <summary>
        /// Builds a complete URL for a thumbnail
        /// </summary>
        /// <param name="thumbnailPath">The filename of the thumbnail</param>
        /// <returns>Complete URL to the thumbnail</param>
        public string GetThumbnailUrl(string thumbnailPath)
        {
            return GetFileUrl("courses", thumbnailPath);
        }
    }
}
