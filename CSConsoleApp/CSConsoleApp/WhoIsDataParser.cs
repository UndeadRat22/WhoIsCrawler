using HtmlAgilityPack;
using System.Linq;

namespace Crawler
{
    public class WhoIsDataParser
    {
        public DomainInformation ParseHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var table = doc.DocumentNode.SelectSingleNodeByClass("df-block");
            var rows = table.SelectNodesByClass("df-row");

            var domain = SelectByInnerTextValue(rows, "Domain").InnerText;
            var registrar = SelectByInnerTextValue(rows, "Registrar").InnerText;
            var registered = SelectByInnerTextValue(rows, "Registered On").InnerText;
            var expires = SelectByInnerTextValue(rows, "Expires On").InnerText;
            var updated = SelectByInnerTextValue(rows, "Updated On").InnerText;
            var statuses = SelectByInnerTextValue(rows, "Status").InnerHtml.Split("<br>");
            var servers = SelectByInnerTextValue(rows, "Name Servers").InnerHtml.Split("<br>");

            return new DomainInformation
            {
                Domain = domain,
                Registrar = registrar,
                Registered = registered,
                Expires = expires,
                Updated = updated,
                Status = statuses,
                Servers = servers
            };
        }

        private HtmlNode SelectByInnerTextValue(HtmlNodeCollection collection, string innerText)
        {
            return collection
                .First(node => node.InnerText.Contains(innerText))
                .ChildNodes
                .First(node => node.HasClass("df-value"));
        }
    }
}
