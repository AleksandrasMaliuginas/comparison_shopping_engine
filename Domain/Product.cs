using System.Collections.Generic;

namespace Domain
{
    public class Product
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long Popularity { get; set; }

        public ICollection<RealProduct> RealProducts { get; set; }
        public ICollection<ProductAttribute> Attributes { get; set; }
    }
}
