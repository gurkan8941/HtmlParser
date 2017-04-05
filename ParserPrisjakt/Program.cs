using ParserPrisjakt.Model;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ParserPrisjakt
{
    class Program
    {
        static void Main(string[] args)
        {
            //var executingpath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            //var location = Path.Combine(executingpath, "Files", "cyberphoto-prisjakt-katalog.txt");

            //var lines = File.ReadAllLines(location);
            //foreach(var line in lines)
            //{
            //    var result = ProcessLine(line);

            //    var categories = result.Select(p => p.Category).Distinct();
            //}

            //var crawler = new KungsmoblerCrawler(new HttpCommunicator(4000));
            //var result = crawler.GetItems();

            var nilsoncrawler = new NilsonCrawler(new HttpCommunicator(5000));
            var r = nilsoncrawler.GetItems();
        }

        private static IEnumerable<Product> ProcessLine(string line)
        {
            var lineParts = line.Split(new char[] { '|' });
            var index = 0;
            var productlength = 10;

            var products = new List<Product>();

            while (index < lineParts.Length)
            {
                var productParts = lineParts.Skip(index).Take(productlength).ToList();
                var productName = productParts[1];
                var productCategory = productParts[2];
                var productBrand = productParts[6];
                var product = new Product { Name = productName, Category = productCategory, Brand = productBrand };

                products.Add(product);

                index = index + productlength;
            }

            return products;
        }
    }
}
