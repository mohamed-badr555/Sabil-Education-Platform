﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public float TotalPrice { get; set; }
        public float TotalPriceBefore { get; set; }
        public bool IsVerified { get; set; }
        public string Currency { get; set; } = "EGP";
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public string Coupon { get; set; }
    }
}
