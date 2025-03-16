namespace electro_shop_backend.Models.DTOs.Return
{
    public class CreateReturnResponseDto
    {
        public int ReturnId {  get; set; }
        public string? Status { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
