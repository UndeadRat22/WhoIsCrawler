using HtmlAgilityPack;

using WhoIsCrawler.Extensions;

using System.Linq;
using WhoIsCrawler.Models;

namespace WhoIsCrawler.Services
{
    public class WhoIsDataParser
    {
        public DomainInformation ParseDomainInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var table = doc.DocumentNode.SelectSingleNodeByClass("df-block");
            if (table == null)
                return null;

            var rows = table.SelectNodesByClass("df-row");
            if (rows == null)
                return null;
            return new DomainInformation {
                Domain = GetFieldValue(rows, "Domain"),
                Registrar = GetFieldValue(rows, "Registrar"),
                Registered = GetFieldValue(rows, "Registered On"),
                Expires = GetFieldValue(rows, "Expires On"),
                Updated = GetFieldValue(rows, "Updated On"),
                Status = GetFieldHtml(rows, "Status")?.Split("<br>"),
                Servers = GetFieldHtml(rows, "Name Servers")?.Split("<br>"),
            };
        }
        public RegistrantInformation ParseRegistrantInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var tables = doc.DocumentNode.SelectNodesByClass("df-block");
            if (tables == null)
                return null;

            var table = tables.First(node => node.InnerText.Contains("Registrant Contact"));

            var rows = table.SelectNodesByClass("df-row");
            if (rows == null)
                return null;

            return new RegistrantInformation
            {
                Name = GetFieldValue(rows, "Name"),
                Organization = GetFieldValue(rows, "Organization"),
                Street = GetFieldValue(rows, "Street"),
                City = GetFieldValue(rows, "City"),
                PostalCode = GetFieldValue(rows, "Postal Code"),
                Country = GetFieldValue(rows, "Country"),
                State = GetFieldValue(rows, "State"),
                Phone = GetFieldValue(rows, "Phone"),
                Fax = GetFieldValue(rows, "Fax"),
            };
        }

        private string GetFieldHtml(HtmlNodeCollection nodes, string field)
        {
            try
            {
                var res = nodes.SelectByInnerText(field).InnerHtml;
                return res;
            }
            catch
            {
                return null;
            }
        }

        private string GetFieldValue(HtmlNodeCollection nodes, string field)
        {
            try
            {
                var res = nodes.SelectByInnerText(field).InnerText;
                return res;
            }
            catch
            {
                return null;
            }
        }
    }
}
