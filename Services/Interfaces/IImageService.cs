namespace electro_shop_backend.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageByUrlAsync(string imageUrl);
    }

}
