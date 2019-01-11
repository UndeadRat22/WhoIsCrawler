using HtmlAgilityPack;
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
    }
}
