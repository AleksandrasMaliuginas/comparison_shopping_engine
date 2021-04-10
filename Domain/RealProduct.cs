using System;

namespace Domain
{
    public class RealProduct
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public DateTime LastCheck { get; set; }

        public long AbstractProductId { get; set; }
        public Product AbstractProduct { get; set; }

        //public long EShopId { get; set; }
        //public EShop EShop { get; set; }
    }
}
