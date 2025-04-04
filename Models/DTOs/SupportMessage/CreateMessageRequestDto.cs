﻿using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.SupportMessage
{
    public class CreateMessageRequestDto
    {
        public string? ReceiverId { get; set; }
        [Required]
        public string Message { get; set; } = null!;
        [Required]
        public bool IsFromAdmin { get; set; }

    }
}
