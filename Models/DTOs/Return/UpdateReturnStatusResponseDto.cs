namespace electro_shop_backend.Models.DTOs.Return
{
    public class UpdateReturnStatusResponseDto
    {
        public int ReturnId {  get; set; }
        public string? ReturnStatus { get; set; }
        public string? AdminComment { get; set; }
    }
}
