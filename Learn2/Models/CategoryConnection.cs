using System;
using System.Collections.Generic;

namespace Learn2.Models
{
    public partial class CategoryConnection
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int PhoneId { get; set; }
    }
}
