namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnDetailAdminResponseDto
    {
        public int ReturnId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Reason { get; set; }
        public string? Detail { get; set; }
        public string? Status { get; set; }
        public string? ReturnMethod { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<ReturnProductDto> ReturnProducts { get; set; } = [];
        public List<string> ReturnImageUrls { get; set; } = [];
    }
}
