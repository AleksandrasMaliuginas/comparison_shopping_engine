using ComparisonEngine;
using Domain;

namespace DbService_Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var context = new DatabaseContext();

            var product = new Product
            {
                Title = "Test_Product",
                Popularity = 0
            };

            context.Products.Add(product);

            //context.SaveChanges();
        }
    }
}
