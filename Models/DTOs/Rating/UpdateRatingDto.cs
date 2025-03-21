using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Rating
{
    public class UpdateRatingDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5.")]
        public int RatingScore { get;  set; }
        public string RatingContent { get; set; }
        public DateTime? TimeStamp { get;  set; }
    }
}
