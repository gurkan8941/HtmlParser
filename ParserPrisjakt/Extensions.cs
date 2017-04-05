using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserPrisjakt
{
    public static class Extensions
    {
        public static IEnumerable<HtmlAgilityPack.HtmlNode> GetElementWithClass(this HtmlAgilityPack.HtmlNode htmlNode, string classname)
        {
            var cssClassAttribute = "class";
            var elements = htmlNode.Descendants().Where(node => node.Attributes[cssClassAttribute] != null && 
                node.Attributes[cssClassAttribute].Value.Contains(classname)).ToList();

            return elements;
        }
    }
}
