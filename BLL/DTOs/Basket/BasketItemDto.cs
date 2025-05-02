using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Basket
{
    public class BasketItemDto
    {
        [Required]
        public string ItemId { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public string ItemUrl { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [Range(0.01, float.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public float Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public float PriceBefore { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        [Required]
        public int Type { get; set; } = 1; // 1: Course only (as per your requirement)
        public string OnlineStudentId { get; set; }
    }
}
