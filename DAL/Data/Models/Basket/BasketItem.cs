using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DAL.Data.Models.Basket
{
    public class BasketItem
    {
        public string ItemId { get; set; }
        public string ItemUrl { get; set; }
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; } = 1;   // 1: Course, 2: OnlineEdu, 3: Book
        public string OnlineStudentId { get; set; } // Required if type == 2

        // For value types like float, use WhenWritingDefault instead of WhenWritingNull
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public float PriceBefore { get; set; }
    }
}
