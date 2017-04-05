using ParserPrisjakt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserPrisjakt
{
    public class NilsonCrawler
    {
        private string startUrlMale = "https://feetfirst.se/nilsonshoes/skor/herr";
        //private string startUrlMale = "https://feetfirst.se/rockport-purpose-plain-bruna-307457";
        private string startUrlFemale = "https://feetfirst.se/nilsonshoes/skor/dam";
        private string productTileClass = "grid-item product-card";
        private string baseUrl = "https://feetfirst.se";


        private HttpCommunicator _httpCommunicator;

        public NilsonCrawler(HttpCommunicator httpCommunicator)
        {
            _httpCommunicator = httpCommunicator;
        }

        public IEnumerable<ProductTextCategoryItem> GetItems()
        {
            var productLinks = GetProductLinks();

            var result = new List<ProductTextCategoryItem>();

            foreach(var productLink in productLinks)
            {
                var productHtml = _httpCommunicator.GetWebPage($"{baseUrl}{productLink}", Encoding.UTF8);
                var productText = GetProductDescriptionText(productHtml);

                result.Add(new ProductTextCategoryItem() { ProductText = productText, Category = "Skor" });
            }

            return result;
        }

        private string GetProductDescriptionText(string htmlContent)
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(htmlContent);

            var productDescription = html.DocumentNode.Descendants("p").FirstOrDefault(p => p.Attributes["itemprop"] != null
                && p.Attributes["itemprop"].Value == "description");

            return productDescription.InnerText;

        }

        private IEnumerable<string> GetProductLinks()
        {
            var startHtml = _httpCommunicator.GetWebPage(startUrlMale, Encoding.UTF8);

            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(startHtml);

            var products = html.GetElementbyId("result-products");
            var productTiles = products.Descendants("div").Where(d => d.Attributes["class"].Value.Contains(productTileClass)).ToList();

            var productInnerTiles = productTiles.SelectMany(s => 
                s.Descendants("div").Where(d => d.Attributes["class"].Value.Contains("grid-item-inner"))).ToList();

            var productLinks = productInnerTiles.Select(e => e.Descendants("a").FirstOrDefault()).Select(e => e.Attributes["href"].Value).ToList();

            return productLinks;
        }
    }
}
