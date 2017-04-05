using System.Collections.Generic;
using ParserPrisjakt.Model;
using System.Linq;
using System;

namespace ParserPrisjakt
{
    public class KungsmoblerCrawler
    {
        private HttpCommunicator _httpCommunicator;
        private string startUrl = "http://kungsmobler.se/kategori/soffor/skinnsoffor";
        private string categoryNavigationClass = "product-categories";

        public KungsmoblerCrawler(HttpCommunicator httpCommunicator)
        {
            _httpCommunicator = httpCommunicator;
        }

        public IEnumerable<ProductTextCategoryItem> GetItems()
        {
            var categoryLinks = GetCategoryLinks();

            var result = new List<ProductTextCategoryItem>();

            foreach (var categoryLink in categoryLinks)
            {
                var categoryHtml = _httpCommunicator.GetWebPage(categoryLink, System.Text.Encoding.UTF8);

                var category = categoryLink.Replace("http://", String.Empty).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[2]; 
                var productTexts = GetProductTexts(categoryHtml);

                var productCategoryTexts = productTexts.Select(p => 
                new ProductTextCategoryItem() { Category = category, ProductText = p }).ToList();

                result.AddRange(productCategoryTexts);
            }

            return result;
        }

        private IEnumerable<string> GetProductTexts(string htmlContent)
        {
            try
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(htmlContent);

                var productTitleElements = html.DocumentNode.Descendants("p").Where(p => p.Attributes["class"] != null && p.Attributes["class"].Value.Contains("product-title")).ToList();

                var productTexts = productTitleElements.Select(e => System.Web.HttpUtility.HtmlDecode(e.InnerText)).ToList();
                return productTexts;
            }
            catch
            {
                return new List<string>();
            }
        }

        private IEnumerable<string> GetCategoryLinks()
        {
            var startHtml = _httpCommunicator.GetWebPage(startUrl, System.Text.Encoding.UTF8);

            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(startHtml);

            var sidebar = html.GetElementbyId("shop-sidebar");
            var categoryUl = sidebar.Descendants("ul").Where(d => d.Attributes["class"].Value.Contains(categoryNavigationClass)).FirstOrDefault();

            var headCategories = categoryUl.Descendants("li");

            var categoryLinks = new List<string>();

            foreach (var category in headCategories)
            {
                var categoryLinkElement = category.Descendants("a").FirstOrDefault();
                var categoryLink = categoryLinkElement.Attributes["href"].Value;

                categoryLinks.Add(categoryLink);
            }

            return categoryLinks;
        }
    }
}
