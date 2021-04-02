using System;

namespace Domain
{
    public class RealProduct
    {
        public double Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public DateTime LastCheck { get; set; }

        public double AbstractProductId { get; set; }
        public Product AbstractProduct { get; set; }

        public double EShopId { get; set; }
        public EShop EShop { get; set; }
    }
}
