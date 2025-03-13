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
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_imageFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imageUrl = $"{_baseUrl}/images/{fileName}";
            return imageUrl;
        }

        public async Task<bool> DeleteImageAsync(string imageName, string userId)
        {
            // Nếu cần kiểm tra quyền sở hữu ảnh, bạn có thể bổ sung logic ở đây
            var filePath = Path.Combine(_imageFolder, imageName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
    }

}
