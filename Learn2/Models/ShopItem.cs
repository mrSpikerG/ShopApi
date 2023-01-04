using System;
using System.Collections.Generic;

namespace Learn2.Models
{
    public partial class ShopItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? UserId { get; set; }
    }
}
