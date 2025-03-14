using electro_shop_backend.Services.Interfaces;

namespace electro_shop_backend.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _imageFolder;
        private readonly string _baseUrl;

        public ImageService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _imageFolder = Path.Combine(_env.WebRootPath, "images");

            if (!Directory.Exists(_imageFolder))
            {
                Directory.CreateDirectory(_imageFolder);
            }
            _baseUrl = configuration["BaseUrl"] ?? "";
        }

        public async Task<string> UploadImageAsync( IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("File ảnh không hợp lệ.");
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_imageFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imageUrl = $"{_baseUrl}/images/{fileName}";
            return imageUrl;
        }

        public async Task<bool> DeleteImageByUrlAsync(string imageUrl)
        {
            if (!ValidateImageUrl(imageUrl, out string fileName))
            {
                return false;
            }

            var filePath = Path.Combine(_imageFolder, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        private bool ValidateImageUrl(string imageUrl, out string fileName)
        {
            fileName = string.Empty;

            if (string.IsNullOrEmpty(imageUrl))
            {
                return false;
            }

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
            {
                return false;
            }

            var localPath = uri.LocalPath;

            if (!localPath.StartsWith("/images", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            fileName = Path.GetFileName(localPath);
            return true;
        }
    }

}
