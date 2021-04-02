using System.Collections.Generic;

namespace Domain
{
    public class Product
    {
        public double Id { get; set; }
        public string Title { get; set; }
        public double Popularity { get; set; }

        public ICollection<RealProduct> RealProducts { get; set; }
        public ICollection<ProductAttribute> Attributes { get; set; }
    }
}
