using ComparisonEngine;
using Domain;
using System.Collections.Generic;

namespace DbService_Example
{
    public class Program
    {
        static DatabaseContext context = new DatabaseContext();

        public static void Main(string[] args)
        {
            var product1 = new Product
            {
                Title = "Test_Product2",
                Popularity = 2
            };

            var attritute = new Attribute
            {
                Name = "Attr2",
                Value = "ValueASDF2"
            };

            var realProduct = new RealProduct
            {
                Title = "RealProd2",
                Price = 10
            };

            product1.RealProducts = new List<RealProduct>
            {
                realProduct
            };

            //var product2 = new Product
            //{
            //    Title = "Test_Product2",
            //    Popularity = 2
            //};

            context.Products.Add(product1);
            context.Attributes.Add(attritute);

            context.ProductsAttributes.Add(new ProductAttribute
            {
                Product = product1,
                Attribute = attritute
            });

            context.SaveChanges();
        }
    }
}
