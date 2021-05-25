using ComparisonEngine;
using Domain;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DbService_Example
{
    public class Program
    {
        static private DatabaseContext context;
        public static void Main(string[] args)
        {
            context = new DatabaseContext();

            var options = new ChromeOptions();
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            options.AddArguments("--window-size=1920,1080", "--no-sandbox", "--headless");

            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            IWebDriver driverPigu = new ChromeDriver(chromeDriverService, options);

            driver.Navigate().GoToUrl("https://www.varle.lt/mobilieji-telefonai/");
            var products = driver.FindElements(By.CssSelector("div.grid-item.product"));

            foreach (var product in products)
            {
                string[] productInfo = product.Text.Split('\n');

                string absTitle = productInfo[0].Replace("(Atnaujinta)","").
                    Replace("(Pažeista pakuotė)","").
                    Replace("(Ekspozicinė prekė)", "");
                int index = absTitle.IndexOf("+DOVANA");
                if (index != -1) absTitle = absTitle.Substring(0, index);

                var dbProduct = new Product
                {
                    Title = absTitle,
                    Popularity = 0
                };
                var addedProduct = context.Products.Add(dbProduct);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException) { }

                CreateAttribute(addedProduct.Entity, productInfo, "Gamintojas: ", "Manufacturer");
                CreateAttribute(addedProduct.Entity, productInfo, "Ekrano įstrižainė (coliais): ", "Size");
                CreateAttribute(addedProduct.Entity, productInfo, "Vidinė atmintis (GB): ", "Memory");
                CreateAttribute(addedProduct.Entity, productInfo, "Atmintis (RAM) (GB): ", "Ram");
                CreateAttribute(addedProduct.Entity, productInfo, "Ekrano raiška: ", "Resolution");

                var urlVarle = product.FindElement(By.CssSelector("a.title")).GetAttribute("href");
                var priceVarle = product.FindElement(By.CssSelector("span.price")).Text;
                priceVarle = priceVarle.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                priceVarle = priceVarle.Substring(0, priceVarle.Length - 2);
                var realProductVarle = new RealProduct
                {
                    AbstractProduct = addedProduct.Entity,
                    LastCheck = DateTime.Now,
                    Title = productInfo[0],
                    Price = decimal.Parse(priceVarle),
                    Url = urlVarle
                };
                context.RealProducts.Add(realProductVarle);
                addedProduct.Entity.RealProducts.Add(realProductVarle);

                driverPigu.Navigate().GoToUrl("https://pigu.lt/lt/search?q=" + absTitle);
                string urlPigu;
                try
                {
                    var productsPigu = driverPigu.FindElement(By.CssSelector("div.product-list.all-products-visible.clearfix.product-list--equal-height"));
                    string pricePigu = productsPigu.FindElement(By.CssSelector("span.price.notranslate")).Text.Substring(2);
                    pricePigu = pricePigu.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                    pricePigu = Regex.Replace(pricePigu, @"\s+", "");
                    string titlePigu = productsPigu.FindElement(By.CssSelector("p.product-name")).Text;
                    urlPigu = productsPigu.FindElement(By.CssSelector("a")).GetAttribute("href");
                    var realProducPigu = new RealProduct
                    {
                        AbstractProduct = addedProduct.Entity,
                        LastCheck = DateTime.Now,
                        Title = titlePigu,
                        Price = decimal.Parse(pricePigu),
                        Url = urlPigu
                    };
                    context.RealProducts.Add(realProducPigu);
                    addedProduct.Entity.RealProducts.Add(realProducPigu);
                }
                catch (NoSuchElementException) { }
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException) { }
            }

            driverPigu.Close();
            driverPigu.Quit();
            driver.Close();
            driver.Quit();
        }

        static bool CreateAttribute(Product addedProduct, string[] productInfo, string htmlAttrName, string attrName)
        {
            if (Array.Find(productInfo, s => s.StartsWith(htmlAttrName)) != null)
            {
                var localAttribute = new Domain.Attribute
                {
                    Name = attrName,
                    Value = Array.Find(productInfo, s => s.StartsWith(htmlAttrName)).Substring(htmlAttrName.Length).Trim()
                };

                var foundAttribute = context.Attributes.Where(a => a.Value == localAttribute.Value && a.Name == localAttribute.Name).FirstOrDefault();
                if (foundAttribute == null)
                {
                    context.Attributes.Add(localAttribute);
                    foundAttribute = localAttribute;
                }

                context.ProductsAttributes.Add(new ProductAttribute
                {
                    Product = addedProduct,
                    Attribute = foundAttribute
                });
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException) { }
                return true;
            }
            return false;
        }
    }
}
