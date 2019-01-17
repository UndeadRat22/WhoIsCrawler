using HtmlAgilityPack;

using WhoIsCrawler.Extensions;

using System.Linq;
using WhoIsCrawler.Models;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WhoIsCrawler.Services
{
    public class WhoIsDataParser
    {

        public RawInformation GetRawInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var raw = doc.DocumentNode.SelectSingleNodeByClass("df-raw");
            var rawInfo = new RawInformation();
            if (raw == null)
                return null;
            var pattern = @"^[A-Z][A-Za-z ]+:\s+.*$";
            foreach (var line in raw.InnerText.Split('\n'))
            {
                if (Regex.IsMatch(line, pattern))
                    rawInfo.Raw += $"{line}\n";
            }
            return rawInfo;
        }

        public DomainInformation ParseDomainInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var table = doc.DocumentNode.SelectSingleNodeByClass("df-block");
            if (table == null)
            {
                return null;
            }

            var rows = table.SelectNodesByClass("df-row");
            var raw = doc.DocumentNode.SelectSingleNodeByClass("df-raw");
            if (rows != null)
            {
                return new DomainInformation
                {
                    LastUpdate = GetLastUpdate(raw.InnerText != null ? raw.InnerText : ""),
                    Domain = GetFieldValue(rows, "Domain"),
                    Registrar = GetFieldValue(rows, "Registrar"),
                    Registered = GetFieldValue(rows, "Registered On"),
                    Expires = GetFieldValue(rows, "Expires On"),
                    Updated = GetFieldValue(rows, "Updated On"),
                    Status = GetFieldHtml(rows, "Status")?.Split("<br>"),
                    Servers = GetFieldHtml(rows, "Name Servers")?.Split("<br>"),
                };
            }
            if (raw == null)
                return null;
            var info = ParseRawForDomainInfo(raw.InnerText);
            info.LastUpdate = GetLastUpdate(raw.InnerText);
            return info;
        }
        public RegistrantInformation ParseRegistrantInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var tables = doc.DocumentNode.SelectNodesByClass("df-block");
            if (tables == null)
            {
                return null;
            }
            HtmlNode table = null;
            try
            {
                table = tables.First(node => node.InnerText.Contains("Registrant Contact"));
            }
            catch
            {

            }
            if (table != null)
            {
                var rows = table.SelectNodesByClass("df-row");
                return new RegistrantInformation
                { 
                    Name = GetFieldValue(rows, "Name:"),
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
            else
            {
                var raw = doc.DocumentNode.SelectSingleNodeByClass("df-raw");
                if (raw == null)
                {
                    return null;
                }
                return ParseRawForRegistrantInfo(raw.InnerText);
            }
        }

        private string GetLastUpdate(string raw)
        {
            if (raw == null)
                return null;
            var pattern = @"[&gt;]{3}.*: (.*)Z.[&lt;]{3}";
            foreach (var line in raw.Split('\n'))
            {
                var match = Regex.Match(line, pattern);
                if (match.Success)
                    return match.Groups[1].Value;
            }
            return null;
        }

        private DomainInformation ParseRawForDomainInfo(string raw)
        {
            var pattern = "([A-Z][a-z ]*):\t*([a-z.0-9-A-Z \":/@]*)";
            var lines = raw.Split('\n');
            var info = new DomainInformation();
            var servers = new List<string>();
            foreach (var line in lines)
            {
                if (line.StartsWith('%'))
                    continue;
                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    var name = match.Groups[1].Value;
                    var value = match.Groups[2].Value;
                    if (name.Contains("Domain"))
                        info.Domain = value;
                    if (name.Contains("Registered"))
                        info.Registered = value;
                    if (name.Contains("Expires"))
                        info.Expires = value;
                    if (name.Contains("Registrar"))
                        info.Registrar = value;
                    if (name.Contains("Nameserver"))
                        servers.Add(value);
                }
            }
            info.Servers = servers.ToArray();
            return info;
        }

        private RegistrantInformation ParseRawForRegistrantInfo(string raw)
        {
            var pattern = "([A-Z][a-z ]*):\t*([a-z.0-9-A-Z \":/@]*)";
            var lines = raw.Split('\n');
            var info = new RegistrantInformation();
            foreach (var line in lines)
            {
                if (line.StartsWith('%'))
                    continue;
                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    var name = match.Groups[1].Value;
                    var value = match.Groups[2].Value;
                    if (name.Contains("Domain"))
                        info.Name = value;
                    if (name.Contains("Registrar"))
                    {
                        info.Organization = value;
                        break;
                    }
                }
            }
            return info;
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
