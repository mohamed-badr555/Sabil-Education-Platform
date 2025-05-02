using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Basket
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public float TotalPrice { get; set; }
        public float TotalPriceBefore { get; set; }
        public bool IsVerified { get; set; }
        public string Currency { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public string Coupon { get; set; }
    }
}
