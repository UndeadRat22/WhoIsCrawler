using HtmlAgilityPack;
using System.Linq;

namespace WhoIsCrawler.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static HtmlNode SelectSingleNodeByClass(this HtmlNode node, string classToFind)
        {
            if (node == null)
                return null;
            return node.SelectSingleNode($"//*[contains(@class,'{classToFind}')]");
        }
        public static HtmlNodeCollection SelectNodesByClass(this HtmlNode node, string classToFind)
        {
            if (node == null)
                return null;
            return node.SelectNodes($"//*[contains(@class,'{classToFind}')]");
        }

        public static HtmlNode SelectByInnerText(this HtmlNodeCollection collection, string innerText)
        {
            if (collection == null)
                return null;
            return collection
                .First(node => node.InnerText.Contains(innerText))
                .ChildNodes
                .First(node => node.HasClass("df-value"));
        }
    }
}
