using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Rating
{
    public class CreateRatingRequestDto
    {
        public int ProductId { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5.")]
        public int RatingScore { get; set; }
        
        [MaxLength(1000, ErrorMessage = "Nội dung đánh giá không được vượt quá 1000 ký tự.")]
        public string RatingContent { get; set; }
        public string Status { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
