using HtmlAgilityPack;
namespace WhoIsCrawler.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static HtmlNode SelectSingleNodeByClass(this HtmlNode node, string classToFind)
        {
            return node.SelectSingleNode($"//*[contains(@class,'{classToFind}')]");
        }
        public static HtmlNodeCollection SelectNodesByClass(this HtmlNode node, string classToFind)
        {
            return node.SelectNodes($"//*[contains(@class,'{classToFind}')]");
        }
    }
}
