using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Rating
{
    public class CreateRatingRequestDto
    {
        public int ProductId { get; internal set; }
        public string UserId { get; internal set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5.")]
        public int RatingScore { get; internal set; }
        
        [MaxLength(1000, ErrorMessage = "Nội dung đánh giá không được vượt quá 1000 ký tự.")]
        public string RatingContent { get; set; }
        public string Status { get; internal set; }
        public DateTime? TimeStamp { get; internal set; }

        internal Entities.Rating ToRatingFromCreate()
        {
            throw new NotImplementedException();
        }
    }
}
