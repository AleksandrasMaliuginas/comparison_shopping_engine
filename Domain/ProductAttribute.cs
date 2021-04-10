namespace Domain
{
    public class ProductAttribute
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }

        public long AttributeId { get; set; }
        public Attribute Attribute { get; set; }

    }
}
