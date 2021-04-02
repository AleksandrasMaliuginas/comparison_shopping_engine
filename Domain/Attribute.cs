using System.Collections.Generic;

namespace Domain
{
    public class Attribute
    {
        public double Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ICollection<ProductAttribute> Products { get; set; }
    }
}
